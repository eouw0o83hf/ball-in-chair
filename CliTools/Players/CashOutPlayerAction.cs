using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Players
{
    public class CashOutPlayerAction : SearchPlayerActionBase
    {
        private readonly ILedgerService _ledgerService;

        public CashOutPlayerAction(IPlayerService playerService, ILedgerService ledgerService)
            : base(playerService)
        {
            _ledgerService = ledgerService;
        }

        public override string CommandName => "player cash out";
        protected override string PreInputMessage => "Cashing out credits - who are we cashing out?";

        protected override void ExecuteCore(Guid playerId)
        {
            var player = PlayerService.GetPlayer(playerId);
            
            var balance = _ledgerService.GetBalance(playerId);
            if(balance <= 0)
            {
                ConsoleHelpers.WriteRedLine($"This player has {balance} credits and is ineligible to cash out.");
                return;
            }

            Console.WriteLine($"{player.Name} has {balance} credits. How many are you cashing out?");
            var creditsString = Console.ReadLine();
            int credits;

            if(string.IsNullOrEmpty(creditsString) || !int.TryParse(creditsString, out credits))
            {
               ConsoleHelpers.WriteRedLine("Invalid input, try again"); 
               return;
            }

            if(credits <= 0)
            {
                return;
            }
            if(credits > balance)
            {
                ConsoleHelpers.WriteRedLine($"{player.Name} only has {balance} credits. You can't cash out {credits}.");
                return;
            }

            _ledgerService.PurchaseCredits(playerId, -credits);
            ConsoleHelpers.WriteGreenLine($"Successfully cashed out {player.Name} with {credits} credits");
        }
    }
}