using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using PlayerRoles;

namespace CISpy
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
		public bool Debug { get; set; }

		public List<RoleTypeId> SpyRoles { get; set; } = new List<RoleTypeId>() { RoleTypeId.NtfPrivate, RoleTypeId.NtfSergeant };

		[Description("Whether or not the CISpy should spawn with a HE Grenade. If it is false, they will spawn with a flash instead.")]
		public bool SpawnWithGrenade { get; set; } = false;

		[Description("The spawn percent chance of a CISpy during a wave. Do not edit unless you do not want the spy to spawn 100% of the time.")]
		public int SpawnChance { get; set; } = 100;
		
		[Description("The spawn percent chance of a CISpy as a Guard during the start of the game. Set to 0 to disable.")]
		public int GuardSpawnChance { get; set; } = 15;
		
		[Description("The minimum squad size of NTF required before a spy can spawn. If you want to have it spawn no matter what, it is suggested to a minimum of 4 or else you will have gameplay issues!")]
		public int MinimumSquadSize { get; set; } = 6;
	}
}
