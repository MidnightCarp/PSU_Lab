namespace Compiler.Core;

/// <summary>
/// Информация об ошибке.
/// </summary>
internal struct Err
{
    internal TextPosition ErrorPosition { get; set; }

    internal byte ErrorCode { get; set; }

    internal Err(TextPosition errorPosition, byte errorCode)
    {
        ErrorPosition = errorPosition;
        ErrorCode = errorCode;
    }
}