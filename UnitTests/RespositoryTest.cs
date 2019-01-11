using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    using System.Collections.Generic;
    using Bridge1C;
    using Bridge1C.DomainEntities;

    [TestClass]
    public class RespositoryTest
    {
        [TestMethod]
        public void AddNewWaybillTest()
        {
            Repository repository = new Repository(new Connector(@"C:\Users\Максим\Documents\InfoBase7", "", ""));
            RepositoryService service = new RepositoryService(@"C:\Users\Максим\Documents\InfoBase7", "", "");

            var supplier = repository.GetCounteragent(Requisites.Name, "Комос");
            var warehouse = repository.GetWareHouse(Requisites.Name, "Склад");
            var shop = repository.GetShop(warehouse);
            var rows = new List<WaybillRow>()
            {
                new WaybillRow
                {
                        Count = 5,
                        Price = 100,
                        TaxAmount = 50,
                        TaxRate = 10,
                        Ware = service.GetWare(Requisites.Code, "00-00005159")
                },
                new WaybillRow
                {
                        Count = 5,
                        Price = 200,
                        TaxAmount = 100,
                        TaxRate = 10,
                        Ware = service.GetWare(Requisites.Code, "00-00005148")
                },
                new WaybillRow
                {
                        Count = 5,
                        Price = 500,
                        TaxAmount = 250,
                        TaxRate = 10,
                        Ware = service.GetWare(Requisites.Code, "00-00005020")
                }
            };

            Assert.IsTrue(repository.AddNewWaybill("123456789", DateTime.Now, supplier, warehouse, shop, rows));
        }
    }
}
