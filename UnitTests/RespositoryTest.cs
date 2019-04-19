using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    using System.Collections.Generic;
    using DAL;
    using DAL.DomainEntities.DocWaybill;
	using EdiModuleCore;

    [TestClass]
    public class RespositoryTest
    {
		Repository repository { get; set; }

		[TestInitialize]
		public void TestInit()
		{
			//CoreInit.Connect("Админ", "123", @"C:\Базы данных\1С\Розница для тестов ЕДИ модуля");
			repository = new Repository(new Connector(@"C:\Базы данных\1С\Розница для тестов ЕДИ модуля", "Админ", "123"));
		}

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

		[TestMethod]
		public void GetWareHouseTest()
		{
			Repository repository = new Repository(new Connector(@"C:\Users\Максим\Documents\InfoBase7", "Админ", "123"));
			var warehouse = repository.GetWareHouse(Requisites.GLN, "1832027380051");
			Assert.IsFalse(string.IsNullOrWhiteSpace(warehouse?.Код));
		}

		[TestMethod]
		public void GetCounteragentTest()
		{
			Repository repository = new Repository(new Connector(@"C:\Users\Максим\Documents\InfoBase7", "Админ", "123"));
			var counteragent = repository.GetWareHouse(Requisites.GLN, "4607068529991");
			Assert.IsFalse(string.IsNullOrWhiteSpace(counteragent?.Код));
		}

		/// <summary>
		/// Получить товар по его коду.
		/// </summary>
		[TestMethod]
		public void GetWareByCodeTest()
		{
			var ware = repository.GetWare(Requisites.Code, "00-00000001");
			Assert.IsTrue(ware != null && !string.IsNullOrWhiteSpace(ware.Код));
		}

		/// <summary>
		/// Получить товар по внешнему коду у поставщика.
		/// </summary>
		[TestMethod]
		public void GetWareByExCodeTest()
		{
			var ware = repository.GetWare(Requisites.ExCode_Ware, "123456", "123456789");
			Assert.IsTrue(ware != null && ware.Код == "00-00000002");
		}

		[TestMethod]
		public void GetWareExCodesTest()
		{
			var ware = repository.GetWare(Requisites.Code, "00-00000002");
			List<dynamic> codes = repository.GetWareExCodes(ware);
			Assert.IsTrue(codes != null && codes.Count == 2);
		}

		[TestMethod]
		public void AddNewExCodeTest()
		{
			var ware = repository.GetWare(Requisites.Code, "00-00000002");
			repository.AddNewExCode(ware, "1234656789", "55555");
			var resultWare = repository.GetWare(Requisites.ExCode_Ware, "55555", "1234656789");
			Assert.IsTrue(resultWare != null && ware.Код == "00-00000002");
		}

			
	}
}
