# WebGL-Email

Unity Plugin that allows sending emails from WebGL builds using the [smtpjs.com](https://smtpjs.com/) API.

``` cs
using UE.Email;

Email.SendEmail(from, to, subject, body, smtp, user, password);

Email.SendEmailToken(from, to, subject, body, token);

```

## HTML

Use this class for easy HTML formating:

``` cs

"This is " + HTML.Bold("bold") + " and this is " + HTML.Italic("italic") + "." + HTML.P + 
"This is a new paragraph."


```

# Knows issues

- Sending mails with attachment currently does not work.
