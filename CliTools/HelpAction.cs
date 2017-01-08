using System;
using System.Collections.Generic;
using System.Linq;

namespace BallInChair.CliTools
{
    public class HelpAction : CliActionBase
    {
        public override string CommandName => "help";
        private readonly ICollection<CliActionBase> _actions;

        public HelpAction(ICollection<CliActionBase> actions)
        {
            _actions = actions;
        }

        public override void Execute()
        {
            Console.WriteLine("Actions:");
            foreach(var action in _actions)
            {
                Console.WriteLine($"\t{action.CommandName}");
            }
        }
    }
}