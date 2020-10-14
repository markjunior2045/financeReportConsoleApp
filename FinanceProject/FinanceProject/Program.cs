using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using FinanceProject.Classes;
using FinanceProject.Enum;
using MySql.Data.MySqlClient;

namespace FinanceProject
{
    class Program
    {
        static void Main(string[] args)
        {
            FinanceService _service = new FinanceService();
            Options _options = new Options();
            Account LoggedUser = new Account();



            //Conectando com o bd
            bool testConnection;
            int op;
            do
            {
                testConnection = _service.TestDatabaseConnection();
                if (!testConnection)
                {
                    Console.WriteLine("Erro ao conectar com o banco");
                    Console.WriteLine();
                    Console.WriteLine("Tentar novamente (1) / Sair (0)");
                    op = _options.OptionChoose(0, 1);
                    if (op == 0)
                    {
                        Console.WriteLine("-- A aplicação será encerrada");
                        Console.ReadLine();
                        return;
                    }
                }
            } while (!testConnection);

            Console.Clear();
            _options.Title();
            Console.WriteLine("Logar (0) / Criar conta (1)");
            op = _options.OptionChoose(0, 1);
            if (op == 0)
                LoggedUser = _options.LoginScreen();
            else
                LoggedUser = _options.CreateAccount();
               
            int opMenu;
            do
            {
                Console.Clear();
                BaseValues baseValues = _service.GetBaseValues(LoggedUser.Id);
                if (baseValues == null)
                {
                    Console.WriteLine("-- Erro ao buscar informações no banco. O programa será encerrado.");
                    Console.ReadLine();
                    return;
                }

                _options.Title();
                Console.WriteLine("-- Conectado com sucesso!");
                Console.WriteLine();
                Console.WriteLine("-- Salário Total: " + baseValues.Salary.ToString("F2", CultureInfo.InvariantCulture));
                Console.WriteLine("-- Salário Disponível: " + baseValues.SalaryAvailable.ToString("F2", CultureInfo.InvariantCulture));
                Console.WriteLine("-- Total Gasto: " + baseValues.TotalSpent.ToString("F2", CultureInfo.InvariantCulture));
                Console.WriteLine("-- Total Disponível: " + baseValues.TotalAvailable.ToString("F2", CultureInfo.InvariantCulture));
                Console.WriteLine();
                _options.Menu();

                opMenu = _options.OptionChoose(0, 4);

                if (opMenu == (int)MenuEnum.NewItem)
                {
                    Item novoItem = new Item();
                    Console.Clear();
                    _options.Title();
                    Console.WriteLine("-- Adicionar Item");
                    Console.WriteLine();
                    Console.WriteLine("Nome: ");
                    novoItem.Name = Console.ReadLine();
                    Console.WriteLine("Preço: ");
                    novoItem.Price = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    novoItem.BuyDate = DateTime.Now;
                    novoItem.AccountId = LoggedUser.Id;
                    _service.InsertItem(novoItem);
                }
                else if (opMenu == (int)MenuEnum.ListItems)
                {
                    Console.Clear();
                    _options.Title();
                    Console.WriteLine("-- Listar Itens");
                    Console.WriteLine();
                    if (_service.HaveItems(LoggedUser.Id))
                    {
                        _service.ListAllItems(LoggedUser.Id);
                        Console.WriteLine($"-- Total: R${baseValues.TotalSpent.ToString("F2", CultureInfo.InvariantCulture)}");
                    }
                    else
                        Console.WriteLine("-- Não existem items na lista! --");

                    Console.ReadLine();
                }
                else if (opMenu == (int)MenuEnum.EditItem)
                {
                    Console.Clear();
                    _options.Title();

                    Console.WriteLine("-- Editar Item");
                    Console.WriteLine();
                    if (_service.HaveItems(LoggedUser.Id))
                    {
                        _service.ListAllItems(LoggedUser.Id);
                        Console.WriteLine("Nome do item para editar: ");
                        string nomeProc = Console.ReadLine();

                        _service.UpdateItem(nomeProc, LoggedUser.Id);
                    }
                    else
                        Console.WriteLine("-- Não existem items na lista! --");

                    Console.ReadLine();
                }
                else if (opMenu == (int)MenuEnum.DeleteItem)
                {
                    Console.Clear();
                    _options.Title();

                    Console.WriteLine("-- Deletar Item");
                    Console.WriteLine();
                    if (_service.HaveItems(LoggedUser.Id))
                    {
                        _service.ListAllItems(LoggedUser.Id);
                        Console.WriteLine("Nome do Item: ");
                        string nomeProcDel = Console.ReadLine();
                        Console.WriteLine("Preco total do Item: ");
                        double precoProcDel = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                        _service.DeleteItem(nomeProcDel, precoProcDel, LoggedUser.Id);
                    }
                    else
                        Console.WriteLine("-- Não existem items na lista! --");

                    Console.ReadLine();
                }
                else if (opMenu == (int)MenuEnum.Exit)
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
