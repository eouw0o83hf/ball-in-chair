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
            var inputState = new InputState();
            
            while(true)
            {
                var input = Console.ReadKey(true);
                
                switch(input.Key)
                {
                    case ConsoleKey.Tab:
                        if(input.Modifiers.HasFlag(ConsoleModifiers.Shift))
                        {
                            inputState.TabIndex = (inputState.TabIndex ?? 0) - 1;
                        }
                        else
                        {
                            if(inputState.TabIndex.HasValue)
                            {
                                ++inputState.TabIndex;
                            }
                            else
                            {
                                inputState.TabIndex = 0;
                            }
                        }

                        var filteredActions = GetAction(inputState.Buffer).ToList();
                        // Account for possible negative tab index values and
                        // lop off any excess
                        inputState.TabIndex = (inputState.TabIndex + filteredActions.Count) % filteredActions.Count;

                        if(filteredActions.Any())
                        {
                            var currentAction = filteredActions[inputState.TabIndex.Value];
                            ClearCurrentConsoleLine();
                            Console.Write(currentAction.CommandName);
                            inputState.TabCompletedBuffer = currentAction.CommandName;
                        }
                        break;

                    case ConsoleKey.Enter:
                        // We need a newline regardless of what the next action is
                        Console.WriteLine();

                        var selectedAction = GetAction(inputState.Buffer).FirstOrDefault();
                        if(selectedAction != null)
                        {
                            selectedAction.Execute();
                        }
                        else
                        {
                            Console.WriteLine("No action with that name recognized, try again or ask for `help`");
                        }

                        inputState = new InputState();
                        break;

                    case ConsoleKey.Escape:
                        inputState = new InputState();
                        ClearCurrentConsoleLine();
                        break;

                    case ConsoleKey.Backspace:
                        Console.Write("\b \b");
                        inputState.Backspace();
                        break;

                    default:
                        inputState.Append(input.KeyChar);
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

        private class InputState
        {
            private readonly StringBuilder _buffer = new StringBuilder();

            public void Append(string appendix)
            {
                _buffer.Append(appendix);
                TabIndex = null;
            }

            public void Append(char appendix)
            {
                _buffer.Append(appendix);
                TabIndex = null;
            }

            public void Backspace()
            {
                if(TabCompletedBuffer != null 
                    && _buffer.Length < TabCompletedBuffer.Length)
                {
                    _buffer.Clear().Append(TabCompletedBuffer);
                    TabCompletedBuffer = null;
                }
                _buffer.Remove(_buffer.Length - 1, 1);
                TabIndex = null;
            }

            public string TabCompletedBuffer { get; set; }
            public string Buffer => _buffer.ToString();
            public int? TabIndex { get; set; }
        }
    }
}