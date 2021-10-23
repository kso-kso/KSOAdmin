using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSOAdmin.Core.ConsoleHelper
{
    public static class ConsoleHelper
    { 
        /// <summary>
        /// 打印错误信息
        /// </summary>
        /// <param name="message">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteErrorLine(this string message, ConsoleColor color = ConsoleColor.Red)
        {
            WriteColorLine(message, color);
        }

        /// <summary>
        /// 打印警告信息
        /// </summary>
        /// <param name="message">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteWarningLine(this string message, ConsoleColor color = ConsoleColor.Yellow)
        {
            WriteColorLine(message, color);
        }

        /// <summary>
        /// 打印正常信息
        /// </summary>
        /// <param name="message">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteInfoLine(this string message, ConsoleColor color = ConsoleColor.White)
        {
            WriteColorLine(message, color);
        }

        /// <summary>
        /// 打印成功的信息
        /// </summary>
        /// <param name="message">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteSuccessLine(this string message, ConsoleColor color = ConsoleColor.Green)
        {
            WriteColorLine(message, color);
        }

        private static void WriteColorLine(string message, ConsoleColor color)
        {
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = foregroundColor;
        }

    }
}
