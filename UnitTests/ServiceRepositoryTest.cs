using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    using Bridge1C;
    using Bridge1C.DomainEntities;
    using System.Collections.Generic;
    [TestClass]
    public class ServiceRepositoryTest
    {
        [TestMethod]
        public void AddNewWaybillTest()
        {
            RepositoryService repositoryService = new RepositoryService(@"C:\Users\Максим\Documents\InfoBase7", "", "");
            Waybill waybill = new Waybill
            {
                Number = "123456789",
                Date = DateTime.Now,
                Supplier = repositoryService.GetCounteragent(Requisites.Name, "Комос"),
                Warehouse = repositoryService.GetWarehouse(Requisites.Name, "Склад"),
                Positions = new List<WaybillRow>()
                {
                    new WaybillRow
                    {
                        Count = 5,
                        Price = 100,
                        TaxAmount = 50,
                        TaxRate = 10,
                        Unit = repositoryService.GetUnit(Requisites.InternationalReduction_Unit, "PCE"),
                        Ware = repositoryService.GetWare(Requisites.Code, "00-00005159")
                    },
                    new WaybillRow
                    {
                        Count = 5,
                        Price = 200,
                        TaxAmount = 100,
                        TaxRate = 10,
                        Unit = repositoryService.GetUnit(Requisites.InternationalReduction_Unit, "PCE"),
                        Ware = repositoryService.GetWare(Requisites.Code, "00-00005148")
                    },
                    new WaybillRow
                    {
                        Count = 5,
                        Price = 500,
                        TaxAmount = 250,
                        TaxRate = 10,
                        Unit = repositoryService.GetUnit(Requisites.InternationalReduction_Unit, "PCE"),
                        Ware = repositoryService.GetWare(Requisites.Code, "00-00005020")
                    }
                }
            };
            Assert.IsTrue(repositoryService.AddNewWaybill(waybill));
        }

		[TestMethod]
		public void GetGLNTest()
		{
			RepositoryService repositoryService = new RepositoryService(@"C:\Users\Максим\Documents\InfoBase7", "Админ", "123");
			Repository repository = new Repository(new Connector(@"C:\Users\Максим\Documents\InfoBase7", "Админ", "123"));
			var warehouse = repository.GetWareHouse(Requisites.Name, "Склад 3");
			var gln = repositoryService.GetGLN(warehouse);
		}
	}
}
