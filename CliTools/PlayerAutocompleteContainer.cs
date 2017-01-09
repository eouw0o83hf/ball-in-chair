using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;
using BallInChair.Persistence;

namespace BallInChair.CliTools
{
    public class PlayerNameQueryContainer : ITabCompletableQueryContainer
    {
        private readonly IPlayerService _playerService;
        private readonly Action<Guid> _action;
        
        public PlayerNameQueryContainer(IPlayerService playerService, Action<Guid> action)
        {
            _playerService = playerService;
            _action = action;
        }

        public IEnumerable<ITabCompletableResponseItem> GetMatches(string input)
        {
            return from p in _playerService.GetAllPlayers()
                   where p.Name.IndexOf(input, StringComparison.OrdinalIgnoreCase) > -1
                   select new PlayerResponse(p.Name, p.Id, _action);
        }

        private class PlayerResponse : ITabCompletableResponseItem
        {
            private readonly string _name;
            private readonly Guid _id;
            private readonly Action<Guid> _executeMethod;
            
            public PlayerResponse(string name, Guid id, Action<Guid> executeMethod)
            {
                _name = name;
                _id = id;
                _executeMethod = executeMethod;
            }
            
            public string FullText => _name;
            public void Execute() => _executeMethod(_id);
        }
    }
}