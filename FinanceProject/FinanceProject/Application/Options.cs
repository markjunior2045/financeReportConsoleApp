using FinanceProject.Application;
using FinanceProject.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceProject
{
    class Options
    {
        AccountService _accountService = new AccountService();

        public void Title()
        {
            Console.WriteLine("-- Sistema de Gerenciamento Financeiro Pessoal");
            Console.WriteLine();
        }

        public void Menu()
        {
            Console.WriteLine("-- Qual operação deseja fazer?");
            Console.WriteLine("-- (1) Adicionar uma nova compra");
            Console.WriteLine("-- (2) Listar compras do Mes");
            Console.WriteLine("-- (3) Editar Item");
            Console.WriteLine("-- (4) Deletar Item");
            Console.WriteLine("-- (0) Sair");
            Console.WriteLine();
        }

        public int OptionChoose(int min, int max)
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

        public Account LoginScreen()
        {
            bool tryLogin = true;
            Account account;
            do
            {
                Console.Clear();
                Title();
                Console.WriteLine("-- Nome de usuário: ");
                string username = Console.ReadLine();
                Console.WriteLine("-- Senha: ");
                string password = Console.ReadLine();
                account = _accountService.Login(username, password);
                if (account == null)
                {
                    Console.WriteLine("-- Usuário ou senha incorretos! Aperte enter e tente novamente.");
                    Console.ReadLine();
                }
                else
                    tryLogin = false;

            } while (tryLogin);

            return account;
        }

        public Account CreateAccount()
        {
            bool tryLogin = true;
            Account newAccount = new Account();
            do
            {
                Console.Clear();
                Title();
                Console.WriteLine("-- Nome de usuário (Max 15 dig.): ");
                string username = Console.ReadLine();
                Console.WriteLine("-- Senha (Max 10 dig.): ");
                string password = Console.ReadLine();
                Console.WriteLine("-- Salário: ");
                double salary = double.Parse(Console.ReadLine());
                Console.WriteLine("-- Quanto a ser guardado? (Padrão = 30): ");
                string save = Console.ReadLine();
                if (save == string.Empty)
                    save = "30";

                if (username.Length > 15 || password.Length > 10)
                {
                    Console.WriteLine("-- Usuário ou senha inválidos! Aperte enter e tente novamente.");
                    Console.ReadLine();
                }
                else
                {
                    newAccount = _accountService.CreateAccount(username, password,salary,int.Parse(save));
                    tryLogin = false;
                }
            } while (tryLogin);

            return newAccount;
        }
    }
}
