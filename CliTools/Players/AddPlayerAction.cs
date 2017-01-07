using System;
using System.Collections.Generic;
using System.Linq;

namespace BallInChair.CliTools.Players
{
    public class AddPlayerAction : IAutocompletingCliAction
    {
        public string CommandName => "player add";

        public void Execute()
        {
            Console.WriteLine("Adding a player");
        }

        public IEnumerable<string> GetDataOptions(string stub)
        {
            return Enumerable.Empty<string>();
        }
    }
}