namespace Bridge1C.DomainEntities
{
    using Spr;

    public interface IDocRow
    {
        Ware Ware { get; set; }
        Unit Unit { get; set; }
        float Count { get; set; }
        decimal Price { get; set; }
    }
}
