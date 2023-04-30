using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;
using NLog;
using System.IO;
using System.Xml.Serialization;
using NLog.Fluent;

namespace Lab5Anikaev
{
    public partial class Form1 : Form
    {
        public static Random r = new Random();
        public static Control form1;
        public static string[] names_prod = { "Стілець", "Стіл кухонний", "Обідній стіл", "Крісло", "Шафа", 
            "Раковина", "Ванна", "Тумба", "Комп'ютерне крісло", "Підвісне крісло"};

        List<Store> stores = new List<Store>();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Form1()
        {
            InitializeComponent();
        }


        [Serializable]
        public class Product
        {
            public string name_product; //Назва продукту
            public int quantity_of_goods; //кількість товару
            public int price; //вартість продукту для продажу
            public int purchase_price; //закупівельна ціна

            public Product() { }
            public Product(string nm)
            {
                int temp = r.Next(1, 300);

                Name_product = nm; //Назва продукту
                Quantity_of_goods = temp; //кількість товару

                temp = r.Next(400, 5000);
                Price = temp; //товар для продажу

                if (temp <= 1000) Purchase_price = temp - 300;
                else if (temp > 1000 && temp <= 3000) Purchase_price = temp - 700;
                else Purchase_price = temp - 1200;
            }
            public string Name_product
            {
                get => this.name_product;
                set => this.name_product = value;
            }
            public int Quantity_of_goods
            {
                get => this.quantity_of_goods;
                set => this.quantity_of_goods = value;
            }
            public int Price
            {
                get => this.price;
                set => this.price = value;
            }
            public int Purchase_price
            {
                get => this.purchase_price;
                set => this.purchase_price = value;
            }
        }

        [Serializable]
        public class Store
        {
            public string name_store; //Назва магазину
            public int number_of_buyers; //Кількість покупців в магазині
            public int cash_register; //Касса магазину
            public List<Product> products = new List<Product>();

            public Store()
            {
                Name_store = "Меблевий магазин";//назва магазину
                Number_of_buyers = 0;//кількість покупців
                Cash_register = 50000;

                for (int i = 0; i < 10; i++)
                {
                    products.Add(new Product(names_prod[i]) { });//un6 - тип кімнати
                }
            }
            public string Name_store
            {
                get => this.name_store;
                set => this.name_store = value;
            }
            public int Number_of_buyers
            {
                get => this.number_of_buyers;
                set => this.number_of_buyers = value;
            }
            public int Cash_register
            {
                get => this.cash_register;
                set => this.cash_register = value;
            }
        }

        public void Works()
        {
            int temp = r.Next(1, 20);
            stores[0].number_of_buyers = temp;//генерація кількості покупців
            for (int i = 0; i < temp; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    if (stores[0].products[j].quantity_of_goods > 0)
                    {
                        int temp1 = r.Next(1, stores[0].products[j].quantity_of_goods);
                        stores[0].products[j].quantity_of_goods -= temp1;
                        stores[0].cash_register += temp1 * stores[0].products[j].price;
                        logger.Debug("Покупець номер: " + i);
                        logger.Debug("Товар: " + stores[0].products[j].name_product);
                        logger.Debug("Кількість купленого товару: " + temp1);
                        logger.Debug("Кількість товару: " + stores[0].products[j].quantity_of_goods);
                        logger.Debug("Витрати: " + temp1 * stores[0].products[j].price);
                        logger.Debug("Каса магазину: " + stores[0].cash_register);
                        
                    }
                    dataGridView1.Invoke(new MethodInvoker(delegate ()
                    {
                        BindingSource binding1 = new BindingSource();
                        binding1.DataSource = stores;
                        dataGridView1.DataSource = binding1;
                    }));
                    dataGridView2.Invoke(new MethodInvoker(delegate ()
                    {
                        BindingSource binding1 = new BindingSource();
                        binding1.DataSource = stores[0].products;
                        dataGridView2.DataSource = binding1;
                    }));
                    Thread.Sleep(2000);
                }
                
                stores[0].number_of_buyers -= 1;
            }
            
        }
        private void stocktaking()
        {
            int temp;
            for (int i = 0; i < 10; i++)
            {
                if(stores[0].cash_register > 200000)
                {
                    temp = r.Next(100, 200);
                    stores[0].products[i].quantity_of_goods += temp;
                    stores[0].cash_register -= temp * stores[0].products[i].purchase_price;
                    logger.Debug("Магазин на переобліку: ");
                    logger.Debug("Товар: " + stores[0].products[i].name_product);
                    logger.Debug("Кількість купленого товару: " + temp); 
                    logger.Debug("Кількість товару: " + stores[0].products[i].quantity_of_goods);
                    logger.Debug("Вартість: " + temp * stores[0].products[i].purchase_price);
                    logger.Debug("Каса магазину: " + stores[0].cash_register);
                }
                dataGridView1.Invoke(new MethodInvoker(delegate ()
                {
                    BindingSource binding1 = new BindingSource();
                    binding1.DataSource = stores;
                    dataGridView1.DataSource = binding1;
                }));
                dataGridView2.Invoke(new MethodInvoker(delegate ()
                {
                    BindingSource binding1 = new BindingSource();
                    binding1.DataSource = stores[0].products;
                    dataGridView2.DataSource = binding1;
                }));
                Thread.Sleep(1000);
            }

        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            stores.Add(new Store() { });

            BindingSource binding = new BindingSource();
            binding.DataSource = stores;
            dataGridView1.DataSource = binding;
            BindingSource binding1 = new BindingSource();
            binding1.DataSource = stores[0].products;
            dataGridView2.DataSource = binding1;

            
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(Works);
            thread.IsBackground = true;
            thread.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var thread = new Thread(stocktaking);
            thread.IsBackground = true;
            thread.Start();

        }

      
    }
}
