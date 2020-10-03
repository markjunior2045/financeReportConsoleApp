using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace FinanceProject
{
    class FinanceService
    {
        private readonly MySqlConnection Connection = new MySqlConnection("Server=localhost;Port=3306;Database=finance;Uid=root;Pwd='';");
        private readonly MySqlCommand Command;
        private MySqlDataReader DataReader;

        public FinanceService()
        {
            Command = Connection.CreateCommand();
        }

        public bool TestDatabaseConnection()
        {
            try
            {
                Connection.Open();
                Connection.Close();
                return true;
            }
            catch
            {
                Console.WriteLine("-- Erro ao conectar com o Banco.");
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                return false;
            }
        }

        public void InsertItem(Item item)
        {
            if (item == null)
            {
                Console.WriteLine("-- Erro ao adicionar item!");
                return;
            }

            try
            {
                Connection.Open();
                Command.Parameters.Clear();
                Command.CommandText = "INSERT INTO item (id, name, price, installments, installmentprice, buydate) VALUES (@id,@name,@price,@installmentsqtd,@installmentprice,@buydate)";
                Command.Parameters.AddWithValue("@id", Guid.NewGuid());
                Command.Parameters.AddWithValue("@name", item.Name);
                Command.Parameters.AddWithValue("@price", item.Price);
                Command.Parameters.AddWithValue("@installmentsqtd", item.InstallmentsQuantity);
                Command.Parameters.AddWithValue("@installmentprice", item.InstallmentPrice);
                Command.Parameters.AddWithValue("@buydate", item.BuyDate);
                Command.ExecuteNonQuery();
                Connection.Close();
                Console.WriteLine("-- Adicionado com sucesso!");
                Console.ReadLine();
            }
            catch (Exception error)
            {
                Console.WriteLine("-- Erro ao conectar com o Banco.");
                Console.WriteLine();
                Console.WriteLine(error);
                Console.ReadLine();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
        }

        public void UpdateItem(string nomeProc)
        {
            try
            {
                Item novoItem = new Item();
                Options options = new Options();

                Connection.Open();
                Command.Parameters.Clear();
                Command.CommandText = $"SELECT * FROM item WHERE name LIKE '%{nomeProc}%'";
                DataReader = Command.ExecuteReader();
                DataReader.Read();
                if (DataReader.HasRows)
                {
                    novoItem.Id = DataReader.GetGuid(0);
                    novoItem.Name = DataReader.GetString(1);
                    novoItem.Price = DataReader.GetDouble(2);
                    novoItem.InstallmentsQuantity = DataReader.GetInt32(3);
                    novoItem.InstallmentPrice = DataReader.GetDouble(4);
                    novoItem.BuyDate = DataReader.GetDateTime(5);
                    Connection.Close();
                    Console.WriteLine($"Item: {novoItem.Name}, R${novoItem.Price.ToString("F2", CultureInfo.InvariantCulture)}, {novoItem.InstallmentsQuantity} Parcela(s), {novoItem.BuyDate:dd/MM/yyyy}");
                    Console.WriteLine("-- (1)Item Correto / (2)Item Incorreto");
                    int op = options.OptionChoose(1, 2);
                    if (op == 2)
                        return;
                    else if (op == 1)
                    {
                        Console.WriteLine("Nome: ");
                        novoItem.Name = Console.ReadLine();
                        Console.WriteLine("Preço: ");
                        novoItem.Price = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                        Console.WriteLine("Parcelas: ");
                        novoItem.InstallmentsQuantity = int.Parse(Console.ReadLine());
                        novoItem.InstallmentPrice = novoItem.Price / novoItem.InstallmentsQuantity;
                        //Console.WriteLine("Data de compra: ");
                        novoItem.BuyDate = DateTime.Now;

                        Connection.Open();
                        Command.Parameters.Clear();
                        Command.CommandText = "UPDATE item SET name = @name, price = @price, installments = @installmentsqtd, installmentprice = @installmentprice, buydate = @buydate WHERE id = @id";
                        Command.Parameters.AddWithValue("@id", novoItem.Id);
                        Command.Parameters.AddWithValue("@name", novoItem.Name);
                        Command.Parameters.AddWithValue("@price", novoItem.Price);
                        Command.Parameters.AddWithValue("@installmentsqtd", novoItem.InstallmentsQuantity);
                        Command.Parameters.AddWithValue("@installmentprice", novoItem.InstallmentPrice);
                        Command.Parameters.AddWithValue("@buydate", novoItem.BuyDate);
                        Command.ExecuteNonQuery();
                        Connection.Close();
                        Console.WriteLine("-- Item atualizado com sucesso!");
                    }
                }
                else
                {
                    Console.WriteLine("-- Item não encontrado!");
                }
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

            }
            catch (Exception error)
            {
                Console.WriteLine("-- Erro ao conectar com o Banco.");
                Console.WriteLine();
                Console.WriteLine(error);
                Console.ReadLine();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
        }

        public void DeleteItem(string itemName, double itemPrice)
        {
            try
            {
                Item toDeleteItem = new Item();
                Options options = new Options();

                Connection.Open();
                Command.Parameters.Clear();
                Command.CommandText = $"SELECT * FROM item WHERE name LIKE '%{itemName}%' AND price LIKE '%{itemPrice.ToString(CultureInfo.InvariantCulture)}%'";
                DataReader = Command.ExecuteReader();
                DataReader.Read();
                if (DataReader.HasRows)
                {
                    toDeleteItem.Id = DataReader.GetGuid(0);
                    toDeleteItem.Name = DataReader.GetString(1);
                    toDeleteItem.Price = DataReader.GetDouble(2);
                    toDeleteItem.InstallmentsQuantity = DataReader.GetInt32(3);
                    toDeleteItem.InstallmentPrice = DataReader.GetDouble(4);
                    toDeleteItem.BuyDate = DataReader.GetDateTime(5);
                    Connection.Close();
                    Console.WriteLine($"Item: {toDeleteItem.Name}, R${toDeleteItem.Price.ToString("F2", CultureInfo.InvariantCulture)}, {toDeleteItem.InstallmentsQuantity} Parcela(s), {toDeleteItem.BuyDate:dd/MM/yyyy}");
                    Console.WriteLine("-- (1)Item Correto / (2)Item Incorreto");
                    int op = options.OptionChoose(1, 2);
                    if (op == 2)
                        return;
                    else if (op == 1)
                    {
                        Console.WriteLine("-- Tem certeza que deseja DELETAR este item? (1)Sim / (2)Cancelar");
                        op = options.OptionChoose(1, 2);
                        if (op == 2)
                            return;
                        else
                        {
                            Connection.Open();
                            Command.Parameters.Clear();
                            Command.CommandText = "DELETE FROM item WHERE id = @id";
                            Command.Parameters.AddWithValue("@id", toDeleteItem.Id);
                            Command.ExecuteNonQuery();
                            Connection.Close();
                            Console.WriteLine("-- Item deletado com sucesso!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("-- Item não encontrado!");
                }
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

            }
            catch (Exception error)
            {
                Console.WriteLine("-- Erro ao conectar com o Banco.");
                Console.WriteLine();
                Console.WriteLine(error);
                Console.ReadLine();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
        }
        public List<Item> GetAllItems()
        {
            Item novoItem;
            List<Item> items = new List<Item>();

            try
            {
                Connection.Open();
                Command.Parameters.Clear();
                Command.CommandText = "SELECT name, price, installmentprice, buydate FROM item ORDER BY buydate";
                DataReader = Command.ExecuteReader();
                while (DataReader.Read())
                {
                    novoItem = new Item();
                    novoItem.Name = DataReader.GetString(0);
                    novoItem.Price = DataReader.GetDouble(1);
                    novoItem.InstallmentPrice = DataReader.GetDouble(2);
                    novoItem.BuyDate = DataReader.GetDateTime(3);
                    items.Add(novoItem);
                }
                Connection.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine("-- Erro ao conectar com o Banco.");
                Console.WriteLine();
                Console.WriteLine(error);
                Console.ReadLine();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

            return items;
        }

        public bool ListAllItems()
        {
            List<Item> items = GetAllItems();

            if (items.Count == 0)
            {
                Console.WriteLine("-- Não existem items na lista! --");
                return false;
            }
            else
            {
                items.ForEach(x =>
                {
                    Console.WriteLine($"-- {x.Name} ---------- R${x.Price.ToString("F2", CultureInfo.InvariantCulture)} --------- {x.BuyDate:dd/MM/yyyy}");
                    Console.WriteLine();
                });
                return true;
            };
        }
        public BaseValues GetBaseValues()
        {
            BaseValues baseValues = new BaseValues();

            Connection.Open();
            Command.CommandText = "SELECT * FROM salary";
            DataReader = Command.ExecuteReader();
            DataReader.Read();
            baseValues.Salary = DataReader.GetDouble(0);
            baseValues.SalaryAvailable = DataReader.GetDouble(1);
            Connection.Close();

            Connection.Open();
            Command.CommandText = "SELECT installmentprice FROM item";
            DataReader = Command.ExecuteReader();
            while (DataReader.Read())
                baseValues.TotalSpent += DataReader.GetDouble(0);
            Connection.Close();

            baseValues.TotalAvailable = baseValues.SalaryAvailable - baseValues.TotalSpent;

            return baseValues;
        }
    }
}
