using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConvertAPI.ParseHelper
{
    /// <summary>
    /// The definition of C Sharp content.
    /// </summary>
    public class CSharpLine
    {
        private string _line = string.Empty;
        public CSharpLine(string line)
        {
            _line = line;
        }
        public string LineContent
        {
            get
            {
                return _line;
            }
        }
        public KIND_OF_LINE GetKindOfLine
        {
            get
            {
                KIND_OF_LINE result = KIND_OF_LINE.UNKNOWN;
                string lineContent = _line;
                bool success = lineContent.StartsWith("using");
                if (success)
                {
                    return result = KIND_OF_LINE.USING;
                }
                success = lineContent.StartsWith("namespace");
                if (success)
                {
                    return result = KIND_OF_LINE.NAMESPACE;
                }
                var match = Regex.Match(lineContent, " class ");
                success = match.Success;
                if (success)
                {
                    return result = KIND_OF_LINE.CLASS;
                }
                if (success = lineContent.TrimStart().StartsWith("//"))
                {
                    return result = KIND_OF_LINE.COMMENT_LINE;
                }
                if (success = lineContent.TrimStart().StartsWith("[")
                    && lineContent.TrimStart().EndsWith("]"))
                {
                    return result = KIND_OF_LINE.ATTRIBUTE;
                }
                if (success = lineContent.TrimStart().StartsWith("{"))
                {
                    return result = KIND_OF_LINE.LEFT_BIG_PARANTHESES;
                }
                if (success = lineContent.TrimStart().StartsWith("public")
                    && lineContent.EndsWith(")"))
                {
                    return result = KIND_OF_LINE.METHOD;
                }
                if (success = lineContent.TrimStart().StartsWith("}"))
                {
                    return result = KIND_OF_LINE.RIGHT_BIG_PARANTHESES;
                }
                if (success = (lineContent.Trim() == "" || lineContent.Trim() == string.Empty))
                {
                    return result = KIND_OF_LINE.WARP;
                }
                return KIND_OF_LINE.CODE;
            }
        }
    }
}
