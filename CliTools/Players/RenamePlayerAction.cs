using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Players
{
    public class RenamePlayerAction : SearchPlayerActionBase
    {
        public RenamePlayerAction(IPlayerService playerService)
            : base(playerService) { }

        public override string CommandName => "player rename";

        protected override string PreInputMessage => "Renaming a player: who should we rename?";

        protected override void ExecuteCore(Guid playerId)
        {
            var player = PlayerService.GetPlayer(playerId);

            Console.WriteLine($"What should we rename `{player.Name}` to?");

            var newName = Console.ReadLine();
            if(string.IsNullOrEmpty(newName))
            {
                Console.WriteLine("Canceled renaming");
                return;
            }

            PlayerService.UpdatePlayerName(playerId, newName.Trim());
            ConsoleHelpers.WriteGreenLine($"Successfully renamed `{player.Name}` to `{newName}`");
        }
    }
}