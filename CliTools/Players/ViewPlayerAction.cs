using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Players
{
    public class ViewPlayerAction : SearchPlayerActionBase
    {
        public ViewPlayerAction(IPlayerService playerService)
            : base(playerService) { }

        public override string CommandName => "player view";
        protected override string PreInputMessage => "Who are you looking for?";

        protected override void ExecuteCore(Guid playerId)
        {
            var player = PlayerService.GetPlayer(playerId);
            var creditPlural = player.Balance != 1 ? "s" : string.Empty;
            Console.WriteLine($"{player.Name} currently has {player.Balance} credit{creditPlural}.");
        }
    }
}