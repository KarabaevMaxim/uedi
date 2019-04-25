namespace DAL.DomainEntities
{
    using System;
    using System.Collections.Generic;
    using Spr;

    public interface IDoc
    {
        string Number { get; set; }
        DateTime Date { get; set; }
        Counteragent Supplier { get; set; }
        IEnumerable<Warehouse> WarehouseList { get; set; }
        Organization Organization { get; set; }
        IEnumerable<IDocRow> Positions { get; set; }
    }
}
