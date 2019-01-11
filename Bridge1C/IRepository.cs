namespace Bridge1C
{
    using System;
    using System.Collections.Generic;

    public interface IRepository
    {
        dynamic GetCounteragent(Requisites propertyName, string propertyValue);
        dynamic GetWareHouse(Requisites propertyName, string propertyValue);
        dynamic GetShop(dynamic warehouse);
        dynamic GetTaxRate(TaxRates rate);
        dynamic GetUnit(Requisites propertyName, string propertyValue);
        dynamic GetWare(Requisites propertyName, string propertyValue, string counteragentCode);
        dynamic GetBarCodeType(BarcodeTypes type);
        string AddNewWare(string name, string fullName, string unit, List<string> barCodes);
        bool AddNewBarcode(dynamic ware, string value);
        bool AddNewExCode(dynamic ware, dynamic counteragent, string value);
        bool AddNewWaybill(string number, DateTime date, dynamic counteragent, dynamic warehouse, dynamic shop, List<DomainEntities.WaybillRow> rows);
    }
}
