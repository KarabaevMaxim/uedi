namespace EdiModuleCore.XEntities
{
    public interface IDocPosition
    {
        string Number { get; set; }
        string WareName { get; set; }
        float Quantity { get; set; }
        string Unit { get; set; }
        string Barcode { get; set; }
    }
}
