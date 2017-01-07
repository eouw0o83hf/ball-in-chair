using System;
using System.Collections.Generic;
using System.Linq;

namespace BallInChair.CliTools.Players
{
    public class DeletePlayerAction : IAutocompletingCliAction
    {
        public string CommandName => "player delete";

        public void Execute()
        {
            Console.WriteLine("Deleting a player");
        }

        public IEnumerable<string> GetDataOptions(string stub)
        {
            return Enumerable.Empty<string>();
        }
    }
}