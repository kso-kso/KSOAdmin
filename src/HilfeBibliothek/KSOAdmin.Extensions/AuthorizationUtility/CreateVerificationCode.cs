using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace KSOAdmin.Extensions.AuthorizationUtility
{
    public class CreateVerificationCode:ICreateVerificationCode
    {
        private static readonly string[] _chars = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public string RandomText()
        {
            string code = "";//产生的随机数
            int temp = -1;
            Random rand = new Random();
            for (int i = 1; i < 5; i++)
            {
                int t = rand.Next(61);
                code += _chars[t];
            }
            return code;
        }
        /// <summary>
        /// 转化为64位编码返回
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string CreateBase64Imgage(string code)
        {
            Random random = new Random();
            //验证码颜色集合
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //验证码字体集合
            string[] fonts = { "Small Fonts", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "Kristen ITC", "方正舒体", "Tempus Sans ITC", "Segoe Script"  , "Papyrus" };

            using var img = new Bitmap((int)code.Length * 18, 34);
            using var g = Graphics.FromImage(img);
            g.Clear(Color.Wheat);//背景 

            //在随机位置画背景点
            for (int i = 0; i < 80; i++)
            {
                int x = random.Next(img.Width);
                int y = random.Next(img.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 4, 4);
            }
            //验证码绘制在g中
            for (int i = 0; i < code.Length; i++)
            {
                int cindex = random.Next(7);//随机颜色索引值
                int findex = random.Next(8);//随机字体索引值
                Font f = new Font(fonts[findex], 17, FontStyle.Bold);//字体
                Brush b = new SolidBrush(c[cindex]);//颜色
                int ii = 4;
                if ((i + 1) % 3 == 0)//控制验证码不在同一高度
                {
                    ii = 3;
                }
                g.DrawString(code.Substring(i, 1), f, b, 3 + (i * 12), ii);//绘制一个验证字符
            }
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Jpeg);
                byte[] b = stream.ToArray();
                return Convert.ToBase64String(stream.ToArray());
            }
        }
    }
}
