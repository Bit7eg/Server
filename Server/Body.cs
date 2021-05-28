using System;
using System.Collections;
using System.Resources.NetStandard;
using System.IO;

namespace First_task
{
    class Program
    {
        private static Catalog main = new Catalog();
        private static Catalog current = main;
        private static string output = "";
        private static string path = "";
        private static string command = "";
        private static string part = "";
        private static bool isError = false;
        static void NextCatalog(string name)
        {
            current = current.FindCatalog(name); //Найти каталог по имени
            if (current == null) PreviosCatalog();
        }

        static void PreviosCatalog()
        {
            current = main;
            for (int i = 0, j = 1; j < path.Length; i++)
                if ((i + 1 >= path.Length) || (path[i + 1] == '\x1'))
                {
                    NextCatalog(path.Substring(j, i - j + 1));
                    j = i + 2;
                }
        }
        /// <summary>
        /// Отделяет параметр от строки
        /// </summary>
        /// <param name="parametr">куда сохранить параметр</param>
        /// <param name="str">Исходная строка</param>
        static void GetParametrFromString(ref string parametr, ref string str)
        {
            while ((str != "") && (str[0] != ' '))
            {
                parametr += str[0];
                str = str.Remove(0, 1);
            }
        }
        /// <summary>
        /// Считывание параметров
        /// </summary>
        /// <param name="data">массив для записи</param>
        /// <param name="number">число параметров</param>
        static bool ReadParameters(string[] data, int number)
        {
            if (number == 1)
            {
                if ((command == "") || (command[0] == ' ')) //Проверка, есть ли параметр
                {
                    output += "Missing parameter name\n";
                    return false;
                }
                GetParametrFromString(ref data[0], ref command); //Считывание параметра до первого пробела
            }
            else if (number == 2)
            {
                if ((command == "") || (command[0] == ' ')) //Проверка, есть ли параметр
                {
                    output += "Missing parameters name, count\n";
                    return false;
                }
                GetParametrFromString(ref data[0], ref command); //Считывание параметра до первого пробела
                if (command != "") command = command.Remove(0, 1); //удаление пробела

                if (command == "") //Проверка, есть ли параметр
                {
                    output += "Missing parameters count\n";
                    return false;
                }
                while ((command != "") && (command[0] != ' ')) //Проверка параметра на корректность
                {
                    if ((command[0] < '0') || (command[0] > '9'))
                    {
                        output += "Only numeric values can be assigned to count\n";
                        return false;
                    }
                    data[1] += command[0];
                    command = command.Remove(0, 1);
                }

                if ((command != "") && (command[0] != ' ')) return false;
            }
            else if (number == 3)
            {
                if ((command == "") || (command[0] == ' ')) //Проверка всех параметров
                {
                    output += "Missing parameters name, count, cost\n";
                    return false;
                }
                GetParametrFromString(ref data[0], ref command); //Считывание параметра до первого пробела
                if (command != "") command = command.Remove(0, 1);

                if (command == "")
                {
                    output += "Missing parameters count, cost\n";
                    return false;
                }
                while ((command != "") && (command[0] != ' '))
                {
                    if ((command[0] < '0') || (command[0] > '9'))
                    {
                        output += "Only numeric values can be assigned to count\n";
                        return false;
                    }
                    data[1] += command[0];
                    command = command.Remove(0, 1);
                }
                if (command == "")
                {
                    output += "Missing parameter cost\n";
                    return false;
                }
                else if (command[0] == ' ') command = command.Remove(0, 1);
                else return false;

                while ((command != "") && (command[0] != ' '))
                {
                    if ((command[0] < '0') || (command[0] > '9'))
                    {
                        output += "Only numeric values can be assigned to cost\n";
                        return false;
                    }
                    data[2] += command[0];
                    command = command.Remove(0, 1);
                }
                if ((command != "") && (command[0] != ' ')) return false;
            }
            else return false;
            return true;
        }
        /// <summary>
        /// Справка по данному модулю
        /// </summary>
        static void ShowHelp()
        {
            output += "\nacat - добавить каталог name\n" +
                "ag - добавить товар name count price\n" +
                "chct - изменить количество товаров name new_count\n" +
                "dcat - удалить каталог name\n" +
                "dg - удалить товар name\n" +
                "open - перейти в каталог name\n" +
                "back - вернуться вверх по иерархии\n" +
                "sall - вывести на экран все каталоги с информацией о количестве товаров и их общей стоимости\n" +
                "exit - выход\n" +
                "При вводе названий, избегайте пробелов, например, используйте нижнее подчеркивание '_'\n\n";
        }
        static void LoadCatalogs()
        {
            try
            {
                using (ResXResourceReader resxReader = new ResXResourceReader(@".\data.resx"))
                {
                    IDictionaryEnumerator dictionary = resxReader.GetEnumerator();
                    while (dictionary.MoveNext())
                    {
                        if (((string)dictionary.Key) == "main")
                        {
                            main = (Catalog)dictionary.Value;
                            current = main;
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                using (ResXResourceWriter resxWriter = new ResXResourceWriter(@".\data.resx"))
                {
                    resxWriter.AddResource("main", main);
                }
            }
        }
        static void SaveCatalogs()
        {
            using (ResXResourceWriter resxWriter = new ResXResourceWriter(@".\data.resx"))
            {
                resxWriter.AddResource("main", main);
            }
        }
        static void Execute(string str)
        {
            LoadCatalogs();
            string[] data = new string[3]; //массив данных: название - кол-во - стоимость
            while (part != "exit")
            {
                if (part != "sall")
                {
                    if (part != "help")
                    {
                        SaveCatalogs();
                        output = "";
                    }
                }
                output = output.Insert(0, current.PrintCatalogContent()); //вывести содержимое каталога
                output += ">"; //Приглашение
                Console.Clear();
                Console.Write(output);
                output = "";
                command = Console.ReadLine();
                part = ""; //Часть от введенной строки, отвечающая за команду
                GetParametrFromString(ref part, ref command); //Считывание параметра до первого пробела
                if (command != "") command = command.Remove(0, 1); //удалить пробел

                data[0] = data[1] = data[2] = ""; //заполнение данных пробелами
                if (part == "acat") //Добавить каталог
                    if (ReadParameters(data, 1)) current.AddCatalog((new Catalog(data[0])));
                    else isError = true;
                else if (part == "ag") //Добавить товар
                    if (ReadParameters(data, 3)) current.AddGoods(new Goods(data));
                    else isError = true;
                else if (part == "chct") //Изменить кол-во
                    if (ReadParameters(data, 2)) current.ChangeGoods(data[0], int.Parse(data[1]));
                    else isError = true;
                else if (part == "dcat") //Удалить каталог
                    if (ReadParameters(data, 1)) current.DeleteCatalog(data[0]);
                    else isError = true;
                else if (part == "dg") //Удалить товар
                    if (ReadParameters(data, 1)) current.DeleteGoods(data[0]);
                    else isError = true;
                else if (part == "open") //Открыть каталог
                    if (ReadParameters(data, 1))
                    {
                        NextCatalog(data[0]);
                        path += '\x1' + data[0];
                    }
                    else isError = true;
                else if (part == "back")
                    if (path.Length == 0)
                    {
                        output += "This is root\n";
                        isError = true;
                    }
                    else
                    {
                        path = path.Remove(path.LastIndexOf('\x1'));
                        PreviosCatalog();
                    }
                else if (part == "sall")
                {
                    output += "\n" + current.PrintAllCatalogsContent();
                }
                else if (part == "help")
                {
                    ShowHelp();
                }
                else if (part != "exit")
                {
                    output += "Unknown command. You can see help.\n"; //Не распознана команда
                    isError = true;
                }

                if (isError)
                {
                    output += "\nPress any key to continue\n";
                    Console.ReadKey();
                    isError = false;
                }
            }
            SaveCatalogs();
        }
    }
}