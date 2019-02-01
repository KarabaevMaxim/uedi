using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	using EdiModuleCore;
	using EdiModuleCore.Model;

	[TestClass]
	public class MatchingModuleTest
	{
		[TestInitialize]
		public void TestInit()
		{
			CoreInit.Connect("Админ", "123", @"C:\Базы данных\1С\Розница для тестов ЕДИ модуля");
		}

		[TestMethod]
		public void AutomaticWHMatchingTest()
		{	
			//var wh = MatchingModule.AutomaticWHMatching(new Bridge1C.DomainEntities.Warehouse { GLN = "215125253625" });
			//Assert.IsFalse(string.IsNullOrWhiteSpace(wh.Code));
		}

		[TestMethod]
		public void AutomaticSupMatchingNotMatchedTest()
		{
			var sup = MatchingModule.AutomaticSupMatching(new ExCounteragent { GLN = "ФИвпукркок" });
			Assert.IsNull(sup.InnerCounteragent);
		}

		[TestMethod]
		public void AutomaticSupMatchingTest()
		{
			var sup = MatchingModule.AutomaticSupMatching(new ExCounteragent { GLN = "4607068529991" });
			Assert.IsFalse(string.IsNullOrWhiteSpace(sup.InnerCounteragent.Code));
		}
	}
}
