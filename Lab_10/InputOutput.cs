using System;
using System.Collections.Generic;
using System.IO;
using Compiler.Core;

namespace Compiler.IO;

/// <summary>
/// Модуль ввода-вывода: чтение исходного файла, вывод строк и ошибок.
/// </summary>
internal class InputOutput
{
    private static readonly Dictionary<byte, string> ErrorMessages = new Dictionary<byte, string>
    {
        // Лексические ошибки
        [0] = "Недопустимый символ",
        [8] = "Символьная константа не закрыта кавычкой",
        [18] = "Комментарий закрыт неправильно или не закрыт",
        [76] = "Целая константа превышает допустимый диапазон (maxint = 32767)",

        // Синтаксические ошибки
        [100] = "Ожидался идентификатор",
        [101] = "Ожидался символ ';'",
        [102] = "Ожидался символ ':'",
        [103] = "Ожидался символ ':='",
        [104] = "Ожидался символ 'begin'",
        [105] = "Ожидался символ 'end'",
        [106] = "Ожидался символ '.'",
        [107] = "Ожидался символ ')'",
        [108] = "Ожидался символ '('",
        [109] = "Неожиданный конец файла",
        [122] = "Ожидался символ 'program'",

        // Семантические ошибки
        [200] = "Идентификатор не объявлен",
        [201] = "Повторное объявление идентификатора",
        [202] = "Несовпадение типов при присваивании",
        [203] = "Неизвестный тип",
        [204] = "Несовместимые типы операндов в выражении",
        [205] = "Неверное количество аргументов при вызове функции",
        [206] = "Неверный тип аргумента при вызове функции",
        [207] = "Деление на ноль",
        [208] = "Операция не применима к данному типу"
    };

    private static readonly List<Err> AllErrors = new List<Err>();

    private static StreamReader? _file;
    private static string? _currentLine;
    private static string? _previousLine;
    private static int _currentIndex;
    private static uint _errCount;

    internal static char Ch { get; private set; }

    // Поле, а не свойство, так как это mutable struct — мутируется на месте.
    internal static TextPosition PositionNow;

    internal static string? LineToPrint { get; private set; }

    internal static string? PreviousLine => _previousLine;

    internal static string? CurrentLine => _currentLine;

    internal static int CurrentIndex => _currentIndex;

    internal static int CurrentLineLength => _currentLine?.Length ?? 0;

    internal static bool IsEndOfLine => _currentLine == null || _currentIndex >= _currentLine.Length;

    internal static void OpenFile(string path)
    {
        _file = new StreamReader(path);
        AllErrors.Clear();
        _errCount = 0;
        PositionNow.LineNumber = 0;
        PositionNow.CharNumber = 0;
        _currentIndex = 0;
        ReadNextLine();
        NextCh();
    }

    internal static void CloseFile()
    {
        _file?.Close();
    }

    internal static void NextCh()
    {
        if (_currentLine == null)
        {
            Ch = '\0';
            return;
        }

        if (_currentIndex >= _currentLine.Length)
        {
            SetLineToPrint(_currentLine);
            ReadNextLine();

            if (_currentLine != null && _currentIndex < _currentLine.Length)
            {
                Ch = _currentLine[_currentIndex];
                PositionNow.CharNumber = (byte)_currentIndex;
                _currentIndex++;
            }
            else
            {
                Ch = '\0';
            }

            return;
        }

        Ch = _currentLine[_currentIndex];
        PositionNow.CharNumber = (byte)_currentIndex;
        _currentIndex++;
    }

    internal static char PeekChar()
    {
        if (_currentLine == null)
        {
            return '\0';
        }

        if (_currentIndex < _currentLine.Length)
        {
            return _currentLine[_currentIndex];
        }

        if (_file != null && !_file.EndOfStream)
        {
            return '\n';
        }

        return '\0';
    }

    internal static void SetLineToPrint(string? line)
    {
        LineToPrint = line;
    }

    internal static void PrintCurrentLine()
    {
        if (LineToPrint is not null)
        {
            Console.WriteLine($"[ {LineToPrint} ]");
            LineToPrint = null;
        }
    }

    internal static void ReadNextLineForComment()
    {
        if (_file == null || _file.EndOfStream)
        {
            _currentLine = null;
            return;
        }

        _currentLine = _file.ReadLine();
        PositionNow.LineNumber++;
        _currentIndex = 0;
    }

    internal static void Error(byte errorCode, TextPosition position)
    {
        var newErr = new Err(position, errorCode);
        AllErrors.Add(newErr);
        _errCount++;
    }

    internal static void PrintErrorSummary()
    {
        Console.WriteLine();

        if (_errCount == 0)
        {
            Console.WriteLine("Ошибок нет");
            return;
        }

        Console.WriteLine($"Всего ошибок: {_errCount}");
        Console.WriteLine("Список ошибок:");

        uint number = 0;
        foreach (var item in AllErrors)
        {
            number++;
            string errorMessage = GetErrorMessage(item.ErrorCode);
            Console.WriteLine($"  {number}. Код {item.ErrorCode}: {errorMessage} (строка {item.ErrorPosition.LineNumber}, позиция {item.ErrorPosition.CharNumber})");
        }
    }

    private static void ReadNextLine()
    {
        if (_file == null || _file.EndOfStream)
        {
            _currentLine = null;
            return;
        }

        _previousLine = _currentLine;
        _currentLine = _file.ReadLine();
        PositionNow.LineNumber++;
        _currentIndex = 0;
    }

    private static string GetErrorMessage(byte code)
    {
        return ErrorMessages.TryGetValue(code, out var message)
            ? message
            : "Неизвестная ошибка";
    }
}