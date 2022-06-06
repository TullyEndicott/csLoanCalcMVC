using csLoanCalcMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csLoanCalcMVC.Helpers
{
    public class LoanHelper
    {

        public Loan GetPayments(Loan loan)
        {
            //var payments = new List<Loan>(); 
            //payments.Add(loan);
            // Calculate monthly payment
            loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);

            //Create loop from 1 to the term
            var balance = loan.Amount;
            var totalInterest = 0.0m; //interest paid to date
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m; //amount of payment applied to balance
            var monthlyRate = CalcMonthlyRate(loan.Rate);

            //Loop over each month until we reach the term
            for (int Month = 1; Month <= loan.Term; Month++)
            {
                monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                LoanPayment loanPayment = new LoanPayment();

                loanPayment.Month = Month;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyPrincipal = monthlyPrincipal;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;

                // push the object into loan model
                loan.Payments.Add(loanPayment);

            }
            // not in amortization table
            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest; //laon amount plus accumulating interest

            //return the loan to the view
            return loan;
        }

        private decimal CalcPayment(decimal amount, decimal rate, int term)
        {

            var monthlyRate = CalcMonthlyRate(rate);

            var rateD = Convert.ToDouble(monthlyRate);

            var amountD = Convert.ToDouble(amount);
            // let payment = loanAmt*(i/1200)*Math.pow((1+i/1200), months) / (Math.pow((1+i/1200), months) - 1);
            // var paymentD = (amountD * rateD) * Math.Pow((1 + rateD), term) / (Math.Pow((1 + rateD), term) - 1);

            var paymentD = (amountD * rateD) / (1 - Math.Pow(1 + rateD, -term));

            return Convert.ToDecimal(paymentD);
        }

        private decimal CalcMonthlyRate(decimal rate) //annual percentage rate to monthly (4.5 to 0.00375)
        {
            return rate / 1200;
        }

        private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;
        }

    }
}
