using System;
using System.Collections.Generic;

namespace Server
{
    [Serializable]
    public class Catalog : IComparable<Catalog>
    {
        public string Name { get; private set; } //название
        private SortedSet<Catalog> internalCatalogs; //Отсортированный список каталогов
        private SortedSet<Goods> internalGoods; //Отсортированный список товаров
        public Catalog(string name = "root", SortedSet<Catalog> contentCatalogs = null, SortedSet<Goods> contentGoods = null)
        {
            Name = name;
            if (contentCatalogs == null) internalCatalogs = new SortedSet<Catalog>();
            else internalCatalogs = contentCatalogs;

            if (contentGoods == null) internalGoods = new SortedSet<Goods>();
            else internalGoods = contentGoods;
        }
        public void AddCatalog(Catalog catalog)
        {
            internalCatalogs.Add(catalog);
        }
        public void AddGoods(Goods goods)
        {
            internalGoods.Add(goods);
        }
        public void DeleteCatalog(string catalogName)
        {
            Catalog catalog = FindCatalog(catalogName);
            if (catalog != null) internalCatalogs.Remove(catalog);
        }
        public void DeleteGoods(string goodsName)
        {
            Goods goods = FindGoods(goodsName);
            if (goods != null) internalGoods.Remove(goods);
        }
        public void ChangeGoods(string goodsName, int count)
        {
            Goods goods = FindGoods(goodsName);
            if (goods != null) goods.ChangeCount(count);
        }
        public Catalog FindCatalog(string name)
        {
            if (internalCatalogs != null)
                foreach (var item in internalCatalogs)
                    if (item.Name == name)
                    {
                        return item;
                    }
            return null;
        }
        public Goods FindGoods(string name)
        {
            if (internalGoods != null)
                foreach (var item in internalGoods)
                    if (item.Name == name)
                    {
                        return item;
                    }
            return null;
        }
        public int[] Info()
        {
            int[] data = new int[2] { 0, 0 };
            int[] info;
            if (internalGoods != null)
                foreach (var item in internalGoods)
                {
                    info = item.Info();
                    data[0] += info[0];
                    data[1] += info[1] * info[0];
                }
            if (internalCatalogs != null)
                foreach (var item in internalCatalogs)
                {
                    info = item.Info();
                    data[0] += info[0];
                    data[1] += info[1];
                }
            return data;
        }
        public string PrintAllCatalogsContent(int degree = 0)
        {
            string output = "";
            int[] data;
            if (internalCatalogs != null)
                foreach (var item in internalCatalogs)
                {
                    data = item.Info();
                    for (int i = 0; i < degree; ++i)
                        output += "\t";
                    output += "[" + item.Name + "]" + " содержит " + data[0].ToString() + " товаров на общую сумму " + data[1].ToString() + "\n" + item.PrintAllCatalogsContent(degree + 1);
                }
            if (degree == 0)
            {
                data = Info();
                output += "В текущем каталоге содержится " + data[0].ToString() + " товаров на общую сумму " + data[1].ToString() + "\n";
            }
            if (internalGoods != null)
                foreach (var item in internalGoods)
                {
                    data = item.Info();
                    for (int i = 0; i < degree; ++i)
                        output += "\t";
                    output += "[" + item.Name + "]" + " стоимостью " + data[1].ToString() + " находится на складе в количестве " + data[0].ToString() + "\n";
                }
            return output;
        }
        public string PrintCatalogContent()
        {
            string output = "";
            int[] data;
            output += "Вы находитесь в " + Name + "\n";
            output += "Catalog name\tGoods count\tGoods cost\n";
            if (internalCatalogs != null)
                foreach (var item in internalCatalogs)
                {
                    data = item.Info();
                    if (item.Name.Length <= 7) output += item.Name + '\t' + '\t' + data[0].ToString() + '\t' + '\t' + data[1].ToString() + "\n";
                    else output += item.Name + '\t' + data[0].ToString() + '\t' + '\t' + data[1].ToString() + "\n";
                }
            output += "\n";
            output += "Goods name\tCount\t\tCost\n";
            if (internalGoods != null)
                foreach (var item in internalGoods)
                {
                    data = item.Info();
                    if (item.Name.Length <= 7) output += item.Name + '\t' + '\t' + data[0].ToString() + '\t' + '\t' + data[1].ToString() + "\n";
                    else output += item.Name + '\t' + data[0].ToString() + '\t' + '\t' + data[1].ToString() + "\n";
                }
            output += "\n";
            return output;
        }
        public int CompareTo(Catalog catalog)
        {
            return Name.CompareTo(catalog.Name);
        }
    }
}