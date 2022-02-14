using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;
using Business;

namespace CheckYourPremiumMVC.Controllers
{
    public class AdminTravelViewController : Controller
    {
        // GET: AdminTravelView

        public Admin1TravelBusiness IAdmin1TravelBusiness;
        public AdminTravelViewController()
        {
            IAdmin1TravelBusiness = new Admin1TravelBusiness();

        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TrevelListData(SearchTravelInsurance objusertravel)
        {
            try
            {
                if (Session["EmailID"] != null)
                {

                }
                else
                {

                    return RedirectToAction("LoginDetails", "RegistrationLogin");
                }
                List<SearchTravelInsurance> registration = IAdmin1TravelBusiness.GetTravelList(objusertravel);
                var d = registration.ToList();
                return View(d);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();

        }
    }
}