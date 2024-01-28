using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using SwiftChat.Models.Configurations;

namespace SwiftChat.Services
{
	public class GmailServiceHelper
	{
		public static async Task<GmailService> GetGmailServiceAsync(GmailEmailSettings settings)
		{
			UserCredential credential;

			using (var stream = new FileStream("./SensitiveGmailData/gmail_credentials.json", FileMode.Open, FileAccess.Read))
			{
				string credPath = "./SensitiveGmailData/token.json";
				credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.FromStream(stream).Secrets,
					settings.Scopes,
					"user",
					CancellationToken.None,
					new FileDataStore(credPath, true));
				Console.WriteLine("Credential file saved to: " + credPath);
			}

			// Creating the Gmail API service here
			var service = new GmailService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = settings.ApplicationName,
			});

			return service;
		}
	}
}
