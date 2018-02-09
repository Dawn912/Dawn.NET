using Dawn.Utils.Configuration;
using Dawn.Utils.Log;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Dawn.Utils.Net
{
    public static class MailHelper
    {
        private static readonly ILog log = LogHelper.GetILogInstance();
        private static readonly string address = AppSettings.Get("Dawn.Utils.Net.MailHelper.Address");
        private static readonly string displayName = AppSettings.Get("Dawn.Utils.Net.MailHelper.DisplayName");
        private static readonly string userName = AppSettings.Get("Dawn.Utils.Net.MailHelper.UserName");
        private static readonly string password = AppSettings.Get("Dawn.Utils.Net.MailHelper.Password");
        private static readonly string host = AppSettings.Get("Dawn.Utils.Net.MailHelper.Host");

        public static void Send(string mailTo, string subject, string body)
        {
            //使用指定的邮件地址初始化MailAddress实例
            MailAddress maddr = new MailAddress(address, displayName, System.Text.Encoding.UTF8);
            //初始化MailMessage实例
            MailMessage myMail = new MailMessage();

            //向收件人地址集合添加邮件地址
            myMail.To.Add(mailTo);

            //发件人地址
            myMail.From = maddr;
            //电子邮件的标题
            myMail.Subject = subject;
            //电子邮件的主题内容使用的编码
            myMail.SubjectEncoding = Encoding.UTF8;
            //电子邮件正文
            myMail.Body = body;
            //电子邮件正文的编码
            myMail.BodyEncoding = Encoding.Default;
            myMail.Priority = MailPriority.Normal;
            myMail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            //指定发件人的邮件地址和密码以验证发件人身份
            smtp.Credentials = new System.Net.NetworkCredential(userName, password);

            //设置SMTP邮件服务器
            smtp.Host = host;
#if DEBUG
            Console.WriteLine("mailTo:{0}, subject:{1}, body:{2}", mailTo, subject, body);
#else
            //将邮件发送到SMTP邮件服务器
            try
            {
                smtp.Send(myMail);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
#endif
        }
    }
}
