using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpargoTech_Test.Control;
    
namespace SpargoTech_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var commands = new Commands();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(@"АО ""Спарго технологии""" + "\n" + "Тестовое задание (Разработчик C#)" + "\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("+++ АПТЕКА +++\n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Команды приложения:\nadd_product - создать товар \n"
                                + "del_product - удалить товар и все партии во всех аптеках, связанные с ним \n"
                                + "add_pharmacy - создать аптеку \n"
                                + "del_pharmacy - удалить аптеку, все склады аптеки и партии на складах\n"
                                + "add_warehouse - создать склад \n"
                                + "del_warehouse - удалить склад, все данные о партиях на этом складе \n"
                                + "add_batch - добавить партию \n"
                                + "del_batch - удалить партию \n"
                                + "sel_all_products - вывести список товаров и количество в выбранной аптеке \n");
            while (true)
            {
                var command = Console.ReadLine();
                switch (command)
                {
                    case "add_product":
                        Console.WriteLine("ДОБАВЛЕНИЕ ТОВАРА");
                        Console.WriteLine("Введите наименование товара: ");
                        Console.WriteLine(commands.AddProduct(Console.ReadLine())+"\n");
                        break;
                    case "del_product":
                        Console.WriteLine("УДАЛЕНИЕ ТОВАРА");
                        Console.WriteLine("Введите наименование товара: ");
                        Console.WriteLine(commands.DeleteProduct(Console.ReadLine()) + "\n");
                        break;
                    case "add_pharmacy":
                        Console.WriteLine("ДОБАВЛЕНИЕ АПТЕКИ");
                        Console.WriteLine("Введите наименование аптеки: ");
                        var pharmacyName = Console.ReadLine();
                        Console.WriteLine("Введите адрес аптеки: ");
                        var pharmacyAddress = Console.ReadLine();
                        Console.WriteLine("Введите номер телефона аптеки: ");
                        var pharmacyPhone = Console.ReadLine();
                        Console.WriteLine(commands.AddPharmacy(pharmacyName, pharmacyAddress, pharmacyPhone) + "\n");
                        break;
                    case "del_pharmacy":
                        Console.WriteLine("УДАЛЕНИЕ АПТЕКИ");
                        Console.WriteLine("Введите наименование аптеки: ");
                        pharmacyName = Console.ReadLine();
                        Console.WriteLine(commands.DeletePharmacy(pharmacyName) + "\n");
                        break;
                    case "add_warehouse":
                        Console.WriteLine("ДОБАВЛЕНИЕ СКЛАДА");
                        Console.WriteLine("Введите наименование аптеки: ");
                        pharmacyName = Console.ReadLine();
                        Console.WriteLine("Введите наименование склада: ");
                        var warehouseName = Console.ReadLine();
                        Console.WriteLine(commands.AddWarehouse(pharmacyName, warehouseName) + "\n");
                        break;
                    case "del_warehouse":
                        Console.WriteLine("УДАЛЕНИЕ СКЛАДА");
                        Console.WriteLine("Введите наименование склада: ");
                        warehouseName = Console.ReadLine();
                        Console.WriteLine(commands.DeleteWarehouse(warehouseName) + "\n");
                        break;
                    case "add_batch":
                        Console.WriteLine("ДОБАВЛЕНИЕ ПАРТИИ");
                        Console.WriteLine("Введите наименование склада: ");
                        warehouseName = Console.ReadLine();
                        Console.WriteLine("Введите наименование товара: ");
                        var productName = Console.ReadLine();
                        Console.WriteLine("Введите количество товара: ");
                        var quantity = Console.ReadLine();
                        int q=0;
                        if (Int32.TryParse(quantity, out q))
                            Console.WriteLine(commands.AddBatch(productName, warehouseName, q) + "\n");
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Количество задано неверно...\n");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    case "del_batch":
                        Console.WriteLine("УДАЛЕНИЕ ПАРТИИ");
                        Console.WriteLine("Введите ID партии: ");
                        var batchID = Console.ReadLine();
                        Console.WriteLine(commands.DeleteBatch(batchID) + "\n");
                        break;
                    case "sel_all_products":
                        Console.WriteLine("ВСЕ ТОВАРЫ АПТЕКИ");
                        Console.WriteLine("Введите название аптеки: ");
                        pharmacyName = Console.ReadLine();
                        DataTable dt = commands.SelAllProduct(pharmacyName);
                        foreach (DataRow r in dt.Rows)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{r[0]}{r[1]}");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.WriteLine("");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Команда не поддерживается...\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
        }
    }
}
