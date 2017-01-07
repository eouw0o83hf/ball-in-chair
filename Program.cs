using System;
using System.Text;
using BallInChair.CliTools;
using BallInChair.CliTools.Players;

namespace BallInChair
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Ball on Chair!");

            var autocompleteActions = new IAutocompletingCliAction[]
            {
                new HelpAction(),
                new AddPlayerAction(),
                new RenamePlayerAction(),
                new DeletePlayerAction()
            };

            var root = new RootAction(autocompleteActions);
            root.Execute();
        }
    }
}
