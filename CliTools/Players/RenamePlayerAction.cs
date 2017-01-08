using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Players
{
    public class RenamePlayerAction : CliActionBase
    {
        public override string CommandName => "player rename";

        private readonly IPlayerService _playerService;
        private readonly ITabCompletableQueryContainer _playerCompletionContainer;
        private readonly ITabCompletionProvider _playerCompletionProvider;

        public RenamePlayerAction(IPlayerService playerService)
        {
            _playerService = playerService;
            _playerCompletionContainer = new PlayerNameQueryContainer(playerService);
            _playerCompletionProvider = new TabCompletionProvider(_playerCompletionContainer);
        }

        public override void Execute()
        {
            Console.WriteLine("Renaming a player: who should we rename?");
            _playerCompletionProvider.DoTabCompletion();
        }
    }

    public class PlayerNameQueryContainer : ITabCompletableQueryContainer
    {
        private readonly IPlayerService _playerService;
        
        public PlayerNameQueryContainer(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public IEnumerable<ITabCompletableResponseItem> GetMatches(string input)
        {
            return from p in _playerService.GetAllPlayers()
                   where p.Name.IndexOf(input, StringComparison.OrdinalIgnoreCase) > -1
                   select new PlayerResponse(p.Name, p.Id, Rename);
        }

        private void Rename(Guid playerId, string name)
        {
            Console.WriteLine($"What should we rename `{name}` to?");

            var newName = Console.ReadLine();
            if(string.IsNullOrEmpty(newName))
            {
                Console.WriteLine("Canceled renaming");
            }

            _playerService.UpdatePlayerName(playerId, newName.Trim());
            Console.WriteLine($"Successfully renamed `{name}` to `{newName}`");
        }

        private class PlayerResponse : ITabCompletableResponseItem
        {
            private readonly string _name;
            private readonly Guid _id;
            private readonly Action<Guid, string> _executeMethod;
            
            public PlayerResponse(string name, Guid id, Action<Guid, string> executeMethod)
            {
                _name = name;
                _id = id;
                _executeMethod = executeMethod;
            }
            
            public string FullText => _name;
            public void Execute() => _executeMethod(_id, _name);
        }
    }
}