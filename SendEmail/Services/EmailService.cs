using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using System.IO;
using System.Threading.Tasks;

namespace SendEmail.Services
{
    public class EmailService : IEmailService
    {
        static string[] Scopes = { GmailService.Scope.GmailSend };
        static string ApplicationName = "Email Robotization API";

        public object MimeKit { get; private set; }

        public async Task SendMail(string email, string subject, string message)
        {
            UserCredential credential;

            //urs: https://developers.google.com/gmail/api/quickstart/dotnet
            //Habilite o Gmail API, baixe o arquivo json de autenticação e salve na pasta de inicialização
            using (var stream =
            new FileStream("Send.json", FileMode.Open,
              FileAccess.Read))
            {
                string credPath = "Auth-jason.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                             GoogleClientSecrets.Load(stream).Secrets,
                              Scopes,
                              "user",
                              System.Threading.CancellationToken.None,
                              new Google.Apis.Util.Store.FileDataStore(credPath, true)).Result;
            }

            // Criar API do serviço de gmail
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parâmetros para o corpo do email           
            string plainText = string.Format("To:{0}\r\n", email) +
                             string.Format("Subject: {0}\r\n", subject) +
                             string.Format("Content-Type: text/html; charset=utf-8\r\n\r\n") +
                             string.Format("<h2>{0}<h1>", message);

            var newMsg = new Google.Apis.Gmail.v1.Data.Message();
            newMsg.Raw = Encode(plainText.ToString());
            service.Users.Messages.Send(newMsg, "me").Execute();

            await Task.CompletedTask;
            
        }
        public static string Encode(string text)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);

            return System.Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
    }
}
