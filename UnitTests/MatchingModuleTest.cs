using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	using System.Collections.Generic;
	using System.Reflection;
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

		[TestMethod]
		public void AutomaticWareMatchingTest()
		{
			var unit = new Bridge1C.DomainEntities.Unit
			{
				Code = "796",
				Name = "шт",
				International = "PCE",
				FullName = "Штука"
			};

			var innerCounteragent = new Bridge1C.DomainEntities.Counteragent
			{
				Code = "00-000001",
				Name = "Комос",
				FullName = "Комос",
				GLN = "4607068529991"
			};

			var exWare = new ExWare
			{
				Barcode = "12345",
				Code = "ПЖП1016463",
				Name = "ВРС Яйцо Деревенское столовое отборная кат",
				Supplier = new MatchedCounteragent
				{
					ExCounteragent = new ExCounteragent
					{
						GLN = "4607068529991"
					},
					InnerCounteragent = innerCounteragent
				},
				Unit = unit
			};

			var exceptedMatchedWare = new MatchedWare
			{
				ExWare = new ExWare(),
				InnerWare = new Bridge1C.DomainEntities.Ware
				{
					Code = "00-00000004",
					Name = "ВРС Яйцо Деревенское столовое отборная кат",
					FullName = "ВРС Яйцо Деревенское столовое отборная кат",
					Unit = new Bridge1C.DomainEntities.Unit(),
					BarCodes = new List<string> { "2400000016663" },
					ExCodes = new List<Bridge1C.DomainEntities.WareExCode>()
				}
			};

			exceptedMatchedWare.InnerWare.ExCodes = new List<Bridge1C.DomainEntities.WareExCode>
			{
				new Bridge1C.DomainEntities.WareExCode
				{
					Counteragent = innerCounteragent,
					Value = "ПЖП1016463"
				}
			};

			// Копируем ЕИ
			foreach (var item in unit.GetType().GetProperties())
			{
				item.SetValue(exceptedMatchedWare.InnerWare.Unit, item.GetValue(unit));
			}

			// Копируем Внешний товар
			foreach (var item in exWare.GetType().GetProperties())
			{
				item.SetValue(exceptedMatchedWare.ExWare, item.GetValue(exWare));
			}


			var matchedWare = MatchingModule.AutomaticMatching(exWare);

			Assert.IsTrue(matchedWare.Equals(exceptedMatchedWare));
		}

		[TestMethod]
		public void ManualMatchingTest()
		{
			MatchedWare matchedWare = new MatchedWare
			{
				ExWare = new ExWare
				{
					Barcode = "000001",
					Code = "000001",
					Name = "Яйцо",
					Supplier = new MatchedCounteragent
					{
						ExCounteragent = new ExCounteragent
						{
							GLN = "4607068529991"
						},
						InnerCounteragent = new Bridge1C.DomainEntities.Counteragent
						{
							Code = "00-000001",
							Name = "Комос",
							FullName = "Комос",
							GLN = "4607068529991"
						}
					},
					Unit = new Bridge1C.DomainEntities.Unit
					{
						Code = "796",
						Name = "шт",
						International = "PCE",
						FullName = "Штука"
					}
				}
			};
			Bridge1C.DomainEntities.Ware ware = CoreInit.RepositoryService.GetWare(Bridge1C.Requisites.Code, "00-00000001");
			MatchingModule.ManualMatching(ware, matchedWare);

			Bridge1C.DomainEntities.Ware resultWare = CoreInit.RepositoryService.GetWare(Bridge1C.Requisites.ExCode_Ware, "000001", matchedWare.ExWare.Supplier.ExCounteragent.GLN);

			Assert.IsTrue(resultWare.Equals(ware));
		}
	}
}
