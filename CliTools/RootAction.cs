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
                _actionCompletionProvider.DoTabCompletion();
            }
        }        
    }
}