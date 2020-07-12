using ConvertAPI.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConvertAPI.ParseHelper
{
    /// <summary>
    /// C Sharp 檔案的 Parse 主要類別
    /// </summary>
    public class CSharpFile : IComparable
    {
        public static string _csharpStr = string.Empty;
        public MyStringBuilder _csharpLines = null;
        private MyStringBuilder Content
        {
            get
            {
                if (_csharpLines == null)
                {
                    _csharpLines = new MyStringBuilder();
                }
                return _csharpLines;
            }
        }
        public CSharpFile()
        {
        }
        public CSharpFile(string csharpFileName)
        {
            Content.Append(FileHelper.ReadTextFileToEnd(csharpFileName));
        }
        public new string ToString()
        {
            return _csharpLines.ToString();
        }
        public int LineCount()
        {
            return Regex.Matches(Content.ToString(), Environment.NewLine).Count;
        }
        public string GetLine(int index)
        {
            return _csharpLines[index];
            /*
            string[] result = Content.ToString()
                .Split(new char[] { '\r' })
                .ToArray();

            if (index > LineCount())
            {
                $"第 {index} 列已經超出範圍！".Dump();
            }
            return result[index].TrimStart();
            */
        }
        public void InsertNewLine(int startIndex, string content)
        {
            _csharpLines.Insert(startIndex, content);
        }
        public CSharpLine this[int index]
        {
            get
            {
                return new CSharpLine(GetLine(index));
            }
            set
            {
                _csharpLines[index] = value.LineContent;
            }
        }
        public int CompareTo(object obj)
        {
            return int.Parse(this.Equals(obj).ToString());
        }
    }
}
