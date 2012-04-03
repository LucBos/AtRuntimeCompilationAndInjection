using System;

namespace TestCompilation
{
    public interface IExpression
    {
        Func<string, bool> Expression { get; }
        string Answer { get; }
    }
}