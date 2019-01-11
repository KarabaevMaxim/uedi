namespace Bridge1C.DomainEntities
{
    using System;
    using System.Collections.Generic;

    public class Waybill
    {
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public Counteragent Supplier { get; set; }
        public Warehouse Warehouse { get; set; }
        public Organization Organization { get; set; }
        public Shop Shop { get; set; }
        public List<WaybillRow> Positions { get; set; }
    }
}
