using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSOAdmin.Models.Core
{
    /// <summary>
    /// web返回消息泛型版本
    /// </summary>
    /// <typeparam name="Tele"></typeparam>
    public class ResponseModel<Tele>
    {
        /// <summary>
        ///  状态码
        /// </summary>
        public int Code { get; set; } = 200;

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; } = "执行成功";

        /// <summary>
        /// 返回数据集合
        /// </summary>
        public Tele? Data { get; set; } = default(Tele);

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static ResponseModel<Tele> Success(string msg,int Code=200)
        {
            return Message(true, msg, default, Code);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static ResponseModel<Tele> Success( int Code = 200)
        {
            return Message(true, "操作成功", default, Code);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <returns></returns>
        public static ResponseModel<Tele> Success(string msg, Tele response,int Code=200)
        {
            return Message(true, msg, response, Code);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static ResponseModel<Tele> Fail(string msg, int Code = 500)
        {
            return Message(false, msg, default, Code);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <returns></returns>
        public static ResponseModel<Tele> Fail(string msg, Tele response, int Code=500)  
        {
            return Message(false, msg, response, Code);
        }
        /// <summary>
        /// 自定义返回消息
        /// </summary>
        /// <param name="success">失败/成功</param>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <returns></returns>
        public static ResponseModel<Tele> Message(bool success, string msg, Tele response,int Code=500)
        {
            return new ResponseModel<Tele>() { Msg = msg, Data = response, IsSuccess = success, Code = Code };
        }
    }


    /// <summary>
    /// web 返回消息
    /// </summary>
    public class ResponseModel
    {
        public int Code { get; set; } = 200;
        public bool IsSuccess { get; set; } = true;
        public string Msg { get; set; } = "执行成功";
        public object? Data { get; set; } = null;

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="Status">Status</param>
        /// <returns></returns>
        public static ResponseModel Success(string msg,int Status=200)
        {
            return Message(true, msg, default, Status);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="Status">Status</param>
        /// <returns></returns>
        public static ResponseModel Success(int Status = 200)
        {
            return Message(true, "操作成功", default, Status);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <returns></returns>
        public static ResponseModel Success(string msg, object response, int Status = 200)
        {
            return Message(true, msg, response, Status);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="Status">Status</param>
        /// <returns></returns>
        public static ResponseModel Fail(string msg,int Status=500)
        {
            return Message(false, msg, default, Status);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <param name="Status">数据</param>
        /// <returns></returns>
        public static ResponseModel Fail(string msg, object response, int Status = 500)
        {
            return Message(false, msg, response, Status);
        }
        /// <summary>
        /// 自定义返回消息
        /// </summary>
        /// <param name="success">失败/成功</param>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <returns></returns>
        public static ResponseModel Message(bool success, string msg, object response,int status)
        {
            return new ResponseModel() { Msg = msg, Data = response, IsSuccess = success , Code = status };
        }
    }


}
