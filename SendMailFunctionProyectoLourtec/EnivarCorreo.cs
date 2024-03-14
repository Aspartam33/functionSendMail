using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;

namespace SendMailFunctionProyectoLourtec
{
    public static class EnivarCorreo
    {
        [FunctionName("SendMail")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "sendemail")] HttpRequest req,
            ILogger log)
        {

            string smtpAuthUsername = "user";
            string smtpAuthPassword = "pasword";
            string subject = "Hola esta es una prueba de correo desde azure";
            var htmlContent = "<html><body><h1>Quick send email test</h1><br/><h4>This email message is sent from Azure Communication Service Email.</h4><p>Elaborado por David Chaprro</p></body></html>";
           // var htmlContent = "Hello there";
         
            string sender = "donotreply@1bff0bb0-5c95-40e3-8211-b37e0347d836.azurecomm.net";
            string recipient = "davidjosechaparro@hotmail.com";

            try
            {
                SmtpClient cliente = new SmtpClient();
                cliente.UseDefaultCredentials = false;
                cliente.Credentials = new NetworkCredential(smtpAuthUsername, smtpAuthPassword);
                cliente.Port = 587;
                cliente.Host = "smtp.azurecomm.net";
                cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
                cliente.EnableSsl = true;

                log.LogInformation("sendemail client constructed");

                MailMessage _mailMessage = new MailMessage();
                _mailMessage.From = new MailAddress( sender);
                _mailMessage.To.Add(recipient);
                _mailMessage.Subject = subject;
                _mailMessage.IsBodyHtml = true;
                _mailMessage.Body = htmlContent;


                log.LogInformation("sendemail mail constructed");

                cliente.Send(_mailMessage);

                return new OkObjectResult(true);
            }
            catch ( Exception ex)
            {
                log.LogInformation("sendemail failed " + ex.Message);
                log.LogInformation("sendemail failed " + ex);
                System.Diagnostics.Trace.TraceError(ex.Message);
                var errorObjectResult = new ObjectResult(ex.Message);
                errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

                return new OkObjectResult(true);
            }

           
        }
    }
}
