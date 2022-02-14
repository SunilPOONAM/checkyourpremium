using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public interface IHomeBusiness
    {
        List<CompanyPlanDetails> GetPlanDetails(CompanyPlanDetails srchTrvlIns);
        List<CompanyPlanDetails> GetPlanBuyDetails(CompanyPlanDetails srchTrvlIns);
        long SaveBuyerDetails(BuyInfoDetails Responcedetail);
       
       
    }
   public class HomeBusiness:IHomeBusiness
    {
       private readonly CheckyourpremiumliveEntities db;
          public HomeBusiness()
        {

            db = new CheckyourpremiumliveEntities();

        }
          public List<CompanyPlanDetails> GetPlanDetails(CompanyPlanDetails srchTrvlIns)
          {
              try
              {
                  var data = db.tbl_BuyPlanInfo.AsEnumerable().Select(
                       p => new CompanyPlanDetails
                       //var data = db.All_Company_Details.Where(x => x.Company_Id == objuserlhealth.Company_Id)
                       //.Select(x => new AllCompanyDetails
                       {
                           Id = p.ID,
                           CompanyLogo = p.CompanyLogo,
                           CompanyName = p.CompanyName,
                           PlanInfo = p.PlanInfo,
                           BuyNow = p.BuyNowUrl,
                           PolicyXURL = p.CheckPremiumUrl,
                           Status = p.Status,

                       }).ToList();

                  return data;

              }
              catch (Exception ex)
              {

                  Console.WriteLine(ex.Message);
              }

              return new List<CompanyPlanDetails>();



          }
          public List<CompanyPlanDetails> GetPlanBuyDetails(CompanyPlanDetails srchTrvlIns)
          {
              try
              {
                  var data = db.tbl_BuyPlanInfo.AsEnumerable().Where(x => x.Status == srchTrvlIns.Status).Select(
                       p => new CompanyPlanDetails
                       //var data = db.All_Company_Details.Where(x => x.Company_Id == objuserlhealth.Company_Id)
                       //.Select(x => new AllCompanyDetails
                       {
                           Id = p.ID,
                           CompanyLogo = p.CompanyLogo,
                           CompanyName = p.CompanyName,
                           PlanInfo = p.PlanInfo,
                           BuyNow = p.BuyNowUrl,
                           PolicyXURL = p.CheckPremiumUrl,
                           Status = p.Status,
                           PolicyBoucher=p.PolicyBoucher,
                           PolicyWording=p.PolicyWording

                       }).ToList();

                  return data;

              }
              catch (Exception ex)
              {

                  Console.WriteLine(ex.Message);
              }

              return new List<CompanyPlanDetails>();



          }

          public long SaveBuyerDetails(BuyInfoDetails Responcedetail)
          {
              try
              {
                  var tDetails = new tbl_BuyInfoData
                  {

                      Name = Responcedetail.Name,
                      Email = Responcedetail.Email,
                      Phone = Responcedetail.Phone,
                      Status = "True",
                      Type = Responcedetail.Status,
                      Link = Responcedetail.dataforjoin,
                      createDate=DateTime.Now

                  };
                  db.tbl_BuyInfoData.Add(tDetails);
                  db.SaveChanges();
                  var tid = tDetails.ID;
                  return Convert.ToInt64(tid);
              }
              catch (Exception ex)
              {
                  Console.WriteLine(ex.Message);
              }
              return 0;
          }
        
    }
}
