using System;

namespace First_task
{
    [Serializable]
    class Goods : IComparable<Goods>
    {
        public string Name { get; private set; } //название
        private int count; //количество
        private int cost; //стоимость
        public Goods(string[] data) //конструктор класса
        {
            Name = data[0];
            count = int.Parse(data[1]);
            cost = int.Parse(data[2]);
        }
        /// <summary>
        /// Получить информацию о количестве и стоимости
        /// </summary>
        public int[] Info()
        {
            return new int[2] { count, cost };
        }
        /// <summary>
        /// Изменить стоимость товара
        /// </summary>
        /// <param name="num">новая цена</param>
        public void ChangeCount(int num)
        { 
            count = num;
        }
        public int CompareTo(Goods goods)
        {
            return Name.CompareTo(goods.Name);
        }
    }
}