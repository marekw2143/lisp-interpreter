using LispInterpreter.Interpreter;
using Microsoft.Extensions.DependencyInjection;

var sampleInputs = new []
{
    "( + 1 2 3 4 PIEC )",
    "(define DWA 2)",
    "(define TRZY 3)",
    "(define CZTERY 4 )",
    "( define PIEC 5 )",
    "(+ 1 2 3 4)",
    "( / 6 3 )",
    "( if (< 3 1) (+ 2 7) (+ 9 10 CZTERY DWA ) )",
    "( if (< PIEC 1) (+ 2 7) (+ 9 10) )",
    "(if (< PIEC TRZY) ( * 2 CZTERY ) (DodajDwa 8) )",
    "( define ( DodajDwa X ) ( + 2 X )",
    "( DodajDwa 13 )",
    "( define ( DodajPiec X )  ( + PIEC X )",
    "( DodajDwa ( * 2 ( DodajPiec 3 ) ) )",
    "( define GB 10 )",
    "( define ( DodajGB GB ) ( + GB GB ) )", // global variable GB is hidden by function parameter named the same ("GB")
};

var services = new ServiceCollection();

services.RegisterInterpreter();

var provider = services.BuildServiceProvider();

var interpreter = provider.GetService<Interpreter>();

var results = interpreter.ProcessInputs(sampleInputs);

foreach (var (lineDescriptor, resultValue) in results)
{
    Console.WriteLine($"Result of processing line {lineDescriptor.LineContent} is {resultValue}");
}
