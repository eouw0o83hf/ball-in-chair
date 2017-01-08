using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BallInChair.CliTools.Framework
{
    public interface ITabCompletableQueryContainer
    {
        IEnumerable<ITabCompletableResponseItem> GetMatches(string input);
    }

    public interface ITabCompletableResponseItem
    {
        string FullText { get; }
        void Execute(string input);
    }
}