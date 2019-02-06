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
		public void GetExCodes()
		{
			var exCodes = this.repository.GetExCodes("0000001832");
			Assert.IsTrue(exCodes.Any());
		}
	}
}
