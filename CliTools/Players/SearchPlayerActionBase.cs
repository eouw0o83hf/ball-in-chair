using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Players
{
    public abstract class SearchPlayerActionBase : CliActionBase
    {
        protected readonly IPlayerService PlayerService;
        private readonly ITabCompletableQueryContainer _playerCompletionContainer;
        private readonly ITabCompletionProvider _playerCompletionProvider;

        protected SearchPlayerActionBase(IPlayerService playerService)
        {
            PlayerService = playerService;
            _playerCompletionContainer = new PlayerNameQueryContainer(playerService, ExecuteCore);
            _playerCompletionProvider = new TabCompletionProvider(_playerCompletionContainer);
        }

        protected abstract string PreInputMessage { get; }
        protected abstract void ExecuteCore(Guid playerId);

        public override void Execute()
        {
            Console.WriteLine(PreInputMessage);
            _playerCompletionProvider.DoTabCompletion();
        }
    }
}