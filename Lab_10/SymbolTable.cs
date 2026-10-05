using System.Collections.Generic;

namespace Compiler.Syntax;

/// <summary>
/// Вид идентификатора.
/// </summary>
internal enum SymbolKind
{
    Variable,
    Function
}

/// <summary>
/// Информация об идентификаторе.
/// </summary>
internal class Symbol
{
    internal string Name { get; }

    internal string Type { get; }

    internal SymbolKind Kind { get; }

    internal List<string> ParameterTypes { get; }

    internal Symbol(string name, string type, SymbolKind kind)
    {
        Name = name;
        Type = type;
        Kind = kind;
        ParameterTypes = new List<string>();
    }
}

/// <summary>
/// Таблица символов для хранения информации об идентификаторах.
/// </summary>
internal class SymbolTable
{
    private readonly Dictionary<string, Symbol> _symbols;

    internal SymbolTable()
    {
        _symbols = new Dictionary<string, Symbol>();
    }

    /// <summary>
    /// Добавляет переменную в таблицу.
    /// </summary>
    internal bool Add(string name, string type)
    {
        if (_symbols.ContainsKey(name))
        {
            return false;
        }

        _symbols[name] = new Symbol(name, type, SymbolKind.Variable);
        return true;
    }

    /// <summary>
    /// Добавляет функцию в таблицу.
    /// </summary>
    internal bool AddFunction(string name, string returnType, List<string> parameterTypes)
    {
        if (_symbols.ContainsKey(name))
        {
            return false;
        }

        var symbol = new Symbol(name, returnType, SymbolKind.Function);
        symbol.ParameterTypes.AddRange(parameterTypes);
        _symbols[name] = symbol;
        return true;
    }

    internal bool Contains(string name)
    {
        return _symbols.ContainsKey(name);
    }

    internal Symbol? Get(string name)
    {
        return _symbols.TryGetValue(name, out var symbol) ? symbol : null;
    }

    internal string GetType(string name)
    {
        return _symbols.TryGetValue(name, out var symbol) ? symbol.Type : string.Empty;
    }
}