using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Players
{
    public class AddPlayerAction : CliActionBase
    {
        public override string CommandName => "player add";

        private readonly IPlayerService _playerService;

        public AddPlayerAction(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public override void Execute()
        {
            Console.WriteLine("Add a player");
            Console.WriteLine("What is this player's name?");
            
            var name = Console.ReadLine();
            var newId = Guid.NewGuid();
            _playerService.CreatePlayer(newId, name);

            ConsoleHelpers.WriteGreenLine($"Successfully added [{name}] as Player ID [{newId}]");
        }
    }
}