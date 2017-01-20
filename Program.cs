using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BallInChair.CliTools;
using BallInChair.CliTools.Framework;
using BallInChair.CliTools.Players;
using BallInChair.CliTools.Rounds;
using BallInChair.Persistence;
using NodaTime;

namespace BallInChair
{
    public class Program
    {
        private static readonly string HomeDirectory = Environment.GetEnvironmentVariable("HOME");
        private static readonly string PersistenceDirectory = Path.Combine(HomeDirectory, "Dropbox/Code/ball-in-chair");
        
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Ball on Chair!");

            // Wire up dependencies
            var ledgerService = new JsonBackedLedgerService(SystemClock.Instance, PersistenceDirectory);
            var playerService = new JsonBackedPlayerService(PersistenceDirectory);
            var roundService = new JsonBackedRoundService(playerService, ledgerService, SystemClock.Instance, PersistenceDirectory);            

            // Link Actions
            var exit = new ExitContainer();
            var autocompleteActions = new List<CliActionBase>
            {
                new AddPlayerAction(playerService),
                new CashOutPlayerAction(playerService, ledgerService),
                new CreditPlayerAction(playerService, ledgerService),
                new ViewPlayerAction(playerService, ledgerService, roundService),
                new RenamePlayerAction(playerService),
                new ListRoundsAction(roundService),
                new ViewRoundAction(roundService, playerService),
                new ListPlayersAction(playerService, ledgerService),
                new ExitAction(exit),
                new StartRoundAction(roundService),
                new AddPlayerToRoundAction(playerService, roundService, ledgerService),
                new ListEntrantsRoundAction(roundService, playerService),
                new WinRoundAction(playerService, roundService)
            };

            var helpAction = new HelpAction(autocompleteActions);
            autocompleteActions.Add(helpAction);

            var rootCompletionContainer = new AutoCompletingCliActionsContainer(autocompleteActions);
            var rootCompletionProvider = new TabCompletionProvider(rootCompletionContainer);

            // Run it
            var root = new RootAction(rootCompletionProvider, exit);
            root.Execute();
        }
    }
}
