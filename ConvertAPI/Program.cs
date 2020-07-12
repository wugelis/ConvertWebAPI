using ConvertAPI.Common;
using ConvertAPI.ParseHelper;
using System;
using System.IO;
using System.Windows.Forms;

namespace ConvertAPI
{
    class Program
    {
        static string GetHelp
        {
            
            get
            {
                return @".NET Core 3.0 Web API Convert Preview 1 - (.NET Conf 2019 台北)
參數用法：
    -B, --BaseController: 忽略 ApiController 的 Base 類型
        例：
        ConvertWebAPI.exe -B MyTestController.cs

    -G, --HttpGet: 忽略 API 方法上的 [HttpGet] 屬性
        例：
        ConvertWebAPI.exe -G MyTestController.cs

   -UI, --ShowUI: 顯示 UI 介面
        例：
        ConvertWebAPI.exe -UI MyTestController.cs

Copyright © Gelis Wu 2019";
            }
        }
        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Console.WriteLine(GetHelp);
                return;
            }

            ArgumentResult argumentResult = new ArgumentResult(args);
            if (argumentResult.ErrMsg.GetCountByArguments("ERR") > 0)
            {
                Console.WriteLine(argumentResult.ErrMsg["ERR"]);
                return;
            }

            bool isHaveBaseController = argumentResult.BaseController.GetCountByArguments("-B", "--BaseController") > 0;
            bool isHaveHttpGet = argumentResult.BaseController.GetCountByArguments("-G", "--HttpGet") > 0;
            bool isShowUI = argumentResult.ShowUI.GetCountByArguments("-ui", "--ShowUI") > 0;
            string apiFileName = argumentResult.APIControllerFileName["api"];
            if (!File.Exists(apiFileName))
            {
                Console.WriteLine($"檔案 {apiFileName} 不存在！請確認。");
                return;
            }


            string originalContent = FileHelper.ReadTextFileToEnd(apiFileName);

            if (isShowUI)
            {
                frmConvertWebAPI contextForm = new frmConvertWebAPI(originalContent);
                CSharpFile cs = ParseApiController(apiFileName);
                contextForm.SetTargetCsSource(cs.ToString());

                DialogResult result = contextForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    FileHelper.WriteTextToFile(apiFileName, cs.ToString());
                    Console.WriteLine($"寫入 {apiFileName} Ok！");
                }
                else
                {
                    Console.WriteLine("使用者取消...");
                }
            }
            else
            {
                CSharpFile cs = ParseApiController(apiFileName);
                FileHelper.WriteTextToFile(apiFileName, cs.ToString());
            }

            //Console.ReadLine();
        }

        private static CSharpFile ParseApiController(string apiFileName)
        {
            CSharpFile cs = new CSharpFile(apiFileName);

            cs.InsertNewLine(0, "using Microsoft.AspNetCore.Mvc;");

            for (int count = 0; count < cs.LineCount(); count++)
            {
                if (cs[count].GetKindOfLine == KIND_OF_LINE.USING)
                {
                    if (cs.GetLine(count).ToLower() == "using system.web.http;")
                    {
                        cs[count] = new CSharpLine("");
                    }
                }
            }

            for (int count = 0; count < cs.LineCount(); count++)
            {
                if (cs[count].GetKindOfLine == KIND_OF_LINE.CLASS)
                {
                    string mvcRoute = "[Route(\"api/[controller]\")]";
                    cs.InsertNewLine(count, mvcRoute);

                    string apiControllerAttr = "[ApiController]";
                    cs.InsertNewLine(count, apiControllerAttr);
                    break;
                }
            }

            for (int count = 0; count < cs.LineCount(); count++)
            {
                if (cs[count].GetKindOfLine == KIND_OF_LINE.METHOD)
                {
                    string getAttr = "[HttpGet]";
                    cs.InsertNewLine(count, getAttr);
                    break;
                }
            }

            for (int count = 0; count < cs.LineCount(); count++)
            {

                if (cs[count].GetKindOfLine == KIND_OF_LINE.CLASS)
                {
                    string procLine = cs.GetLine(count);
                    //procLine = procLine.Replace("ApiController", "ControllerBase");
                    string[] classLine = procLine.Split(new char[] { ':' });
                    procLine = $"{classLine[0]}: {classLine[1].Replace("ApiController", "ControllerBase")}";
                    cs[count] = new CSharpLine(procLine);
                }
            }

            return cs;
        }

    }
}