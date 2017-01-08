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

            var playerService = new InMemoryPlayerService();

            var autocompleteActions = new List<CliActionBase>
            {
                new AddPlayerAction(playerService),
                new RenamePlayerAction(),
                new ListPlayersAction(playerService)
            };

            var helpAction = new HelpAction(autocompleteActions);
            autocompleteActions.Add(helpAction);

            var rootCompletionContainer = new AutoCompletingCliActionsContainer(autocompleteActions);
            var rootCompletionProvider = new TabCompletionProvider(rootCompletionContainer);

            var root = new RootAction(rootCompletionProvider);
            root.Execute();
        }
    }
}
