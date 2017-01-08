using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BallInChair.CliTools.Framework;

namespace BallInChair.CliTools
{
    public class RootAction
    {
        private readonly ITabCompletionProvider _actionCompletionProvider;
        
        public RootAction(ITabCompletionProvider actionCompletionProvider)
        {
            _actionCompletionProvider = actionCompletionProvider;
        }

        public void Execute()
        {
            while(true)
            {
                var nextAction = _actionCompletionProvider.DoTabCompletion();
                nextAction.Execute(nextAction.FullText);
            }
        }        
    }
}