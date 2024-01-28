using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using MimeKit;
using SwiftChat.Interfaces;


namespace SwiftChat.Services
{
	public class GmailEmailService : IEmailService
	{
		private readonly GmailService _gmailService;

		public GmailEmailService(GmailService gmailService)
		{
			_gmailService = gmailService;
		}

		public async Task SendEmailAsync(string to, string subject, string messageBody)
		{
			var emailMessage = new Message
			{
				Raw = EncodeEmail(to, subject, messageBody)
			};

			await _gmailService.Users.Messages.Send(emailMessage, "me").ExecuteAsync();
		}

		public string EncodeEmail(string to, string subject, string messageBody)
		{

			string? senderEmail = Environment.GetEnvironmentVariable("SWIFTCHAT_SENDER_EMAIL"); // Nullable
			if (string.IsNullOrEmpty(senderEmail))
			{
				throw new InvalidOperationException("Sender email missing. Should be in ENV VAR or hardcoded");
			}

			// Using MimeKit to preserve format
			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse(senderEmail));
			email.To.Add(MailboxAddress.Parse(to));
			email.Subject = subject;

			// message body
			var builder = new BodyBuilder { HtmlBody = messageBody };
			email.Body = builder.ToMessageBody();

			// COnverting MimeMessage to a byte array
			byte[] messageBytes;
			using (var memoryStream = new MemoryStream())
			{
				email.WriteTo(memoryStream);
				messageBytes = memoryStream.ToArray();
			}

			// Converting byte array to a base64url string per API docs
			var base64EncodedEmail = Convert.ToBase64String(messageBytes)
				.Replace('+', '-')
				.Replace('/', '_')
				.TrimEnd('=');

			return base64EncodedEmail;
		}
	}
}
