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
            FinanceService _service = new FinanceService();
            Options _options = new Options();
            int op = 0;
            int opMenu = 0;
            //string nomeProc;
            List<Item> items;
            Item novoItem = new Item();
            BaseValues baseValues = new BaseValues();

            _options.Title();
            Console.WriteLine("-- Aperte 1 para iniciar ou 0 para sair");
            op = _options.OptionChoose(0, 1);

            if (op == 0)
                return;
            else
            {
                //Conectando com o bd
                //TODO Tratar caso não consiga conectar com o banco
                _service.TestDatabaseConnection();
            }

            do
            {
                Console.Clear();
                baseValues = _service.GetBaseValues();

                _options.Title();
                Console.WriteLine("-- Conectado com sucesso!");
                Console.WriteLine();
                Console.WriteLine("-- Salário Total: " + baseValues.Salary.ToString("F2", CultureInfo.InvariantCulture));
                Console.WriteLine("-- Salário Disponível: " + baseValues.SalaryAvailable.ToString("F2", CultureInfo.InvariantCulture));
                Console.WriteLine("-- Total Gasto: " + baseValues.TotalSpent.ToString("F2", CultureInfo.InvariantCulture));
                Console.WriteLine("-- Total Disponível: " + baseValues.TotalAvailable.ToString("F2", CultureInfo.InvariantCulture));
                Console.WriteLine();
                _options.Menu();

                //TODO OpMenu ser enum
                opMenu = _options.OptionChoose(0, 4);

                if (opMenu == 1)
                {
                    novoItem = new Item();
                    Console.Clear();
                    _options.Title();
                    Console.WriteLine("-- Adicionar Item");

                    Console.WriteLine("Nome: ");
                    novoItem.Name = Console.ReadLine();
                    Console.WriteLine("Preço: ");
                    novoItem.Price = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    Console.WriteLine("Qnt Parcelas: ");
                    novoItem.InstallmentsQuantity = int.Parse(Console.ReadLine());
                    novoItem.InstallmentPrice = novoItem.Price / novoItem.InstallmentsQuantity;
                    novoItem.BuyDate = DateTime.Now;

                    _service.InsertItem(novoItem);
                }
                else if (opMenu == 2)
                {
                    Console.Clear();
                    _options.Title();
                    Console.WriteLine("-- Listar Itens");
                    items = _service.GetAllItems();
                    if (items.Count == 0)
                    {
                        Console.WriteLine("-- Não existem items na lista! --");
                    }
                    else
                    {
                        items.ForEach(x =>
                        {
                            Console.WriteLine($"-- {x.Name} ---------- R${x.Price.ToString("F2", CultureInfo.InvariantCulture)} --------- {x.BuyDate:dd/MM/yyyy}");
                            Console.WriteLine();
                        });
                        Console.WriteLine($"-- Total: R${baseValues.TotalSpent.ToString("F2", CultureInfo.InvariantCulture)}");
                    };
                    Console.ReadLine();
                }
                else if (opMenu == 3)
                {
                    Console.Clear();
                    _options.Title();

                    Console.WriteLine("-- Editar Item");

                    Console.WriteLine("Nome do item: ");
                    string nomeProc = Console.ReadLine();

                    _service.UpdateItem(nomeProc);

                    Console.ReadLine();
                }
                else if (opMenu == 4)
                {
                    Console.Clear();
                    _options.Title();

                    Console.WriteLine("-- Deletar Item");

                    Console.WriteLine("Nome do Item: ");
                    string nomeProcDel = Console.ReadLine();
                    Console.WriteLine("Preco total do Item: ");
                    double precoProcDel = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    _service.DeleteItem(nomeProcDel, precoProcDel);

                    Console.ReadLine();
                }
                else if (opMenu == 0)
                {
                    Console.Clear();
                    _options.Title();
                    Console.WriteLine("-- Sair");
                    return;
                }
            } while (opMenu != 0);
        }
    }
}
