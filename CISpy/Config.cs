using Exiled.API.Interfaces;
using System.Collections.Generic;
using PlayerRoles;

namespace CISpy
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
		public bool Debug { get; set; }

		public List<RoleTypeId> SpyRoles { get; set; } = new List<RoleTypeId>() { RoleTypeId.NtfPrivate, RoleTypeId.NtfSergeant };

		public bool SpawnWithGrenade { get; set; } = true;

		public int SpawnChance { get; set; } = 40;
		public int GuardSpawnChance { get; set; } = 50;
		public int MinimumSquadSize { get; set; } = 6;
	}
}
