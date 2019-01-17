namespace Bridge1C
{
    using System.Collections.Generic;

    public enum Requisites
    {
        GLN_Counteragent,
        GLN_WareHouse,
        Name,
        Code,
        InternationalReduction_Unit,
        ExCode_Ware
    }

    public enum TaxRates
    {
        Tax_0,
        Tax_10,
        Tax_18,
        Tax_None
    }

    public enum BarcodeTypes
    {
        Ean_8,
        Ean_13,
        Ean_128
    }

    public enum EntityTypes
    {
        Контрагенты,
        Номенклатура,
        Склады,
        НашиОрганизации
    }

    public static class RequisiteBindingConfig
    {
        static RequisiteBindingConfig()
        {
            RequisiteBindingConfig.RequisiteBingings = new Dictionary<Requisites, string>
            {
                { Requisites.GLN_Counteragent, "ГЛН_Контрагента" },
                { Requisites.GLN_WareHouse, "ГЛН_Склада" },
                { Requisites.Name, "Наименование" },
                { Requisites.Code, "Код" },
                { Requisites.InternationalReduction_Unit, "МеждународноеСокращение" },
                { Requisites.ExCode_Ware, "Пусто" }
            };

            RequisiteBindingConfig.TaxBindings = new Dictionary<TaxRates, string>
            {
                { TaxRates.Tax_0, "0" },
                { TaxRates.Tax_10, "10" },
                { TaxRates.Tax_18, "18" },
                { TaxRates.Tax_None, "None" },
            };
        }

        public static Dictionary<Requisites, string> RequisiteBingings { get; private set; }
        public static Dictionary<TaxRates, string> TaxBindings { get; private set; }
    }
}
