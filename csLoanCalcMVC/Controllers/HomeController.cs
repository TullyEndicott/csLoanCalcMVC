using csLoanCalcMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using csLoanCalcMVC.Helpers;

namespace csLoanCalcMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult App() 
        { 
            Loan loan = new();

            loan.Payment = 0.0m;
            loan.TotalInterest = 0.0m;
            loan.TotalCost = 0.0m;
            loan.Rate = 3.5m;
            loan.Amount = 15000.0m;
            loan.Term = 60;

            return View(loan); 
        } 
        
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public IActionResult App(Loan loan) //Loan are values from form; loan are values to be returned
        {
            //Calculate loan and get the payments
            var loanHelper = new LoanHelper();
            
            Loan newLoan = loanHelper.GetPayments(loan);    

            return View(newLoan);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
