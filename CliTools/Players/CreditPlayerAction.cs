using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Players
{
    public class CreditPlayerAction : SearchPlayerActionBase
    {
        private readonly ILedgerService _ledgerService;

        public CreditPlayerAction(IPlayerService playerService, ILedgerService ledgerService)
            : base(playerService)
        {
            _ledgerService = ledgerService;
        }

        public override string CommandName => "player buy credit";
        protected override string PreInputMessage => "Purchasing credits - who are we crediting?";

        protected override void ExecuteCore(Guid playerId)
        {
            var player = PlayerService.GetPlayer(playerId);
            
            Console.WriteLine($"How many credits did {player.Name} purchase?");
            var creditsString = Console.ReadLine();
            int credits;

            if(string.IsNullOrEmpty(creditsString) || !int.TryParse(creditsString, out credits))
            {
               ConsoleHelpers.WriteRedLine("Invalid input, try again"); 
               return;
            }

            _ledgerService.PurchaseCredits(playerId, credits);
            ConsoleHelpers.WriteGreenLine($"Successfully credited {player.Name} with {credits} credits");
        }
    }
}