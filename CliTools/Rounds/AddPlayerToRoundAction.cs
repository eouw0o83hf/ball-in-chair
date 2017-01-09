using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.CliTools.Players;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Rounds
{
    public class AddPlayerToRoundAction : SearchPlayerActionBase
    {
        private readonly IRoundService _roundService;
        private readonly ILedgerService _ledgerService;

        public AddPlayerToRoundAction(IPlayerService playerService, IRoundService roundService, ILedgerService ledgerService)
            : base(playerService)
        {
            _roundService = roundService;
            _ledgerService = ledgerService;
        }

        public override string CommandName => "round make election";
        protected override string PreInputMessage => "Who do you want to add?";

        protected override void ExecuteCore(Guid playerId)
        {
            var player = PlayerService.GetPlayer(playerId);
            var balance = _ledgerService.GetBalance(playerId);
            
            if(balance <= 0)
            {
                ConsoleHelpers.WriteRedLine($"{player.Name} does not have enough credits to elect.");
                return;
            }

            var round = _roundService.GetOpenRoundId();
            if(round == null)
            {
                ConsoleHelpers.WriteRedLine("No Round is currently open - open a new Round first.");
                return;
            }

            _roundService.AddElectionToRound(round.Value, playerId);

            ConsoleHelpers.WriteGreenLine($"Added {player.Name} to the Round");
        }
    }
}