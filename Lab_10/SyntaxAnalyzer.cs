using System;
using System.Collections.Generic;
using Compiler.Core;
using Compiler.IO;
using Compiler.Lexica;

namespace Compiler.Syntax;

/// <summary>
/// Синтаксический и семантический анализатор языка Pascal.
/// </summary>
internal class SyntaxAnalyzer
{
    private readonly LexicalAnalyzer _lexer;
    private readonly SymbolTable _symbolTable;

    private byte _currentSymbol;

    internal SyntaxAnalyzer(LexicalAnalyzer lexer)
    {
        _lexer = lexer;
        _symbolTable = new SymbolTable();
    }

    /// <summary>
    /// Запускает процесс синтаксического анализа.
    /// </summary>
    internal void Parse()
    {
        _currentSymbol = _lexer.NextSym();
        ParseProgram();
    }

    private void ParseProgram()
    {
        if (_currentSymbol == LexicalAnalyzer.ProgramSy)
        {
            NextSym();
            if (_currentSymbol == LexicalAnalyzer.Ident)
            {
                NextSym();
                if (_currentSymbol == LexicalAnalyzer.Semicolon)
                {
                    NextSym();
                    ParseBlock();

                    if (_currentSymbol == LexicalAnalyzer.Point)
                    {
                        NextSym();
                    }
                    else
                    {
                        InputOutput.Error(106, InputOutput.PositionNow);
                    }
                }
                else
                {
                    InputOutput.Error(101, InputOutput.PositionNow);
                }
            }
            else
            {
                InputOutput.Error(100, InputOutput.PositionNow);
            }
        }
        else
        {
            InputOutput.Error(122, InputOutput.PositionNow);
        }
    }

    private void ParseBlock()
    {
        ParseVariableDeclarationPart();
        ParseFunctionDeclarationPart();
        ParseStatementPart();
    }

    private void ParseVariableDeclarationPart()
    {
        if (_currentSymbol == LexicalAnalyzer.VarSy)
        {
            NextSym();
            ParseVariableDeclaration();
            while (_currentSymbol == LexicalAnalyzer.Ident)
            {
                ParseVariableDeclaration();
            }
        }
    }

    private void ParseVariableDeclaration()
    {
        var names = new List<string>();

        if (_currentSymbol == LexicalAnalyzer.Ident)
        {
            names.Add(_lexer.AddrName);
            NextSym();

            while (_currentSymbol == LexicalAnalyzer.Comma)
            {
                NextSym();
                if (_currentSymbol == LexicalAnalyzer.Ident)
                {
                    names.Add(_lexer.AddrName);
                    NextSym();
                }
                else
                {
                    InputOutput.Error(100, InputOutput.PositionNow);
                    break;
                }
            }

            if (_currentSymbol == LexicalAnalyzer.Colon)
            {
                NextSym();
                if (_currentSymbol == LexicalAnalyzer.Ident)
                {
                    string typeName = _lexer.AddrName;
                    if (!IsStandardType(typeName))
                    {
                        InputOutput.Error(203, InputOutput.PositionNow);
                    }
                    else
                    {
                        foreach (var name in names)
                        {
                            if (!_symbolTable.Add(name, typeName))
                            {
                                InputOutput.Error(201, InputOutput.PositionNow);
                            }
                        }
                    }
                    NextSym();

                    if (_currentSymbol == LexicalAnalyzer.Semicolon)
                    {
                        NextSym();
                    }
                    else
                    {
                        InputOutput.Error(101, InputOutput.PositionNow);
                    }
                }
                else
                {
                    InputOutput.Error(100, InputOutput.PositionNow);
                }
            }
            else
            {
                InputOutput.Error(102, InputOutput.PositionNow);
            }
        }
        else
        {
            InputOutput.Error(100, InputOutput.PositionNow);
        }
    }

    /// <summary>
    /// Разбор объявления функций. Поддерживает групповое объявление параметров
    /// вида: function Add(x, y: integer): integer;
    /// </summary>
    private void ParseFunctionDeclarationPart()
    {
        while (_currentSymbol == LexicalAnalyzer.FunctionSy)
        {
            NextSym();

            if (_currentSymbol != LexicalAnalyzer.Ident)
            {
                InputOutput.Error(100, InputOutput.PositionNow);
                SkipTo(new[] { LexicalAnalyzer.Semicolon, LexicalAnalyzer.BeginSy });
                return;
            }

            string funcName = _lexer.AddrName;
            NextSym();

            var paramTypes = new List<string>();

            // Разбор списка параметров
            if (_currentSymbol == LexicalAnalyzer.LeftPar)
            {
                NextSym();

                while (_currentSymbol == LexicalAnalyzer.Ident)
                {
                    // Собираем имена параметров с общим типом: x, y, z
                    var paramNames = new List<string> { _lexer.AddrName };
                    NextSym();

                    while (_currentSymbol == LexicalAnalyzer.Comma)
                    {
                        NextSym();
                        if (_currentSymbol == LexicalAnalyzer.Ident)
                        {
                            paramNames.Add(_lexer.AddrName);
                            NextSym();
                        }
                        else
                        {
                            InputOutput.Error(100, InputOutput.PositionNow);
                            break;
                        }
                    }

                    if (_currentSymbol == LexicalAnalyzer.Colon)
                    {
                        NextSym();
                        if (_currentSymbol == LexicalAnalyzer.Ident)
                        {
                            string paramType = _lexer.AddrName;
                            if (!IsStandardType(paramType))
                            {
                                InputOutput.Error(203, InputOutput.PositionNow);
                            }

                            // Тип применяется ко всем именам в группе
                            foreach (var _ in paramNames)
                            {
                                paramTypes.Add(paramType);
                            }
                            NextSym();
                        }
                        else
                        {
                            InputOutput.Error(100, InputOutput.PositionNow);
                        }
                    }
                    else
                    {
                        InputOutput.Error(102, InputOutput.PositionNow);
                    }

                    if (_currentSymbol == LexicalAnalyzer.Semicolon)
                    {
                        NextSym();
                    }
                    else
                    {
                        break;
                    }
                }

                if (_currentSymbol == LexicalAnalyzer.RightPar)
                {
                    NextSym();
                }
                else
                {
                    InputOutput.Error(107, InputOutput.PositionNow);
                }
            }

            // Тип возвращаемого значения
            if (_currentSymbol == LexicalAnalyzer.Colon)
            {
                NextSym();
                if (_currentSymbol == LexicalAnalyzer.Ident)
                {
                    string returnType = _lexer.AddrName;
                    if (!IsStandardType(returnType))
                    {
                        InputOutput.Error(203, InputOutput.PositionNow);
                    }

                    if (!_symbolTable.AddFunction(funcName, returnType, paramTypes))
                    {
                        InputOutput.Error(201, InputOutput.PositionNow);
                    }

                    NextSym();

                    if (_currentSymbol == LexicalAnalyzer.Semicolon)
                    {
                        NextSym();
                        ParseBlock();
                        if (_currentSymbol == LexicalAnalyzer.Semicolon)
                        {
                            NextSym();
                        }
                        else
                        {
                            InputOutput.Error(101, InputOutput.PositionNow);
                        }
                    }
                    else
                    {
                        InputOutput.Error(101, InputOutput.PositionNow);
                    }
                }
                else
                {
                    InputOutput.Error(100, InputOutput.PositionNow);
                }
            }
            else
            {
                InputOutput.Error(102, InputOutput.PositionNow);
            }
        }
    }

    private void ParseStatementPart()
    {
        if (_currentSymbol == LexicalAnalyzer.BeginSy)
        {
            NextSym();
            ParseStatementList();

            if (_currentSymbol == LexicalAnalyzer.EndSy)
            {
                NextSym();
            }
            else
            {
                InputOutput.Error(105, InputOutput.PositionNow);
            }
        }
        else
        {
            InputOutput.Error(104, InputOutput.PositionNow);
        }
    }

    private void ParseStatementList()
    {
        ParseStatement();
        while (_currentSymbol == LexicalAnalyzer.Semicolon)
        {
            NextSym();
            if (_currentSymbol == LexicalAnalyzer.EndSy || _currentSymbol == 0)
            {
                break;
            }
            ParseStatement();
        }
    }

    private void ParseStatement()
    {
        if (_currentSymbol == LexicalAnalyzer.Ident)
        {
            string varName = _lexer.AddrName;
            TextPosition varPos = InputOutput.PositionNow;
            NextSym();

            if (_currentSymbol == LexicalAnalyzer.Assign)
            {
                NextSym();
                string exprType = ParseExpression();

                // Семантическая проверка левой части
                var symbol = _symbolTable.Get(varName);
                if (symbol == null)
                {
                    InputOutput.Error(200, varPos);
                }
                else if (!AreTypesCompatible(symbol.Type, exprType))
                {
                    InputOutput.Error(202, InputOutput.PositionNow);
                }
            }
            else
            {
                InputOutput.Error(103, InputOutput.PositionNow);
                SkipTo(new[] { LexicalAnalyzer.Semicolon, LexicalAnalyzer.EndSy });
            }
        }
        else
        {
            InputOutput.Error(0, InputOutput.PositionNow);
            SkipTo(new[] { LexicalAnalyzer.Semicolon, LexicalAnalyzer.EndSy });
        }
    }

    /// <summary>
    /// Разбор выражения. Возвращает тип выражения (integer, real, boolean, char).
    /// </summary>
    private string ParseExpression()
    {
        string type = ParseTerm();

        while (_currentSymbol == LexicalAnalyzer.Plus
            || _currentSymbol == LexicalAnalyzer.Minus
            || _currentSymbol == LexicalAnalyzer.Equal
            || _currentSymbol == LexicalAnalyzer.Later
            || _currentSymbol == LexicalAnalyzer.Greater
            || _currentSymbol == LexicalAnalyzer.LaterEqual
            || _currentSymbol == LexicalAnalyzer.GreaterEqual
            || _currentSymbol == LexicalAnalyzer.LaterGreater
            || _currentSymbol == LexicalAnalyzer.OrSy)
        {
            byte op = _currentSymbol;
            NextSym();
            string rightType = ParseTerm();

            type = CombineTypes(op, type, rightType);
        }

        return type;
    }

    private string ParseTerm()
    {
        string type = ParseFactor();

        while (_currentSymbol == LexicalAnalyzer.Star
            || _currentSymbol == LexicalAnalyzer.Slash
            || _currentSymbol == LexicalAnalyzer.DivSy
            || _currentSymbol == LexicalAnalyzer.ModSy
            || _currentSymbol == LexicalAnalyzer.AndSy)
        {
            byte op = _currentSymbol;
            NextSym();
            string rightType = ParseFactor();

            type = CombineTypes(op, type, rightType);
        }

        return type;
    }

    private string ParseFactor()
    {
        string type = "integer";

        if (_currentSymbol == LexicalAnalyzer.Ident)
        {
            string name = _lexer.AddrName;
            var symbol = _symbolTable.Get(name);

            if (symbol == null)
            {
                InputOutput.Error(200, InputOutput.PositionNow);
                NextSym();
                return "integer";
            }

            if (symbol.Kind == SymbolKind.Function)
            {
                // Вызов функции
                NextSym();
                var argTypes = new List<string>();

                if (_currentSymbol == LexicalAnalyzer.LeftPar)
                {
                    NextSym();
                    if (_currentSymbol != LexicalAnalyzer.RightPar)
                    {
                        argTypes.Add(ParseExpression());
                        while (_currentSymbol == LexicalAnalyzer.Comma)
                        {
                            NextSym();
                            argTypes.Add(ParseExpression());
                        }
                    }

                    if (_currentSymbol == LexicalAnalyzer.RightPar)
                    {
                        NextSym();
                    }
                    else
                    {
                        InputOutput.Error(107, InputOutput.PositionNow);
                    }
                }

                // Проверка параметров
                if (argTypes.Count != symbol.ParameterTypes.Count)
                {
                    InputOutput.Error(205, InputOutput.PositionNow);
                }
                else
                {
                    for (int i = 0; i < argTypes.Count; i++)
                    {
                        if (!AreTypesCompatible(symbol.ParameterTypes[i], argTypes[i]))
                        {
                            InputOutput.Error(206, InputOutput.PositionNow);
                        }
                    }
                }

                return symbol.Type;
            }
            else
            {
                type = symbol.Type;
                NextSym();
            }
        }
        else if (_currentSymbol == LexicalAnalyzer.IntC)
        {
            type = "integer";
            NextSym();
        }
        else if (_currentSymbol == LexicalAnalyzer.FloatC)
        {
            type = "real";
            NextSym();
        }
        else if (_currentSymbol == LexicalAnalyzer.CharC)
        {
            type = "char";
            NextSym();
        }
        else if (_currentSymbol == LexicalAnalyzer.LeftPar)
        {
            NextSym();
            type = ParseExpression();
            if (_currentSymbol == LexicalAnalyzer.RightPar)
            {
                NextSym();
            }
            else
            {
                InputOutput.Error(107, InputOutput.PositionNow);
            }
        }
        else if (_currentSymbol == LexicalAnalyzer.NotSy)
        {
            NextSym();
            string operandType = ParseFactor();
            if (operandType != "boolean")
            {
                InputOutput.Error(204, InputOutput.PositionNow);
            }
            type = "boolean";
        }
        else
        {
            InputOutput.Error(0, InputOutput.PositionNow);
            SkipTo(new[] { LexicalAnalyzer.Semicolon, LexicalAnalyzer.RightPar, LexicalAnalyzer.EndSy });
            type = "integer";
        }

        return type;
    }

    /// <summary>
    /// Вычисляет результирующий тип операции и проверяет совместимость операндов.
    /// </summary>
    private string CombineTypes(byte op, string left, string right)
    {
        // Операторы сравнения: результат — boolean
        if (op == LexicalAnalyzer.Equal
            || op == LexicalAnalyzer.Later
            || op == LexicalAnalyzer.Greater
            || op == LexicalAnalyzer.LaterEqual
            || op == LexicalAnalyzer.GreaterEqual
            || op == LexicalAnalyzer.LaterGreater)
        {
            if (!AreTypesCompatible(left, right) && !AreTypesCompatible(right, left))
            {
                InputOutput.Error(204, InputOutput.PositionNow);
            }
            return "boolean";
        }

        // Логические операторы and, or: только для boolean
        if (op == LexicalAnalyzer.AndSy || op == LexicalAnalyzer.OrSy)
        {
            if (left != "boolean" || right != "boolean")
            {
                InputOutput.Error(208, InputOutput.PositionNow);
            }
            return "boolean";
        }

        // div, mod: только для integer
        if (op == LexicalAnalyzer.DivSy || op == LexicalAnalyzer.ModSy)
        {
            if (left != "integer" || right != "integer")
            {
                InputOutput.Error(208, InputOutput.PositionNow);
            }
            return "integer";
        }

        // Арифметические операторы с boolean/char — ошибка
        if (left == "boolean" || right == "boolean" || left == "char" || right == "char")
        {
            InputOutput.Error(204, InputOutput.PositionNow);
            return "integer";
        }

        // Деление всегда даёт real
        if (op == LexicalAnalyzer.Slash)
        {
            return "real";
        }

        // Если один из операндов real — результат real
        if (left == "real" || right == "real")
        {
            return "real";
        }

        return "integer";
    }

    /// <summary>
    /// Проверяет, совместимы ли типы при присваивании.
    /// Integer можно присвоить в Real, но не наоборот.
    /// </summary>
    private static bool AreTypesCompatible(string target, string source)
    {
        if (target == source) return true;
        if (target == "real" && source == "integer") return true;
        return false;
    }

    private static bool IsStandardType(string typeName)
    {
        return typeName == "integer"
            || typeName == "real"
            || typeName == "boolean"
            || typeName == "char";
    }

    private void NextSym()
    {
        _currentSymbol = _lexer.NextSym();
    }

    private void SkipTo(byte[] targets)
    {
        while (_currentSymbol != 0 && Array.IndexOf(targets, _currentSymbol) == -1)
        {
            NextSym();
        }
    }
}