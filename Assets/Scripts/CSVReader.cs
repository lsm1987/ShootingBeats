using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

/// <summary>
/// CSV 읽기
/// </summary>
public class CSVReader
{
    private string[] _lines; // 텍스트 어셋으로부터 읽어들인 문자열을 줄 단위로 저장
    private int _nextReadLine = 0; // ReadLine() 에서 몇 번째 라인을 읽을 차례인가?

    public CSVReader(string path)
    {
        // 초기화
        _lines = null;
        _nextReadLine = 0;
        
        // 파일 찾기
        TextAsset asset = Resources.Load(path) as TextAsset;
        if (asset == null)
        {
            Debug.LogError("[CSVReader] Invalid path:" + path);
            return;
        }

        string text = asset.text;
        text = text.Trim().Replace("\r", "") + "\n";
        _lines = text.Split('\n');
    }
    
    public string[] ReadLine()
    {
        if (_lines == null)
        {
            return null;
        }

        while (_nextReadLine < _lines.Length)
        {
            string line = _lines[_nextReadLine];
            if (string.IsNullOrEmpty(line) || line.StartsWith("//"))
            {
                // 빈 줄
                _nextReadLine++;
                continue;
            }

            _nextReadLine++;
            return ParseLine(line);
        }

        return null;
    }

    private string[] ParseLine(string line)
    {
        string pattern = @"
             # Match one value in valid CSV string.
             (?!\s*$)                                      # Don't match empty last value.
             \s*                                           # Strip whitespace before value.
             (?:                                           # Group for value alternatives.
               '(?<val>[^'\\]*(?:\\[\S\s][^'\\]*)*)'       # Either $1: Single quoted string,
             | ""(?<val>[^""\\]*(?:\\[\S\s][^""\\]*)*)""   # or $2: Double quoted string,
             | (?<val>[^,'""\s\\]*(?:\s+[^,'""\s\\]+)*)    # or $3: Non-comma, non-quote stuff.
             )                                             # End group of value alternatives.
             \s*                                           # Strip whitespace after value.
             (?:,|$)                                       # Field ends on comma or EOS.
             ";

        string[] values = (from Match m in Regex.Matches(line, pattern,
                               RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline)
                           select m.Groups[1].Value).ToArray();

        for (int i = 0; i < values.Length; ++i)
        {
            values[i] = values[i].Replace("\\\"", "\"")
                .Replace("\\n", "\n");
        }

        return values;
    }
}