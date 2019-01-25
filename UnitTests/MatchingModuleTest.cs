using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	using EdiModuleCore;
	[TestClass]
	public class MatchingModuleTest
	{
		[TestInitialize]
		public void TestInit()
		{
			CoreInit.Connect("Админ", "123", @"C:\Users\Максим\Documents\InfoBase7");
		}

		[TestMethod]
		public void AutomaticWHMatchingTest()
		{	
			var wh = MatchingModule.AutomaticWHMatching(new Bridge1C.DomainEntities.Warehouse { GLN = "215125253625" });
			Assert.IsFalse(string.IsNullOrWhiteSpace(wh.Code));
		}

		[TestMethod]
		public void AutomaticSupMatchingTest()
		{
			var sup = MatchingModule.AutomaticSupMatching(new Bridge1C.DomainEntities.Counteragent { GLN = "123456789" });
			Assert.IsFalse(string.IsNullOrWhiteSpace(sup.Code));
		}
	}
}
