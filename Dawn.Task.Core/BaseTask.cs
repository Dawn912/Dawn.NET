using Dawn.Utils.Log;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawn.Task.Core
{
    public abstract class BaseTask : IModule
    {
        protected readonly ILog log;
        protected string TaskName { get; private set; }
        protected string[] Args { get; private set; }
        protected bool PauseEnd { get; private set; }
        /// <summary>
        /// 默认的异常处理逻辑执行后，是否将异常继续抛出
        /// </summary>
        protected virtual bool ThrowException
        {
            get
            {
                return true;
            }
        }

        public BaseTask()
        {
            log = GetLogInstance();
            Init();
        }
        private void Init()
        {
            this.TaskName = this.GetType().FullName;
        }

        private string TryGet(string[] args, int index)
        {
            string result = String.Empty;
            try
            {
                result = args[index];
            }
            catch (Exception ex)
            {
                Error(String.Format("参数读取值异常，索引为：{0}", index), ex);
            }
            return result;
        }

        protected virtual ILog GetLogInstance()
        {
            return LogHelper.GetILogInstance();
        }

        public void Execute(string[] args)
        {
            if (args != null && args.Any())
            {
                Args = args;
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "/pause":
                            PauseEnd = TryGet(args, i + 1) == "1";
                            break;
                        default:
                            break;
                    }
                }
            }
            Execute();
            if (PauseEnd)
            {
                Console.ReadLine();
            }
        }

        public virtual void Execute()
        {
            OnProgramStart();
            try
            {
                HandleExecute();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("[{0}]发生未处理异常", TaskName), ex);
                HandleException(ex);
            }
            OnProgramEnd();
        }
        protected virtual void OnProgramStart()
        {
            log.Info(String.Format("{0}启动", TaskName));
        }
        protected virtual void OnProgramEnd()
        {
            log.Info(String.Format("{0}正常结束", TaskName));
        }
        protected abstract void HandleExecute();
        protected virtual void HandleException(Exception ex)
        {
            ErrorSendMail("全局未处理异常", ex.StackTrace);
            if (ThrowException)
            {
                throw ex;
            }
        }
        protected virtual void Error(string message, Exception ex)
        {
            log.Error(message, ex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        protected void ErrorSendMail(string email, string title, string message)
        {
            string subject = String.Format("[{0}]发生错误:{1}", TaskName, title);
            StringBuilder body = new StringBuilder(TaskName);
            body.AppendLine("<br />");
            body.AppendLine(message);
            body.AppendLine("<br />");
            throw new NotImplementedException("发送邮件功能需自行实现");
        }

        /// <summary>
        /// 错误发送邮件告警
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">内容</param>
        protected void ErrorSendMail(string title, string message)
        {
            string subject = String.Format("[{0}]发生错误:{1}", TaskName, title);
            StringBuilder body = new StringBuilder(TaskName);
            body.AppendLine("<br />");
            body.AppendLine(message);
            body.AppendLine("<br />");
            throw new NotImplementedException("发送邮件功能需自行实现");
        }

        /// <summary>
        /// 错误发送邮件告警并记录日志
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">内容</param>
        protected void ErrorSendMailAndLog(string email, string title, string message)
        {
            log.Error($"【{title}】\n{message}");
            ErrorSendMail(email, title, message);
        }

        /// <summary>
        /// 错误发送邮件告警并记录日志
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">内容</param>
        protected void ErrorSendMailAndLog(string title, string message)
        {
            log.Error(title);
            log.Error(message);
            ErrorSendMail(title, message);
        }
        /// <summary>
        /// 错误发送邮件告警并记录日志并输出
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">内容</param>
        protected void ErrorSendMailAndLogPrint(string title, string message)
        {
            log.Error(title);
            log.Error(message);
            Console.WriteLine(title);
            if (!string.IsNullOrWhiteSpace(message) && message.Length <= 100)
            {
                Console.WriteLine(message);
            }
            ErrorSendMail(title, message);
        }
        /// <summary>
        /// 短信提醒
        /// </summary>
        protected virtual void SendMessage(string phone, string message)
        {
            throw new NotImplementedException("发送消息功能需自行实现");
        }
        /// <summary>
        /// 记录并在控制台输出日志
        /// </summary>
        /// <param name="message"></param>
        protected void LogAndPrint(string message)
        {
            LogAndPrint(message, null);
        }
        /// <summary>
        /// 记录并在控制台输出日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="arg"></param>
        protected void LogAndPrint(string message, params object[] arg)
        {
            Console.WriteLine(message, arg);
            Log(message, arg);
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="arg"></param>
        protected void Log(string message)
        {
            Log(message, null);
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="arg"></param>
        protected void Log(string message, params object[] arg)
        {
            log.InfoFormat(message, arg);
        }
    }
}
