using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UE.Email;


public class MailSender : MonoBehaviour
{

    public FloatList results;


    void Start()
    {
        MailMessage mail = new MailMessage();

        string from = "toeb1992@live.dk";
        string to = "toeb1992@live.dk";
        string subject = "Test Mail";

        string body = "";

        for (int i = 0; i < results.list.Count; i++)
        {
            body += results.list[i].ToString() + ", ";
        }


        string smtp = "mail.omk.dk";
        string user = "admin@omk.dk";
        string password = "OMKnet1338";
        string token = "";




        Email.SendEmail(from, to, subject, body, smtp, user, password);

        Email.SendEmailToken(from, to, subject, body, token);
    }
}