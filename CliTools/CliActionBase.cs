using System;
using System.Collections.Generic;
using System.Linq;
using BallInChair.CliTools.Framework;

namespace BallInChair.CliTools
{
    public abstract class CliActionBase : ITabCompletableResponseItem
    {
        public abstract string CommandName { get; }
        public abstract void Execute();

        string ITabCompletableResponseItem.FullText => CommandName;
    }

    public class AutoCompletingCliActionsContainer : ITabCompletableQueryContainer
    {
        private readonly ICollection<CliActionBase> _actions;

        public AutoCompletingCliActionsContainer(ICollection<CliActionBase> actions)
        {
            _actions = actions;
        }
        
        public IEnumerable<ITabCompletableResponseItem> GetMatches(string input)
        {
            return _actions.Where(a => a.CommandName.StartsWith(input, StringComparison.OrdinalIgnoreCase));
        }
    }
}