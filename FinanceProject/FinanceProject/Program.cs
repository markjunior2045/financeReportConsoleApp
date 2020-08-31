using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace FinanceProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Server=localhost;Port=3306;Database=finance;Uid=root;Pwd='';";
            var connection = new MySqlConnection(connectionString);
            var command = connection.CreateCommand();
            MySqlDataReader dataReader;
            int op = 0;
            int opMenu = 0;
            string nomeProc;
            List<Item> items = new List<Item>();
            Item novoItem = new Item();
            BaseValues values = new BaseValues();

            Title();
            Console.WriteLine("-- Aperte 1 para entrar ou 0 para sair");
            op = OptionChoose(0, 1);

            if (op == 0)
                return;
            else
            {
                //Conectando com o bd
                try
                {
                    connection.Open();
                    connection.Close();
                }
                catch (Exception error)
                {
                    Console.WriteLine("-- Erro ao conectar com o Banco.");
                    Console.WriteLine();
                    Console.WriteLine(error);
                    Console.ReadLine();
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            do
            {
                Console.Clear();
                values = new BaseValues();
                connection.Open();
                command.CommandText = "SELECT * FROM salary";
                dataReader = command.ExecuteReader();
                dataReader.Read();
                values.Salary = dataReader.GetDouble(0);
                values.SalaryAvailable = dataReader.GetDouble(1);
                connection.Close();
                connection.Open();
                command.CommandText = "SELECT intallmentprice FROM item";
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                    values.TotalSpent += dataReader.GetDouble(0);
                connection.Close();
                values.TotalAvailable = values.SalaryAvailable - values.TotalSpent;

                Title();
                Console.WriteLine("-- Conectado");
                Console.WriteLine("-- Salário Total: " + values.Salary.ToString("F2", CultureInfo.InvariantCulture));
                Console.WriteLine("-- Salário Disponível: " + values.SalaryAvailable.ToString("F2", CultureInfo.InvariantCulture));
                Console.WriteLine("-- Total Gasto: " + values.TotalSpent.ToString("F2", CultureInfo.InvariantCulture));
                Console.WriteLine("-- Total Disponível: " + values.TotalAvailable.ToString("F2", CultureInfo.InvariantCulture));
                Console.WriteLine();
                Menu();

                opMenu = OptionChoose(0, 3);

                if (opMenu == 1)
                {
                    novoItem = new Item();
                    Console.Clear();
                    Title();
                    Console.WriteLine("-- Adicionar Item");
                    try
                    {
                        Console.WriteLine("Nome: ");
                        novoItem.Name = Console.ReadLine();
                        Console.WriteLine("Preço: ");
                        novoItem.Price = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                        Console.WriteLine("Qnt Parcelas: ");
                        novoItem.InstallmentsQuantity = int.Parse(Console.ReadLine());
                        novoItem.InstallmentPrice = novoItem.Price / novoItem.InstallmentsQuantity;
                        novoItem.BuyDate = DateTime.Now;
                        connection.Open();
                        command.Parameters.Clear();
                        command.CommandText = "INSERT INTO item (id, name, price, installments, intallmentprice, buydate) VALUES (@id,@name,@price,@installmentsqtd,@installmentprice,@buydate)";
                        command.Parameters.AddWithValue("@id", Guid.NewGuid());
                        command.Parameters.AddWithValue("@name", novoItem.Name);
                        command.Parameters.AddWithValue("@price", novoItem.Price);
                        command.Parameters.AddWithValue("@installmentsqtd", novoItem.InstallmentsQuantity);
                        command.Parameters.AddWithValue("@installmentprice", novoItem.InstallmentPrice);
                        command.Parameters.AddWithValue("@buydate", novoItem.BuyDate);
                        command.ExecuteNonQuery();
                        connection.Close();
                        Console.WriteLine("-- Adicionado com sucesso!");
                        Console.ReadLine();
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine("-- Erro ao conectar com o Banco.");
                        Console.WriteLine();
                        Console.WriteLine(error);
                        Console.ReadLine();
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                    }


                }
                else if (opMenu == 2)
                {
                    Console.Clear();
                    items.Clear();
                    items = new List<Item>();
                    Title();
                    Console.WriteLine("-- Listar Itens");
                    try
                    {
                        connection.Open();
                        command.Parameters.Clear();
                        command.CommandText = "SELECT name, intallmentprice, buydate FROM item ORDER BY buydate";
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            novoItem = new Item();
                            novoItem.Name = dataReader.GetString(0);
                            novoItem.InstallmentPrice = dataReader.GetDouble(1);
                            novoItem.BuyDate = dataReader.GetDateTime(2);
                            items.Add(novoItem);
                        }
                        connection.Close();
                        items.ForEach(x =>
                        {
                            Console.WriteLine($"-- {x.Name} ---------- R${x.InstallmentPrice.ToString("F2", CultureInfo.InvariantCulture)} --------- {x.BuyDate:dd/MM/yyyy}");
                            Console.WriteLine();
                        });
                        Console.WriteLine($"-- Total: R${values.TotalSpent.ToString("F2", CultureInfo.InvariantCulture)}");
                        Console.ReadLine();
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine("-- Erro ao conectar com o Banco.");
                        Console.WriteLine();
                        Console.WriteLine(error);
                        Console.ReadLine();
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                    }
                }
                else if (opMenu == 3)
                {
                    Console.Clear();
                    Title();
                    Console.WriteLine("-- Editar Item");
                    try
                    {
                        novoItem = new Item();
                        Console.WriteLine("Nome do item: ");
                        nomeProc = Console.ReadLine();
                        connection.Open();
                        command.Parameters.Clear();
                        command.CommandText = $"SELECT * FROM item WHERE name LIKE '%{nomeProc}%'";
                        dataReader = command.ExecuteReader();
                        dataReader.Read();
                        if (dataReader.HasRows)
                        {
                            novoItem.Id = dataReader.GetGuid(0);
                            novoItem.Name = dataReader.GetString(1);
                            novoItem.Price = dataReader.GetDouble(2);
                            novoItem.InstallmentsQuantity = dataReader.GetInt32(3);
                            novoItem.InstallmentPrice = dataReader.GetDouble(4);
                            novoItem.BuyDate = dataReader.GetDateTime(5);
                            connection.Close();
                            Console.WriteLine($"Item: {novoItem.Name}, R${novoItem.Price.ToString("F2", CultureInfo.InvariantCulture)}, {novoItem.InstallmentsQuantity} Parcela(s), {novoItem.BuyDate:dd/MM/yyyy}");
                            Console.WriteLine("-- (1)Item Correto / (2)Item Incorreto");
                            op = OptionChoose(1,2);
                            if (op == 2)
                                break;
                            else if(op == 1)
                            {
                                Console.WriteLine("Nome: ");
                                novoItem.Name = Console.ReadLine();
                                Console.WriteLine("Preço: ");
                                novoItem.Price = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                                Console.WriteLine("Parcelas: ");
                                novoItem.InstallmentsQuantity = int.Parse(Console.ReadLine());
                                Console.WriteLine("Data de compra: ");
                                novoItem.BuyDate = DateTime.Parse(Console.ReadLine());
                            }
                        }
                        else
                        {
                            Console.WriteLine("Item não encontrado!");
                        }
                        if (connection.State == ConnectionState.Open)
                            connection.Close();

                    }
                    catch (Exception error)
                    {
                        Console.WriteLine("-- Erro ao conectar com o Banco.");
                        Console.WriteLine();
                        Console.WriteLine(error);
                        Console.ReadLine();
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                    }
                    Console.ReadLine();
                }
                else if (opMenu == 0)
                {
                    Console.Clear();
                    Title();
                    Console.WriteLine("-- Sair");
                    return;
                }
            } while (opMenu != 0);
        }

        static void Title()
        {
            Console.WriteLine("-- Sistema de Gerenciamento Financeiro Pessoal");
            Console.WriteLine();
        }

        static void Menu()
        {
            Console.WriteLine("-- Qual operação deseja fazer?");
            Console.WriteLine("-- (1) Adicionar uma nova compra");
            Console.WriteLine("-- (2) Listar compras do Mes");
            Console.WriteLine("-- (3) Editar Item");
            Console.WriteLine("-- (0) Sair");
            Console.WriteLine();
        }

        static int OptionChoose(int min, int max)
        {
            int answer = int.MinValue;
            do
            {
                try
                {
                    answer = int.Parse(Console.ReadLine());
                    if (answer < min || answer > max)
                        Console.WriteLine("Opção Inválida!");
                }
                catch
                {
                    Console.WriteLine("Opção Inválida!");
                }

            } while (answer < min || answer > max);

            return answer;
        }
    }
}
