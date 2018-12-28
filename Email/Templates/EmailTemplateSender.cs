using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Remax.Web.Server
{
    /// <summary>
    /// Handles sending templated emails
    /// </summary>
    public class EmailTemplateSender : IEmailTemplateSender
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="details"></param>
        /// <param name="title"></param>
        /// <param name="content1"></param>
        /// <param name="content2"></param>
        /// <param name="buttonText"></param>
        /// <param name="buttonUrl"></param>
        /// <returns></returns>
        public async Task<SendEmailResponse> SendGeneralEmailAsync(SendEmailDetails details, string title, string content1, string content2, string buttonText, string buttonUrl)
        {
            string templateText;

            // Read the general template from file
            // TODO: Replace with IoC Flat data provider
            using (var reader = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream("Remax.Web.Server.Email.Templates.GeneralTemplate.htm"), Encoding.UTF8))
            {
                // Read file contents
                templateText = await reader.ReadToEndAsync();
            }

            // Replace special values with those inside the template
            templateText = templateText.Replace("--Title--", title)
                                        .Replace("--Content1--", content1)
                                        .Replace("--Content2--", content2)
                                        .Replace("--ButtonText--", buttonText)
                                        .Replace("--ButtonUrl--", buttonUrl);

            // Set the details content to this template content
            details.Content = templateText;

            // Send email
            return await DI.EmailSender.SendEmailAsync(details);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="details"></param>
        /// <param name="title"></param>
        /// <param name="content1"></param>
        /// <param name="content2"></param>
        /// <param name="buttonText"></param>
        /// <param name="buttonUrl"></param>
        /// <returns></returns>
        public async Task<SendEmailResponse> SendCoworkingSpaceEmailAsync(SendEmailDetails details, string title, string content1, string content2, string buttonText, string buttonUrl)
        {
            string templateText;

            // Read the general template from file
            // TODO: Replace with IoC Flat data provider
            using (var reader = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream("Remax.Web.Server.Email.Templates.GeneralTemplate.htm"), Encoding.UTF8))
            {
                // Read file contents
                templateText = await reader.ReadToEndAsync();
            }

            // Replace special values with those inside the template
            templateText = templateText.Replace("--Title--", title)
                                        .Replace("--Content1--", content1)
                                        .Replace("--Content2--", content2)
                                        .Replace("--ButtonText--", buttonText)
                                        .Replace("--ButtonUrl--", buttonUrl);

            // Set the details content to this template content
            details.Content = templateText;

            // Send email
            return await DI.EmailSender.SendEmailAsync(details);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="details"></param>
        /// <param name="title"></param>
        /// <param name="content1"></param>
        /// <param name="content2"></param>
        /// <param name="content3"></param>
        /// <param name="buttonText"></param>
        /// <param name="buttonUrl"></param>
        /// <returns></returns>
        public async Task<SendEmailResponse> SendCoworkingMembershipConfirmationAsync(SendEmailDetails details, string title, string content1, string content2, string content3, string buttonText, string buttonUrl)
        {
            string templateText;

            // Read the general template from file
            // TODO: Replace with IoC Flat data provider
            using (var reader = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream("Remax.Web.Server.Email.Templates.CoworkingMembershipSummaryDetails.htm"), Encoding.UTF8))
            {
                // Read file contents
                templateText = await reader.ReadToEndAsync();
            }

            // Replace special values with those inside the template
            templateText = templateText.Replace("--Title--", title)
                                        .Replace("--Content1--", content1)
                                        .Replace("--Content2--", content2)
                                        .Replace("--Content3--", content3)
                                        .Replace("--ButtonText--", buttonText)
                                        .Replace("--ButtonUrl--", buttonUrl);

            // Set the details content to this template content
            details.Content = templateText;

            // Send email
            return await DI.EmailSender.SendEmailAsync(details);
        }
    }
}
