using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BallInChair.CliTools.Framework
{
    public interface ITabCompletionProvider
    {
        void DoTabCompletion();
    }

    public class TabCompletionProvider : ITabCompletionProvider
    {
        private readonly ITabCompletableQueryContainer _container;
        
        public TabCompletionProvider(ITabCompletableQueryContainer container)
        {
            _container = container;
        }

        public void DoTabCompletion()
        {
            var inputState = new InputState();
            Console.Write('>');
            
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

                        var action = GetAction(inputState);
                        if(action != null)
                        {
                            ClearCurrentConsoleLine();
                            Console.Write(action.FullText);
                            inputState.TabCompletedBuffer = action.FullText;
                        }
                        break;

                    case ConsoleKey.Enter:
                        // We need a newline regardless of what the next action is
                        Console.WriteLine();

                        var selectedAction = GetAction(inputState);
                        if(selectedAction != null)
                        {
                            selectedAction.Execute();
                            return;
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

        private ITabCompletableResponseItem GetAction(InputState inputState)
        {
            var actions = _container.GetMatches(inputState.Buffer)
                                    .OrderBy(a => a.FullText)
                                    .ToList();

            if(actions.Any())
            {
                // Account for possible negative tab index values and lop off any excess
                inputState.TabIndex = (inputState.TabIndex + actions.Count) % actions.Count;
                return actions[inputState.TabIndex.Value];
            }

            return null;
        }

        private static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth)); 
            Console.SetCursorPosition(0, currentLineCursor);
            Console.Write('>');
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
                if(Buffer.Length > 0)
                {
                    if(TabCompletedBuffer != null 
                        && _buffer.Length < TabCompletedBuffer.Length)
                    {
                        _buffer.Clear().Append(TabCompletedBuffer);
                        TabCompletedBuffer = null;
                    }
                    _buffer.Remove(_buffer.Length - 1, 1);
                }
                TabIndex = null;
            }

            public string TabCompletedBuffer { get; set; }
            public string Buffer => _buffer.ToString();
            public int? TabIndex { get; set; }
        }
    }
}