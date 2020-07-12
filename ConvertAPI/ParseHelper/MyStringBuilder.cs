using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertAPI.ParseHelper
{
    /// <summary>
    /// 針對需求重新設計的輕量 StringBuilder
    /// </summary>
    public class MyStringBuilder
    {
        private List<string> _cs_content = new List<string>();
        private List<int> _cs_warp = new List<int>();
        private char? _warpChar = null;
        private int _count;
        public MyStringBuilder()
        {
        }
        public MyStringBuilder(string content)
        {
            Append(content);
        }
        public void Append(string content)
        {
            string[] contents = content.Split(new char[] { '\n' });
            _count = 0;
            foreach (string csLine in contents)
            {
                //MatchCollection matchs = Regex.Matches(csLine, " "); //計算 Tab 數量（縮排用）
                var asciiTest = csLine.AsEnumerable()
                    .Cast<char>()
                    .Select(c => new { ASCII = (int)c });

                int wordCount = 0;
                foreach (var c in asciiTest.ToArray())
                {
                    if (c.ASCII != 32 && c.ASCII != 9)
                    {
                        //$"程式碼從 {count} 開始".Dump();
                        break;
                    }
                    else
                    {
                        //紀錄前面使用的字元為 tab 或 Space
                        if (!_warpChar.HasValue)
                        {
                            _warpChar = (char)c.ASCII;
                        }
                    }
                    wordCount++;
                }
                _cs_warp.Add(wordCount);
                _cs_content.Add(csLine.TrimStart().TrimEnd());
                _count++;
            }
        }
        public void AppendLine(string lineContent)
        {
            _cs_content.Add(lineContent);
        }
        public void Insert(int startIndex, string content)
        {
            int tabs = _cs_warp[startIndex];
            _cs_content.Insert(startIndex, content);
            _cs_warp.Insert(startIndex, tabs);
        }
        public string this[int index]
        {
            get
            {
                return _cs_content[index];
            }
            set
            {
                _cs_content[index] = value;
            }
        }
        public override string ToString()
        {
            string result = string.Empty;
            string tabs = string.Empty;
            int count = 0;
            foreach (var s in _cs_content.AsEnumerable())
            {
                tabs = string.Empty;
                tabs = tabs.PadLeft(_cs_warp[count], _warpChar.Value);
                result += ($"{tabs}{s}\r\n");
                count++;
            }
            return result;
        }
        public int GetTabCounts(int index)
        {
            return _cs_warp[index];
        }
    }
}
