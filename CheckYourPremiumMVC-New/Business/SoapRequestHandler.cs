using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;
using System.Globalization;
namespace Business
{
    public class SoapRequestHandler
    {
        public ITermLifeBusiness _ITermLifeBusiness;
        public static void SubmitSoapRequestbhartiAXATravel(string requestFor)
        {
            string requestBaseURL = string.Empty;
            string soapAction = string.Empty;

            switch (requestFor.ToLower())
            {
                case "bhartiAXATravel":
                    //requestBaseURL = ConfigurationSettings.AppSettings["BhartiAXABaseURL"].ToString();
                    //soapAction = ConfigurationSettings.AppSettings["BhartiAXASoapAction"].ToString();
                    break;
                case "indiafirst":
                    requestBaseURL = ConfigurationSettings.AppSettings["IndiaFirstSoapAction"].ToString();
                    soapAction = "http://oracle.com/determinations/server/12.2.1/rulebase/types/Assess";
                    break;
                default:

                    break;
            }
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestBaseURL);
            req.Headers.Add("SOAPAction", soapAction);
            req.Headers.Add("Username", "apiuser");
            req.Headers.Add("Password", "ApiUser@123");

            req.Method = "POST"; // Post method
            req.ContentType = "text/xml";// content type

            req.KeepAlive = false;
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
            req.ProtocolVersion = HttpVersion.Version10;
            req.PreAuthenticate = true;

            String XML = "";// GetXML();

            byte[] buffer = Encoding.ASCII.GetBytes(XML);

            req.ContentLength = buffer.Length;

            // Wrap the request stream with a text-based writer

            Stream writer = req.GetRequestStream();

            // Write the XML text into the stream

            writer.Write(buffer, 0, buffer.Length);

            writer.Close();


            try
            {

                WebResponse rsp = req.GetResponse();
                StreamReader responseStream = new StreamReader(rsp.GetResponseStream());
                var result = responseStream.ReadToEnd();


            }
            catch (WebException wex)
            {
                var pageContent = new StreamReader(wex.Response.GetResponseStream())
                                      .ReadToEnd();
            }


        }
        public static string SubmitSoapRequest(string ageSelf, string Gender, string Smoke, string fullName, string DOB, string Email, string phone, string sumAssured, string POLICY_TERM, string Frequency, string city, string PemiumType, string riskoption, string Income)
        {
            string url = "";
            string policy_type = "";


            int age = Convert.ToInt32(ageSelf);
            int sumasurred = Convert.ToInt32(sumAssured);
            if (sumasurred < 5000000)
            {
             //   url = "https://iflictest1.custhelp.com/determinations-server/assess/soap/generic/12.2.1/IFL__Anytime__Plan?wsdl";
                url = "https://iflicproduction.custhelp.com/iflicproduction/determinations-server/assess/soap/generic/12.2.1/IFLAnytimePlan?wsdl";
            }
            else
            {
                //url = "https://iflictest1.custhelp.com/determinations-server/assess/soap/generic/12.2.1/IFLOnlineTermPlan?wsdl";
                url = " https://iflicproduction.custhelp.com/iflicproduction/determinations-server/assess/soap/generic/12.2.1/IFLOnlineTermPlan?wsdl";
            }
            ITermLifeBusiness _ITermLifeBusiness = new TermLifeBusiness();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            //HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://iflictest1.custhelp.com/determinations-server/assess/soap/generic/12.2.1/IFLOnlineTermPlan?wsdl");
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Headers.Add("SOAPAction", "http://oracle.com/determinations/server/12.2.1/rulebase/types/Assess");
            req.Headers.Add("Username", "apiuser");
            req.Headers.Add("Password", "ApiUser@123");
            req.Method = "POST"; // Post method
            req.ContentType = "text/xml";// content type
            req.KeepAlive = false;
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
            req.ProtocolVersion = HttpVersion.Version10; req.PreAuthenticate = true;
            String XML = GetXML(ageSelf, Gender, Smoke, fullName, DOB, Email, phone, sumAssured, POLICY_TERM, Frequency, city, PemiumType, riskoption, Income);
            byte[] buffer = Encoding.ASCII.GetBytes(XML);
            req.ContentLength = buffer.Length;
            Stream writer = req.GetRequestStream();
            writer.Write(buffer, 0, buffer.Length);
            writer.Close();
            try
            {
                WebResponse rsp = req.GetResponse();
                StreamReader responseStream = new StreamReader(rsp.GetResponseStream());
                var result = responseStream.ReadToEnd();

                return result;



            }
            catch (WebException wex)
            {
                return new StreamReader(wex.Response.GetResponseStream())
                                      .ReadToEnd();
            }
        }

        public static string GetXML(string ageSelf, string Gender, string Smoke, string fullName, string DOB, string Email, string phone, string sumAssured, string POLICY_TERM, string Frequency, string City, string PemiumType, string riskoption, string Income)
        {
            try
            {
                int POLICYt = Convert.ToInt32(POLICY_TERM);
                if (Smoke == "Yes")
                {
                    Smoke = "true";
                }
                else if (Smoke == "No")
                {
                    Smoke = "false";
                }
                string sm = "";
                string pre = "";
                if (PemiumType == "Single")
                {
                    Frequency = "Yearly";
                    pre = "SP";
                }
                else if (PemiumType == "Regular")
                {
                    pre = "RP";
                }

                if (Smoke == "true")
                {
                    sm = "S";
                }
                else
                {
                    sm = "NS";
                }
                //..........................
                string url = "";
                int age = Convert.ToInt32(ageSelf);
                int sumasurred = Convert.ToInt32(sumAssured);
                if (POLICYt <= 40)
                {
                    if (POLICYt >= 5)
                    {
                        if (sumasurred < 5000000)
                        {

                            return @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:typ='http://oracle.com/determinations/server/12.2.1/rulebase/assess/types'>
   <soapenv:Header>
      <o:Security soapenv:mustUnderstand='1' xmlns:o='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'>
         <o:UsernameToken>
            <o:Username>apiuser</o:Username>
            <o:Password>ApiUser@123</o:Password>
         </o:UsernameToken>
      </o:Security>
   </soapenv:Header>
   <soapenv:Body>
      <typ:assess-request>
         <typ:config>
            <typ:outcome>
               <typ:entity id='global'>
                  <typ:attribute-outcome id='LA_age' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
                  <typ:attribute-outcome id='policyholder_age' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
                  <typ:attribute-outcome id='prem_pay_term' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
                  <typ:attribute-outcome id='maturity_age' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
                  <typ:attribute-outcome id='annual_prem' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
                  <typ:attribute-outcome id='instalment_prem' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
                  <typ:attribute-outcome id='instal_prem_with_ST' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
               </typ:entity>
            </typ:outcome>
         </typ:config>
         <typ:global-instance>
            <typ:attribute id='LA_name'>
               <typ:text-val>" + fullName + @"</typ:text-val>
            </typ:attribute>
         	  <typ:attribute id='LA_DOB'>
               <typ:date-val>" + Convert.ToDateTime(DOB).ToString("yyyy-MM-dd") + @"</typ:date-val>
            </typ:attribute>
            <typ:attribute id='policyholder_name'>
               <typ:text-val>" + fullName + @"</typ:text-val>
            </typ:attribute>
            <typ:attribute id='policyholder_DOB'>
               <typ:date-val>" + Convert.ToDateTime(DOB).ToString("yyyy-MM-dd") + @"</typ:date-val>
            </typ:attribute>
            <typ:attribute id='advisor_name'>
               <typ:text-val>" + fullName + @"</typ:text-val>
            </typ:attribute>
            <typ:attribute id='LA_gender'>
               <typ:text-val>" + Gender + @"</typ:text-val>
            </typ:attribute>
         	  <typ:attribute id='prem_type'>
               <typ:text-val>" + PemiumType + @"</typ:text-val>
            </typ:attribute>
            <typ:attribute id='smoker_indicator'>
               <typ:text-val>" + sm + @"</typ:text-val>
            </typ:attribute>
            <typ:attribute id='prem_freq'>
               <typ:text-val>" + Frequency + @"</typ:text-val>
            </typ:attribute>
            <typ:attribute id='sum_assured'>
               <typ:number-val>" + sumAssured + @"</typ:number-val>
            </typ:attribute>
            <typ:attribute id='policy_term'>
               <typ:number-val>" + POLICY_TERM + @"</typ:number-val>
            </typ:attribute>
			<typ:attribute id='the_state'>
               <typ:text-val>" + City + @"</typ:text-val>
            </typ:attribute>
          </typ:global-instance>   
      </typ:assess-request>
   </soapenv:Body>
</soapenv:Envelope>";
                        }
                        else
                        {
                            if (POLICYt >= 10)
                            {
                                if (Gender == "Male") { Gender = "MALE"; }
                                else if (Gender == "Female") { Gender = "FEMALE"; }
                                else if (Gender == "Transgender") { Gender = "TRANSGENDER"; }
                                if (Frequency == "Yearly") { Frequency = "1"; }
                                else if (Frequency == "Half-Yearly") { Frequency = "2"; }
                                else if (Frequency == "Quarterly") { Frequency = "4"; }
                                else if (Frequency == "Monthly") { Frequency = "12"; }
                                return @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:typ='http://oracle.com/determinations/server/12.2.1/rulebase/assess/types'>
   <soapenv:Header> 	<o:Security soapenv:mustUnderstand='1' xmlns:o='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'>
			<o:UsernameToken>
				<o:Username>apiuser</o:Username>
				<o:Password>ApiUser@123</o:Password>
			</o:UsernameToken>
		</o:Security>
</soapenv:Header>
   <soapenv:Body>
      <typ:assess-request>
         <!--Optional:-->
        <typ:config>
				<typ:outcome>
					<typ:entity id='global'>
						<typ:attribute-outcome id='RISK1_ANNUAL_GROSS_PREMIUM' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK1_INSTALL_PREMIUM_BEFORETAX' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK1_INSTALL_PREMIUM_TAXINCLUSIVE' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK3_ANNUAL_GROSS_PREMIUM' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK3_INSTALL_PREMIUM_BEFORETAX' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK3_INSTALL_PREMIUM_TAXINCLUSIVE' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK4_ANNUAL_GROSS_PREMIUM' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK4_INSTALL_PREMIUM_BEFORETAX' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK4_INSTALL_PREMIUM_TAXINCLUSIVE' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>	
						<typ:attribute-outcome id='RISK5_ANNUAL_GROSS_PREMIUM' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK5_INSTALL_PREMIUM_BEFORETAX' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK5_INSTALL_PREMIUM_TAXINCLUSIVE' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK6_ANNUAL_GROSS_PREMIUM' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK6_INSTALL_PREMIUM_BEFORETAX' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK6_INSTALL_PREMIUM_TAXINCLUSIVE' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK7_ANNUAL_GROSS_PREMIUM' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK7_INSTALL_PREMIUM_BEFORETAX' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK7_INSTALL_PREMIUM_TAXINCLUSIVE' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK8_ANNUAL_GROSS_PREMIUM' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK8_INSTALL_PREMIUM_BEFORETAX' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
						<typ:attribute-outcome id='RISK8_INSTALL_PREMIUM_TAXINCLUSIVE' known-outcome-style='value-only' unknown-outcome-style='decision-report'/>
					</typ:entity>				
				</typ:outcome>
			</typ:config>
			<typ:global-instance>
				<!-- Input Attributes -->
				<typ:attribute id='RISK_OPTION'>
					<number-val>" + riskoption + @"</number-val>
				</typ:attribute>
				<typ:attribute id='PREMIUM_TYPE'>
					<text-val>" + pre + @"</text-val>
				</typ:attribute>
				<typ:attribute id='GENDER'>
					<text-val>" + Gender + @"</text-val>
				</typ:attribute>
				<typ:attribute id='SMOKE_INDICATOR'>
					<boolean-val>" + Smoke + @"</boolean-val>
				</typ:attribute>
				<typ:attribute id='AGE_AT_ENTRY'>
					<number-val>" + ageSelf + @"</number-val>
				</typ:attribute>
				<typ:attribute id='PREMIUM_FREQ'>
					<number-val>" + Frequency + @"</number-val>
				</typ:attribute>
				<typ:attribute id='POLICY_TERM'>
					<number-val>" + POLICY_TERM + @"</number-val>
				</typ:attribute>
				<typ:attribute id='SUM_ASSURED'>
					<number-val>" + sumAssured + @"</number-val>
				</typ:attribute>
				<typ:attribute id='CURRENT_INCOME'>
					<number-val>" + Income + @"</number-val>
				</typ:attribute>	
				<typ:attribute id='THE_STATE'>
					<text-val>" + City + @"</text-val>
				</typ:attribute>

			</typ:global-instance>
            <!--Zero or more repetitions:-->
          
      </typ:assess-request>
   </soapenv:Body>
</soapenv:Envelope>";//reader.ReadToEnd();
                            }
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }

        public static string SubmitSoapRequestForHDFC(string premiumWaiver, string ageSelf, string Gender, string Smoke, string fullName, string DOB, string Email, string phone, string sumAssured, string POLICY_TERM, string Frequency, string city)
        {
            ITermLifeBusiness _ITermLifeBusiness = new TermLifeBusiness();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://soauat2.hdfclife.com/TEBT_QuoteGenerationWeb/sca/QuoteGeneration_ThirdPartyExport");
            // req.Headers.Add("SOAPAction", "http://oracle.com/determinations/server/12.2.1/rulebase/types/Assess");
            //req.Headers.Add("Username", "apiuser");
            //req.Headers.Add("Password", "ApiUser@123");
            req.Method = "POST"; // Post method
            req.ContentType = "text/xml";// content type
            req.KeepAlive = false;
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
            req.ProtocolVersion = HttpVersion.Version10; req.PreAuthenticate = true;
            String XML = GetXMLHDFC(premiumWaiver, ageSelf, Gender, Smoke, fullName, DOB, Email, phone, sumAssured, POLICY_TERM, Frequency, city);
            byte[] buffer = Encoding.ASCII.GetBytes(XML);
            req.ContentLength = buffer.Length;
            Stream writer = req.GetRequestStream();
            writer.Write(buffer, 0, buffer.Length);
            writer.Close();
            try
            {

                WebResponse rsp = req.GetResponse();
                StreamReader responseStream = new StreamReader(rsp.GetResponseStream());
                var result = responseStream.ReadToEnd();

                return result;



            }

            catch (WebException wex)
            {
                return new StreamReader(wex.Response.GetResponseStream())
                                      .ReadToEnd();
            }
        }

        public static string GetXMLHDFC(string premiumWaiver, string ageSelf, string Gender, string Smoke, string fullName, string DOB, string Email, string phone, string sumAssured, string POLICY_TERM, string Frequency, string city)
        {
            try
            {
                if (Smoke == "true")
                {
                    Smoke = "Y";
                }
                else
                {
                    Smoke = "N";
                }

                if (Gender == "Male") { Gender = "MALE"; } else if (Gender == "Female") { Gender = "Female"; }
                if (Frequency == "Yearly") { Frequency = "FREQ_1"; }
                else if (Frequency == "Half-Yearly") { Frequency = "FREQ_2"; }
                else if (Frequency == "Quarterly") { Frequency = "FREQ_3"; }
                else if (Frequency == "Monthly") { Frequency = "FREQ_4"; }
                else if (Frequency == "Single") { Frequency = "FREQ_5"; }
                string pumr = "";
                if (Frequency == "Monthly")
                {
                    pumr = "1";
                }
                if (premiumWaiver == "" || premiumWaiver == null)
                {
                    premiumWaiver = "Life";
                }
                string fullN = fullName + "";
                var names = fullN.Split(' ');
                string lastName = "";
                string firstName = "";
                if (names.Length == 1)
                {
                    firstName = names[0];
                    lastName = "test";
                }
                else
                {
                    firstName = names[0];
                    lastName = names[1];
                }


                //return @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:quot='http://TEBT_QuoteGeneration/QuoteGeneration_TP'> <soapenv:Header/> <soapenv:Body> <quot:generatequote> <generatequoteReq> <head> <source>OCP_ONLINE</source> <userid>pindianbank1</userid> <txnid>C2P3D_Life_Option</txnid> </head> <body> <quotedtls> <incpDt>17/02/2020</incpDt> <qtDt>17/02/2020</qtDt> <prmmulfactor>1</prmmulfactor> <jnk>N</jnk> <nri>N</nri> <chnlpartner>Online</chnlpartner> <premium>0</premium> <sumAssured>55000000</sumAssured> <term>10</term> <ppt>10</ppt> <freq>FREQ_0.507</freq> <paymethod>MOP_ONLINE</paymethod> <touchpoint>OCP</touchpoint> <product>C2P3DPV9ER</product> <option>Life</option> <qtstatus>FINALIZED</qtstatus> <tobstatus>N</tobstatus> <isInsured>Y</isInsured> <qniproduct>C2P3DP</qniproduct> <lifeassured> <fname>Termlife</fname> <lname>testing</lname> <dob>01/01/1985</dob> <gender>GEN_M</gender> <email>Termlife@tester.com</email> <mobnopre>91</mobnopre> <mobno>9999999999</mobno> <residentstatus>RESS_RI</residentstatus> <state>MAHA</state> <city>CTY_MUMB21</city> </lifeassured> <pptOption>REGULAR</pptOption> <topupFlag>Y</topupFlag> <topupPercentage>10</topupPercentage> <proposerBuyingForItself>Yes</proposerBuyingForItself> <isproposer>Y</isproposer> </quotedtls> <outputFmt>JSON</outputFmt> <totpremium>0</totpremium> <agentcd>00399206</agentcd> <opsjourney>LG</opsjourney> <generateonlyflag>N</generateonlyflag> <opssource>C2P3D</opssource> </body> </generatequoteReq> </quot:generatequote> </soapenv:Body> </soapenv:Envelope>";
                return @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:quot='http://TEBT_QuoteGeneration/QuoteGeneration_TP'> <soapenv:Header/> <soapenv:Body> <quot:generatequote> <generatequoteReq> <head> <source>OCP_ONLINE</source> <userid>pindianbank1</userid> <txnid>C2P3D_Life_Option</txnid> </head> <body> <quotedtls> <incpDt>17/02/2020</incpDt> <qtDt>17/02/2020</qtDt> <prmmulfactor>" + pumr + @"</prmmulfactor> <jnk>N</jnk> <nri>N</nri> <chnlpartner>Online</chnlpartner> <premium>0</premium> <sumAssured>" + sumAssured + @"</sumAssured> <term>" + POLICY_TERM + @"</term> <ppt>" + POLICY_TERM + @"</ppt> <freq>" + Frequency + @"</freq> <paymethod>MOP_ONLINE</paymethod> <touchpoint>OCP</touchpoint> <product>C2P3DPV9ER</product> <option>" + premiumWaiver + @"</option> <qtstatus>FINALIZED</qtstatus> <tobstatus>" + Smoke + @"</tobstatus> <isInsured>Y</isInsured> <qniproduct>C2P3DP</qniproduct> <lifeassured> <fname>" + firstName + @"</fname> <lname>" + lastName + @"</lname> <dob>" + DOB + @"</dob> <gender>" + Gender + @"</gender> <email>" + Email + @"</email> <mobnopre>91</mobnopre> <mobno>" + phone + @"</mobno> <residentstatus>RESS_RI</residentstatus> <state>MAHA</state> <city>CTY_MUMB21</city> </lifeassured> <pptOption>REGULAR</pptOption> <topupFlag>Y</topupFlag> <topupPercentage>10</topupPercentage> <proposerBuyingForItself>Yes</proposerBuyingForItself> <isproposer>Y</isproposer> </quotedtls> <outputFmt>JSON</outputFmt> <totpremium>0</totpremium> <agentcd>00399206</agentcd> <opsjourney>LG</opsjourney> <generateonlyflag>N</generateonlyflag> <opssource>C2P3D</opssource> </body> </generatequoteReq> </quot:generatequote> </soapenv:Body> </soapenv:Envelope>";
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
         return  string.Empty; 
        }
        //ICICI pppppppppppppppppppppppppppppppppppppp
        public static string SubmitSoapRequestForICICI(string ageSelf, string Gender, string Smoke, string fullName, string DOB, string Email, string phone, string sumAssured, string POLICY_TERM, string Frequency, string city, string PemiumType, string riskoption, string Income)
        {
            string url = "";
            string policy_type = "";

            if (Gender == "Female")
            {
                Gender = "Female";
            }
            else
            {
                Gender = "Male";
            }
            if (Smoke == "Yes")
            {
                Smoke = "1";
            }
            else
            {
                Smoke = "0";
            }
            int age = Convert.ToInt32(ageSelf);


            url = "https://api.iciciprulife.com/quote";

            ITermLifeBusiness _ITermLifeBusiness = new TermLifeBusiness();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            //  req.Headers.Add("SOAPAction", "https://api.iciciprulife.com/quote");
            //    req.Headers.Add("Username", "apiuser");
            req.Headers.Add("x-api-key", "czYomLHSyTW1iqnKr6ADvyiz7ks5v6nq");
            req.Method = "POST"; // Post method
            req.ContentType = "text/xml";// content type x-api-key
            req.KeepAlive = false;
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
            req.ProtocolVersion = HttpVersion.Version10; req.PreAuthenticate = true;
            String XML = GetXMLICICI(ageSelf, Gender, Smoke, fullName, DOB, Email, phone, sumAssured, POLICY_TERM, Frequency, city, PemiumType, riskoption, Income);
            byte[] buffer = Encoding.ASCII.GetBytes(XML);
            req.ContentLength = buffer.Length;
            Stream writer = req.GetRequestStream();
            writer.Write(buffer, 0, buffer.Length);
            writer.Close();
            try
            {
                WebResponse rsp = req.GetResponse();
                StreamReader responseStream = new StreamReader(rsp.GetResponseStream());
                var result = responseStream.ReadToEnd();
                string RESULT1 = result.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
                string Result2 = RESULT1.Replace("soap:Envelope xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><soap:Body>", "");

                return Result2;



            }
            catch (WebException wex)
            {
                return new StreamReader(wex.Response.GetResponseStream())
                                      .ReadToEnd();
            }
        }
        public static string GetXMLICICI(string ageSelf, string Gender, string Smoke, string fullName, string DOB, string Email, string phone, string sumAssured, string POLICY_TERM, string Frequency, string City, string PemiumType, string riskoption, string Income)
        {
            try
            {

                string url = "";
                int age = Convert.ToInt32(ageSelf);
                string fullN = fullName + "";
                var names = fullN.Split(' ');
                string lastName = "";
                string firstName = "";
                if (names.Length == 1)
                {
                    firstName = names[0];
                    lastName = "test";
                }
                else
                {
                    firstName = names[0];
                    lastName = names[1];
                }
                string date = DateTime.ParseExact(DOB, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                //DateTime sourceDate = DateTime.Parse(DOB);
                //  string formatted = sourceDate.ToString("yyyy-MM-dd");
                //var dob = DateTime.ParseExact(DOB, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                //var dob2 = DateTime.Now.AddYears(Convert.ToInt32("-" + DOB.ToString())).ToString("MMMM dd, yyyy");

                int sumasurred = Convert.ToInt32(sumAssured);
                return @"<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                <soap:Header>
 <AuthSoapHd xmlns='http://tempuri.org/'>

                    </AuthSoapHd>
                </soap:Header>
                <soap:Body>
 <GenerateEBIDigital xmlns='http://tempuri.org/'>
          <strInputXML>
         <![CDATA[
<EBIRequest>

<FirstName>" + fullName + @"</FirstName>

<LastName>" + lastName + @"</LastName>

<DateOfBirth>" + date + @"</DateOfBirth>

<Gender>" + Gender + @"</Gender>

 

<MaritalStatus></MaritalStatus>

<ProductDetails>

<Product>

<ProductType>TRADITIONAL</ProductType>

<ProductName>iProtectSmart</ProductName>

<ProductCode>T51</ProductCode>

<ModeOfPayment>Yearly</ModeOfPayment>

<ModalPremium>0</ModalPremium>

<AnnualPremium>0</AnnualPremium>

<Term>" + POLICY_TERM + @"</Term>

<DeathBenefit>5000000</DeathBenefit>

<PremiumPaymentTerm>27</PremiumPaymentTerm>

<Tobacco>" + Smoke + @"</Tobacco>

<SalesChannel>1</SalesChannel>

<RGFTerm>0</RGFTerm>

<PremiumPaymentOption>Regular Pay</PremiumPaymentOption>

<ADHB>1000000</ADHB>

<LifeCoverOption>All in One</LifeCoverOption>

<CIBenefit>1000000</CIBenefit>

<DeathBenefitOption>Lump-Sum</DeathBenefitOption>

<LumpsumPercentage>0</LumpsumPercentage>

<IPSDiscount>false</IPSDiscount>

</Product>

</ProductDetails>

</EBIRequest>
]]>
 </strInputXML>
 </GenerateEBIDigital>
                </soap:Body>

</soap:Envelope>";

                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }
    }
}
