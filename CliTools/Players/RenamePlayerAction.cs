using System;
using System.Collections.Generic;
using System.Linq;

namespace BallInChair.CliTools.Players
{
    public class RenamePlayerAction : IAutocompletingCliAction
    {
        public string CommandName => "player rename";

        public void Execute()
        {
            Console.WriteLine("Renaming a player");
        }
    }
}