using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceProject
{
    class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int InstallmentsQuantity { get; set; }
        public DateTime BuyDate { get; set; }

        public Item(int id, string name, double price, int installmentsQuantity, DateTime buyDate)
        {
            Id = id;
            Name = name;
            Price = price;
            InstallmentsQuantity = installmentsQuantity;
            BuyDate = buyDate;
        }

        public Item()
        {
        }
    }
}
