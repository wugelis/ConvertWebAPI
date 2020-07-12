using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ConvertAPI
{
    /// <summary>
    /// 命令列參數處裡類別
    /// </summary>
    public class ArgumentResult
    {
        public ArgumentResult(string[] args)
        {
            foreach (string arg in args)
            {
                if (arg.ToLower() == "-b"
                    || arg.ToLower() == "--basecontroller")
                {
                    BaseController[arg.ToLower()] = arg.ToLower();
                    continue;
                }
                else if (arg.ToLower() == "-g"
                    || arg.ToLower() == "--httpget")
                {
                    HttpGet[arg.ToLower()] = arg.ToLower();
                    continue;
                }
                else if (arg.ToLower() == "-ui"
                    || arg.ToLower() == "--showui")
                {
                    ShowUI[arg.ToLower()] = arg.ToLower();
                    continue;
                }
                else
                {
                    string apiFileName = string.Empty;
                    if (Path.IsPathRooted(arg))
                    {
                        apiFileName = Path.GetFileName(arg);
                    }
                    else
                    {
                        apiFileName = arg;
                    }
                    Regex regex = new Regex(@"^[\w,\s-]+Controller\.cs$");
                    Regex regexLower = new Regex(@"^[\w,\s-]+controller\.cs$");
                    if (regex.IsMatch(apiFileName) || regexLower.IsMatch(apiFileName))
                    {
                        APIControllerFileName["api"] = arg.ToLower(); //存入還是存入完整路徑+檔名
                    }
                    else
                    {
                        //如果連檔名也不是，就儲存錯誤訊息
                        ErrMsg["err"] = "參數輸入錯誤！或者檔案名稱不是 Controller 結尾！";
                    }
                }
            }
        }
        private ArgumentContent _baseController;
        public ArgumentContent BaseController
        {
            get
            {
                if (_baseController == null)
                {
                    _baseController = new ArgumentContent();
                }
                return _baseController;
            }
        }

        private ArgumentContent _httpGet;
        public ArgumentContent HttpGet
        {
            get
            {
                if (_httpGet == null)
                {
                    _httpGet = new ArgumentContent();
                }
                return _httpGet;
            }
        }

        private ArgumentContent _showUI;
        public ArgumentContent ShowUI
        {
            get
            {
                if (_showUI == null)
                {
                    _showUI = new ArgumentContent();
                }
                return _showUI;
            }
        }
        private ArgumentContent _apiControllerFileName;
        public ArgumentContent APIControllerFileName
        {
            get
            {
                if (_apiControllerFileName == null)
                {
                    _apiControllerFileName = new ArgumentContent();
                }
                return _apiControllerFileName;
            }
        }
        private ArgumentContent _errMsg = null;

        public ArgumentContent ErrMsg
        {
            get
            {
                if (_errMsg == null)
                {
                    _errMsg = new ArgumentContent();
                }
                return _errMsg;
            }
        }
    }

    public class ArgumentContent
    {
        private Hashtable _content2;
        private Hashtable getHashContent
        {
            get
            {
                if (_content2 == null)
                {
                    _content2 = new Hashtable();
                }
                return _content2;
            }
        }
        public string this[string key]
        {
            get
            {
                return getHashContent[key.ToLower()] as string;
            }
            set
            {
                if (getHashContent[key.ToLower()] == null)
                {
                    getHashContent[key.ToLower()] = value;
                }
            }
        }
        public int GetCountByArguments(params string[] args)
        {
            int count = 0;
            foreach (var arg in args)
            {
                if (this[arg.ToLower()] != null)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
