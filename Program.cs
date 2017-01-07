using System;
using System.Text;
using BallInChair.CliTools;

namespace BallInChair
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Ball on Chair!");

            var autocompleteActions = new IAutocompletingCliAction[]
            {
                new HelpAction()
            };

            var root = new RootAction(autocompleteActions);
            root.Execute();
        }
    }
}
