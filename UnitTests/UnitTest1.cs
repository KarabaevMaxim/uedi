using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    using Bridge1C;
    using System.Collections.Generic;
    using EdiModule;
    using Bridge1C.DomainEntities;

    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    Repository repository = new Repository(new Connector(@"C:\Users\Максим\Documents\InfoBase7", "", ""));
        //    var warehouse = repository.GetWareHouse("Наименование", "Склад");
        //    var shop = repository.GetShop(warehouse);
        //    var ware = repository.GetWare("Код", "", "00-00004972");
        //    var counteragent = repository.GetCounteragent("Наименование", "Комос");

        //    List<WaybillRow> waybillRows = new List<WaybillRow>();
        //    waybillRows.Add(new WaybillRow { Товар = ware, Количество = 5, СтавкаНДС = 0, Цена = 10, СуммаНДС = 0 });

        //    Assert.IsTrue(repository.AddNewWaybill("ergergege", DateTime.Now, counteragent, warehouse, shop, waybillRows));
        //}

        //[TestMethod]
        //public void GetCounteragentTest()
        //{
        //    Repository repository = new Repository(new Connector(@"C:\Users\Максим\Documents\InfoBase7", "", ""));

        //    var sup = repository.GetCounteragent(Requisites.GLN_Counteragent, "4607068529991");
        //    Console.WriteLine(sup.Наименование);
        //}
        //[TestMethod]
        //public void RepositoryTest()
        //{
        //    Repository repository = new Repository(new Connector(@"C:\Users\Максим\Documents\InfoBase7", "", ""));
        //    dynamic supplier = repository.GetCounteragent(Requisites.Name, "Комос");
        //    dynamic ware = repository.GetWare(Requisites.ExCode_Ware, supplier, "ПЖП1016461");
        //    var wares = repository.GetAllWares();

        //    Console.WriteLine(ware.Наименование);
        //}

        [TestMethod]
        public void AddNewWareTest()
        {
            RepositoryService service = new RepositoryService(@"C:\Users\Максим\Documents\InfoBase7", "", "");

            Ware startWare = new Ware
            {
                Code = "12345абвг",
                Name = "Клавиатура черная",
                FullName = "Клавиатура черная Defender",
                Unit = service.GetUnit(Requisites.InternationalReduction_Unit, "PCE"),
                BarCodes = new List<string>
                {
                    "123456",
                    "8765432",
                    "3535353"
                },
                ExCodes = new List<WareExCode>
                {
                    new WareExCode
                    {
                        Counteragent = service.GetCounteragent(Requisites.Name, "Комос"),
                        Value = "EX12345"
                    },
                    new WareExCode
                    {
                        Counteragent = service.GetCounteragent(Requisites.Name, "Комос"),
                        Value = "EX54321"
                    }
                }
            };
            Assert.IsTrue(service.AddNewWare(startWare));
        }



		[TestMethod]
        public void ServiceTest()
        {
            RepositoryService repositoryService = new RepositoryService(@"C:\Users\Максим\Documents\InfoBase7", "", "");

            var ware = repositoryService.GetWare(Requisites.Name, "Товар");

            foreach (var item in ware.ExCodes)
            {
                Console.WriteLine(item);
            }
        }

    }
}
