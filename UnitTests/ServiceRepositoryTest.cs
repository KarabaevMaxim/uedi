namespace UnitTests
{
	using System;
	using System.Threading.Tasks;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Bridge1C;
    using Bridge1C.DomainEntities;
    using System.Collections.Generic;
	using EdiModuleCore;
	using System.Linq;

    [TestClass]
    public class ServiceRepositoryTest
    {
		[TestInitialize]
		public void TestInit()
		{
			CoreInit.Connect("Админ", "123", @"C:\Базы данных\1С\Розница для тестов ЕДИ модуля");
		}

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
		public void UpdateWHGLN()
		{
			var wh = CoreInit.RepositoryService.GetWarehouse(Requisites.GLN, "1234567890");

			if (wh == null || string.IsNullOrWhiteSpace(wh.Code))
				Assert.Fail("Склад с указанным ГЛН не найден.");

			string newGLN = "0987654321";
			var result = CoreInit.RepositoryService.UpdateWarehouseGLN(wh.Code, newGLN);

			var wh2 = CoreInit.RepositoryService.GetWarehouse(Requisites.GLN, newGLN);

			if (wh2 == null || string.IsNullOrWhiteSpace(wh2.Code))
				Assert.Fail("У склада не изменился ГЛН.");

		}

		[TestMethod]
		public async Task GetAllCounteragentsTest()
		{
			var list = await CoreInit.RepositoryService.GetAllCounteragentsAsync();
			Assert.IsTrue(list.Any());
		}

		[TestMethod]
		public async Task RematchingCounteragentAsyncTest()
		{
			Counteragent counter = new Counteragent { Code = "00001", Name = "Поликон", FullName = "ООО Поликон" };
			var list = await CoreInit.RepositoryService.RematchingCounteragentAsync(counter, "12345");

		}
	}
}
