using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceProject
{
    class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int InstallmentsQuantity { get; set; }
        public double InstallmentPrice { get; set; }
        public DateTime BuyDate { get; set; }

        public Item(Guid id, string name, double price, int installmentsQuantity, double installmentPrice, DateTime buyDate)
        {
            Id = id;
            Name = name;
            Price = price;
            InstallmentsQuantity = installmentsQuantity;
            InstallmentPrice = installmentPrice;
            BuyDate = buyDate;
        }

        public Item()
        {
        }
    }
}
