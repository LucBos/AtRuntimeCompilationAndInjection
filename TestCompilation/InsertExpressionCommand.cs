using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;

namespace TestCompilation
{
    class InsertExpressionCommand : ICommand
    {
        public ConsoleKey Activator
        {
            get { return ConsoleKey.I; }
        }

        public string Description
        {
            get { return "Insert Expression"; }
        }

        public void Handle()
        {
            InsertNewExpression();
        }

        private void InsertNewExpression()
        {
            Console.WriteLine();
            Console.WriteLine("Insert an evaluation to the answer I must give you by providing a func<string,bool> and closing off with a semicolon");
            Console.WriteLine("eg: answer => answer.Equals(\"Test\");");
            var lamda = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("When your evaluation is true, what do you want me to answer?");
            var answer = Console.ReadLine();
            Console.WriteLine();
            var sourceCode = CreateSourceCode(lamda, answer);
            CompileCode(sourceCode);
        }

        private string CreateSourceCode(string lambda, string answer)
        {
            return
    @"using System;
    using System.ComponentModel.Composition;
    using TestCompilation;
    [Export(typeof(IExpression))]
    class NewExpression : IExpression
    {
        public Func<string, bool> Expression
        {
            get
            {
                return " + lambda + @"
            }
        }

        public string Answer
        {
            get
            {
                return " +
    "\"" + answer + "\"" + @";
            }
        }
    }";
        }

        private void CompileCode(string sourceCode)
        {
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.ComponentModel.Composition.dll", "ConsumerContracts.dll" },
                                                    location + "\\Extensions\\" + Path.GetRandomFileName() + ".dll",
                                                    true);

            CompilerResults results = csc.CompileAssemblyFromSource(parameters, sourceCode);

            if (results.Errors.Count > 0)
            {
                results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.Clear();
            }
        }
    }
}