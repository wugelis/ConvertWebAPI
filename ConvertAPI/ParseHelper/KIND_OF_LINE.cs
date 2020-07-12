using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertAPI.ParseHelper
{
    public enum KIND_OF_LINE
    {
        USING = 0,
        NAMESPACE = 1,
        WARP = 3,
        CLASS = 4,
        LEFT_BIG_PARANTHESES = 5,
        RIGHT_BIG_PARANTHESES = 6,
        COMMENT_LINE = 7,
        METHOD = 8,
        CODE = 9,
        ATTRIBUTE = 10,
        UNKNOWN = 999
    }
}
