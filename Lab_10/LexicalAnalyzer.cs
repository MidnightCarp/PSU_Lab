using System;
using Compiler.Core;
using Compiler.IO;

namespace Compiler.Lexica;

/// <summary>
/// Лексический анализатор языка Pascal.
/// </summary>
internal class LexicalAnalyzer
{
    internal const byte Star = 21;
    internal const byte Slash = 60;
    internal const byte Equal = 16;
    internal const byte Comma = 20;
    internal const byte Semicolon = 14;
    internal const byte Colon = 5;
    internal const byte Point = 61;
    internal const byte Arrow = 62;
    internal const byte LeftPar = 9;
    internal const byte RightPar = 4;
    internal const byte LBracket = 11;
    internal const byte RBracket = 12;
    internal const byte Later = 65;
    internal const byte Greater = 66;
    internal const byte LaterEqual = 67;
    internal const byte GreaterEqual = 68;
    internal const byte LaterGreater = 69;
    internal const byte Plus = 70;
    internal const byte Minus = 71;
    internal const byte Assign = 51;
    internal const byte TwoPoints = 74;
    internal const byte Ident = 2;
    internal const byte FloatC = 82;
    internal const byte IntC = 15;
    internal const byte CharC = 83;  // <-- ДОБАВЛЕНО: код для символьных констант

    internal const byte CaseSy = 31;
    internal const byte ElseSy = 32;
    internal const byte FileSy = 57;
    internal const byte GotoSy = 33;
    internal const byte ThenSy = 52;
    internal const byte TypeSy = 34;
    internal const byte UntilSy = 53;
    internal const byte DoSy = 54;
    internal const byte WithSy = 37;
    internal const byte IfSy = 56;
    internal const byte InSy = 100;
    internal const byte OfSy = 101;
    internal const byte OrSy = 102;
    internal const byte ToSy = 103;
    internal const byte EndSy = 104;
    internal const byte VarSy = 105;
    internal const byte DivSy = 106;
    internal const byte AndSy = 107;
    internal const byte NotSy = 108;
    internal const byte ForSy = 109;
    internal const byte ModSy = 110;
    internal const byte NilSy = 111;
    internal const byte SetSy = 112;
    internal const byte BeginSy = 113;
    internal const byte WhileSy = 114;
    internal const byte ArraySy = 115;
    internal const byte ConstSy = 116;
    internal const byte LabelSy = 117;
    internal const byte DownToSy = 118;
    internal const byte PackedSy = 119;
    internal const byte RecordSy = 120;
    internal const byte RepeatSy = 121;
    internal const byte ProgramSy = 122;
    internal const byte FunctionSy = 123;
    internal const byte ProcedureSy = 124;

    private const int MaxInt = 32767;

    private static readonly Keywords Keywords = new Keywords();

    internal byte Symbol { get; private set; }
    internal string AddrName { get; private set; } = string.Empty;
    internal int NmbInt { get; private set; }
    internal float NmbFloat { get; private set; }
    internal char OneSymbol { get; private set; }  // <-- ДОБАВЛЕНО: для хранения символьной константы

    private TextPosition _token;

    /// <summary>
    /// Считывает следующий токен из входного потока.
    /// </summary>
    internal byte NextSym()
    {
        // Если достигнут конец файла, возвращаем 0 и выходим
        if (InputOutput.Ch == '\0')
        {
            return 0;
        }

        while (true)
        {
            while (InputOutput.Ch == ' ' || InputOutput.Ch == '\t')
            {
                InputOutput.NextCh();
            }

            _token.LineNumber = InputOutput.PositionNow.LineNumber;
            _token.CharNumber = InputOutput.PositionNow.CharNumber;

            if (InputOutput.Ch == '(' && InputOutput.PeekChar() == '*')
            {
                SkipCommentOldStyle();
                if (InputOutput.Ch == '\0') return 0;
                continue;
            }

            if (InputOutput.Ch == '{')
            {
                SkipCommentBrace();
                if (InputOutput.Ch == '\0') return 0;
                continue;
            }

            break;
        }

        if (char.IsLetter(InputOutput.Ch))
        {
            string name = string.Empty;
            uint startLine = InputOutput.PositionNow.LineNumber;

            while (char.IsLetterOrDigit(InputOutput.Ch) && InputOutput.PositionNow.LineNumber == startLine)
            {
                name += InputOutput.Ch;
                InputOutput.NextCh();
            }

            Symbol = Keywords.CheckKeyword(name);
            if (Symbol == Ident)
            {
                AddrName = name;
            }
            else
            {
                AddrName = string.Empty;
            }

            return Symbol;
        }

        if (char.IsDigit(InputOutput.Ch))
        {
            NmbInt = 0;

            while (char.IsDigit(InputOutput.Ch))
            {
                byte digit = (byte)(InputOutput.Ch - '0');

                if (NmbInt <= (MaxInt - digit) / 10)
                {
                    NmbInt = NmbInt * 10 + digit;
                    InputOutput.NextCh();
                }
                else
                {
                    InputOutput.Error(76, InputOutput.PositionNow);
                    // Пропускаем оставшиеся цифры и выходим из цикла,
                    // чтобы не "съесть" следующий символ (например, ';')
                    while (char.IsDigit(InputOutput.Ch))
                    {
                        InputOutput.NextCh();
                    }
                    NmbInt = 0;
                    break;
                }
            }

            if (InputOutput.Ch == '.' && char.IsDigit(InputOutput.PeekChar()))
            {
                InputOutput.NextCh();

                float frac = 0;
                float factor = 0.1f;

                while (char.IsDigit(InputOutput.Ch))
                {
                    frac += (InputOutput.Ch - '0') * factor;
                    factor *= 0.1f;
                    InputOutput.NextCh();
                }

                NmbFloat = NmbInt + frac;
                Symbol = FloatC;
            }
            else
            {
                Symbol = IntC;
            }

            return Symbol;
        }

        // ИСПРАВЛЕНО: символьная константа теперь возвращает CharC, а не FloatC
        if (InputOutput.Ch == '\'')
        {
            InputOutput.NextCh();

            if (InputOutput.Ch == '\'')
            {
                // Пустая кавычка — недопустимо
                InputOutput.Error(8, InputOutput.PositionNow);
                OneSymbol = '\0';
            }
            else
            {
                OneSymbol = InputOutput.Ch;
                InputOutput.NextCh();

                if (InputOutput.Ch != '\'')
                {
                    InputOutput.Error(8, InputOutput.PositionNow);
                }
            }

            InputOutput.NextCh();
            Symbol = CharC;
            return Symbol;
        }

        switch (InputOutput.Ch)
        {
            case '<':
                InputOutput.NextCh();
                if (InputOutput.Ch == '=') { Symbol = LaterEqual; InputOutput.NextCh(); }
                else if (InputOutput.Ch == '>') { Symbol = LaterGreater; InputOutput.NextCh(); }
                else Symbol = Later;
                break;
            case '>':
                InputOutput.NextCh();
                if (InputOutput.Ch == '=') { Symbol = GreaterEqual; InputOutput.NextCh(); }
                else Symbol = Greater;
                break;
            case ':':
                InputOutput.NextCh();
                if (InputOutput.Ch == '=') { Symbol = Assign; InputOutput.NextCh(); }
                else Symbol = Colon;
                break;
            case ';': Symbol = Semicolon; InputOutput.NextCh(); break;
            case '.': Symbol = Point; InputOutput.NextCh(); break;
            case ',': Symbol = Comma; InputOutput.NextCh(); break;
            case '=': Symbol = Equal; InputOutput.NextCh(); break;
            case '+': Symbol = Plus; InputOutput.NextCh(); break;
            case '-': Symbol = Minus; InputOutput.NextCh(); break;
            case '*': Symbol = Star; InputOutput.NextCh(); break;
            case '/': Symbol = Slash; InputOutput.NextCh(); break;
            case '(': Symbol = LeftPar; InputOutput.NextCh(); break;
            case ')': Symbol = RightPar; InputOutput.NextCh(); break;
            case '[': Symbol = LBracket; InputOutput.NextCh(); break;
            case ']': Symbol = RBracket; InputOutput.NextCh(); break;
            case '^': Symbol = Arrow; InputOutput.NextCh(); break;
            default:
                InputOutput.Error(0, InputOutput.PositionNow);
                InputOutput.NextCh();
                Symbol = 0;
                break;
        }

        return Symbol;
    }

    private void SkipCommentOldStyle()
    {
        InputOutput.NextCh();
        InputOutput.NextCh();
        bool errorReported = false;

        while (InputOutput.Ch != '\0')
        {
            if (InputOutput.Ch == '*')
            {
                char next = InputOutput.PeekChar();
                if (next == ')')
                {
                    InputOutput.NextCh();
                    InputOutput.NextCh();
                    return;
                }

                if (!errorReported)
                {
                    InputOutput.Error(18, InputOutput.PositionNow);
                    errorReported = true;
                }
            }
            InputOutput.NextCh();
        }
    }

    private void SkipCommentBrace()
    {
        InputOutput.NextCh();

        while (InputOutput.Ch != '}' && InputOutput.Ch != '\0')
        {
            if (InputOutput.CurrentIndex >= InputOutput.CurrentLineLength)
            {
                InputOutput.SetLineToPrint(InputOutput.CurrentLine);
                InputOutput.PrintCurrentLine();
                InputOutput.ReadNextLineForComment();

                if (InputOutput.CurrentLine == null) break;
                continue;
            }
            InputOutput.NextCh();
        }

        if (InputOutput.Ch == '}') InputOutput.NextCh();
        else InputOutput.Error(18, InputOutput.PositionNow);
    }
}