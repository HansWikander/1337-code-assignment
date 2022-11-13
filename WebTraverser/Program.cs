namespace WebTraverser
{
    internal class Program
    {
        private static string _website = "https://tretton37.com";
        private static string _outputFolder = "result";

        static void Main(string[] args)
        {
            if (args.Length > 0) 
            {
                _outputFolder = args[0];
            }
            if (args.Length > 1)
            {
                _website = args[1];
            }

            var traverser = new Traverser(_website, _outputFolder);

            Console.WriteLine($"Starting traversing {_website} into {_outputFolder}");
            try
            {
                Task.Run(async () => { await traverser.Run(); }).Wait();
            }
            catch (AggregateException aggregateException)
            {
                Console.WriteLine("");
                Console.WriteLine("ERRORS OCCURRED");
                DisplayErrors(aggregateException);
                Console.WriteLine("");
            }
            Console.WriteLine($"Done traversing");
        }

        private static void DisplayErrors(AggregateException aggregateException)
        {
            foreach (var exception in aggregateException.InnerExceptions)
            {
                if (exception is AggregateException innerAggregate)
                {
                    DisplayErrors(innerAggregate);
                }
                else
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }
    }
}