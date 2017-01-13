using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Players
{
    public class ListPlayersAction : CliActionBase
    {
        public override string CommandName => "player list";
        private const int ColumnPaddingSpaces = 4;

        private readonly IPlayerService _playerService;
        private readonly ILedgerService _ledgerService;

        public ListPlayersAction(IPlayerService playerService, ILedgerService ledgerService)
        {
            _playerService = playerService;
            _ledgerService = ledgerService;
        }

        public override void Execute()
        {
            var players = _playerService.GetAllPlayers();
            if(!players.Any())
            {
                return;
            }
            
            var maxNameLength = players.Max(a => a.Name.Length);

            var builder = new StringBuilder();
            builder.Append("Name").Append(' ', maxNameLength - 4);
            builder.Append(' ', ColumnPaddingSpaces);
            builder.Append("Balance");
            builder.AppendLine();
            builder.Append('-', builder.Length - 1);
            builder.AppendLine();

            foreach(var p in players.OrderBy(a => a.Name))
            {
                var balance = _ledgerService.GetBalance(p.Id);

                builder.Append(p.Name).Append(' ', maxNameLength - p.Name.Length);
                builder.Append(' ', ColumnPaddingSpaces);
                builder.Append(balance);
                builder.AppendLine();
            }

            Console.WriteLine(builder.ToString());
        }
    }
}