using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Rounds
{
    public class AddPlayerToRoundAction : CliActionBase
    {
        public override string CommandName => "round make election";

        private readonly IPlayerService _playerService;
        private readonly IRoundService _roundService;
        private readonly ITabCompletableQueryContainer _playerCompletionContainer;
        private readonly ITabCompletionProvider _playerCompletionProvider;

        public AddPlayerToRoundAction(IPlayerService playerService, IRoundService roundService)
        {
            _playerService = playerService;
            _roundService = roundService;
            _playerCompletionContainer = new PlayerNameQueryContainer(playerService, ExecuteCore);
            _playerCompletionProvider = new TabCompletionProvider(_playerCompletionContainer);
        }
        
        public override void Execute()
        {
            Console.WriteLine("Who should we add?");
            _playerCompletionProvider.DoTabCompletion();
        }

        private void ExecuteCore(Guid playerId)
        {
            var player = _playerService.GetPlayer(playerId);
            if(player.Balance <= 0)
            {
                Console.WriteLine($"{player.Name} does not have enough credits to elect.");
                return;
            }

            var round = _roundService.GetOpenRoundId();
            if(round == null)
            {
                Console.WriteLine("No Round is currently open - open a new Round first.");
            }

            _roundService.AddElectionToRound(round.Value, playerId);

            Console.WriteLine($"Added {player.Name} to the Round");
        }
    }
}