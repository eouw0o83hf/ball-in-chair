using System;
using System.Collections.Generic;
using BallInChair.CliTools;
using BallInChair.CliTools.Framework;
using BallInChair.CliTools.Players;
using BallInChair.Persistence;

namespace BallInChair
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Ball on Chair!");

            // Wire up dependencies
            var playerService = new InMemoryPlayerService();

            var autocompleteActions = new List<CliActionBase>
            {
                new AddPlayerAction(playerService),
                new RenamePlayerAction(playerService),
                new ListPlayersAction(playerService)
            };

            var helpAction = new HelpAction(autocompleteActions);
            autocompleteActions.Add(helpAction);

            var rootCompletionContainer = new AutoCompletingCliActionsContainer(autocompleteActions);
            var rootCompletionProvider = new TabCompletionProvider(rootCompletionContainer);

            // Populate initial/test data
            InitializePlayers(playerService);

            // Run it
            var root = new RootAction(rootCompletionProvider);
            root.Execute();
        }

        private static void InitializePlayers(IPlayerService playerService)
        {
            playerService.CreatePlayer(Guid.NewGuid(), "Nathan Landis");
            playerService.CreatePlayer(Guid.NewGuid(), "Nathan Lande");
            playerService.CreatePlayer(Guid.NewGuid(), "Mike Balling");
            playerService.CreatePlayer(Guid.NewGuid(), "Chris Maffin");
            playerService.CreatePlayer(Guid.NewGuid(), "Sam Nissim");
            playerService.CreatePlayer(Guid.NewGuid(), "Andrew MacGill");
            playerService.CreatePlayer(Guid.NewGuid(), "Alfred Mwagari");
            playerService.CreatePlayer(Guid.NewGuid(), "Scott Gould");
            playerService.CreatePlayer(Guid.NewGuid(), "Chris Brown");
            playerService.CreatePlayer(Guid.NewGuid(), "Katy Brown");
        }
    }
}
