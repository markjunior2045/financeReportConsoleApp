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

        public void LoginScreen()
        {
            bool tryLogin = true;
            do
            {
                Console.Clear();
                Title();
                Console.WriteLine("Nome de usuário: ");
                string username = Console.ReadLine();
                Console.WriteLine("Senha: ");
                string password = Console.ReadLine();
                Account account = _accountService.Login(username, password);
                if (account == null)
                {
                    Console.WriteLine("-- Usuário ou senha incorretos!");
                    Console.ReadLine();
                }
                else
                    tryLogin = false;

            } while (tryLogin);
        }
    }
}
