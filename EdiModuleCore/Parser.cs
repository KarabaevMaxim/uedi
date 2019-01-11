using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EdiModuleCore
{
    using System.Xml;
    using XEntities;

    /// <summary>
    /// XML теги документов.
    /// </summary>
    public enum XmlTag
    {
        NUMBER,
        DATE,
        HEAD,
        BUYER,
        SUPPLIER,
        DELIVERYPLACE,
        POSITION,
        POSITIONNUMBER,
        DESCRIPTION,
        DELIVEREDUNIT,
        DELIVEREDQUANTITY,
        AMOUNTWITHVAT,
        PRICE,
        AMOUNT,
        PRODUCT,
        PRODUCTIDSUPPLIER,
        TAXRATE
    }

    /// <summary>
    /// Вспомогательный класс для парсинга XML файлов.
    /// </summary>
    public static class Parser
    {
        public static string GetDocumentTypeName(string xml)
        {
            try
            {
                XDocument xDocument = XDocument.Parse(xml);
                return xDocument.Root.Name.ToString();
            }
            catch(XmlException ex)
            {
                throw ex;
            }
        }

        public static string GetDocumentEncodingName(string xml)
        {
            try
            {
                XDocument xDocument = XDocument.Parse(xml);
                return xDocument.Declaration.Encoding;
            }
            catch(XmlException ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<XElement> GetXmlElements(string xml)
        {
            try
            {
                XDocument xDocument = XDocument.Parse(xml);
                return xDocument.Descendants();
            }
            catch (XmlException ex)
            {
                throw ex;
            }
        }

        public static Waybill GetWaybill(string xml)
        {
            IEnumerable<XElement> xElements = Parser.GetXmlElements(xml);
            try
            {
                Waybill waybill = new Waybill
                {
                    Number = xElements.FirstOrDefault(e => e.Name == XmlTag.NUMBER.ToString()).Value,
                    Date = DateTime.Parse(xElements.FirstOrDefault(e => e.Name == XmlTag.DATE.ToString()).Value),
                    Header = Parser.GetHeader(xElements)
                };

                return waybill;
            }
            catch (FormatException ex)
            {
                throw ex;
            }
        }

        public static Header GetHeader(IEnumerable<XElement> xElements)
        {
            try
            {
                Header header = new Header
                {
                    SupplierGln = xElements.FirstOrDefault(e => e.Name == XmlTag.SUPPLIER.ToString()).Value,
                    BuyerGln = xElements.FirstOrDefault(e => e.Name == XmlTag.BUYER.ToString()).Value,
                    DeliveryPlace = xElements.FirstOrDefault(e => e.Name == XmlTag.DELIVERYPLACE.ToString()).Value,
                    Positions = Parser.GetWarePositions(xElements)
                };
                return header;
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
        }

        public static List<WarePosition> GetWarePositions(IEnumerable<XElement> xElements)
        {
            List<WarePosition> warePositions = new List<WarePosition>();
            IEnumerable<XElement> elements = xElements.Where(e => e.Name == XmlTag.POSITION.ToString());
            foreach (var item in elements)
            {
                warePositions.Add(Parser.GetWarePosition(item));
            }

            return warePositions;
        }

        public static WarePosition GetWarePosition(XElement root)
        {
            try
            {
                WarePosition warePosition = new WarePosition
                {
                    Number = root.Element(XmlTag.POSITIONNUMBER.ToString()).Value,
                    Amount = float.Parse(root.Element(XmlTag.AMOUNT.ToString()).Value.Replace('.', ',')),
                    AmountWithVat = float.Parse(root.Element(XmlTag.AMOUNTWITHVAT.ToString()).Value.Replace('.', ',')),
                    Barcode = root.Element(XmlTag.PRODUCT.ToString()).Value,
                    Price = float.Parse(root.Element(XmlTag.PRICE.ToString()).Value.Replace('.', ',')),
                    WareSupplierCode = root.Element(XmlTag.PRODUCTIDSUPPLIER.ToString()).Value,
                    Quantity = float.Parse(root.Element(XmlTag.DELIVEREDQUANTITY.ToString()).Value.Replace('.', ',')),
                    TaxRate = int.Parse(root.Element(XmlTag.TAXRATE.ToString()).Value),
                    Unit = root.Element(XmlTag.DELIVEREDUNIT.ToString()).Value,
                    WareName = root.Element(XmlTag.DESCRIPTION.ToString()).Value
                };

                return warePosition;
            }
            catch(NullReferenceException ex)
            {
                throw ex;
            }
            catch (FormatException ex)
            {
                throw ex;
            }
        }
    }
}
