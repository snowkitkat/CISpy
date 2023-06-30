using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;

namespace CISpy
{
	partial class EventHandlers
	{
		internal static Dictionary<Player, bool> spies = new Dictionary<Player, bool> ();
		private List<Player> ffPlayers = new List<Player>();

		private bool isDisplayFriendly = false;
		//private bool isDisplaySpy = false;

		private Random rand = new Random();

		public void OnRoundStart()
		{
			spies.Clear();
			ffPlayers.Clear();
			if (rand.Next(1, 101) <= CISpy.instance.Config.GuardSpawnChance)
			{
				Player player = Player.List.FirstOrDefault(x => x.Role == RoleTypeId.FacilityGuard);
				if (player != null)
				{
					Timing.CallDelayed(0.8f, () =>
					{
						MakeSpy(player);
					});
				}
			}
		}

		public void OnTeamRespawn(RespawningTeamEventArgs ev)
		{
			if (ev.NextKnownTeam == Respawning.SpawnableTeamType.NineTailedFox && rand.Next(1, 101) <= CISpy.instance.Config.SpawnChance && ev.Players.Count >= CISpy.instance.Config.MinimumSquadSize)
			{
				List<Player> respawn = new List<Player>(ev.Players);
				Timing.CallDelayed(0.1f, () =>
				{
					List<Player> roleList = respawn.Where(x => CISpy.instance.Config.SpyRoles.Contains(x.Role)).ToList();
					if (roleList.Count > 0)
					{
						Player player = roleList[rand.Next(roleList.Count)];
						if (player != null)
						{
							Timing.CallDelayed(0.8f, () =>
							{
								MakeSpy(player);
							});
						}
					}
				});
			}
		}

		public void OnEscaping(EscapingEventArgs ev)
		{
			if (ev.Player.Role == RoleTypeId.ClassD && ev.Player.IsCuffed && spies.ContainsKey(ev.Player.Cuffer))
			{
				Timing.CallDelayed(0.1f, () =>
				{
					Timing.CallDelayed(0.8f, () =>
					{
						MakeSpy(ev.Player);
					});
				});
			}
		}

		public void OnSetClass(ChangingRoleEventArgs ev)
		{
			if (spies.ContainsKey(ev.Player))
			{
				Timing.CallDelayed(0.1f, () => spies.Remove(ev.Player));
			}
		}

		public void OnPlayerDie(DiedEventArgs ev)
		{
			if (spies.ContainsKey(ev.Player))
			{
				spies.Remove(ev.Player);
			}

			CheckSpies(ev.Player);
		}

		public void OnPlayerLeave(LeftEventArgs ev)
		{
			CheckSpies(ev.Player);
		}

		public void OnHandcuffing(HandcuffingEventArgs ev)
		{
			if ((spies.ContainsKey(ev.Target) && ev.Player.Role.Team == Team.ChaosInsurgency) ||
				(spies.ContainsKey(ev.Player) && ev.Target.Role.Team == Team.ChaosInsurgency))
			{
				ev.IsAllowed = false;
			}
		}

		public void OnPlayerHurt(HurtingEventArgs ev)
		{
			if (ffPlayers.Contains(ev.Attacker))
			{
				Timing.CallDelayed(0.1f, () =>
				{
					RemoveFF(ev.Attacker);
				});
			}

			Player scp035 = null;

			try
			{
				scp035 = TryGet035();
			} 
			catch (Exception x)
			{
				Log.Debug("SCP-035 not installed, skipping method call...");
			}

			if (spies.ContainsKey(ev.Player) && !spies.ContainsKey(ev.Attacker) && ev.Player.Id != ev.Attacker.Id && (ev.Attacker.Role.Team == Team.ChaosInsurgency || ev.Attacker.Role.Team == Team.ClassD) &&  ev.Attacker.Id != scp035?.Id)
			{
				if (!isDisplayFriendly)
				{
					ev.Attacker.Broadcast(3, "<i>You are shooting a <b><color=\"green\">CISpy!</color></b></i>");
					isDisplayFriendly = true;
				}
				Timing.CallDelayed(3f, () =>
				{
					isDisplayFriendly = false;
				});
				ev.IsAllowed = false;
			}
			else if (!spies.ContainsKey(ev.Player) && spies.ContainsKey(ev.Attacker) && (ev.Player.Role.Team == Team.ChaosInsurgency || ev.Player.Role.Team == Team.ClassD) && ev.Player.Id != scp035?.Id)
			{
				ev.IsAllowed = false;
			}
			/*else if (spies.ContainsKey(ev.Attacker) && spies.ContainsKey(ev.Player))
			{
				if (!isDisplaySpy)
				{
					ev.Attacker.Broadcast(3, "You are shooting another <b><color=\"green\">CISpy!</color></b>", false);
					isDisplaySpy = true;
				}
				Timing.CallDelayed(3f, () =>
				{
					isDisplaySpy = false;
				});
				ev.Amount = 0;
			}*/ 
		}

		public void OnShoot(ShootingEventArgs ev)
		{
			Player target = Player.Get(ev.TargetNetId);
			if (target == null) return;

			Player scp035 = null;

			if (CISpy.isScp035)
			{
				scp035 = TryGet035();
			}

			if (spies.ContainsKey(ev.Player) && !spies.ContainsKey(target) && (target.Role.Team == Team.Scientists || target.Role.Team == Team.FoundationForces) && target.Id != scp035?.Id)
			{
				if (!spies[ev.Player])
				{
					spies[ev.Player] = true;
					ev.Player.Broadcast(10, $"<i>You have attacked a {(target.Role.Team == Team.FoundationForces ? "<color=#00b0fc>Nine Tailed Fox" : "<color=#fcff8d>Scientist")}</color>, you are now able to be killed by <color=#00b0fc>Nine Tailed Fox</color> and <color=#fcff8d>Scientists</color>.</i>");
				}
				GrantFF(ev.Player);
			}
			else if (spies.ContainsKey(target) && !spies.ContainsKey(ev.Player) && (ev.Player.Role.Team == Team.FoundationForces || ev.Player.Role.Team == Team.Scientists))
			{
				if (spies[target])
				{
					GrantFF(ev.Player);
				}
			}
		}
	}
}
