using System;
using System.Text;
using BallInChair.CliTools;
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

            var autocompleteActions = new IAutocompletingCliAction[]
            {
                new HelpAction(),
                new AddPlayerAction(playerService),
                new RenamePlayerAction(),
                new ListPlayersAction(playerService)
            };

            var root = new RootAction(autocompleteActions);
            root.Execute();
        }
    }
}
