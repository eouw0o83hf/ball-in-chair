using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Players
{
    public class ViewPlayerAction : SearchPlayerActionBase
    {
        private readonly ILedgerService _ledgerService;
        
        public ViewPlayerAction(IPlayerService playerService, ILedgerService ledgerService)
            : base(playerService) 
        { 
            _ledgerService = ledgerService;
        }

        public override string CommandName => "player view";
        protected override string PreInputMessage => "Who are you looking for?";

        protected override void ExecuteCore(Guid playerId)
        {
            var player = PlayerService.GetPlayer(playerId);
            var balance = _ledgerService.GetBalance(playerId);
            var creditPlural = balance != 1 ? "s" : string.Empty;
            Console.WriteLine($"{player.Name} currently has {balance} credit{creditPlural}.");
        }
    }
}