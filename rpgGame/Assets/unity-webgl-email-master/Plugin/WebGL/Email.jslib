/* Adapted from SmtpJS.com - v2.0.1 https://smtpjs.com/v2/smtp.js*/ 

var EmailLib = {

  	SendMail : function (from, to, subject, body, smtp, user, password)
  	{
//  		window.alert("Sending mail from " + Pointer_stringify(from) 
//  			+ " to: " + Pointer_stringify(to) 
//  			+ " subject: " + Pointer_stringify(subject) 
//  			+ " body: " + Pointer_stringify(body) 
//  			+ " smtp: " + Pointer_stringify(smtp)
//  			+ " user: " + Pointer_stringify(user)
//  			+ " pw: " + Pointer_stringify(password));

	  	Email.Send(Pointer_stringify(from), 
	  		Pointer_stringify(to), 
	  		Pointer_stringify(subject), 
	  		Pointer_stringify(body), 
	  		Pointer_stringify(smtp), 
	  		Pointer_stringify(user), 
	  		Pointer_stringify(password),
	  		null); //callback
  	},

	SendMailToken : function (from, to, subject, body, tok)
	{
//  		window.alert("Sending mail (token) from " + Pointer_stringify(from) 
//  			+ " to: " + Pointer_stringify(to) 
//  			+ " subject: " + Pointer_stringify(subject) 
//  			+ " body: " + Pointer_stringify(body) 
//  			+ " token: " + Pointer_stringify(tok));

	  	Email.Send(Pointer_stringify(from), 			//e
	  		Pointer_stringify(to), 						//o
	  		Pointer_stringify(subject), 				//t
	  		Pointer_stringify(body), 					//n
	  		{token: Pointer_stringify(tok)});			//a
  	},

   	SendMailTokenWithAttachment : function (from, to, subject, body, tok, attachment)
	{
//		window.alert("Sending mail (token) from " + Pointer_stringify(from) 
//  			+ " to: " + Pointer_stringify(to) 
//  			+ " subject: " + Pointer_stringify(subject) 
//  			+ " body: " + Pointer_stringify(body) 
//  			+ " token: " + Pointer_stringify(tok)
//  			+ " attachment: " + Pointer_stringify(attachment));

		Email.SendWithAttachment(
				Pointer_stringify(from),				//e 
	  			Pointer_stringify(to), 					//o
	  			Pointer_stringify(subject), 			//t
	  			Pointer_stringify(body), 				//n
	  			{token: Pointer_stringify(tok)},		//a
	  			null, 									//s
	  			null,									//r
	  			Pointer_stringify(attachment), 			//c
	  			null);									//d
   	},

	// uploadFileToServer : function () {
	// 	var file = event.srcElement.files[0];
	// 	console.log(file);
	// 	var reader = new FileReader();
	// 	reader.readAsBinaryString(file);
	// 	reader.onload = function () {
	// 		var datauri = “data:” + file.type + “;base64,” + btoa(reader.result);
	// 		Email.sendWithAttachment(“from@you.com”,
	// 			“to@them.com”,
	// 			“Subject”,
	// 			“Body”,
	// 			“smtp.server.com”,
	// 			“username”,
	// 			“password”,,
	// 	datauri,
	// 	function done(message) { 
	// 		alert(“Message sent OK”) }
	// 	);
	// 	};
	// 	reader.onerror = function() {
	// 		console.log(‘there are some problems’);	
	// 	};
	// },

  	$Email: {
        Send: function (e, o, t, n, a, s, r, c) 
		{ 
			var d = Math.floor(1e6 * Math.random() + 1), 
			i = "From=" + e; 
			i += "&to=" + o, 
			i += "&Subject=" + encodeURIComponent(t), 
			i += "&Body=" + encodeURIComponent(n), 
			void 0 == a.token ? (
				i += "&Host=" + a, 
				i += "&Username=" + s, 
				i += "&Password=" + r, 
				i += "&Action=Send"
				) : (i += "&SecureToken=" + a.token, 
				i += "&Action=SendFromStored", 
				c = a.callback),
			
			i += "&cachebuster=" + d, 
			Email.ajaxPost("https://smtpjs.com/v2/smtp.aspx?", i, c) 
		}, 

		SendWithAttachment: function (e, o, t, n, a, s, r, c, d) { 
			var i = Math.floor(1e6 * Math.random() + 1), 
			m = "From=" + e; 
			m += "&to=" + o, 
			m += "&Subject=" + encodeURIComponent(t), 
			m += "&Body=" + encodeURIComponent(n), 
			m += "&Attachment=" + encodeURIComponent(c), 
			void 0 == a.token ? (
				m += "&Host=" + a, 
				m += "&Username=" + s, 
				m += "&Password=" + r, 
				m += "&Action=Send"
				) : (
				m += "&SecureToken=" + a.token, 
				m += "&Action=SendFromStored"), 
				m += "&cachebuster=" + i, 
				Email.ajaxPost("https://smtpjs.com/v2/smtp.aspx?", m, d) 
		}, 

		ajaxPost: function (e, o, t) 
		{ 
			var n = Email.createCORSRequest("POST", e); n.setRequestHeader("Content-type", "application/x-www-form-urlencoded"), 
				n.onload = function () { 
					var e = n.responseText; void 0 != t && t(e) 
					}, 
				n.send(o) 
		}, 

		ajax: function (e, o) 
		{ 
			var t = Email.createCORSRequest("GET", e); 
					t.onload = function () { 
						var e = t.responseText; void 0 != o && o(e) 
					}, t.send() 
		}, 

		createCORSRequest: function (e, o) 
		{ 
			var t = new XMLHttpRequest; return "withCredentials" in t ? t.open(e, o, !0) : "undefined" != typeof XDomainRequest ? (t = new XDomainRequest).open(e, o) : t = null, t 
		}
    },
};

autoAddDeps(EmailLib, '$Email');
mergeInto(LibraryManager.library, EmailLib);