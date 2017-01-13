using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Rounds
{
    public class ListRoundsAction : CliActionBase
    {
        public override string CommandName => "round history";

        private readonly IRoundService _roundService;

        public ListRoundsAction(IRoundService roundService)
        {
            _roundService = roundService;
        }

        public override void Execute()
        {
            var rounds = _roundService.GetAllRounds();
            
            foreach(var round in rounds)
            {
                Console.WriteLine($"Round {round.RoundNumber} had {round.EntrantPlayerIds.Count} elections");
            }
        }
    }
}