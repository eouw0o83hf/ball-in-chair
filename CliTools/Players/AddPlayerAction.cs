using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Players
{
    public class AddPlayerAction : IAutocompletingCliAction
    {
        public string CommandName => "player add";

        private readonly IPlayerService _playerService;

        public AddPlayerAction(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public void Execute()
        {
            Console.WriteLine("Add a player");
            Console.WriteLine("What is this player's name?");
            
            var name = Console.ReadLine();
            var newId = Guid.NewGuid();
            _playerService.CreatePlayer(newId, name);

            Console.WriteLine($"Successfully added [{name}] as Player ID [{newId}]");
        }
    }
}