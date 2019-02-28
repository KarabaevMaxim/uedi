namespace UnitTests
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System.Collections.Generic;
	using System.Linq;
	using Bridge1C.DomainEntities;
	using Bridge1C.Itida;

	[TestClass]
	public class ItidaRepositoryTest
	{
		private ItidaRepository repository { get; set; }

		[TestInitialize]
		public void TestInit()
		{
			this.repository = new ItidaRepository(@"Data Source = (local); Initial Catalog = Поликон; User ID = sa; Password = itida");
		}

		[TestMethod]
		public void GetAllWaresTest()
		{
			var wares = this.repository.GetAllWares();
			Assert.IsTrue(wares.Any());
		}

		[TestMethod]
		public void GetUnitTest()
		{
			var unit = this.repository.GetUnit(Bridge1C.Requisites.Code, "шт");
			Assert.IsFalse(unit == null);
		}

		[TestMethod]
		public void GetCounteragentTest()
		{
			var counteragent = this.repository.GetCounteragent(Bridge1C.Requisites.Code, "0000106");
			Assert.IsFalse(counteragent == null);
		}

		[TestMethod]
		public void GetExCodesTest()
		{
			var exCodes = this.repository.GetExCodes("1509");
			Assert.IsTrue(exCodes.Any());
		}

		[TestMethod]
		public void GetOrganizationTest()
		{
			var organization = this.repository.GetOrganization(Bridge1C.Requisites.Code, "0000001");
			Assert.IsNotNull(organization);
		}

		[TestMethod]
		public void GetWarehouseTest()
		{
			var warehouse = this.repository.GetWarehouse(Bridge1C.Requisites.Code, "001");
			Assert.IsNotNull(warehouse);
		}

		[TestMethod]
		public void GetAllWaybillsTest()
		{
			var waybills = this.repository.GetAllWaybills();
			Assert.IsTrue(waybills.Any());
		}

		[TestMethod]
		public void GetWaybillsByCodeTest()
		{
			var waybills = this.repository.GetWaybillsByNumber("A-1");
			Assert.IsTrue(waybills.Any());
		}

		[TestMethod]
		public void GetWaybillRowsTest()
		{
			var rows = this.repository.GetWaybillRows("7239");
			Assert.IsTrue(rows.Any());
		}

		[TestMethod]
		public void GetWareBarcodesTest()
		{
			var barcodes = this.repository.GetWareBarcodes("1509");
			Assert.IsTrue(barcodes.Any());
		}

		[TestMethod]
		public void GetWareByCodeTest()
		{
			var ware = this.repository.GetWare(Bridge1C.Requisites.Code, "1509");
			Assert.IsNotNull(ware);
		}

		[TestMethod]
		public void GetWareByExCodeTest()
		{
			var ware = this.repository.GetWare(Bridge1C.Requisites.ExCode_Ware, "424214241242", "4607068529991");
			Assert.IsNotNull(ware);
		}

		[TestMethod]
		public void GetWareByExCodeWithNonexistentCounteragentGLNTest()
		{
			var ware = this.repository.GetWare(Bridge1C.Requisites.ExCode_Ware, "424214241242", "243423dsdbvdb");
			Assert.IsNull(ware);
		}

		[TestMethod]
		public void GetAllWarehousesTest()
		{
			var warehouses = this.repository.GetAllWarehouses();
			Assert.IsNull(warehouses);
		}

		[TestMethod]
		public void AddNewWareTest()
		{
			var unit = this.repository.GetUnit(Bridge1C.Requisites.Code, "шт");	
			var counteragent1 = this.repository.GetCounteragent(Bridge1C.Requisites.Code, "0000001");
			var counteragent2 = this.repository.GetCounteragent(Bridge1C.Requisites.Code, "0000002");
			var counteragent3 = this.repository.GetCounteragent(Bridge1C.Requisites.Code, "0000003");

			if (unit == null)
				Assert.Fail("ЕИ не найдена в базе");

			if(counteragent1 == null || counteragent2 == null || counteragent3 == null)
				Assert.Fail("Контрагенты не найдены в базе");

			var ware = new Ware
			{
				Name = "Товар из теста с ЕИ",
				FullName = "Товар добавленный из теста с ЕИ",
				Unit = unit,
				BarCodes = new List<string>()
				{
					"423451515",
					"523535345345",
					"5252352551431243"
				},
				ExCodes = new List<WareExCode>
				{
					new WareExCode
					{
						Counteragent = counteragent1,
						Value = "123456789"
					},
					new WareExCode
					{
						Counteragent = counteragent2,
						Value = "5535215"
					},
					new WareExCode
					{
						Counteragent = counteragent3,
						Value = "325325325"
					}
				}
			};
			Assert.IsTrue(this.repository.AddNewWare(ware));
		}

		[TestMethod]
		public void RematchingCounteragentTest()
		{
			var result = this.repository.RematchingCounteragent("0000002", "123456789");
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void RematchingWarehouseTest()
		{
			var result = this.repository.RematchingWarehouse("002", "123456789");
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void AddNewWaybill()
		{
			Organization organization = this.repository.GetOrganization(Bridge1C.Requisites.Code, "0000001");
			Counteragent counteragent = this.repository.GetCounteragent(Bridge1C.Requisites.Code, "0000254");
			Warehouse warehouse = this.repository.GetWarehouse(Bridge1C.Requisites.Code, "001");

			if (organization == null || counteragent == null || warehouse == null)
				Assert.Fail("Объект не найден в базе");

			Waybill waybill = new Waybill
			{
				Number = "ABC123",
				Date = DateTime.Now,
				Organization = organization,
				Supplier = counteragent,
				Warehouse = warehouse,
				Shop = null,
				Positions = new List<WaybillRow>()
			};
			var result = this.repository.AddNewWaybill(waybill);
			Assert.IsTrue(result);
		}
	}
}
