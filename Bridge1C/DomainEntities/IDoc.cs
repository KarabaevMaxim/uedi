namespace Bridge1C.DomainEntities
{
    using System;
    using System.Collections.Generic;
    using Spr;

    public interface IDoc
    {
        string Number { get; set; }
        DateTime Date { get; set; }
        Counteragent Supplier { get; set; }
        Warehouse Warehouse { get; set; }
        Organization Organization { get; set; }
        IEnumerable<IDocRow> Positions { get; set; }
    }
}
