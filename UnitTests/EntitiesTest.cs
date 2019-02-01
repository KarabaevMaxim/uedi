using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    using Bridge1C.DomainEntities;
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

		//    [TestMethod]
		//    public void ModelWaybillEqualTest()
		//    {
		//        EdiModuleCore.Model.Waybill waybill1 = new EdiModuleCore.Model.Waybill
		//        {
		//            Date = new DateTime(2000, 1, 1),
		//            Number = "123",
		//            Organization = new Organization
		//            {
		//                Code = "12345",
		//                GLN = "12345",
		//                Name = "123456"
		//            },
		//            Supplier = new Counteragent
		//            {
		//                Code = "1234",
		//                Name = "12345",
		//                FullName = "1234567890",
		//                GLN = "1234567"
		//            },
		//            Warehouse = new Warehouse
		//            {
		//                Code = "1234",
		//                Name = "12345",
		//                GLN = "1234567"
		//            },
		//            Wares = new List<EdiModuleCore.Model.WaybillRow>
		//            {
		//                new EdiModuleCore.Model.WaybillRow
		//                {
		//                     Amount = 1,
		//                     Count = 4,
		//                     Price = 10,
		//                     TaxAmount = 10,
		//                     TaxRate = 14,
		//                     Ware = new MatchedWare
		//                     {
		//                         ExWare = new ExWare
		//                         {
		//                             Code = "1234",
		//                             Barcode = "123456",
		//                             Name = "123456",
		//                             Supplier = new Counteragent
		//                             {
		//                                Code = "1234",
		//                                Name = "12345",
		//                                FullName = "1234567890",
		//                                GLN = "1234567"
		//                             },
		//                             Unit = new Unit
		//                             {
		//                                 Name = "12345",
		//                                 Code = "12345",
		//                                 FullName = "12345",
		//                                 International = "PCE"
		//                             }
		//                         },
		//                         InnerWare = new Ware
		//                         {
		//                             Code = "12345",
		//                             Name = "12345",
		//                             FullName = "12345",
		//                             Unit = new Unit
		//                             {
		//                                 Name = "12345",
		//                                 Code = "12345",
		//                                 FullName = "12345",
		//                                 International = "PCE"
		//                             },
		//                             BarCodes = new List<string>
		//                             {
		//                                 "12345",
		//                                 "1234568"
		//                             },
		//                             ExCodes = new List<WareExCode>
		//                             {
		//                                 new WareExCode
		//                                 {
		//                                     Counteragent = new Counteragent
		//                                     {
		//                                        Code = "1234",
		//                                        Name = "12345",
		//                                        FullName = "1234567890",
		//                                        GLN = "1234567"
		//                                     },
		//                                     Value = "123456"
		//                                 }
		//                             }
		//                         }
		//                     }
		//                }
		//            }
		//        };

		//        EdiModuleCore.Model.Waybill waybill2 = new EdiModuleCore.Model.Waybill
		//        {
		//            Date = new DateTime(2000, 1, 1),
		//            Number = "123",
		//            Organization = new Organization
		//            {
		//                Code = "12345",
		//                GLN = "12345",
		//                Name = "123456"
		//            },
		//            Supplier = new Counteragent
		//            {
		//                Code = "1234",
		//                Name = "12345",
		//                FullName = "1234567890",
		//                GLN = "1234567"
		//            },
		//            Warehouse = new Warehouse
		//            {
		//                Code = "1234",
		//                Name = "12345",
		//                GLN = "1234567"
		//            },
		//            Wares = new List<EdiModuleCore.Model.WaybillRow>
		//            {
		//                new EdiModuleCore.Model.WaybillRow
		//                {
		//                     Amount = 1,
		//                     Count = 4,
		//                     Price = 10,
		//                     TaxAmount = 10,
		//                     TaxRate = 14,
		//                     Ware = new MatchedWare
		//                     {
		//                         ExWare = new ExWare
		//                         {
		//                             Code = "1234",
		//                             Barcode = "123456",
		//                             Name = "123456",
		//                             Supplier = new Counteragent
		//                             {
		//                                Code = "1234",
		//                                Name = "12345",
		//                                FullName = "1234567890",
		//                                GLN = "1234567"
		//                             },
		//                             Unit = new Unit
		//                             {
		//                                 Name = "12345",
		//                                 Code = "12345",
		//                                 FullName = "12345",
		//                                 International = "PCE"
		//                             }
		//                         },
		//                         InnerWare = new Ware
		//                         {
		//                             Code = "12345",
		//                             Name = "12345",
		//                             FullName = "12345",
		//                             Unit = new Unit
		//                             {
		//                                 Name = "12345",
		//                                 Code = "12345",
		//                                 FullName = "12345",
		//                                 International = "PCE"
		//                             },
		//                             BarCodes = new List<string>
		//                             {
		//                                 "12345",
		//                                 "1234568"
		//                             },
		//                             ExCodes = new List<WareExCode>
		//                             {
		//                                 new WareExCode
		//                                 {
		//                                     Counteragent = new Counteragent
		//                                     {
		//                                        Code = "1234",
		//                                        Name = "12345",
		//                                        FullName = "1234567890",
		//                                        GLN = "1234567"
		//                                     },
		//                                     Value = "123456"
		//                                 }
		//                             }
		//                         }
		//                     }
		//                }
		//            }
		//        };

		//        Assert.IsTrue(waybill1.Equals(waybill2));
		//    }
		//}
	}
}
