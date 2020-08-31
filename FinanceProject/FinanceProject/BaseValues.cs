using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceProject
{
    class BaseValues
    {
        public double TotalSpent { get; set; }
        public double TotalAvailable { get; set; }
        public double Salary { get; set; }
        public double SalaryAvailable { get; set; }

        public BaseValues(double totalSpent, double totalAvailable, double salary, double salaryAvailable)
        {
            TotalSpent = totalSpent;
            TotalAvailable = totalAvailable;
            Salary = salary;
            SalaryAvailable = salaryAvailable;
        }

        public BaseValues()
        {
        }
    }
}
