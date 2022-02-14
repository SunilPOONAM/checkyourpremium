using Domain;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Business
{
    public interface IBhartiAxa
    {
        GetQuotationDetail SubmitProposal(GetQuotationDetail getQuotationDetail);
    }
    public class BhartiAxa : IBhartiAxa
    {
        private readonly CheckyourpremiumliveEntities db;
        private string baseUrl;
        //string secretKey = string.Empty;
        //string apiKey = string.Empty;
        //    private SoapRequestHandler requestHandler = new SoapRequestHandler();
        public BhartiAxa()
        {
            db = new CheckyourpremiumliveEntities();
            baseUrl = System.Configuration.ConfigurationSettings.AppSettings["StarBaseURL"].ToString();
            //secretKey = System.Configuration.ConfigurationSettings.AppSettings["StarSecretKey"].ToString();
            //apiKey = System.Configuration.ConfigurationSettings.AppSettings["StarAPIKey"].ToString();
        }
        public GetQuotationDetail SubmitProposal(GetQuotationDetail getQuotationDetail)
        {
            try
            {
                var value = SubmitSoapRequestBhartiAxa(getQuotationDetail);
                return value;
            }
            catch (Exception ex)
            {
                //throw ex.Message.ToString();
            }
            return new GetQuotationDetail();
        }
        public string GetQuoteNo()
        {
            string quoteNo = string.Empty;
            try
            {

                quoteNo = "QRN" + DateTime.Now.ToString("yyyyMMdd") + (new Random()).Next(10000000).ToString();
            }
            catch (Exception ex)
            {

            }
            return quoteNo;
        }
        public string GetOrderNo()
        {
            string orderNo = string.Empty;
            try
            {
                orderNo = "VDJE" + (new Random()).Next(1000000).ToString();
            }
            catch (Exception ex)
            {

            }
            return orderNo;
        }



        public GetQuotationDetail SubmitSoapRequestBhartiAxa(GetQuotationDetail getQuotationDetail)
        {
            string requestBaseURL = string.Empty;
            string soapAction = string.Empty;
            string postedData = GetXMLBodyQutoe(getQuotationDetail);
            // postedData = GetXMLBody(getQuotationDetail);
            requestBaseURL = ConfigurationSettings.AppSettings["BhartiAXABaseURL"].ToString();
            soapAction = ConfigurationSettings.AppSettings["BhartiAXASoapAction"].ToString();
            ITermLifeBusiness _ITermLifeBusiness = new TermLifeBusiness();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://uat.bhartiaxaonline.co.in/cordys/com.eibus.web.soap.Gateway.wcp?organization=o=B2C,cn=cordys,cn=defaultInst106,o=mydomain.com");
            //req.Headers.Add(soapAction, requestBaseURL);
            //req.Headers.Add("Username", "apiuser");
            //req.Headers.Add("Password", "ApiUser@123");
            req.Method = "POST"; // Post method
            req.ContentType = "text/xml";// content type
            req.KeepAlive = false;
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
            req.ProtocolVersion = HttpVersion.Version10; req.PreAuthenticate = true;
            String XML = postedData;
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

                XDocument docc = XDocument.Parse(result);
                XNamespace ns = "http://schemas.cordys.com/bagi/tparty/core/bpm/1.0";

                IEnumerable<XElement> responses1 = docc.Descendants(ns + "OrderNo");
                foreach (XElement responsee in responses1)
                {
                    result = (string)responsee.Value;
                    if (result != "NA")
                    {
                        getQuotationDetail.OrderNo = result;
                    }
                    IEnumerable<XElement> responses2 = docc.Descendants(ns + "QuoteNo");
                    foreach (XElement respons in responses2)
                    {
                        result = (string)respons.Value;
                        if (result != "NA")
                        {
                            getQuotationDetail.QuoteNo = result;
                        }
                    }

                }

                IEnumerable<XElement> responses = docc.Descendants(ns + "SessionData");


                foreach (XElement response in responses)
                {

                    result = (string)response.Element(ns + "ID");
                    // getdata.ID = Convert.ToInt32(result);
                    result = (string)response.Element(ns + "Index");
                    // getdata.Index = result;
                    result = (string)response.Element(ns + "UserName");
                    getQuotationDetail.UserName = result;
                    //result = (string)response.Element(ns + "OrderNo");
                    //getQuotationDetail.OrderNo = result;
                    //result = (string)response.Element(ns + "QuoteNo");
                    //getQuotationDetail.QuoteNo = result;
                    result = (string)response.Element(ns + "Route");
                    getQuotationDetail.Route = result;
                    result = (string)response.Element(ns + "Contract");
                    getQuotationDetail.Contract = result;
                    result = (string)response.Element(ns + "Channel");
                    getQuotationDetail.Channel = "cypStr"; //result;
                    getQuotationDetail.Amount = getQuotationDetail.Amount;
                }
                // XmlDocument xd = new XmlDocument();
                // xd.LoadXml(result);
                //XmlNode xn = xd.DocumentElement;

                //XmlNodeList resultt = xn.SelectNodes("processTPRequestResponse/response/Session/SessionData/ID");

                return getQuotationDetail;

            }
            catch (WebException wex)
            {
                return new GetQuotationDetail();
            }
        }

        //.......................Proposal....................

        public string GetXMLBodyQutoe(GetQuotationDetail getQuotationDetail)
        {
            try
            {
                string faimly = "";
                if (getQuotationDetail.NoOfAdults == "1" && getQuotationDetail.NoOfChildren == "0")
                {
                    faimly = "S";
                    getQuotationDetail.dOB = Convert.ToDateTime(getQuotationDetail.dOB).ToString("yyyy-MM-dd");
                    getQuotationDetail.insured0dOB = "";
                    getQuotationDetail.insured1dOB = "";
                    getQuotationDetail.insured2dOB = "";
                }
                else if (getQuotationDetail.NoOfAdults == "2" && getQuotationDetail.NoOfChildren == "0")
                {
                    faimly = "SS";
                    getQuotationDetail.dOB = Convert.ToDateTime(getQuotationDetail.dOB).ToString("yyyy-MM-dd");
                    getQuotationDetail.insured0dOB = Convert.ToDateTime(getQuotationDetail.insured0dOB).ToString("yyyy-MM-dd");
                    getQuotationDetail.insured1dOB = "";
                    getQuotationDetail.insured2dOB = "";
                }
                else if (getQuotationDetail.NoOfAdults == "2" && getQuotationDetail.NoOfChildren == "1")
                {
                    faimly = "SSC";
                    getQuotationDetail.dOB = Convert.ToDateTime(getQuotationDetail.dOB).ToString("yyyy-MM-dd");
                    getQuotationDetail.insured0dOB = Convert.ToDateTime(getQuotationDetail.insured0dOB).ToString("yyyy-MM-dd");
                    getQuotationDetail.insured1dOB = Convert.ToDateTime(getQuotationDetail.insured1dOB).ToString("yyyy-MM-dd");
                    getQuotationDetail.insured2dOB = "";
                }
                else if (getQuotationDetail.NoOfAdults == "2" && getQuotationDetail.NoOfChildren == "2")
                {
                    faimly = "SS2C";
                    getQuotationDetail.dOB = Convert.ToDateTime(getQuotationDetail.dOB).ToString("yyyy-MM-dd");
                    getQuotationDetail.insured0dOB = Convert.ToDateTime(getQuotationDetail.insured0dOB).ToString("yyyy-MM-dd");
                    getQuotationDetail.insured1dOB = Convert.ToDateTime(getQuotationDetail.insured1dOB).ToString("yyyy-MM-dd");
                    getQuotationDetail.insured2dOB = Convert.ToDateTime(getQuotationDetail.insured2dOB).ToString("yyyy-MM-dd");
                }
                else if (getQuotationDetail.NoOfAdults == "1" && getQuotationDetail.NoOfChildren == "2")
                {
                    faimly = "S2C";
                    getQuotationDetail.dOB = Convert.ToDateTime(getQuotationDetail.dOB).ToString("yyyy-MM-dd");

                    getQuotationDetail.insured1dOB = Convert.ToDateTime(getQuotationDetail.insured1dOB).ToString("yyyy-MM-dd");
                    getQuotationDetail.insured2dOB = Convert.ToDateTime(getQuotationDetail.insured2dOB).ToString("yyyy-MM-dd");
                    getQuotationDetail.insured0dOB = "";

                }
                else if (getQuotationDetail.NoOfAdults == "1" && getQuotationDetail.NoOfChildren == "1")
                {
                    faimly = "SC";
                    getQuotationDetail.dOB = Convert.ToDateTime(getQuotationDetail.dOB).ToString("yyyy-MM-dd");
                    getQuotationDetail.insured1dOB = Convert.ToDateTime(getQuotationDetail.insured1dOB).ToString("yyyy-MM-dd");
                    getQuotationDetail.insured2dOB = "";
                    getQuotationDetail.insured0dOB = "";

                }
                List<bhartiAxaCredential> bhartiCredentials = db.bhartiAxaCredentials.ToList();
                string bodyXML = string.Empty;
                string bodyQuate = string.Empty;
                string nantional = string.Empty;
                //...............Male Female......................
                if (getQuotationDetail.gender == "Female") { getQuotationDetail.gender = "F"; }
                if (getQuotationDetail.insured0gender == "Female") { getQuotationDetail.insured0gender = "F"; }
                if (getQuotationDetail.insured1gender == "Female") { getQuotationDetail.insured1gender = "F"; }
                if (getQuotationDetail.insured2gender == "Female") { getQuotationDetail.insured2gender = "F"; }
                if (getQuotationDetail.gender == "Male") { getQuotationDetail.gender = "M"; }
                if (getQuotationDetail.insured0gender == "Male") { getQuotationDetail.insured0gender = "M"; }
                if (getQuotationDetail.insured1gender == "Male") { getQuotationDetail.insured1gender = "M"; }
                if (getQuotationDetail.insured2gender == "Male") { getQuotationDetail.insured2gender = "M"; }
                if (getQuotationDetail.gender == "Transgender") { getQuotationDetail.gender = "T"; }
                if (getQuotationDetail.insured0gender == "Transgender") { getQuotationDetail.insured0gender = "T"; }
                if (getQuotationDetail.insured1gender == "Transgender") { getQuotationDetail.insured1gender = "T"; }
                if (getQuotationDetail.insured2gender == "Transgender") { getQuotationDetail.insured2gender = "T"; }
                //............................................
                if (getQuotationDetail.IsIndianPassport == "Yes") { getQuotationDetail.IsIndianPassport = "Y"; nantional = "India"; }
                else { getQuotationDetail.IsIndianPassport = "N"; nantional = "Other"; }
                getQuotationDetail.proposerdOB = getQuotationDetail.proposerdOB;
                getQuotationDetail.travelstartdate = getQuotationDetail.travelstartdate;
                if (bhartiCredentials.Count > 0)
                {
                    string Edate = DateTime.Today.AddYears(+1).AddDays(-1).ToString("yyyy-MM-dd");
                    string Sdatetoday = DateTime.Today.ToString("yyyy-MM-dd");
                    DateTime dob1 = Convert.ToDateTime(getQuotationDetail.dOB);
                    TimeSpan tm = (DateTime.Now - dob1);
                    int age = (tm.Days / 365);
                    getQuotationDetail.age = age.ToString();
                    string totaldays = getQuotationDetail.staydays;
                    bodyQuate = @"<Envelope xmlns='http://schemas.xmlsoap.org/soap/envelope/'>
                   <Body>
        <serve xmlns='http://schemas.cordys.com/gateway/Provider'>
            <SessionDoc><Session>
	<SessionData>
		<Index>1</Index>
		<InitTime>" + DateTime.Now.ToString("ddd MMM dd yyyy HH:mm:ss 'GMT'") + @"</InitTime>
		        <UserName>cypStr</UserName>
        <Password>AZg3Q1SktWKLz0Os/H0MlAxFfI75pjnpKjn9xrV9vimyyS7/5Ilil/ftcP5oHxC7IFYLVF0C3MAJcIznwrZvDA==</Password>
		       <QuoteNo>" + getQuotationDetail.applicationId + @"</QuoteNo>
        <OrderNo>" + getQuotationDetail.policyID + @"</OrderNo>
		<Route>INT</Route>
		<Contract>STR</Contract>
		<ProductType>B2C</ProductType>
		<Channel/>
		<TransactionType>Quote</TransactionType>
		<TransactionStatus>Fresh</TransactionStatus>
		<ID>146097008602217542258697</ID>
		<UserAgentID>2C000024</UserAgentID>
		<Source>2C000024</Source>
	</SessionData>
	<Travel>
		 <TypeOfBusiness>NB</TypeOfBusiness>
        <TypeOfPolicy>I/F</TypeOfPolicy>
        <TypeOfTrip>S</TypeOfTrip>
        <DepartureDate>" + getQuotationDetail.travelstartdate + @"T00:04:00:40</DepartureDate>
        <ReturnDate>" + getQuotationDetail.travelenddate + @"T00:04:00:40</ReturnDate>
        <TripDuration>" + totaldays + @"</TripDuration>
        <TypeOfCover>I</TypeOfCover>
        <TypeOfTavel>NON_SCH</TypeOfTavel>
        <MaxPerTripDuration>11</MaxPerTripDuration>
        <GeographicalExtension>1</GeographicalExtension>
        <FamilyType>" + faimly + @"</FamilyType>
	</Travel>
	<Quote>
		<PolicyStartDate>" + getQuotationDetail.travelstartdate + @"T00:04:00:40</PolicyStartDate>
		<PolicyEndDate>" + getQuotationDetail.travelenddate + @"T00:04:00:40</PolicyEndDate>
				<AgentNumber>" + getQuotationDetail.proposerName + @"</AgentNumber>
		<AgentName>" + getQuotationDetail.proposerName + @"</AgentName>
		<PlanSelected>" + getQuotationDetail.PlanId + @"</PlanSelected>
       <Amount>" + getQuotationDetail.Amount + @"</Amount>
		<Stage/>
	</Quote>
	 <Client>
     <ClientType>Individual</ClientType>
		<CltDOB>" + getQuotationDetail.dOB + @"</CltDOB>
		<GivName>" + getQuotationDetail.insuredPersonName + @"</GivName>
		<LastName></LastName>
		<EmailID>" + getQuotationDetail.email + @"</EmailID>
		<MobileNo>" + getQuotationDetail.mobile + @"</MobileNo>
		<TPClientRefNo>5FE81A2ADB704C9</TPClientRefNo>
		<CltSex>" + getQuotationDetail.gender + @"</CltSex>
		<Age>" + getQuotationDetail.age + @"</Age>
		<Salut>" + getQuotationDetail.prefix + @"</Salut>
		<Occupation>" + getQuotationDetail.occupation + @"</Occupation>
		<CltAddr01>" + getQuotationDetail.address + @"</CltAddr01>
		<CltAddr02>" + getQuotationDetail.address + @"</CltAddr02>
		<CltAddr03/>
		<City>" + getQuotationDetail.city + @"</City>
		<State>" + getQuotationDetail.state + @"</State>
		<PinCode>" + getQuotationDetail.pinCode + @"</PinCode>
		<MemberDetails>
			<Member>
				<CurrentLocation>" + getQuotationDetail.country + @"</CurrentLocation>
				<FirstName>" + getQuotationDetail.insuredPersonName + @"</FirstName>
               <LastName></LastName>
				<Gender>" + getQuotationDetail.gender + @"</Gender>
				<HoldPassport>" + getQuotationDetail.IsIndianPassport + @"</HoldPassport>
				<MemberDOB>" + getQuotationDetail.dOB + @"</MemberDOB>
				<Nationality>" + nantional + @"</Nationality>
				<Age>" + getQuotationDetail.age + @"</Age>
				<NomineeName>" + getQuotationDetail.NomineeName + @"</NomineeName>
				<AppointeeName>" + getQuotationDetail.assigneeName + @"</AppointeeName>
				<AppointeeRelationCode>" + getQuotationDetail.assigneeRelationshipId + @"</AppointeeRelationCode>
				<NomineeRelationCode>" + getQuotationDetail.Relation + @"</NomineeRelationCode>
				<PassportNo>" + getQuotationDetail.passportNo + @"</PassportNo>
				<ProposerRelationCode>" + getQuotationDetail.assigneeRelationshipId + @"</ProposerRelationCode>
				<Salut>" + getQuotationDetail.insuredPrefix + @"</Salut>
			</Member>
	<Member>
				
				<FirstName>" + getQuotationDetail.insured0insuredPersonName + @"</FirstName>
               <LastName></LastName>
				<Gender>" + getQuotationDetail.insured0gender + @"</Gender>
				<HoldPassport>" + getQuotationDetail.insured0IsIndianPassport + @"</HoldPassport>
				<MemberDOB>" + getQuotationDetail.insured0dOB + @"</MemberDOB>
				<Nationality></Nationality>
				<Age>" + getQuotationDetail.insured0age + @"</Age>
				<NomineeName>" + getQuotationDetail.insured0NomineeName + @"</NomineeName>
				<AppointeeName>" + getQuotationDetail.insured0assigneeName + @"</AppointeeName>
				<AppointeeRelationCode>" + getQuotationDetail.insured0assigneeRelationshipId + @"</AppointeeRelationCode>
				<NomineeRelationCode>" + getQuotationDetail.insured0Relation + @"</NomineeRelationCode>
				<PassportNo>" + getQuotationDetail.insured0passportNo + @"</PassportNo>
				<ProposerRelationCode>" + getQuotationDetail.insured0assigneeRelationshipId + @"</ProposerRelationCode>
				
			</Member><Member>
			
				<FirstName>" + getQuotationDetail.insured1insuredPersonName + @"</FirstName>
               <LastName></LastName>
				<Gender>" + getQuotationDetail.insured1gender + @"</Gender>
				<HoldPassport>" + getQuotationDetail.insured1IsIndianPassport + @"</HoldPassport>
				<MemberDOB>" + getQuotationDetail.insured1dOB + @"</MemberDOB>
				<Nationality></Nationality>
				<Age>" + getQuotationDetail.insured1age + @"</Age>
				<NomineeName>" + getQuotationDetail.insured1NomineeName + @"</NomineeName>
				<AppointeeName>" + getQuotationDetail.insured1assigneeName + @"</AppointeeName>
				<AppointeeRelationCode>" + getQuotationDetail.insured1assigneeRelationshipId + @"</AppointeeRelationCode>
				<NomineeRelationCode>" + getQuotationDetail.insured1Relation + @"</NomineeRelationCode>
				<PassportNo>" + getQuotationDetail.insured1passportNo + @"</PassportNo>
				<ProposerRelationCode>" + getQuotationDetail.insured1assigneeRelationshipId + @"</ProposerRelationCode>
				
			</Member><Member>
				
				<FirstName>" + getQuotationDetail.insured2insuredPersonName + @"</FirstName>
               <LastName></LastName>
				<Gender>" + getQuotationDetail.insured2gender + @"</Gender>
				<HoldPassport>" + getQuotationDetail.insured2IsIndianPassport + @"</HoldPassport>
				<MemberDOB>" + getQuotationDetail.insured2dOB + @"</MemberDOB>
				<Nationality></Nationality>
				<Age>" + getQuotationDetail.insured2age + @"</Age>
				<NomineeName>" + getQuotationDetail.insured2NomineeName + @"</NomineeName>
				<AppointeeName>" + getQuotationDetail.insured2assigneeName + @"</AppointeeName>
				<AppointeeRelationCode>" + getQuotationDetail.insured2assigneeRelationshipId + @"</AppointeeRelationCode>
				<NomineeRelationCode>" + getQuotationDetail.insured2Relation + @"</NomineeRelationCode>
				<PassportNo>" + getQuotationDetail.insured2passportNo + @"</PassportNo>
				<ProposerRelationCode>" + getQuotationDetail.insured2assigneeRelationshipId + @"</ProposerRelationCode>
			
			</Member>
		</MemberDetails>
		<Nominee>
			<Name>" + getQuotationDetail.NomineeName + @"</Name>
			<Age>" + getQuotationDetail.age + @"</Age>
			<NomineeRelationCode>" + getQuotationDetail.Relation.Replace(",", "") + @"</NomineeRelationCode>
			<AppointeeName>" + getQuotationDetail.assigneeName + @"</AppointeeName>
			<AppointeeRelationCode>" + getQuotationDetail.assigneeRelationshipId + @"</AppointeeRelationCode>
		</Nominee>

		<SportsPerson>N</SportsPerson>
		<DangerousActivity>N</DangerousActivity>
		<PreExistingIllness>N</PreExistingIllness>
      </Client>
</Session>
</SessionDoc>
        </serve>
    </Body>
</Envelope>";
                }

                return bodyQuate;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";

        }

        //......................................................

        public string GetXMLBody(View_TravelInsuranceModel getQuotationDetail)
        {
            List<bhartiAxaCredential> bhartiCredentials = db.bhartiAxaCredentials.ToList();
            try
            {
                string bodyXML = string.Empty;
                string bodyQuate = string.Empty;
                string nantional = string.Empty;
                string triptype = string.Empty;
                string traveltype = string.Empty; string Policttype = string.Empty;
                int insured1 = 1;
                int insured2 = 0; int insured3 = 0; int insured4 = 0; int insured5 = 0; int insured6 = 0;
                if (getQuotationDetail.ageSpouse != "0")
                { insured2 = 1; } if (getQuotationDetail.ageChild1 != "0")
                { insured3 = 1; } if (getQuotationDetail.ageChild2 != "0")
                { insured4 = 1; } if (getQuotationDetail.ageFather != "0")
                { insured5 = 1; } if (getQuotationDetail.ageMother != "0") { insured6 = 1; }
                int insuerp = insured1 + insured2 + insured3 + insured4 + insured5 + insured6;
                if (bhartiCredentials.Count > 0)
                {
                    string dobcal = DateTime.Today.ToString("yyyy");
                    int dob = Convert.ToInt32(dobcal) - Convert.ToInt32(getQuotationDetail.ageSelf);
                    string dateage = dob.ToString() + "-01-01";
                    bodyQuate = @"<Envelope xmlns='http://schemas.xmlsoap.org/soap/envelope/'>
                   <Body>
        <serve xmlns='http://schemas.cordys.com/gateway/Provider'>
            <SessionDoc><Session>
	<SessionData>
		<Index>1</Index>
		<InitTime>" + DateTime.Now.ToString("ddd MMM dd yyyy HH:mm:ss 'GMT'") + @"</InitTime>
		        <UserName>cypStr</UserName>
        <Password>AZg3Q1SktWKLz0Os/H0MlAxFfI75pjnpKjn9xrV9vimyyS7/5Ilil/ftcP5oHxC7IFYLVF0C3MAJcIznwrZvDA==</Password>
		       <QuoteNo>NA</QuoteNo>
        <OrderNo>NA</OrderNo>
		<Route>INT</Route>
		<Contract>STR</Contract>
		<ProductType>B2C</ProductType>
		<Channel/>
		<TransactionType>Quote</TransactionType>
		<TransactionStatus>Fresh</TransactionStatus>
		<ID>146097008602217542258697</ID>
		<UserAgentID>2C000024</UserAgentID>
		<Source>2C000024</Source>
	</SessionData>
	<Travel>
		 <TypeOfBusiness>NB</TypeOfBusiness>
        <TypeOfPolicy>I/F</TypeOfPolicy>
        <TypeOfTrip>S</TypeOfTrip>
        <DepartureDate>" + getQuotationDetail.tripStartDate + @"T00:04:00:40</DepartureDate>
        <ReturnDate>" + getQuotationDetail.tripEndDate + @"T00:04:00:40</ReturnDate>
        <TripDuration>" + getQuotationDetail.stayDays + @"</TripDuration>
        <TypeOfCover>I</TypeOfCover>
        <TypeOfTavel>NON_SCH</TypeOfTavel>
        <MaxPerTripDuration>11</MaxPerTripDuration>
        <GeographicalExtension>" + insuerp + @"</GeographicalExtension>
        <FamilyType>S</FamilyType>
	</Travel>
	<Quote>
		<PolicyStartDate>" + getQuotationDetail.tripStartDate + @"T00:04:00:40</PolicyStartDate>
		<PolicyEndDate>" + getQuotationDetail.tripEndDate + @"T00:04:00:40</PolicyEndDate>
				<AgentNumber>" + getQuotationDetail.travellerName + @"</AgentNumber>
		<AgentName>" + getQuotationDetail.travellerName + @"</AgentName>
		<PlanSelected>" + getQuotationDetail.Plan_ID + @"</PlanSelected>
       
		<Stage/>
	</Quote>
	 <Client>
     <ClientType>Individual</ClientType>
		<CltDOB>" + dateage + @"</CltDOB>
		<GivName>" + getQuotationDetail.travellerName + @"</GivName>
		<LastName>Rai</LastName>
		<EmailID>" + getQuotationDetail.Email + @"</EmailID>
		<MobileNo>" + getQuotationDetail.Phone + @"</MobileNo>
		<TPClientRefNo>5FE81A2ADB704C9</TPClientRefNo>
		<CltSex>M</CltSex>
		<Age>" + getQuotationDetail.ageSelf + @"</Age>
		<Salut></Salut>
		<Occupation></Occupation>
		<CltAddr01></CltAddr01>
		<CltAddr02></CltAddr02>
		<CltAddr03/>
		<City></City>
		<State></State>
		<PinCode></PinCode>
		<MemberDetails>
			<Member>
				<CurrentLocation></CurrentLocation>
				<FirstName>" + getQuotationDetail.travellerName + @"</FirstName>
               <LastName>Rai</LastName>
				<Gender>M</Gender>
				<HoldPassport>Y</HoldPassport>
				<MemberDOB>" + dateage + @"</MemberDOB>
					<Nationality>India</Nationality>
				<Age>" + getQuotationDetail.ageSelf + @"</Age>
					<NomineeName>dfdsgfdg</NomineeName>
				<AppointeeName>rdytey</AppointeeName>
				<AppointeeRelationCode>BROTHER</AppointeeRelationCode>
				<NomineeRelationCode>Sister-in-Law,</NomineeRelationCode>
				<PassportNo>k87687687</PassportNo>
				<ProposerRelationCode>BROTHER</ProposerRelationCode>
				<Salut></Salut>
			</Member>
	
		</MemberDetails>
		<Nominee>
				<Name>dfdsgfdg</Name>
			<Age>50</Age>
			<NomineeRelationCode>Sister-in-Law,</NomineeRelationCode>
			<AppointeeName>rdytey</AppointeeName>
			<AppointeeRelationCode>BROTHER</AppointeeRelationCode>
		</Nominee>

		<SportsPerson>N</SportsPerson>
		<DangerousActivity>N</DangerousActivity>
		<PreExistingIllness>N</PreExistingIllness>
      </Client>
</Session>
</SessionDoc>
        </serve>
    </Body>
</Envelope>";
                }
                return bodyQuate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }
        //.......................Queto....................

        public static string GetQuatoBhartiAxa(View_TravelInsuranceModel srchTrvlIns)
        {
            try
            {
                string faimlytype = "";
                if (srchTrvlIns.ageSelf != "0" && srchTrvlIns.ageSpouse == "0" && srchTrvlIns.ageChild1 == "0" && srchTrvlIns.ageChild2 == "0")
                {
                    faimlytype = "S";
                }
                else if (srchTrvlIns.ageSelf != "0" && srchTrvlIns.ageSpouse != "0" && srchTrvlIns.ageChild1 == "0" && srchTrvlIns.ageChild2 == "0")
                {
                    faimlytype = "SS";
                }
                else if (srchTrvlIns.ageSelf != "0" && srchTrvlIns.ageSpouse != "0" && srchTrvlIns.ageChild1 != "0" && srchTrvlIns.ageChild2 == "0")
                {
                    faimlytype = "SSC";
                }
                else if (srchTrvlIns.ageSelf != "0" && srchTrvlIns.ageSpouse != "0" && srchTrvlIns.ageChild1 != "0" && srchTrvlIns.ageChild2 != "0")
                {
                    faimlytype = "SS2C";
                }
                else if (srchTrvlIns.ageSelf != "0" && srchTrvlIns.ageSpouse == "0" && srchTrvlIns.ageChild1 != "0" && srchTrvlIns.ageChild2 == "0")
                {
                    faimlytype = "SC";
                }
                else if (srchTrvlIns.ageSelf != "0" && srchTrvlIns.ageSpouse == "0" && srchTrvlIns.ageChild1 != "0" && srchTrvlIns.ageChild2 != "0")
                {
                    faimlytype = "S2C";
                }
                string bodyXML = string.Empty;
                bodyXML = @"<Envelope xmlns='http://schemas.xmlsoap.org/soap/envelope/'>
      <Body>
        <serve xmlns='http://schemas.cordys.com/gateway/Provider'>
            <SessionDoc>
//                <Session>
//          <SessionData>
//        <Index>1</Index>
//        <InitTime>Wed Mar 04 2020 3:28:09 GMT</InitTime>
//        <UserName>cypStr</UserName>
//        <Password>AZg3Q1SktWKLz0Os/H0MlAxFfI75pjnpKjn9xrV9vimyyS7/5Ilil/ftcP5oHxC7IFYLVF0C3MAJcIznwrZvDA==</Password>
//        <QuoteNo>NA</QuoteNo>
//        <OrderNo>NA</OrderNo>
//        <Route>INT</Route>
//        <Contract>STR</Contract>
//        <ProductType>B2C</ProductType>
//        <Channel/>
//        <TransactionType>Quote</TransactionType>
//        <TransactionStatus>Fresh</TransactionStatus>
//        <ID>146097008602217542258697</ID>
//        <UserAgentID>2C000024</UserAgentID>
//        <Source>2C000024</Source>
//    </SessionData>
//    <Travel>
//        <TypeOfBusiness>NB</TypeOfBusiness>
//        <TypeOfPolicy>I/F</TypeOfPolicy>
//        <TypeOfTrip>S</TypeOfTrip>
//        <DepartureDate>2020-02-29T00:04:00:40</DepartureDate>
//        <ReturnDate>2020-03-12T00:04:00:40</ReturnDate>
//        <TripDuration>13</TripDuration>
//        <TypeOfCover>F</TypeOfCover>
//        <TypeOfTavel>NON_SCH</TypeOfTavel>
//        <MaxPerTripDuration>11</MaxPerTripDuration>
//        <GeographicalExtension>3</GeographicalExtension>
//        <FamilyType>" + faimlytype + @"</FamilyType>
//    </Travel>
//    <Quote>
//        <PolicyStartDate>2020-02-29T00:04:00:40</PolicyStartDate>
//        <PolicyEndDate>2020-03-12T00:04:00:40</PolicyEndDate>
//        <AgentNumber>ghjhfjf</AgentNumber>
//        <AgentName>ghjhfjf</AgentName>
//        <PlanSelected>1</PlanSelected>
//        <Amount>1</Amount>
//        <Stage/>
//    </Quote>
//    <Client>
//        <ClientType>Group</ClientType>
//        <CltDOB>1986-02-06</CltDOB>
//        <GivName>fhfghfg</GivName>
//        <LastName>Rai</LastName>
//        <EmailID>f@gmail.com</EmailID>
//        <MobileNo>8797898797</MobileNo>
//        <TPClientRefNo>5FE81A2ADB704C9</TPClientRefNo>
//        <CltSex>M</CltSex>
//        <Age>36</Age>
//        <Salut></Salut>
//        <Occupation></Occupation>
//        <CltAddr01>lko</CltAddr01>
//        <CltAddr02>lko</CltAddr02>
//        <CltAddr03/>
//        <City>3090</City>
//        <State></State>
//        <PinCode>226016</PinCode>
//        <MemberDetails>
//            <Member>
//                <Age>30</Age>
//                <CurrentLocation>Y</CurrentLocation>
//                <FirstName>Surya</FirstName>
//                <Gender>M</Gender>
//                <GstIn>ASDASD2323</GstIn>
//                <HoldPassport>Y</HoldPassport>
//                <LastName>dandu</LastName>
//                <MemberDOB>1988-04-11</MemberDOB>
//                <Nationality>INDIA</Nationality>
//                <NomineeName>dsasdas</NomineeName>
//                <NomineeRelationCode>Self</NomineeRelationCode>
//                <PassportNo>R1234567</PassportNo>
//                <ProposerRelationCode>EldestMember</ProposerRelationCode>
//                <Salut>MR</Salut>
//            </Member>
//            <Member>
//                <Age>29</Age>
//                <CurrentLocation>Y</CurrentLocation>
//                <FirstName>Surya</FirstName>
//                <Gender>M</Gender>
//                <GstIn>ASDASD2323</GstIn>
//                <HoldPassport>Y</HoldPassport>
//                <LastName>dandu</LastName>
//                <MemberDOB>1989-04-11</MemberDOB>
//                <Nationality>INDIA</Nationality>
//                <NomineeName>dsasdas</NomineeName>
//                <NomineeRelationCode>Spouse</NomineeRelationCode>
//                <PassportNo>R1234567</PassportNo>
//                <ProposerRelationCode>EldestMember</ProposerRelationCode>
//                <Salut></Salut>
//            </Member>
//            <Member>
//                <CurrentLocation></CurrentLocation>
//                <FirstName>fhfghfg</FirstName>
//                <LastName>Rai</LastName>
//                <Gender>M</Gender>
//                <HoldPassport>Y</HoldPassport>
//                <MemberDOB>1992-02-06</MemberDOB>
//                <Nationality>India</Nationality>
//                <Age>50</Age>
//                <NomineeName>dfdsgfdg</NomineeName>
//                <AppointeeName>rdytey</AppointeeName>
//                <AppointeeRelationCode>BROTHER</AppointeeRelationCode>
//                <NomineeRelationCode>Sister-in-Law,</NomineeRelationCode>
//                <PassportNo>k87687687</PassportNo>
//                <ProposerRelationCode>BROTHER</ProposerRelationCode>
//                <Salut></Salut>
//            </Member>
//        </MemberDetails>
//        <Nominee>
//            <Name>dfdsgfdg</Name>
//            <Age>50</Age>
//            <NomineeRelationCode>Sister-in-Law,</NomineeRelationCode>
//            <AppointeeName>rdytey</AppointeeName>
//            <AppointeeRelationCode>BROTHER</AppointeeRelationCode>
//        </Nominee>
//        <SportsPerson>N</SportsPerson>
//        <DangerousActivity>N</DangerousActivity>
//        <PreExistingIllness>N</PreExistingIllness>
//        <NoOfPersons>2</NoOfPersons>
//    </Client>
//</Session>
 <Session>
    <SessionData>
        <Index>1</Index>
        <InitTime>" + DateTime.Now.ToString("ddd MMM dd yyyy HH:mm:ss 'GMT'") + @"</InitTime>
        <UserName>cypStr</UserName>
        <Password>AZg3Q1SktWKLz0Os/H0MlAxFfI75pjnpKjn9xrV9vimyyS7/5Ilil/ftcP5oHxC7IFYLVF0C3MAJcIznwrZvDA==</Password>
        <QuoteNo>NA</QuoteNo>
        <OrderNo>NA</OrderNo>
        <Route>INT</Route>
        <Contract>STR</Contract>
        <ProductType>B2C</ProductType>
        <Channel/>
        <TransactionType>Quote</TransactionType>
        <TransactionStatus>Fresh</TransactionStatus>
        <ID>146097008602217542258697</ID>
        <UserAgentID>2C000024</UserAgentID>
        <Source>2C000024</Source>
    </SessionData>
    <Travel>
        <TypeOfBusiness>NB</TypeOfBusiness>
        <TypeOfPolicy>I/F</TypeOfPolicy>
        <TypeOfTrip>" + srchTrvlIns.tripType + @"</TypeOfTrip>
        <DepartureDate>" + srchTrvlIns.tripStartDate + @"T00:04:00:40</DepartureDate>
        <ReturnDate>" + srchTrvlIns.tripEndDate + @"T00:04:00:40</ReturnDate>
        <TripDuration>" + srchTrvlIns.stayDays + @"</TripDuration>
        <TypeOfCover>F</TypeOfCover>
        <TypeOfTavel>NON_SCH</TypeOfTavel>
        <MaxPerTripDuration>11</MaxPerTripDuration>
        <GeographicalExtension>3</GeographicalExtension>
        <FamilyType>SS</FamilyType>
    </Travel>
    <Quote>
        <PolicyStartDate>" + srchTrvlIns.tripStartDate + @"T00:04:00:40</PolicyStartDate>
        <PolicyEndDate>" + srchTrvlIns.tripEndDate + @"T00:04:00:40</PolicyEndDate>
        <AgentNumber>ghjhfjf</AgentNumber>
        <AgentName>" + srchTrvlIns.travellerName + @"</AgentName>
        <PlanSelected>1</PlanSelected>
        <Amount>1</Amount>
        <Stage/>
    </Quote>
    <Client>
        <ClientType>Group</ClientType>
        <CltDOB>1986-02-06</CltDOB>
        <GivName>fhfghfg</GivName>
        <LastName>Rai</LastName>
        <EmailID>" + srchTrvlIns.Email + @"</EmailID>
        <MobileNo>" + srchTrvlIns.Phone + @"</MobileNo>
        <TPClientRefNo>5FE81A2ADB704C9</TPClientRefNo>
        <CltSex>M</CltSex>
        <Age>" + srchTrvlIns.ageSelf + @"</Age>
        <Salut></Salut>
        <Occupation></Occupation>
        <CltAddr01>lko</CltAddr01>
        <CltAddr02>lko</CltAddr02>
        <CltAddr03/>
        <City>" + srchTrvlIns.City + @"</City>
        <State></State>
        <PinCode>226016</PinCode>
        <MemberDetails>
            <Member>
                <Age>" + srchTrvlIns.ageSpouse + @"</Age>
                <CurrentLocation>Y</CurrentLocation>
                <FirstName>Surya</FirstName>
                <Gender>M</Gender>
                <GstIn>ASDASD2323</GstIn>
                <HoldPassport>Y</HoldPassport>
                <LastName>dandu</LastName>
                <MemberDOB>1988-04-11</MemberDOB>
                <Nationality>INDIA</Nationality>
                <NomineeName>dsasdas</NomineeName>
                <NomineeRelationCode>Self</NomineeRelationCode>
                <PassportNo>R1234567</PassportNo>
                <ProposerRelationCode>EldestMember</ProposerRelationCode>
                <Salut>MR</Salut>
            </Member>
            <Member>
                <Age>" + srchTrvlIns.ageChild1 + @"</Age>
                <CurrentLocation>Y</CurrentLocation>
                <FirstName>Surya</FirstName>
                <Gender>M</Gender>
                <GstIn>ASDASD2323</GstIn>
                <HoldPassport>Y</HoldPassport>
                <LastName>dandu</LastName>
                <MemberDOB>1989-04-11</MemberDOB>
                <Nationality>INDIA</Nationality>
                <NomineeName>dsasdas</NomineeName>
                <NomineeRelationCode>Spouse</NomineeRelationCode>
                <PassportNo>R1234567</PassportNo>
                <ProposerRelationCode>EldestMember</ProposerRelationCode>
                <Salut></Salut>
            </Member>
            <Member>
                <CurrentLocation></CurrentLocation>
                <FirstName>fhfghfg</FirstName>
                <LastName>Rai</LastName>
                <Gender>M</Gender>
                <HoldPassport>Y</HoldPassport>
                <MemberDOB>1992-02-06</MemberDOB>
                <Nationality>India</Nationality>
                <Age>50</Age>
                <NomineeName>dfdsgfdg</NomineeName>
                <AppointeeName>rdytey</AppointeeName>
                <AppointeeRelationCode>BROTHER</AppointeeRelationCode>
                <NomineeRelationCode>Sister-in-Law,</NomineeRelationCode>
                <PassportNo>k87687687</PassportNo>
                <ProposerRelationCode>BROTHER</ProposerRelationCode>
                <Salut></Salut>
            </Member>
        </MemberDetails>
        <Nominee>
            <Name>dfdsgfdg</Name>
            <Age>50</Age>
            <NomineeRelationCode>Sister-in-Law,</NomineeRelationCode>
            <AppointeeName>rdytey</AppointeeName>
            <AppointeeRelationCode>BROTHER</AppointeeRelationCode>
        </Nominee>
        <SportsPerson>N</SportsPerson>
        <DangerousActivity>N</DangerousActivity>
        <PreExistingIllness>N</PreExistingIllness>
        <NoOfPersons>2</NoOfPersons>
    </Client>
</Session>


                
                
            </SessionDoc>
        </serve>
    </Body>
</Envelope>
";
                return bodyXML;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }


        public static string GetQuatorequset(View_TravelInsuranceModel getQuotationDetail)
        {
           
            string requestBaseURL = string.Empty;
            string soapAction = string.Empty;
           // string postedData = GetQuatoBhartiAxa(getQuotationDetail);
            // postedData = GetXMLBody(getQuotationDetail);
            requestBaseURL = ConfigurationSettings.AppSettings["BhartiAXABaseURL"].ToString();
            soapAction = ConfigurationSettings.AppSettings["BhartiAXASoapAction"].ToString();
            ITermLifeBusiness _ITermLifeBusiness = new TermLifeBusiness();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://uat.bhartiaxaonline.co.in/cordys/com.eibus.web.soap.Gateway.wcp?organization=o=B2C,cn=cordys,cn=defaultInst106,o=mydomain.com");
            
            req.Method = "POST"; // Post method
            req.ContentType = "text/xml";// content type
            req.KeepAlive = false;
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
            req.ProtocolVersion = HttpVersion.Version10; req.PreAuthenticate = true;
            String XML = GetQuatoBhartiAxa(getQuotationDetail);
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

                XDocument docc = XDocument.Parse(result);
                XNamespace ns = "http://schemas.cordys.com/bagi/tparty/core/bpm/1.0";

                IEnumerable<XElement> responses1 = docc.Descendants(ns + "OrderNo");
                IEnumerable<XElement> responses2 = docc.Descendants(ns + "QuoteNo");
                //IEnumerable<XElement> res = docc.Descendants(ns + "PlanSet");
                //foreach (XElement response in res)
                //{
                  

                //    result = (string)response.Element(ns + "PlanName");
                //      result = (string)response.Element(ns + "SumInsured");
                //      result = (string)response.Element(ns + "Premium");
                //      result = (string)response.Element(ns + "PlanId");
                //      result = (string)response.Element(ns + "ServiceTax");
                //      result = (string)response.Element(ns + "PremiumPayable");
                //      result = (string)response.Element(ns + "CessApplicable");
                //      result = (string)response.Element(ns + "CessAmount");
                //} 
                return result;
            }
            catch (WebException wex)
            {
                return new StreamReader(wex.Response.GetResponseStream())
                                       .ReadToEnd();
            }
           
        }

        //.......................................................

    }
}
