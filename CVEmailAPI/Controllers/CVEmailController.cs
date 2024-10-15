using Microsoft.AspNetCore.Mvc;
using CVEmailLib;
using System.Net;

namespace CVEmailAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CVEmailController : ControllerBase
    {

        private string logFile = System.Configuration.ConfigurationManager.AppSettings["logFile"] ?? "";

        private readonly ILogger<CVEmailController> _logger;

        public CVEmailController(ILogger<CVEmailController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "SendEmail")]
        public ActionResult<CVEmail> Post(CVEmail email)
        {
            CVEmailer emailer = new CVEmailer(new CVCredentials()
            {
                smtpServer = email.smtpServer,
                smtpPassword = email.smtpPassword,
                smtpPort = email.smtpPort,
                smtpUsername = email.smtpUsername
            }, 100, 3, logFile);
            /**/
            emailer.SendEmail(email.Message, email.Subject, email.To);
            return CreatedAtAction(nameof(Post), email);
        }
    }
}
