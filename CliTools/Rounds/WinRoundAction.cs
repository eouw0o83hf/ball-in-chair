using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.CliTools.Players;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Rounds
{
    public class WinRoundAction : SearchPlayerActionBase
    {
        private readonly IRoundService _roundService;

        public WinRoundAction(IPlayerService playerService, IRoundService roundService)
            : base(playerService)
        {
            _roundService = roundService;
        }

        public override string CommandName => "round declare winner";
        protected override string PreInputMessage => "Who won?";

        protected override void ExecuteCore(Guid playerId)
        {
            var roundId = _roundService.GetOpenRoundId();
            if(roundId == null)
            {
                ConsoleHelpers.WriteRedLine("No Round is currently open - open a new Round first.");
                return;
            }

            var round = _roundService.GetRound(roundId.Value);
            if(!round.EntrantPlayerIds.Contains(playerId))
            {
                ConsoleHelpers.WriteRedLine("That player didn't enter this Round! Try again.");
                return;
            }

            var player = PlayerService.GetPlayer(playerId);

            _roundService.DeclareWinner(roundId.Value, playerId);
            ConsoleHelpers.WriteGreenLine($"{player.Name} won {round.EntrantPlayerIds.Count} credits to end Round {round.RoundNumber}");
        }
    }
}