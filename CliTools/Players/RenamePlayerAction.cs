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
            _playerCompletionContainer = new PlayerNameQueryContainer(playerService, Rename);
            _playerCompletionProvider = new TabCompletionProvider(_playerCompletionContainer);
        }

        public override void Execute()
        {
            Console.WriteLine("Renaming a player: who should we rename?");
            _playerCompletionProvider.DoTabCompletion();
        }

        private void Rename(Guid playerId)
        {
            var player = _playerService.GetPlayer(playerId);

            Console.WriteLine($"What should we rename `{player.Name}` to?");

            var newName = Console.ReadLine();
            if(string.IsNullOrEmpty(newName))
            {
                Console.WriteLine("Canceled renaming");
            }

            _playerService.UpdatePlayerName(playerId, newName.Trim());
            Console.WriteLine($"Successfully renamed `{player.Name}` to `{newName}`");
        }
    }
}