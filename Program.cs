using System;
using System.Collections.Generic;
using System.IO;
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
            var ledgerService = new InMemoryLedgerService(SystemClock.Instance);
            var playerService = new JsonBackedPlayerService(PersistenceDirectory);
            var roundService = new InMemoryRoundService(playerService, ledgerService, SystemClock.Instance);            

            // Link Actions
            var exit = new ExitContainer();
            var autocompleteActions = new List<CliActionBase>
            {
                new AddPlayerAction(playerService),
                new CreditPlayerAction(playerService, ledgerService),
                new ViewPlayerAction(playerService, ledgerService),
                new RenamePlayerAction(playerService),
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

            // Populate initial/test data
            InitializePlayers(playerService);
            InitializeLedger(playerService, ledgerService);

            // Run it
            var root = new RootAction(rootCompletionProvider, exit);
            root.Execute();
        }

        private static void InitializePlayers(IPlayerService playerService)
        {
            try
            {
                playerService.CreatePlayer(Guid.NewGuid(), "Nathan Landis");
                playerService.CreatePlayer(Guid.NewGuid(), "Nathan Lande");
                playerService.CreatePlayer(Guid.NewGuid(), "Mike Balling");
                playerService.CreatePlayer(Guid.NewGuid(), "Chris Maffin");
                playerService.CreatePlayer(Guid.NewGuid(), "Sam Nissim");
                playerService.CreatePlayer(Guid.NewGuid(), "Andrew MacGill");
                playerService.CreatePlayer(Guid.NewGuid(), "Alfred Mwangi");
                playerService.CreatePlayer(Guid.NewGuid(), "Scott Gould");
                playerService.CreatePlayer(Guid.NewGuid(), "Chris Brown");
                playerService.CreatePlayer(Guid.NewGuid(), "Katy Brown");
            }
            catch
            {
                // For an initialized persistence file, this will error. Don't care.
            }
        }

        private static void InitializeLedger(IPlayerService playerService, ILedgerService ledgerService)
        {
            foreach(var player in playerService.GetAllPlayers())
            {
                ledgerService.PurchaseCredits(player.Id, 3);
            }
        }
    }
}
