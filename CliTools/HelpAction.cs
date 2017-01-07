using System;
using System.Collections.Generic;
using System.Linq;

namespace BallInChair.CliTools
{
    public class HelpAction : IAutocompletingCliAction
    {
        public string CommandName => "help";

        public void Execute()
        {
            Console.WriteLine("Help content");
        }

        public IEnumerable<string> GetDataOptions(string stub)
        {
            return Enumerable.Empty<string>();
        }
    }
}