using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class MailSender : MonoBehaviour
{

    void Start()
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("toeb1992@live.dk");
        mail.To.Add("toeb1992@live.dk");
        mail.Subject = "Test Mail";
        mail.Body = "This is for testing SMTP mail from GMAIL";

        SmtpClient smtpServer = new SmtpClient("mail.omk.dk");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("admin@omk.dk", "OMKnet1338") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
        Debug.Log("success");

    }
}