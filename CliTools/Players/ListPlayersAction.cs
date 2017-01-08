using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Players
{
    public class ListPlayersAction : IAutocompletingCliAction
    {
        public string CommandName => "player list";
        private const int ColumnPaddingSpaces = 4;

        private readonly IPlayerService _playerService;

        public ListPlayersAction(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public void Execute()
        {
            var players = _playerService.GetAllPlayers();
            var maxNameLength = players.Max(a => a.Name.Length);

            var builder = new StringBuilder();
            builder.Append("Name").Append(' ', maxNameLength - 4);
            builder.Append(' ', ColumnPaddingSpaces);
            builder.Append("Balance");
            builder.AppendLine();
            builder.Append('-', builder.Length - 1);
            builder.AppendLine();

            foreach(var p in players)
            {
                builder.Append(p.Name).Append(' ', maxNameLength - p.Name.Length);
                builder.Append(' ', ColumnPaddingSpaces);
                builder.Append(p.Balance);
                builder.AppendLine();
            }

            Console.WriteLine(builder.ToString());
        }
    }
}