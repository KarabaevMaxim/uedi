using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
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
		public void GetWaybillsByCode()
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
		public void GetWareBarcodes()
		{
			var barcodes = this.repository.GetWareBarcodes("1509");
			Assert.IsTrue(barcodes.Any());
		}
	}
}
