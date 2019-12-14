using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SendEmail.Services;
//url for reference https://steemit.com/utopian-io/@babelek/how-to-send-email-using-asp-net-core-2-0

namespace SendEmail.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // POST Email
        //api/email
        /// <summary>
        /// POST api/email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns>"200":"Sucesso"</returns>
        /// <returns>"400":"Failed"</returns>
        [HttpPost]
        [Route("api/email")]
        public async Task<IActionResult> SendEmailAsync(string email, string subject, string message)
        {

            try
            {
                await _emailService.SendMail(email, subject, message);
                return new ContentResult()
                {
                    StatusCode = 200,
                    Content = "email enviado com sucesso!"
                };
            }
            catch
            {
                return new ContentResult()
                {
                    StatusCode = 400,
                    Content = email
                };
            } 
        }
    }
}
