using App_NetCoreAngular.Server.Models.Mail;
using App_NetCoreAngular.Server.Models.Shared;
using App_NetCoreAngular.Server.Enums;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using static App_NetCoreAngular.Server.Enums.EnumSystem;

namespace App_NetCoreAngular.SendGrid
{

    public class MailSendGrid
    {
        private readonly IConfiguration _configuration;
        //sendgrid key
        private readonly string apiKey;
        private const string FROM_SUP = "lgv.alonso@gmail.com";
        private const string FROM_SUP_NAME = "";

        public MailSendGrid(IConfiguration configuration)
        {
            _configuration = configuration;
            //apiKey = _configuration.GetSection("SendgridApi").Value; //_config["SendgridApi"];
            apiKey = _configuration["SendgridApi"];
        }
        public async Task<String> SendTest()
        {
            Response response;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("lgv.alonso@gmail.com", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("lgv.alonso@gmail.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            response = await client.SendEmailAsync(msg);
            return response.StatusCode.ToString();
        }
        public async Task<ResponseApi<HttpStatusCode>> SendAttachmentEmail(Mail mailBody, TypeTemplate template)
        {
            var responseApi = new ResponseApi<HttpStatusCode>() { Success = false };
            if (!String.IsNullOrWhiteSpace(mailBody.To) && !String.IsNullOrWhiteSpace(mailBody.Subject))
            {
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(FROM_SUP, FROM_SUP_NAME);
                var subject = mailBody.Subject;
                var to = new EmailAddress(mailBody.To);
                var plainTextContent = "";
                var htmlContent = "";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                msg.SetTemplateId(TemplatesDictionary.TEMPLATES[template]);

                foreach (var a in mailBody.Attachments)
                {
                    msg.AddAttachment(a.GetFileName(), a.GetContent());
                }
                if (mailBody.Substitution != null)
                    msg.SetTemplateData(mailBody.Substitution);

                try
                {
                    var response = await client.SendEmailAsync(msg);
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                        responseApi.Success = true;
                    responseApi.Payload = response.StatusCode;
                }
                catch (Exception e)
                {
                    responseApi.Error =  e.Message;
                }
            }
            return responseApi;
        }

        public async Task<ResponseApi<HttpStatusCode>> SendMultipleAttachmentEmail(Mail mailBody, TypeTemplate template)
        {
            var responseApi = new ResponseApi<HttpStatusCode>() { Success = false };
            if (mailBody.ToList.Count > 0 && !String.IsNullOrWhiteSpace(mailBody.Subject))
            {
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(FROM_SUP, FROM_SUP_NAME);
                var subject = mailBody.Subject;
                var to = new List<EmailAddress>
                {
                    new EmailAddress(mailBody.ToList[0])
                };
                mailBody.ToList.Remove(mailBody.ToList[0]);
                var plainTextContent = "";
                var htmlContent = "";

                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, subject, plainTextContent, htmlContent);
                msg.SetTemplateId(TemplatesDictionary.TEMPLATES[template]);

                foreach (var a in mailBody.Attachments)
                {
                    msg.AddAttachment(a.GetFileName(), a.GetContent());
                }

                foreach (var mail in mailBody.ToList)
                {
                    msg.AddCc(new EmailAddress(mail));
                }

                if (mailBody.Substitution != null)
                    msg.SetTemplateData(mailBody.Substitution);

                try
                {
                    var response = await client.SendEmailAsync(msg);
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                        responseApi.Success = true;
                    responseApi.Payload = response.StatusCode;
                }
                catch (Exception e)
                {
                    responseApi.Error = e.Message;
                }
            }
            return responseApi;
        }

        public async Task<ResponseApi<HttpStatusCode>> SendSimpleEmail(Mail mailBody)
        {
            var responseApi = new ResponseApi<HttpStatusCode>() { Success = false };
            if (!String.IsNullOrWhiteSpace(mailBody.To) && !String.IsNullOrWhiteSpace(mailBody.Subject))
            {
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(FROM_SUP, FROM_SUP_NAME);
                var subject = mailBody.Subject;
                var to = new EmailAddress(mailBody.To);
                var plainTextContent = mailBody.PlainTextContent;
                var htmlContent = "";
                if (!String.IsNullOrEmpty(mailBody.HtmlTextContent))
                    htmlContent = mailBody.HtmlTextContent;

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                if (mailBody.Substitution != null)
                    msg.SetTemplateData(mailBody.Substitution);

                try
                {
                    var response = await client.SendEmailAsync(msg);
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                        responseApi.Success = true;
                    responseApi.Payload = response.StatusCode;
                }
                catch (Exception e)
                {
                    responseApi.Error = e.Message;
                }

            }
            return responseApi;
        }

        public async Task<ResponseApi<HttpStatusCode>> SendTemplateEmail(Mail mailBody, string template)
        {
            var responseApi = new ResponseApi<HttpStatusCode>() { Success = false };
            if (!String.IsNullOrWhiteSpace(mailBody.To) && !String.IsNullOrWhiteSpace(mailBody.Subject))
            {

                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(FROM_SUP, FROM_SUP_NAME);
                var subject = mailBody.Subject;
                var to = new EmailAddress(mailBody.To);
                var plainTextContent = "";
                var htmlContent = "";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                msg.SetTemplateId(template);

                if (mailBody.Substitution != null)
                    msg.SetTemplateData(mailBody.Substitution);

                try
                {
                    var response = await client.SendEmailAsync(msg);
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                        responseApi.Success = true;
                    responseApi.Payload = response.StatusCode;
                }
                catch (Exception e)
                {
                    responseApi.Error = e.Message;
                }
            }
            return responseApi;
        }

        public async Task<ResponseApi<HttpStatusCode>> SendMultipleTemplateEmail(Mail mailBody, TypeTemplate template)
        {
            var responseApi = new ResponseApi<HttpStatusCode>() { Success = false };
            if (mailBody.ToList.Count > 0 && !String.IsNullOrWhiteSpace(mailBody.Subject))
            {
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(FROM_SUP, FROM_SUP_NAME);
                var subject = mailBody.Subject;
                var to = new List<EmailAddress>
                {
                    new EmailAddress(mailBody.ToList[0])
                };
                mailBody.ToList.Remove(mailBody.ToList[0]);

                var plainTextContent = "";
                var htmlContent = "";

                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, subject, plainTextContent, htmlContent);
                msg.SetTemplateId(TemplatesDictionary.TEMPLATES[template]);

                if (mailBody.Substitution != null)
                    msg.SetTemplateData(mailBody.Substitution);

                foreach (var mail in mailBody.ToList)
                {
                    msg.AddCc(new EmailAddress(mail));
                }

                try
                {
                    var response = await client.SendEmailAsync(msg);
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                        responseApi.Success = true;
                    responseApi.Payload = response.StatusCode;
                }
                catch (Exception e)
                {
                    responseApi.Error = e.Message;
                }
            }
            return responseApi;
        }


        public async Task<ResponseApi<bool>> MailError(string correo, string mensaje, string empresa)
        {
            ResponseApi<bool> res = new ResponseApi<bool>() { Success = false };
            var mailBody = new Mail
            {
                Subject = String.Concat("Error ", empresa),
                To = correo,
                PlainTextContent = mensaje
            };
            var resultMail = await SendSimpleEmail(mailBody);
            res.Success = resultMail.Success;

            return res;
        }
    }
}
