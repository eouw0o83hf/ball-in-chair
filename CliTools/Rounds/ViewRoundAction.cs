using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Rounds
{
    public class ViewRoundAction : CliActionBase
    {
        public override string CommandName => "round details";

        private readonly IRoundService _roundService;
        private readonly IPlayerService _playerService;

        public ViewRoundAction(IRoundService roundService, IPlayerService playerService)
        {
            _roundService = roundService;
            _playerService = playerService;
        }

        public override void Execute()
        {
            var rounds = _roundService.GetAllRounds();
            var maxRound = rounds.Max(a => a.RoundNumber);

            Console.WriteLine($"There are {maxRound} Rounds - which one would you like to view?");
            var roundIdString = Console.ReadLine()?.Trim();
            int roundId;
            if(!int.TryParse(roundIdString, out roundId))
            {
                Console.WriteLine("Input was an invalid number.");
            }

            var round = rounds.Single(a => a.RoundNumber == roundId);
            
            Console.WriteLine($"Round {round.RoundNumber} took place on {round.Date}, ID is {round.Id}");

            Console.WriteLine("The following elections were made:");
            foreach(var playerId in round.EntrantPlayerIds)
            {
                var player = _playerService.GetPlayer(playerId);
                Console.WriteLine($" - {player.Name} ({player.Id})");
            }

            if(round.IsOpen)
            {
                Console.WriteLine("This is the current, open Round.");
            }
            else if(round.WinningPlayerId.HasValue)
            {
                var winner = _playerService.GetPlayer(round.WinningPlayerId.Value);
                Console.WriteLine($"The winner was {winner.Name}");
            }
            else
            {
                Console.WriteLine("No winner is listed, which means that this is a legacy Round");
            }
        }
    }
}