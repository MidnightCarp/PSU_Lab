using Compiler.IO;
using Compiler.Lexica;
using Compiler.Syntax;

namespace Compiler.Driver;

/// <summary>
/// Точка входа в приложение компилятора.
/// </summary>
internal class CompilerDriver
{
    private static void Main(string[] args)
    {
        string inputFile = "test.pas";

        if (args.Length > 0)
        {
            inputFile = args[0];
        }

        InputOutput.OpenFile(inputFile);

        var lexer = new LexicalAnalyzer();
        var parser = new SyntaxAnalyzer(lexer);
        parser.Parse();

        InputOutput.CloseFile();
        InputOutput.PrintErrorSummary();
    }
}