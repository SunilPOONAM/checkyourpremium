using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
   public class HDFCDetails
    {
       public RootObject2 RootObject2 { get; set; }
    }
    public class POD
     {
    public string discount { get; set; }
    public int deathSA { get; set; }
    public string topupOpt { get; set; }
    public string prmmulfactor { get; set; }
    public string paymethod { get; set; }
    public string gstrt { get; set; }
    public string tobsts { get; set; }
    public string term { get; set; }
    public string tagline { get; set; }
    public string agentname { get; set; }
    public string sumAssured { get; set; }
    public string ppt { get; set; }
    public string option { get; set; }
    public string topupPercent { get; set; }
    public string gstrt_2yr { get; set; }
    public string nextPremDDt { get; set; }
    public string freq { get; set; }
    public string gstrt_rop { get; set; }
}

public class RD
{
}

public class RootObject2
{
    public string tcdesc { get; set; }
    public string partyid { get; set; }
    public string incpDt { get; set; }
    public int EC_RT { get; set; }
    public List<List<object>> PS { get; set; }
    public int firstPremium { get; set; }
    public string dbdesc { get; set; }
    public List<int> BSA { get; set; }
    public List<int> TOPUP { get; set; }
    public int combinedPremium { get; set; }
    public string qtDt { get; set; }
    public string matbdesc { get; set; }
    public int backdttax { get; set; }
    public string nextPremDDt { get; set; }
    public string quoteid { get; set; }
    public string id { get; set; }
    public int instapremium { get; set; }
    public List<object> cumulativeprem { get; set; }
    public string tibdesc { get; set; }
    public POD POD { get; set; }
    public string jnk { get; set; }
    public int tax { get; set; }
    public string adbdesc { get; set; }
    public RD RD { get; set; }
    public int sumAssured { get; set; }
    public string chMap { get; set; }
    public int premiumWithRider { get; set; }
    public int totPremium { get; set; }
    public int backdtpremium { get; set; }
    public List<object> BD { get; set; }
    public string appnum { get; set; }
    public int appTax { get; set; }
    public string isservice { get; set; }
    public string cibdesc { get; set; }
    public List<List<object>> BT { get; set; }
    public double premium { get; set; }
    public string uin { get; set; }
    public int txBkDt { get; set; }
    public int ST_RT { get; set; }
    public int KTAX { get; set; }
    public string prodname { get; set; }
    public string srnbdesc { get; set; }
    public string new_ill_chngs_eff_dt { get; set; }
    public double premWtoUw { get; set; }
    public string atpdbdesc { get; set; }
    public int totAnnPremium { get; set; }
    public List<object> PD { get; set; }
    public List<List<object>> DB { get; set; }
    public double perFreqPrem { get; set; }
}
}
