using BallInChair.CliTools.Framework;

namespace BallInChair.CliTools
{
    public class RootAction
    {
        private readonly ITabCompletionProvider _actionCompletionProvider;
        private readonly ExitContainer _exit;
        
        public RootAction(ITabCompletionProvider actionCompletionProvider, ExitContainer exit)
        {
            _actionCompletionProvider = actionCompletionProvider;
            _exit = exit;
        }

        public void Execute()
        {
            while(!_exit.TimeToLeaveTown)
            {
                ConsoleHelpers.WriteMagentaLine("Let's play Ball in Chair! Enter a command.");
                _actionCompletionProvider.DoTabCompletion();
            }
        }        
    }
}