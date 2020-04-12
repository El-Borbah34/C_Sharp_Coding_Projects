using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarInsurance.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
            
        }

        public ActionResult GenerateQuote(string firstName, string lastName, string emailAddress, DateTime dateOfBirth, int carYear, string carMake, string carModel, bool dui, int speedingTickets, string coverageType)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(carMake) || string.IsNullOrEmpty(carModel))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            var client = new Client()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                DateOfBirth = dateOfBirth,
                CarYear = carYear,
                CarMake = carMake,
                CarModel = carModel,
                Dui = dui,
                SpeedingTickets = speedingTickets,
                Coverage = coverageType
            };

            var today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;
            decimal quoteEstimate = 50;

            if (age <= 25) quoteEstimate += 25;
            if (age < 18) quoteEstimate += 100;
            if (age > 100) quoteEstimate += 25;
            if (carYear < 2000) quoteEstimate += 25;
            if (carYear > 2015) quoteEstimate += 25;
            if (carMake.ToLower().Equals("porsche")) quoteEstimate += 25;
            if (carMake.ToLower().Equals("porsche") && carModel.ToLower().Equals("carrera")) quoteEstimate += 25;
            if (speedingTickets > 0) quoteEstimate += (speedingTickets * 10);
            if (dui == true) quoteEstimate *= 1.25m;
            if (coverageType.ToLower() == "full coverage") quoteEstimate *= 1.50m;

            using (CarInsuranceEntities db = new CarInsuranceEntities())
            {
                var newClient = new Client();
                newClient = client;
                newClient.Quote = quoteEstimate;

                db.Clients.Add(newClient);
                db.SaveChanges();
            }
            return View("Success");
        } 
    }
}