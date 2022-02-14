using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class SBIHealthRequest
    {
        public string secretKey { get; set; }
        public string apiKey { get; set; }
        public string baseURL { get; set; }
        public RequestHandler request;
        //  public RequestHandler(string stripeCompanyId, string stripeSecretKey)
        public SBIHealthRequest()
        {
            baseURL = System.Configuration.ConfigurationSettings.AppSettings["SBITokenService"].ToString();
            secretKey = System.Configuration.ConfigurationSettings.AppSettings["StarSecretKey"].ToString();
            apiKey = System.Configuration.ConfigurationSettings.AppSettings["StarAPIKey"].ToString();
            request = new RequestHandler();
        }
        public Dictionary<string, string> QuoteCreation(string postData)
        {
            string requestURL = baseURL + "/customers/v1/quotes";
            var result = request.Response(requestURL, postData);
            return result;
        }
    }
}
