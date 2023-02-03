using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using System;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //Организация
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ProductId = 1, Name = "P1"},
                new Product{ProductId = 2, Name = "P2"},
                new Product{ProductId = 3, Name = "P3"},
                new Product{ProductId = 4, Name = "P4"},
                new Product{ProductId = 5, Name = "P5"},
            }.AsQueryable());

            //Действие
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Утверждение
            ProductsListViewModel result = (ProductsListViewModel)controller.List(2).Model;
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Организация - определение вспомогательного метода HTML
            //Это необходимол для применения метода расширения
            HtmlHelper myHelper = null;

            // Организация  - создание данных PageInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            //Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i; 

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //Утвержение
            Assert.AreEqual(result.ToString(), @"<a class=""selected"" href=""Page2"">2</a>");
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //Организация 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product{ProductId = 1,Name = "P1"},
                new Product{ProductId = 2,Name = "P2"},
                new Product{ProductId = 3,Name = "P3"},
                new Product{ProductId = 4,Name = "P4"},
                new Product{ProductId = 5,Name = "P5"},
            }.AsQueryable());
            //Организация
            ProductController controller = new ProductController(mock.Object);
            //Действие
            controller.PageSize = 3;
            //Утверждение
            ProductsListViewModel result = (ProductsListViewModel)controller.List(2).Model;
            PagingInfo pageInfo = result.PagingInfo;

            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);


        }
    }
}
