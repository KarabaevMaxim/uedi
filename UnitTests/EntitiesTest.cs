

namespace UnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Bridge1C.DomainEntities.Spr;
    using EdiModuleCore.Model;
    using System.Collections.Generic;

	[TestClass]
	public class EntitiesTest
	{
		[TestMethod]
		public void UnitEqualsTest()
		{
			Unit unit1 = new Unit { Code = "1", FullName = "FullUnit", Name = "Unit", International = "unt" };
			Unit unit2 = new Unit { Code = "1", FullName = "FullUnit", Name = "Unit", International = "unt" };
			Assert.IsTrue(unit1.Equals(unit2));
		}

		//[TestMethod]
		//public void CounteragentEqualsTest()
		//{
		//    Counteragent counteragent1 = new Counteragent { Code = "1", Name = "Counter", FullName = "FullCounter", GLN = "123" };
		//    Counteragent counteragent2 = new Counteragent { Code = "1", Name = "Counter", FullName = "FullCounter", GLN = "123" };
		//    Assert.IsTrue(counteragent1.Equals(counteragent2));
		//}

		//    [TestMethod]
		//    public void ExWaresEqualsTest()
		//    {
		//        ExWare exWare1 = new ExWare
		//        {
		//            Code = "1", Name = "ExWare", Barcode = "123",
		//            Supplier = new Counteragent
		//            {
		//                Code = "1",
		//                Name = "Counter",
		//                FullName = "FullCounter",
		//                GLN = "123"
		//            },
		//            Unit = new Unit
		//            {
		//                Code = "1",
		//                FullName = "FullUnit",
		//                Name = "Unit",
		//                International = "unt"
		//            }
		//        };

		//        ExWare exWare2 = new ExWare
		//        {
		//            Code = "1",
		//            Name = "ExWare",
		//            Barcode = "123",
		//            Supplier = new Counteragent
		//            {
		//                Code = "1",
		//                Name = "Counter",
		//                FullName = "FullCounter",
		//                GLN = "123"
		//            },
		//            Unit = new Unit
		//            {
		//                Code = "1",
		//                FullName = "FullUnit",
		//                Name = "Unit",
		//                International = "unt"
		//            }
		//        };

		//        Assert.IsTrue(exWare1.Equals(exWare2));
		//    }

		//    [TestMethod]
		//    public void WareEqualsTest()
		//    {
		//        Ware ware1 = new Ware
		//        {
		//            Code = "1",
		//            Name = "Ware",
		//            FullName = "FullWare",
		//            BarCodes = new List<string>
		//            {
		//                "123",
		//                "456"
		//            },
		//            Unit = new Unit
		//            {
		//                Code = "1",
		//                Name = "Unit",
		//                FullName = "FullUnit",
		//                International = "unt"
		//            },
		//            ExCodes = new List<WareExCode>
		//            {
		//                new WareExCode
		//                {
		//                    Counteragent = new Counteragent
		//                    {
		//                        Code = "1",
		//                        Name = "Counteragent",
		//                        FullName = "FullCounteragent",
		//                        GLN = "123"
		//                    },
		//                    Value = "987"
		//                }
		//            }
		//        };
		//        Ware ware2 = new Ware
		//        {
		//            Code = "1",
		//            Name = "Ware",
		//            FullName = "FullWare",
		//            BarCodes = new List<string>
		//            {
		//                "123",
		//                "456"
		//            },
		//            Unit = new Unit
		//            {
		//                Code = "1",
		//                Name = "Unit",
		//                FullName = "FullUnit",
		//                International = "unt"
		//            },
		//            ExCodes = new List<WareExCode>
		//            {
		//                new WareExCode
		//                {
		//                    Counteragent = new Counteragent
		//                    {
		//                        Code = "1",
		//                        Name = "Counteragent",
		//                        FullName = "FullCounteragent",
		//                        GLN = "123"
		//                    },
		//                    Value = "987"
		//                }
		//            }
		//        };
		//        Assert.IsTrue(ware1.Equals(ware2));
		//    }

		//    [TestMethod]
		//    public void MatchedWareEqualsTest()
		//    {
		//        MatchedWare matchedWare1 = new MatchedWare
		//        {
		//            ExWare = new ExWare
		//            {
		//                Code = "1",
		//                Name = "ExWare",
		//                Barcode = "123",
		//                Supplier = new Counteragent
		//                {
		//                    Code = "1",
		//                    Name = "Counter",
		//                    FullName = "FullCounter",
		//                    GLN = "123"
		//                },
		//                Unit = new Unit
		//                {
		//                    Code = "1",
		//                    FullName = "FullUnit",
		//                    Name = "Unit",
		//                    International = "unt"
		//                }
		//            },
		//            InnerWare = new Ware
		//            {
		//                Code = "1",
		//                Name = "Ware",
		//                FullName = "FullWare",
		//                BarCodes = new List<string>
		//            {
		//                "123",
		//                "456"
		//            },
		//                Unit = new Unit
		//                {
		//                    Code = "1",
		//                    Name = "Unit",
		//                    FullName = "FullUnit",
		//                    International = "unt"
		//                },
		//                ExCodes = new List<WareExCode>
		//                {
		//                    new WareExCode
		//                    {
		//                        Counteragent = new Counteragent
		//                        {
		//                            Code = "1",
		//                            Name = "Counteragent",
		//                            FullName = "FullCounteragent",
		//                            GLN = "123"
		//                        },
		//                        Value = "987"
		//                    }
		//                }
		//            }
		//        };
		//        MatchedWare matchedWare2 = new MatchedWare
		//        {
		//            ExWare = new ExWare
		//            {
		//                Code = "1",
		//                Name = "ExWare",
		//                Barcode = "123",
		//                Supplier = new Counteragent
		//                {
		//                    Code = "1",
		//                    Name = "Counter",
		//                    FullName = "FullCounter",
		//                    GLN = "123"
		//                },
		//                Unit = new Unit
		//                {
		//                    Code = "1",
		//                    FullName = "FullUnit",
		//                    Name = "Unit",
		//                    International = "unt"
		//                }
		//            },
		//            InnerWare = new Ware
		//            {
		//                Code = "1",
		//                Name = "Ware",
		//                FullName = "FullWare",
		//                BarCodes = new List<string>
		//            {
		//                "123",
		//                "456"
		//            },
		//                Unit = new Unit
		//                {
		//                    Code = "1",
		//                    Name = "Unit",
		//                    FullName = "FullUnit",
		//                    International = "unt"
		//                },
		//                ExCodes = new List<WareExCode>
		//                {
		//                    new WareExCode
		//                    {
		//                        Counteragent = new Counteragent
		//                        {
		//                            Code = "1",
		//                            Name = "Counteragent",
		//                            FullName = "FullCounteragent",
		//                            GLN = "123"
		//                        },
		//                        Value = "987"
		//                    }
		//                }
		//            }
		//        };
		//        Assert.IsTrue(matchedWare1.Equals(matchedWare2));
		//    }

		//    [TestMethod]
		//    public void CounteragentWithNullPropertyEqualsTest()
		//    {
		//        Counteragent counteragent1 = new Counteragent { Code = "1", Name = "Counter", FullName = "FullCounter", GLN = null };
		//        Counteragent counteragent2 = new Counteragent { Code = "1", Name = "Counter", FullName = "FullCounter", GLN = null };
		//        Assert.IsTrue(counteragent1.Equals(counteragent2));
		//    }

		//    [TestMethod]
		//    public void WareWithNullBarcodesEqualsTest()
		//    {
		//        Ware ware1 = new Ware
		//        {
		//            Code = "1",
		//            Name = "Ware",
		//            FullName = "FullWare",
		//            Unit = new Unit
		//            {
		//                Code = "1",
		//                Name = "Unit",
		//                FullName = "FullUnit",
		//                International = "unt"
		//            },
		//            ExCodes = new List<WareExCode>
		//            {
		//                new WareExCode
		//                {
		//                    Counteragent = new Counteragent
		//                    {
		//                        Code = "1",
		//                        Name = "Counteragent",
		//                        FullName = "FullCounteragent",
		//                        GLN = "123"
		//                    },
		//                    Value = "987"
		//                }
		//            }
		//        };
		//        Ware ware2 = new Ware
		//        {
		//            Code = "1",
		//            Name = "Ware",
		//            FullName = "FullWare",
		//            Unit = new Unit
		//            {
		//                Code = "1",
		//                Name = "Unit",
		//                FullName = "FullUnit",
		//                International = "unt"
		//            },
		//            ExCodes = new List<WareExCode>
		//            {
		//                new WareExCode
		//                {
		//                    Counteragent = new Counteragent
		//                    {
		//                        Code = "1",
		//                        Name = "Counteragent",
		//                        FullName = "FullCounteragent",
		//                        GLN = "123"
		//                    },
		//                    Value = "987"
		//                }
		//            }
		//        };
		//        Assert.IsTrue(ware1.Equals(ware2));
		//    }

		[TestMethod]
		public void ModelWaybillEqualsTest()
		{
			string number = "001";
			DateTime date = DateTime.Now;
			MatchedCounteragent counteragent = new MatchedCounteragent
			{
				ExCounteragent = new ExCounteragent
				{
					GLN = "12345"
				},
				InnerCounteragent = new Counteragent
				{
					Code = "001",
					Name = "Sup",
					FullName = "Supplier",
					GLN = "12345"
				}
			};
			Organization organization = new Organization
			{
				Code = "001",
				GLN = "54321",
				Name = "Org"
			};
			MatchedWarehouse warehouse = new MatchedWarehouse
			{
				ExWarehouse = new ExWarehouse
				{
					GLN = "987654321"
				},
				InnerWarehouse = new Warehouse
				{
					Code = "001",
					Name = "Wh",
					Shop = new Shop
					{
						Code = "001",
						Name = "Shop"
					},
					User = new User
					{
						Code = "001",
						Name = "Max"
					}
				}
			};
			List<EdiModuleCore.Model.WaybillRow> rows = new List<EdiModuleCore.Model.WaybillRow>();
			string fileName = "File.txt";
			float amountWithTax = 499.5f;
			float amount = 450;

			EdiModuleCore.Model.Waybill waybill1 = new EdiModuleCore.Model.Waybill
			{
				Number = number,
				Date = date,
				Amount = amount,
				AmountWithTax = amountWithTax,
				FileName = fileName,
				Organization = organization,
				Supplier = counteragent,
				Warehouse = warehouse,
				Wares = rows
			};

			EdiModuleCore.Model.Waybill waybill2 = new EdiModuleCore.Model.Waybill
			{
				Number = number,
				Date = date,
				Amount = amount,
				AmountWithTax = amountWithTax,
				FileName = fileName,
				Organization = organization,
				Supplier = counteragent,
				Warehouse = warehouse,
				Wares = rows
			};

			Assert.IsTrue(waybill1.Equals(waybill2));
		}

		[TestMethod]
		public void ModelWaybillEqualsWithNullPropertyTest()
		{
			string number = "001";
			DateTime date = DateTime.Now;
			List<EdiModuleCore.Model.WaybillRow> rows = new List<EdiModuleCore.Model.WaybillRow>();
			string fileName = "File.txt";
			float amountWithTax = 499.5f;
			float amount = 450;

			EdiModuleCore.Model.Waybill waybill1 = new EdiModuleCore.Model.Waybill
			{
				Number = number,
				Date = date,
				Amount = amount,
				AmountWithTax = amountWithTax,
				FileName = fileName,
				Organization = null,
				Supplier = null,
				Warehouse = null,
				Wares = rows
			};

			EdiModuleCore.Model.Waybill waybill2 = new EdiModuleCore.Model.Waybill
			{
				Number = number,
				Date = date,
				Amount = amount,
				AmountWithTax = amountWithTax,
				FileName = fileName,
				Organization = null,
				Supplier = null,
				Warehouse = null,
				Wares = rows
			};

			Assert.IsTrue(waybill1.Equals(waybill2));
		}

		[TestMethod]
		public void ModelWaybillEqualsWithNullPropertyAtOneWaybillTest()
		{
			string number = "001";
			DateTime date = DateTime.Now;
			MatchedCounteragent counteragent = new MatchedCounteragent
			{
				ExCounteragent = new ExCounteragent
				{
					GLN = "12345"
				},
				InnerCounteragent = new Counteragent
				{
					Code = "001",
					Name = "Sup",
					FullName = "Supplier",
					GLN = "12345"
				}
			};
			Organization organization = new Organization
			{
				Code = "001",
				GLN = "54321",
				Name = "Org"
			};
			MatchedWarehouse warehouse = new MatchedWarehouse
			{
				ExWarehouse = new ExWarehouse
				{
					GLN = "987654321"
				},
				InnerWarehouse = new Warehouse
				{
					Code = "001",
					Name = "Wh",
					Shop = new Shop
					{
						Code = "001",
						Name = "Shop"
					},
					User = new User
					{
						Code = "001",
						Name = "Max"
					}
				}
			};
			List<EdiModuleCore.Model.WaybillRow> rows = new List<EdiModuleCore.Model.WaybillRow>();
			string fileName = "File.txt";
			float amountWithTax = 499.5f;
			float amount = 450;

			EdiModuleCore.Model.Waybill waybill1 = new EdiModuleCore.Model.Waybill
			{
				Number = number,
				Date = date,
				Amount = amount,
				AmountWithTax = amountWithTax,
				FileName = fileName,
				Organization = organization,
				Supplier = counteragent,
				Warehouse = null,
				Wares = rows
			};

			EdiModuleCore.Model.Waybill waybill2 = new EdiModuleCore.Model.Waybill
			{
				Number = number,
				Date = date,
				Amount = amount,
				AmountWithTax = amountWithTax,
				FileName = fileName,
				Organization = organization,
				Supplier = counteragent,
				Warehouse = warehouse,
				Wares = rows
			};

			Assert.IsFalse(waybill1.Equals(waybill2));
		}
	}
}
