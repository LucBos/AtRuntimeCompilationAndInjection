using System;

namespace TestCompilation
{
    interface ICommand
    {
        ConsoleKey Activator { get; }
        string Description { get; }
        void Handle();
    }
}