using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Rounds
{
    public class WinRoundAction : CliActionBase
    {
        public override string CommandName => "round declare winner";

        private readonly IPlayerService _playerService;
        private readonly IRoundService _roundService;
        private readonly ITabCompletableQueryContainer _playerCompletionContainer;
        private readonly ITabCompletionProvider _playerCompletionProvider;

        public WinRoundAction(IPlayerService playerService, IRoundService roundService)
        {
            _playerService = playerService;
            _roundService = roundService;
            _playerCompletionContainer = new PlayerNameQueryContainer(playerService, ExecuteCore);
            _playerCompletionProvider = new TabCompletionProvider(_playerCompletionContainer);
        }
        
        public override void Execute()
        {
            Console.WriteLine("Who won?");
            _playerCompletionProvider.DoTabCompletion();
        }

        private void ExecuteCore(Guid playerId)
        {
            var roundId = _roundService.GetOpenRoundId();
            if(roundId == null)
            {
                Console.WriteLine("No Round is currently open - open a new Round first.");
            }

            var round = _roundService.GetRound(roundId.Value);
            if(!round.EntrantPlayerIds.Contains(playerId))
            {
                Console.WriteLine("That player didn't enter this Round! Try again.");
            }

            _roundService.DeclareWinner(roundId.Value, playerId);
        }
    }
}