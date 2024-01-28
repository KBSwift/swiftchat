using Microsoft.AspNetCore.Mvc;
using SwiftChat.Interfaces;

namespace SwiftChat.Controllers
{
	public class GmailTestController : Controller
	{
		private readonly IEmailService _emailService;

		public GmailTestController(IEmailService emailService)
		{
			_emailService = emailService;
		}

		[HttpPost]
		public async Task<IActionResult> SendTestEmail()
		{
			string to = "INSERT EMAIL HERE"; 
			string subject = "Welcome to SwiftChat!";
			string messageBody = "<h1>This is a test email</h1><p>Hello, this is a test email sent from SwiftChat using Gmail API.</p>";

			try
			{
				await _emailService.SendEmailAsync(to, subject, messageBody);
				
				return Json(new { success = true, message = "Email sent successfully!" });
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Json(new { success = false, message = "Failed to send email." });
			}
		}
	}
}
