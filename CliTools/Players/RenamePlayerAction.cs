using System;
using System.Collections.Generic;
using System.Linq;

namespace BallInChair.CliTools.Players
{
    public class RenamePlayerAction : CliActionBase
    {
        public override string CommandName => "player rename";

        public override void Execute()
        {
            Console.WriteLine("Renaming a player");
        }
    }
}