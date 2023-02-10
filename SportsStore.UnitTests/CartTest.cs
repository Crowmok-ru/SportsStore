using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SportsStore.Domain.Entities;
using System.Linq;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTest
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            //Организация - создание нескольких тестовых товаров
            var p1 = new Product { ProductId = 1, Name = "P1" };
            var p2 = new Product { ProductId = 2, Name = "P2" };

            //Организация создание новой корзины
            var target = new Cart();
            //Действие 
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            var result = target.Lines.ToArray();

            //Утверждение
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Product, p1);
            Assert.AreEqual(result[1].Product, p2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //Организация - создание нескольких тестовых классов
            var p1 = new Product { ProductId = 1, Name = "P1" };
            var p2 = new Product { ProductId = 2, Name = "P2" };
            //Организация создание новой корзины
            var target = new Cart();
            //Действие 
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            var results = target.Lines.OrderBy(c => c.Product.ProductId).ToArray();
            //Утверждение
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            //Организация - создание нескольких тестовых классов
            var p1 = new Product { ProductId = 1, Name = "P1" };
            var p2 = new Product { ProductId = 2, Name = "P2" };
            var p3 = new Product { ProductId = 3, Name = "P3" };

            //Организация - создание новой корзины
            var target = new Cart();
            //Организация - Добавление некоторых товаров в корзину
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);
            //Действие
            target.RemoveLine(p2);
            //Утверждение
            Assert.AreEqual(target.Lines.Where(c => c.Product == p2).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            //Организация - создание нескольких тестовых классов
            var p1 = new Product { ProductId = 1, Name = "P1", Price=100m };
            var p2 = new Product { ProductId = 2, Name = "P2" , Price=50m};

            //Организация - создание новой корзины
            var target = new Cart();
            //Действие
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            var result = target.ComputeTotalValue();
            //Утверждение
            Assert.AreEqual(result, 450m);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            //Организация - создание нескольких тестовых классов
            var p1 = new Product { ProductId = 1, Name = "P1", Price = 100m };
            var p2 = new Product { ProductId = 2, Name = "P2", Price = 50m };

            //Организация - создание новой корзины
            var target = new Cart();
            //Организация - Добавление нескольких элементов
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            //Действие - сброс корзины
            target.Clear();  
            //Утверждение
            Assert.AreEqual(target.Lines.Count(), 0);
        }
    }
}
