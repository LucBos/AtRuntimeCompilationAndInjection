using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TestCompilation
{
    public class EvaluateExpressionCommand : ICommand
    {
        [ImportMany(typeof(IExpression))]
        public ICollection<IExpression> Expressions = new List<IExpression>();

        public ConsoleKey Activator
        {
            get { return ConsoleKey.E; }
        }

        public string Description
        {
            get { return "Evaluate Expression"; }
        }

        public void Handle()
        {
            LoadExtensions();
            EvaluateExpression();
        }

        private void LoadExtensions()
        {
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var catalog = new DirectoryCatalog(location + "\\Extensions\\");
            var container = new CompositionContainer(catalog);
            var batch = new CompositionBatch();
            batch.AddPart(this);
            container.Compose(batch);
        }

        private void EvaluateExpression()
        {
            Console.WriteLine("Please say something and I will try to answer.");
            var userInput = Console.ReadLine();
            var matchingExpression = Expressions.FirstOrDefault(e => e.Expression(userInput));
            if (matchingExpression == null)
            {
                Console.WriteLine("Could not answer you.");
            }
            else
            {
                Console.WriteLine(matchingExpression.Answer);
            }
            Console.ReadKey();
            Console.Clear();
        }
    }
}