using System;
using System.Collections.Generic;
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
            int lastId = 1;
            List<Item> items = new List<Item>();
            Item novoItem;

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
                catch
                {
                    Console.WriteLine("-- Erro ao conectar com o Banco.");
                    return;
                }
            }
            do
            {
                Console.Clear();
                Title();
                Console.WriteLine("-- Conectado");
                Console.WriteLine();
                Menu();

                opMenu = OptionChoose(1, 3);

                if (opMenu == 1)
                {
                    Console.Clear();
                    Title();
                    Console.WriteLine("-- Adicionar Item");
                    Console.ReadLine();
                    try
                    {
                        connection.Open();
                        command.CommandText = "SELECT id FROM item ORDER BY id";
                        dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                            lastId = dataReader.GetInt32(0);
                        Console.WriteLine(lastId);
                        Console.ReadLine();
                        connection.Close();
                    }
                    catch
                    {
                        Console.WriteLine("-- Erro ao conectar com o Banco.");
                    }


                }
                else if (opMenu == 2)
                {
                    Console.Clear();
                    Title();
                    Console.WriteLine("-- Listar Itens");
                    Console.ReadLine();
                }
                else if (opMenu == 3)
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
            Console.WriteLine("-- (3) Sair");
            Console.WriteLine();
        }

        static int OptionChoose(int min, int max)
        {
            int answer;
            do
            {
                answer = int.Parse(Console.ReadLine());
                if (answer < min || answer > max)
                    Console.WriteLine("Opção Inválida!");
            } while (answer < min || answer > max);

            return answer;
        }
    }
}
