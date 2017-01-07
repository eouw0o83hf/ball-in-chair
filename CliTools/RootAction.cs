using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BallInChair.CliTools
{
    public class RootAction : ICliAction
    {
        private readonly ICollection<IAutocompletingCliAction> _actions;
        
        public RootAction(ICollection<IAutocompletingCliAction> actions)
        {
            _actions = actions;
        }

        public void Execute()
        {
            var buffer = new StringBuilder();
            
            while(true)
            {
                var input = Console.ReadKey(true);
                
                switch(input.Key)
                {
                    case ConsoleKey.Tab:
                        var filteredActions = GetAction(buffer.ToString())
                                                .ToList();

                        if(filteredActions.Any())
                        {
                            var currentAction = filteredActions.First();                            
                            Console.Write(currentAction.CommandName.Substring(buffer.ToString().Length));
                        }

                        break;

                    case ConsoleKey.Enter:
                        var selectedAction = GetAction(buffer.ToString()).FirstOrDefault();
                        if(selectedAction != null)
                        {
                            Console.WriteLine();
                            buffer = new StringBuilder();
                            selectedAction.Execute();
                        }
                        break;

                    case ConsoleKey.Escape:
                        buffer = new StringBuilder();
                        ClearCurrentConsoleLine();
                        break;

                    default:
                        buffer.Append(input.KeyChar);
                        Console.Write(input.KeyChar);
                        break;
                }
            }
        }

        private IEnumerable<IAutocompletingCliAction> GetAction(string cliInput)
        {
            return _actions
                .Where(a => a.CommandName.StartsWith(cliInput, StringComparison.OrdinalIgnoreCase))
                .OrderBy(a => a.CommandName);
        }

        private static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth)); 
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}