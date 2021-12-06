using System;
using System.Collections.Generic;

namespace Amazoom
{
    public class ComputerTests
    {
        private static Computer computer;
        private static Product[] testProducts;

        public ComputerTests()
        {
            computer = new Computer();
            testProducts = new Product[3] { new Product("prod1", 100, 99), new Product("prod2", 100, 99), new Product("prod3", 100, 99) };
        }

        public void TestAddCatalogProducts()
        {
            Computer.UpdateCatalog(new Product[0]); //resets catalog to be empty
            //Product[] testProducts = new Product[3] { new Product("prod1", 100, 99), new Product("prod2", 100, 99), new Product("prod3", 100, 99) };

            foreach(Product product in testProducts)
            {
                product.stock = 0;
                computer.AddNewCatalogItem(product);
            }

            Product[] currCatalog = Computer.ReadCatalog();
            if (currCatalog.Length != testProducts.Length)
            {
                Console.WriteLine("TestAddCatalogProducts FAILED");
                return;
            }
            for(int i=0; i<currCatalog.Length; i++)
            {
                if(currCatalog[i].name!=testProducts[i].name || currCatalog[i].weight != testProducts[i].weight
                    || currCatalog[i].price != testProducts[i].price || currCatalog[i].id == -1)
                {
                    Console.WriteLine("TestAddCatalogProducts FAILED");
                    return;
                }

            }
            Console.WriteLine("TestAddCatalogProducts PASSED");
        }

        public void TestRestockInventory()
        {
            Computer.UpdateInventory(new List<Item>()); //reset inventory to empty
            computer.ReadAndReplaceCatalogStock();
            Product[] currCatalog = Computer.ReadCatalog();
            foreach(Product prod in currCatalog)
            {
                if (prod.stock != computer.maxItemStock)
                {
                    Console.WriteLine("TestRestockInventory FAILED");
                    return;
                }
            }

            List<Item> currInventory = Computer.ReadInventory();
            if (currInventory.Count != testProducts.Length*computer.maxItemStock)
            {
                Console.WriteLine("TestRestockInventory FAILED");
                return;
            }
            for(int i=0; i<currInventory.Count; i++)
            {
                if(currInventory[i].id == -1 || currInventory[i].shelfId == -1)
                {
                    Console.WriteLine("TestRestockInventory FAILED");
                    return;
                }
            }
            Console.WriteLine("TestRestockInventory PASSED");

        }

        public void TestOrderValidation()
        {
            List<(Product, int)> orderItems = new List<(Product, int)>();
            //case 1: order contains quantity below our current stock level --> VALID
            foreach (Product prod in testProducts)
            {
                orderItems.Add((prod, 1)); //test adding 1 quantity of each product
            }
            if(computer.OrderIsValid(new Order(1,orderItems,"")) == false){
                Console.WriteLine("TestOrderValidation FAILED");
                return;
            }
            orderItems.Clear();

            //case 2: order contains quantity exactly equal to our current stock level --> VALID
            foreach (Product prod in testProducts)
            {
                orderItems.Add((prod, computer.maxItemStock)); //test adding maxItemStock quantity of each product
            }
            if (computer.OrderIsValid(new Order(2, orderItems, "")) == false)
            {
                Console.WriteLine("TestOrderValidation FAILED");
                return;
            }
            orderItems.Clear();

            //case 3: order contains quantity exceeding our current stock level --> INVALID
            foreach (Product prod in testProducts)
            {
                orderItems.Add((prod, computer.maxItemStock+1)); //test adding maxItemStock quantity of each product
            }
            if (computer.OrderIsValid(new Order(3, orderItems, "")) == true)
            {
                Console.WriteLine("TestOrderValidation FAILED");
                return;
            }

            Console.WriteLine("TestOrderValidation PASSED");
        }
    }
}
