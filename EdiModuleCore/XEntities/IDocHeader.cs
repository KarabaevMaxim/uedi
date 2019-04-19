namespace EdiModuleCore.XEntities
{
    using System.Collections.Generic;

    public interface IDocHeader
    {
        string SupplierGln { get; set; }
        string BuyerGln { get; set; }
        string DeliveryPlace { get; set; }
        IEnumerable<IDocPosition> Positions { get; set; }
    }
}
