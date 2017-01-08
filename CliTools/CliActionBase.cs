using System;
using System.Collections.Generic;

namespace BallInChair.CliTools
{
    public interface ICliAction
    {
        void Execute();
    }

    public interface IAutocompletingCliAction : ICliAction
    {
        string CommandName { get; }
    }    
}