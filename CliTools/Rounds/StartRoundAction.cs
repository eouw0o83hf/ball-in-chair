using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Rounds
{
    public class StartRoundAction : CliActionBase
    {
        public override string CommandName => "round start";

        private readonly IRoundService _roundService;

        public StartRoundAction(IRoundService roundService)
        {
            _roundService = roundService;
        }

        public override void Execute()
        {
            var existingRound = _roundService.GetOpenRoundId();
            if(existingRound.HasValue)
            {
                ConsoleHelpers.WriteRedLine("There's already an open round, you donkey-brain!");
                return;
            }

            var roundId = Guid.NewGuid();
            _roundService.OpenNewRound(roundId);
            var round = _roundService.GetRound(roundId);
            
            ConsoleHelpers.WriteGreenLine($"Opened Round {round.RoundNumber}");
        }
    }
}