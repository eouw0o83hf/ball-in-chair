using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Rounds
{
    public class ListEntrantsRoundAction : CliActionBase
    {
        public override string CommandName => "round list entrants";

        private readonly IRoundService _roundService;
        private readonly IPlayerService _playerService;

        public ListEntrantsRoundAction(IRoundService roundService, IPlayerService playerService)
        {
            _roundService = roundService;
            _playerService = playerService;
        }

        public override void Execute()
        {
            var roundId = _roundService.GetOpenRoundId();
            if(roundId == null)
            {
                Console.WriteLine("No round is open.");
            }

            var round = _roundService.GetRound(roundId.Value);
            
            Console.WriteLine($"Round {round.RoundNumber} entrants");
            Console.WriteLine("----------------------------------");

            if(!round.EntrantPlayerIds.Any())
            {
                Console.WriteLine("[[ No entrants yet ]]");
                return;
            }

            foreach(var playerId in round.EntrantPlayerIds)
            {
                var player = _playerService.GetPlayer(playerId);
                Console.WriteLine(player.Name);
            }
        }
    }
}