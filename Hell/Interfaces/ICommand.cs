using System.Collections.Generic;

public interface ICommand
{
    IManager Manager { get; }

    IList<string> Args { get; }

    string Execute();
}