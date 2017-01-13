using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BallInChair.CliTools.Framework;
using BallInChair.Persistence;

namespace BallInChair.CliTools.Players
{
    public class ViewPlayerAction : SearchPlayerActionBase
    {
        private readonly ILedgerService _ledgerService;
        private readonly IRoundService _roundService;
        
        public ViewPlayerAction(IPlayerService playerService, ILedgerService ledgerService, IRoundService roundService)
            : base(playerService) 
        { 
            _ledgerService = ledgerService;
            _roundService = roundService;
        }

        public override string CommandName => "player view";
        protected override string PreInputMessage => "Who are you looking for?";

        protected override void ExecuteCore(Guid playerId)
        {
            var player = PlayerService.GetPlayer(playerId);
            var balance = _ledgerService.GetBalance(playerId);
            var creditPlural = balance != 1 ? "s" : string.Empty;
            Console.WriteLine($"{player.Name} currently has {balance} credit{creditPlural}.");
            Console.WriteLine("Detailed credit listing:");

            var textyTable = new Dictionary<string, List<string>>();
            textyTable["Type"] = new List<string>();
            textyTable["Amount"] = new List<string>();
            textyTable["Time"] = new List<string>();
            textyTable["Round"] = new List<string>();

            var history = _ledgerService.GetPlayerHistory(playerId);

            foreach(var item in history.OrderBy(a => a.TransactionTime))
            {
                textyTable["Type"].Add(item.Type.ToString());
                textyTable["Amount"].Add(item.Amount.ToString());
                textyTable["Time"].Add(item.TransactionTime.ToString());

                int? roundNumber = null;
                if(item.RoundId.HasValue)
                {
                    roundNumber = _roundService.GetRound(item.RoundId.Value).RoundNumber;
                }
                textyTable["Round"].Add(roundNumber?.ToString());
            }

            var output = new StringBuilder();
            AppendHeader(textyTable, "Type", output);
            AppendHeader(textyTable, "Amount", output);
            AppendHeader(textyTable, "Time", output);
            AppendHeader(textyTable, "Round", output);
            output.AppendLine();
            output.Append('-', output.Length - 1).AppendLine();

            for(var i = 0; i < history.Count; ++i)
            {
                AppendRow(textyTable, new [] { "Type", "Amount", "Time", "Round" }, output, i);
            }

            Console.WriteLine(output.ToString());
        }

        private static int GetLongest(string key, IEnumerable<string> values) => new [] { key }.Union(values).Where(a => a != null).Max(a => a.Length);
        
        private static void AppendHeader(IDictionary<string, List<string>> dictionary, string column, StringBuilder builder)
        {
            builder.Append(column).Append(' ', GetLongest(column, dictionary[column]) - column.Length).Append(' ');
        }

        private static void AppendRow(IDictionary<string, List<string>> dictionary, IEnumerable<string> columnOrder, StringBuilder builder, int rowIndex)
        {
            foreach(var column in columnOrder)
            {
                var value = dictionary[column][rowIndex] ?? string.Empty;
                builder.Append(value).Append(' ', GetLongest(column, dictionary[column]) - value.Length).Append(' ');
            }
            builder.AppendLine();
        }
    }
}