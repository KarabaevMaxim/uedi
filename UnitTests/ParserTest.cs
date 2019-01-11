using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    using System.Xml.Linq;
    [TestClass]
    public class ParserTest
    {
        //private string testXML = 
        //    "<DESADV>" +
        //        "<NUMBER>123456</NUMBER>" +
        //        "<DATE>2017-01-27</DATE>" +
        //        "<HEAD>" +
        //            "<BUYER>4630018689483</BUYER>" +
        //            "<SUPPLIER>4607101559992</SUPPLIER>" +
        //            "<DELIVERYPLACE>4630018689216</DELIVERYPLACE>" +
        //            "<PACKINGSEQUENCE>" +
        //                "<POSITION>" +
        //                    "<POSITIONNUMBER>1</POSITIONNUMBER>" +
        //                    "<PRODUCT>4607101552641</PRODUCT>" +
        //                    "<PRODUCTIDSUPPLIER>00000000129</PRODUCTIDSUPPLIER>" +
        //                    "<DELIVEREDQUANTITY>2,660</DELIVEREDQUANTITY>" +
        //                    "<DELIVEREDUNIT>KGM</DELIVEREDUNIT>" +
        //                    "<PRICE>203,64</PRICE>" +
        //                    "<AMOUNTWITHVAT>595,85</AMOUNTWITHVAT>" +
        //                    "<AMOUNT>541,68</AMOUNT>" +
        //                    "<TAXRATE>10</TAXRATE>" +
        //                    "<DESCRIPTION>ВСТ Колбаса вареная Восточная Успех кат Б КуМК</DESCRIPTION>" +
        //                "</POSITION>" +
        //                "<POSITION>" +
        //                    "<POSITIONNUMBER>2</POSITIONNUMBER>" +
        //                    "<PRODUCT>32431414</PRODUCT>" +
        //                    "<PRODUCTIDSUPPLIER>42242</PRODUCTIDSUPPLIER>" +
        //                    "<DELIVEREDQUANTITY>4</DELIVEREDQUANTITY>" +
        //                    "<DELIVEREDUNIT>PCE</DELIVEREDUNIT>" +
        //                    "<PRICE>150</PRICE>" +
        //                    "<AMOUNTWITHVAT>595.85</AMOUNTWITHVAT>" +
        //                    "<AMOUNT>541.68</AMOUNT>" +
        //                    "<TAXRATE>10</TAXRATE>" +
        //                    "<DESCRIPTION>Яблоки</DESCRIPTION>" +
        //                "</POSITION>" +
        //            "</PACKINGSEQUENCE>" +
        //        "</HEAD>" +
        //    "</DESADV>";
        [TestMethod]
        public void GetWaybillTest()
        {
        //    Waybill wb = Parser.GetWaybill(Parser.GetXmlElements(testXML));

        //    Console.WriteLine(wb);

        }
    }
}
