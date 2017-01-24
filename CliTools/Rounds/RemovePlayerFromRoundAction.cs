using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.CliTools.Players;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Rounds
{
    public class RemovePlayerFromRoundAction : SearchPlayerActionBase
    {
        private readonly IRoundService _roundService;
        private readonly ILedgerService _ledgerService;

        public RemovePlayerFromRoundAction(IPlayerService playerService, IRoundService roundService, ILedgerService ledgerService)
            : base(playerService)
        {
            _roundService = roundService;
            _ledgerService = ledgerService;
        }

        public override string CommandName => "round undo election";
        protected override string PreInputMessage => "Who do you want to remove?";

        protected override void ExecuteCore(Guid playerId)
        {
            var roundId = _roundService.GetOpenRoundId();
            if(roundId == null)
            {
                ConsoleHelpers.WriteRedLine("No round is currently open");
                return;
            }
            
            var round = _roundService.GetRound(roundId.Value);
            if(!round.EntrantPlayerIds.Contains(playerId))
            {
                ConsoleHelpers.WriteRedLine("That player is not enrolled in the current round.");
                return;
            }

            _roundService.RemoveElectionFromRound(roundId.Value, playerId);

            ConsoleHelpers.WriteGreenLine($"Successfully removed election from the round");
        }
    }
}