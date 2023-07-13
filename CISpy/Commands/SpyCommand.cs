using CommandSystem;
using Exiled.API.Features;
using System;
using RemoteAdmin;
using Exiled.Permissions.Extensions;
using PlayerRoles;

namespace CISpy.Commands.SpyCommand
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SpyCommand : ParentCommand
    {
        public SpyCommand() => LoadGeneratedCommands();
        
        public override string Command { get; } = "cispy";

        public override string[] Aliases { get; } = new string[] { "cis", "spy", "makespy" };

        public override string Description { get; } = "Makes the target into a CISpy.";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("snow.makespy"))
            {
                response = "You do not have permission to use this command! Missing Permission: snow.makespy";
                return false;
            }
            
            if (arguments.Count != 1)
            {
                response = "Invalid Usage! Usage: cispy (player id)";
                return false;
            }
            
            Player ply = Player.Get(arguments.At(0));
            {
                if (ply == null)
                {
                    response = $"Player not found: {arguments.At(0)}";
                    return false;
                }

                if (ply.Role == RoleTypeId.Spectator || ply.Role == RoleTypeId.None || ply.Role.Team == Team.SCPs ||
                    ply.Role.Team == Team.Scientists || ply.Role.Team == Team.ChaosInsurgency ||
                    ply.Role.Team == Team.ClassD || ply.Role.Team == Team.OtherAlive ||
                    ply.Role == RoleTypeId.NtfCaptain)
                {
                    response = $"You cannot make that player a CISpy right now, they are not NTF!";
                    return false;
                }
            }
            EventHandlers.MakeSpy(ply, false, true);
            response = $"Spawned '{ply.Nickname}' as a CISpy.";
            return true;
        }
    }
}