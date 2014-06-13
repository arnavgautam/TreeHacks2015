namespace MobileService.Common.Providers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;

    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.WindowsAzure.Mobile.Service.Security;

    using NotesFor.HtmlToOpenXml;

    public class SharePointProvider
    {
        public static string SharePointUri { get; set; }

        public static async Task<string> RequestAccessToken(ServiceUser user, string authority, string resourceUrl, string activeDirectoryClientId, string activeDirectoryClientSecret)
        {
            // Get the current access token
            var credentials = (await user.GetIdentitiesAsync()).OfType<AzureActiveDirectoryCredentials>().FirstOrDefault();
            var accessToken = credentials.AccessToken;

            // Call ADAL and request a token to SharePoint with the access token
            var authenticationContext = new AuthenticationContext(authority);
            var authenticationResult = authenticationContext.AcquireToken(resourceUrl, new UserAssertion(accessToken), new ClientCredential(activeDirectoryClientId, activeDirectoryClientSecret));
            return authenticationResult.AccessToken;
        }

        public static async Task<bool> UploadFile(string sharepointUri, byte[] document, string token, string clientId)
        {
            using (var client = new HttpClient())
            {
                Func<HttpRequestMessage> requestCreator = () =>
                {
                    var uploadRequest = new HttpRequestMessage(HttpMethod.Post, sharepointUri)
                    {
                        Content = new ByteArrayContent(document)
                    };
                    uploadRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    uploadRequest.Content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));
                    return uploadRequest;
                };

                using (var uploadRequest = requestCreator.Invoke())
                {
                    uploadRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    uploadRequest.Headers.UserAgent.Add(new ProductInfoHeaderValue(clientId, string.Empty));
                    await client.SendAsync(uploadRequest);
                }
            }

            return true;
        }

        public static byte[] BuildDocument(dynamic facRequest)
        {
            const string Content = "<!DOCTYPE html>                                                                                                              "
                                   + "                                                                                                                            "
                                   + "<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">                                                                   "
                                   + "                                                                                                                            "
                                   + "<body style=\"font-family:'Segoe UI';font-weight:100;font-size:12pt;\">                                                     "
                                   + "                                                                                                                            "
                                   + "    <div style=\"width:800px;height:150px;display:inline-block;margin:auto;text-align:center;\">                            "
                                   + "        <div style=\"font-size:16pt;font-weight:400;\">                                                                     "
                                   + "            Facilities Request Form                                                                                         "
                                   + "        </div>                                                                                                              "
                                   + "    </div>                                                                                                                  "
                                   + "                                                                                                                            "
                                   + "    <br /><br />                                                                                                            "
                                   + "    <table width=\"600\">                                                                                                   "
                                   + "        <tr>                                                                                                                "
                                   + "            <td>Requested By:</td>                                                                                          "
                                   + "            <td>{0}</td>                                                                                                    "
                                   + "            <td>On:</td>                                                                                                    "
                                   + "            <td>{1}</td>                                                                                                    "
                                   + "        </tr>                                                                                                               "
                                   + "        <tr>                                                                                                                "
                                   + "            <td>Building:</td>                                                                                              "
                                   + "            <td>{2}</td>                                                                                                    "
                                   + "            <td>Room:</td>                                                                                                  "
                                   + "            <td>{3}</td>                                                                                                    "
                                   + "        </tr>                                                                                                               "
                                   + "            <tr>                                                                                                            "
                                   + "            <td colspan=\"2\"><img width='200' height='280' src='{6}'></td>                                                 "
                                   + "            <td colspan=\"2\"><img width='200' height='280' src='{7}'></td>                                                 "
                                   + "        </tr>                                                                                                               "
                                   + "    </table>                                                                                                                "
                                   + "                                                                                                                            "
                                   + "    <table width=\"600\">                                                                                                   "
                                   + "        <tr>                                                                                                                "
                                   + "            <td>Description:</td>                                                                                           "
                                   + "            <td>{4}</td>                                                                                                    "
                                   + "            <td>Service Notes:</td>                                                                                         "
                                   + "            <td>{5}</td>                                                                                                    "
                                   + "        </tr>                                                                                                               "
                                   + "    </table>                                                                                                                "
                                   + "                                                                                                                            "
                                   + "            <div style=\"text-align:center;\">                                                                              "
                                   + "                <div>Signed:_______________________________</div>                                                           "
                                   + "                <div>Date:_______________________________</div>                                                             "
                                   + "            </div>                                                                                                          "
                                   + "                                                                                                                            "
                                   + "</body>                                                                                                                     "
                                   + "</html>                                                                                                                     ";

            var html = string.Format(
                Content,
                facRequest.User,
                facRequest.RequestedDate.ToString("dd MMM HH:mm"),
                facRequest.Building,
                facRequest.Room,
                facRequest.ProblemDescription,
                facRequest.ServiceNotes,
                facRequest.BeforeImageUrl,
                facRequest.AfterImageUrl);

            return GenerateDocument(html);
        }

        private static byte[] GenerateDocument(string data)
        {
            byte[] document;
            using (var generatedDocument = new MemoryStream())
            {
                using (var package = WordprocessingDocument.Create(generatedDocument, WordprocessingDocumentType.Document))
                {
                    var mainPart = package.MainDocumentPart;
                    if (mainPart == null)
                    {
                        mainPart = package.AddMainDocumentPart();
                        new Document(new Body()).Save(mainPart);
                    }

                    var converter = new HtmlConverter(mainPart);
                    var body = mainPart.Document.Body;

                    var paragraphs = converter.Parse(data);
                    foreach (var t in paragraphs)
                    {
                        body.Append(t);
                    }

                    mainPart.Document.Save();
                }

                document = generatedDocument.ToArray();
            }

            return document;
        }
    }
}