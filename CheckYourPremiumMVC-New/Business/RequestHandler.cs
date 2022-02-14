using Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Security.Cryptography;
using System.Net.Security;

namespace Business
{
    public class RequestHandler
    {
        //public string companySecretKey { get; set; }
        //public string companyStripeAccountID { get; set; }
        //public string stripeURL { get; set; }
        public string secretKey { get; set; }
        public string apiKey { get; set; }
        public string baseURL { get; set; }
        //  public RequestHandler(string stripeCompanyId, string stripeSecretKey)
        public RequestHandler()
        {
            baseURL = System.Configuration.ConfigurationSettings.AppSettings["StarBaseURL"].ToString();
            secretKey = System.Configuration.ConfigurationSettings.AppSettings["StarSecretKey"].ToString();
            apiKey = System.Configuration.ConfigurationSettings.AppSettings["StarAPIKey"].ToString();
        }
        public Dictionary<string, string> Response(string url, string postData)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {

                postData = postData.Replace(",\"insureds\":\"\"", "");
                //  string authorization = "Bearer " + SecretKey;
                var data = Encoding.ASCII.GetBytes(postData);
              //  ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
               ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = "POST";
                //   request.Headers.Add("Authorization", authorization);
                request.ContentType = "application/json";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                if (url.Contains("sbi"))
                {
                    var token = GenerateSBIToken();
                    if (token.Contains("message"))
                    {
                        result.Add("Status", "400");
                        result.Add("response", token);
                        return result;
                    }

                    request.Headers.Add("X-IBM-Client-Id", "79de4de3-d258-43b8-a89a-f42924dddb46");
                    request.Headers.Add("X-IBM-Client-Secret", "lC0eU6gN3sM2mO3vY8xT2nS8sV0rM2xB4xL8uF1uD5lE8jJ1pY");
                    request.Headers.Add("Authorization", token);

                }
                request.UseDefaultCredentials = true;
                request.ContentLength = data.Length;

                using (var reqStream = request.GetRequestStream())
                    reqStream.Write(data, 0, data.Length);
                string responseString = string.Empty;
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                        responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    result.Add("Status", "200");
                    result.Add("response", responseString);
                    (new ErrorLog()).Error("dasd=", responseString);
                }
                catch (WebException e)
                {
                    (new ErrorLog()).Error(url, postData);
                    (new ErrorLog()).Error("getdetail-webexception=", e.Message.ToString());
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        result.Add("Status", "400");
                        using (Stream streamdata = response.GetResponseStream())
                        using (var reader = new StreamReader(streamdata))
                        {
                            string text = reader.ReadToEnd();

                            result.Add("response", text);
                        }
                    }

                }


                return result;
            }
            catch (Exception ex)
            {
                (new ErrorLog()).Error("getdetail-response=", ex.Message.ToString());
                result.Add("Status", "0");
                return result;
            }

        }
        public byte[] httpResponse(string url, string postData)
        {
            try
            {
                postData = postData.Replace(",\"insureds\":\"\"", "");
                //  string authorization = "Bearer " + SecretKey;
                var data = Encoding.ASCII.GetBytes(postData);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = "POST";
                //   request.Headers.Add("Authorization", authorization);
                request.ContentType = "application/json";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                request.UseDefaultCredentials = true;
                request.ContentLength = data.Length;

                using (var reqStream = request.GetRequestStream())
                    reqStream.Write(data, 0, data.Length);

                var response = (HttpWebResponse)request.GetResponse();

                var resByte = ReadFully(response.GetResponseStream());
                ///  var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return resByte;
            }
            catch (Exception ex)
            {
                byte[] res = new byte[1];
                return res;

            }

        }
        public byte[] ReadFully(Stream input)
        {
            try
            {
                int bytesBuffer = 1024;
                byte[] buffer = new byte[bytesBuffer];
                using (MemoryStream ms = new MemoryStream())
                {
                    int readBytes;
                    while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, readBytes);
                    }
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                // Exception handling here:  Response.Write("Ex.: " + ex.Message);
            }
            return (new MemoryStream()).ToArray();
        }
        public Dictionary<string, string> GetResponse(string url)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            try
            {
                //  string authorization = "Bearer " + SecretKey;
                //   var data = Encoding.ASCII.GetBytes(postData);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = "Get";
                //   request.Headers.Add("Authorization", authorization);
                request.ContentType = "application/json";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                request.UseDefaultCredentials = true;
                //  request.ContentLength = data.Length;

                //  using (var reqStream = request.GetRequestStream())
                //    reqStream.Write(data, 0, data.Length);
                string responseString = string.Empty;
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                        responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    result.Add("Status", "200");
                    result.Add("response", responseString);
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        result.Add("Status", "400");
                        using (Stream streamdata = response.GetResponseStream())
                        using (var reader = new StreamReader(streamdata))
                        {
                            string text = reader.ReadToEnd();

                            result.Add("response", text);
                        }
                    }

                }


                return result;

            }
            catch (Exception ex)
            {
                result.Add("Status", "0");
                return result;
            }

        }

        public Dictionary<string, string> GenerateToken(string postData, string refId)
        {
            string requestURL = baseURL + "/policy/proposals/" + refId + "/token";
            var result = Response(requestURL, postData);
            return result;
        }
        //public string GenerateToken(string postData, string refId)
        //{
        //    string requestURL = baseURL + "/policy/proposals/" + refId + "/token";
        //    var result = Response(requestURL, postData);
        //    return result;
        //}
        public Dictionary<string, string> PaymentGatewayRedirection(string postData, string redirectToken)
        {

            string requestURL = "http://igsan.starhealth.in/policy/proposals/purchase/" + redirectToken;
            var result = Response(requestURL, postData);
            return result;
        }
        public Dictionary<string, string> PurchaseStatus(string postData, string purchaseToken)
        {
            string requestURL = baseURL + "/policy/proposals/" + purchaseToken + "/purchase/response";
            var result = Response(requestURL, postData);
            return result;
        }
        public Dictionary<string, string> PolicyStatus(string postData, string refId)
        {

            string requestURL = baseURL + "/policy/proposals/" + refId + "/policystatus";
            var result = Response(requestURL, postData);
            return result;
        }
        public byte[] PolicySchedule(string postData, string refId)
        {

            string requestURL = baseURL + "/policy/proposals/" + refId + "/schedule";
            var result = httpResponse(requestURL, postData);
            return result;
        }

        public Dictionary<string, string> GetCityId(string pinCode)
        {
            string requestURL = baseURL + "/policy/city/details?APIKEY=" + apiKey + "&SECRETKEY=" + secretKey + "&pincode=" + pinCode;
            var result = GetResponse(requestURL);
            return result;
        }
        public Dictionary<string, string> GetAreaId(string pinCode, string cityId)
        {
            string requestURL = baseURL + "/policy/address/details?APIKEY=" + apiKey + "&SECRETKEY=" + secretKey + "&pincode=" + pinCode + "&city_id=" + cityId;
            var result = GetResponse(requestURL);
            return result;
        }
        public String GenerateSBIToken()
        {
            string result = string.Empty;
            try
            {
                Certificates.Instance.GetCertificatesAutomatically();
                var SBIBaseURL = System.Configuration.ConfigurationSettings.AppSettings["SBITokenService"].ToString();
                //  SBIBaseURL = SBIBaseURL.Replace("https", "http");
                //  string authorization = "Bearer " + SecretKey;
                var data = Encoding.ASCII.GetBytes("");
                //  ServicePointManager.Expect100Continue = true;
                //   ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                var request = (HttpWebRequest)WebRequest.Create(SBIBaseURL + "/v1/tokens");
                request.Method = "GET";
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                request.ProtocolVersion = HttpVersion.Version10;
                
               //X509Store  store2= new X509Store("MY", StoreLocation.LocalMachine);

             //  store2.Open(OpenFlags.ReadWrite);
              //  ServicePointManager.Expect100Continue = true;
              //  ServicePointManager.SecurityProtocol = (SecurityProtocolType)192
              //| (SecurityProtocolType)768
              //| (SecurityProtocolType)3072
              //| (SecurityProtocolType)48;
             //   ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
               //  ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12(#406);
              //ServicePointManager.SecurityProtocol=(SecurityProtocolType)(406);
                 //| SecurityProtocolType.Ssl3;
               // ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                //System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                //ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
              //  ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                //   request.UseDefaultCredentials = true;
                // request.ContentLength = data.Length;
                request.Headers.Add("X-IBM-Client-Id", "79de4de3-d258-43b8-a89a-f42924dddb46");
                request.Headers.Add("X-IBM-Client-Secret", "lC0eU6gN3sM2mO3vY8xT2nS8sV0rM2xB4xL8uF1uD5lE8jJ1pY");
                //using (var reqStream = request.GetRequestStream())
                //    reqStream.Write(data, 0, data.Length);
                string responseString = string.Empty;
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                        responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var tokenList = JsonConvert.DeserializeObject<SBIToken>(responseString);
                    result = tokenList.accesstoken;
                }
                catch (WebException e)
                {
                    result = "message-" + e.Message;

                }


                return result;
            }
            catch (Exception ex)
            {

                return result;
            }

        }
        public sealed class Certificates
        {
            private static Certificates instance = null;
            private static readonly object padlock = new object();

            Certificates()
            {
            }

            public static Certificates Instance
            {
                get
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Certificates();
                        }
                        return instance;
                    }
                }
            }
            public void GetCertificatesAutomatically()
            {
                ServicePointManager.ServerCertificateValidationCallback +=
                    new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors)
                        => { return true; });
                
            }

            private static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                //Return true if the server certificate is ok
                if (sslPolicyErrors == SslPolicyErrors.None)
                    return true;

                bool acceptCertificate = true;
                string msg = "The server could not be validated for the following reason(s):\r\n";

                //The server did not present a certificate
                if ((sslPolicyErrors &
                    SslPolicyErrors.RemoteCertificateNotAvailable) == SslPolicyErrors.RemoteCertificateNotAvailable)
                {
                    msg = msg + "\r\n    -The server did not present a certificate.\r\n";
                    acceptCertificate = false;
                }
                else
                {
                    //The certificate does not match the server name
                    if ((sslPolicyErrors &
                        SslPolicyErrors.RemoteCertificateNameMismatch) == SslPolicyErrors.RemoteCertificateNameMismatch)
                    {
                        msg = msg + "\r\n    -The certificate name does not match the authenticated name.\r\n";
                        acceptCertificate = false;
                    }

                    //There is some other problem with the certificate
                    if ((sslPolicyErrors &
                        SslPolicyErrors.RemoteCertificateChainErrors) == SslPolicyErrors.RemoteCertificateChainErrors)
                    {
                        foreach (X509ChainStatus item in chain.ChainStatus)
                        {
                            if (item.Status != X509ChainStatusFlags.RevocationStatusUnknown &&
                                item.Status != X509ChainStatusFlags.OfflineRevocation)
                                break;

                            if (item.Status != X509ChainStatusFlags.NoError)
                            {
                                msg = msg + "\r\n    -" + item.StatusInformation;
                                acceptCertificate = false;
                            }
                        }
                    }
                }

                //If Validation failed, present message box
                if (acceptCertificate == false)
                {
                    msg = msg + "\r\nDo you wish to override the security check?";
                    //          if (MessageBox.Show(msg, "Security Alert: Server could not be validated",
                    //                       MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    acceptCertificate = true;
                }

                return acceptCertificate;
            }

        }
        public String GenerateGodigitToken()
        {
            string result = string.Empty;
            try
            {
                var SBIBaseURL = System.Configuration.ConfigurationSettings.AppSettings["GodigitBaseURL"].ToString();
                var data = Encoding.ASCII.GetBytes("");
                var request = (HttpWebRequest)WebRequest.Create(SBIBaseURL);
                request.Method = "Post";
                request.ProtocolVersion = HttpVersion.Version10;
               ServicePointManager.Expect100Continue = true;
               ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
              request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
              request.Headers.Add("Authorization", "0A9RLAJ5LW5J4M9V1IMP89AUV6SKN7XN");
                string responseString = string.Empty;
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                        responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var tokenList = JsonConvert.DeserializeObject<SBIToken>(responseString);
                    result = tokenList.accesstoken;
                }
                catch (WebException e)
                {
                    result = "message-" + e.Message;

                }


                return result;
            }
            catch (Exception ex)
            {

                return result;
            }

        }
          public Dictionary<string, string> GodigitResponse(string url, string postData)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {

                postData = postData.Replace(",\"insureds\":\"\"", "");
                postData=postData.Replace("'","\"");
                //  string authorization = "Bearer " + SecretKey;
                var data = Encoding.ASCII.GetBytes(postData);
                //  ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = "POST";
                request.Headers.Add("Authorization", "7EG5H388WXNA7M3IK87ES11XNH6V1X47");
                request.ContentType = "application/json";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
               
                request.UseDefaultCredentials = true;
                request.ContentLength = data.Length;

                using (var reqStream = request.GetRequestStream())
                    reqStream.Write(data, 0, data.Length);
                string responseString = string.Empty;
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                        responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    result.Add("Status", "200");
                    result.Add("response", responseString);
                    (new ErrorLog()).Error("dasd=", responseString);
                }
                catch (WebException e)
                {
                    (new ErrorLog()).Error(url, postData);
                    (new ErrorLog()).Error("getdetail-webexception=", e.Message.ToString());
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        result.Add("Status", "400");
                        using (Stream streamdata = response.GetResponseStream())
                        using (var reader = new StreamReader(streamdata))
                        {
                            string text = reader.ReadToEnd();

                            result.Add("response", text);
                        }
                    }

                }


                return result;
            }
            catch (Exception ex)
            {
                (new ErrorLog()).Error("getdetail-response=", ex.Message.ToString());
                result.Add("Status", "0");
                return result;
            }

        }
          //pppppppppppp
          public Dictionary<string, string> GodigitPayment(string transactionNumber)
          {
              Dictionary<string, string> result = new Dictionary<string, string>();
              try
              {
                  Certificates.Instance.GetCertificatesAutomatically();
                  var SBIBaseURL = "http://preprod-digitpolicyissuance.godigit.com/policyservice/v1/getTransactionDetails?transactionNo=" + transactionNumber;
                  var data = Encoding.ASCII.GetBytes("");
                  var request = (HttpWebRequest)WebRequest.Create(SBIBaseURL);
                  request.Method = "GET";
                  request.ProtocolVersion = HttpVersion.Version10;
                  request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                  request.Headers.Add("Authorization", "39KYZNSWRFW92KSXOPDXM61985DD5C48");
                  
                  string responseString = string.Empty;
                  try
                  {
                      HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                      if (response.StatusCode == HttpStatusCode.OK)
                          responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                      result.Add("Status", "200");
                      result.Add("response", responseString);
                  }
                  catch (WebException e)
                  {
                      using (WebResponse response = e.Response)
                      {
                          HttpWebResponse httpResponse = (HttpWebResponse)response;
                          Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                          result.Add("Status", "400");
                          using (Stream streamdata = response.GetResponseStream())
                          using (var reader = new StreamReader(streamdata))
                          {
                              string text = reader.ReadToEnd();

                              result.Add("response", text);
                          }
                      }

                  }


                  return result;
              }
              catch (Exception ex)
              {

                  return result;
              }

          }


          public Dictionary<string, string> GodigitPolicyPDF(string url, string postData)
          {
              Dictionary<string, string> result = new Dictionary<string, string>();
              try
              {

                  postData = postData.Replace(",\"insureds\":\"\"", "");
                  postData = postData.Replace("'", "\"");
                  //  string authorization = "Bearer " + SecretKey;
                  var data = Encoding.ASCII.GetBytes(postData);
                  //  ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                  ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                  var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                  request.Method = "POST";
                  request.Headers.Add("Authorization", "E574A5D92B10E69CB65CA8A5CEE17013");//ppppppp change code only
                  request.ContentType = "application/json";
                  request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                  request.UseDefaultCredentials = true;
                  request.ContentLength = data.Length;

                  using (var reqStream = request.GetRequestStream())
                      reqStream.Write(data, 0, data.Length);
                  string responseString = string.Empty;
                  try
                  {
                      HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                      if (response.StatusCode == HttpStatusCode.OK)
                          responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                      result.Add("Status", "200");
                      result.Add("response", responseString);
                      (new ErrorLog()).Error("dasd=", responseString);
                  }
                  catch (WebException e)
                  {
                      (new ErrorLog()).Error(url, postData);
                      (new ErrorLog()).Error("getdetail-webexception=", e.Message.ToString());
                      using (WebResponse response = e.Response)
                      {
                          HttpWebResponse httpResponse = (HttpWebResponse)response;
                          Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                          result.Add("Status", "400");
                          using (Stream streamdata = response.GetResponseStream())
                          using (var reader = new StreamReader(streamdata))
                          {
                              string text = reader.ReadToEnd();

                              result.Add("response", text);
                          }
                      }

                  }


                  return result;
              }
              catch (Exception ex)
              {
                  (new ErrorLog()).Error("getdetail-response=", ex.Message.ToString());
                  result.Add("Status", "0");
                  return result;
              }

          }
        //ppppppppppp
    }
}
