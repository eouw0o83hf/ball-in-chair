using System;

namespace BallInChair.CliTools
{
    public class ExitAction : CliActionBase
    {
        public override string CommandName => "exit";
        private readonly ExitContainer _exit;

        public ExitAction(ExitContainer exit)
        {
            _exit = exit;
        }

        public override void Execute()
        {
            Console.WriteLine("Goodbye");
            _exit.TimeToLeaveTown = true;
        }
    }

    public class ExitContainer
    {
        public bool TimeToLeaveTown { get; set; }
    }
}