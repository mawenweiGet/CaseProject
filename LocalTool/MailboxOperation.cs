using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LocalTool
{
    public static class MailboxOperation
    {
        static Attachment attachment;
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="serverAddress">服务器地址</param>
        /// <param name="targetMailbox">接受对象邮箱</param>
        /// <param name="initiatorsMailbox">发送者邮箱</param>
        /// <param name="subject">标题</param>
        /// <param name="body">正文</param>
        /// <param name="attachFileName">附件路径</param>
        /// <param name="authorizationCode">邮箱的授权码</param>
        /// <returns></returns>
        public static bool SendEmail(string serverAddress,string targetMailbox,
            string initiatorsMailbox,string subject,string body,string attachFileName,string authorizationCode)
        {
            using (MailMessage mailMessage = new MailMessage())
            using (SmtpClient smtpClient = new SmtpClient(serverAddress))//SMTP服务器
            {
                mailMessage.To.Add(targetMailbox); //接收邮箱
                //mailMessage.CC.Add("");//抄送邮箱
                //mailMessage.Bcc.Add("");//密送
                mailMessage.From = new MailAddress(initiatorsMailbox);//发送邮箱
                mailMessage.Subject = subject;//邮件标题
                mailMessage.Body = body;//邮件正文
                //mailMessage.IsBodyHtml = true; //内容是否可以为html形式
                //mailMessage.BodyEncoding = Encoding.Default;//编码方式
                if (attachFileName != null && attachFileName.Length > 0)
                {
                    Attachment attachment;
                    attachment = new Attachment(attachFileName);
                    mailMessage.Attachments.Add(attachment);
                }
                smtpClient.Credentials = new System.Net.NetworkCredential(initiatorsMailbox, authorizationCode);//参数填smtp用户名、授权码
                smtpClient.Send(mailMessage);
            }
            return true;
        }
    }
}
