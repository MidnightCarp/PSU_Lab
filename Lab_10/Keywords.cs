using System.Collections.Generic;

namespace Compiler.Lexica;

/// <summary>
/// Хранилище ключевых слов языка Pascal, сгруппированных по длине.
/// </summary>
internal class Keywords
{
    private readonly Dictionary<byte, Dictionary<string, byte>> _keywords;

    internal Keywords()
    {
        _keywords = new Dictionary<byte, Dictionary<string, byte>>
        {
            [2] = new Dictionary<string, byte>
            {
                ["do"] = LexicalAnalyzer.DoSy,
                ["if"] = LexicalAnalyzer.IfSy,
                ["in"] = LexicalAnalyzer.InSy,
                ["of"] = LexicalAnalyzer.OfSy,
                ["or"] = LexicalAnalyzer.OrSy,
                ["to"] = LexicalAnalyzer.ToSy,
            },
            [3] = new Dictionary<string, byte>
            {
                ["end"] = LexicalAnalyzer.EndSy,
                ["var"] = LexicalAnalyzer.VarSy,
                ["div"] = LexicalAnalyzer.DivSy,
                ["and"] = LexicalAnalyzer.AndSy,
                ["not"] = LexicalAnalyzer.NotSy,
                ["for"] = LexicalAnalyzer.ForSy,
                ["mod"] = LexicalAnalyzer.ModSy,
                ["nil"] = LexicalAnalyzer.NilSy,
                ["set"] = LexicalAnalyzer.SetSy,
            },
            [4] = new Dictionary<string, byte>
            {
                ["then"] = LexicalAnalyzer.ThenSy,
                ["else"] = LexicalAnalyzer.ElseSy,
                ["case"] = LexicalAnalyzer.CaseSy,
                ["file"] = LexicalAnalyzer.FileSy,
                ["goto"] = LexicalAnalyzer.GotoSy,
                ["type"] = LexicalAnalyzer.TypeSy,
                ["with"] = LexicalAnalyzer.WithSy,
            },
            [5] = new Dictionary<string, byte>
            {
                ["begin"] = LexicalAnalyzer.BeginSy,
                ["while"] = LexicalAnalyzer.WhileSy,
                ["array"] = LexicalAnalyzer.ArraySy,
                ["const"] = LexicalAnalyzer.ConstSy,
                ["label"] = LexicalAnalyzer.LabelSy,
                ["until"] = LexicalAnalyzer.UntilSy,
            },
            [6] = new Dictionary<string, byte>
            {
                ["downto"] = LexicalAnalyzer.DownToSy,
                ["packed"] = LexicalAnalyzer.PackedSy,
                ["record"] = LexicalAnalyzer.RecordSy,
                ["repeat"] = LexicalAnalyzer.RepeatSy,
            },
            [7] = new Dictionary<string, byte> { ["program"] = LexicalAnalyzer.ProgramSy },
            [8] = new Dictionary<string, byte> { ["function"] = LexicalAnalyzer.FunctionSy },
            [9] = new Dictionary<string, byte> { ["procedure"] = LexicalAnalyzer.ProcedureSy },
        };
    }

    internal byte CheckKeyword(string name)
    {
        if (_keywords.TryGetValue((byte)name.Length, out var group)
            && group.TryGetValue(name, out var code))
        {
            return code;
        }

        return LexicalAnalyzer.Ident;
    }

    internal string GetKeywordName(byte code)
    {
        foreach (var group in _keywords)
        {
            foreach (var pair in group.Value)
            {
                if (pair.Value == code)
                {
                    return pair.Key;
                }
            }
        }

        return string.Empty;
    }
}