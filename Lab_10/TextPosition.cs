namespace Compiler.Core;

/// <summary>
/// Позиция текста в исходном файле.
/// </summary>
internal struct TextPosition
{
    internal uint LineNumber { get; set; }

    internal byte CharNumber { get; set; }

    internal TextPosition(uint lineNumber = 0, byte charNumber = 0)
    {
        LineNumber = lineNumber;
        CharNumber = charNumber;
    }
}