using CarInsurance.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarInsurance.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (CarInsuranceEntities db = new CarInsuranceEntities())
            {
                var allClients = (from c in db.Clients select c).ToList();
                var viewClients = new List<ClientVm>();
                foreach (var client in allClients)
                {
                    var viewClient = new ClientVm();
                    viewClient.Id = client.Id;
                    viewClient.FirstName = client.FirstName;
                    viewClient.LastName = client.LastName;
                    viewClient.EmailAddress = client.EmailAddress;
                    viewClient.Quote = client.Quote;
                    viewClients.Add(viewClient);
                }
                
                return View(viewClients);
            }
        }
    }
}