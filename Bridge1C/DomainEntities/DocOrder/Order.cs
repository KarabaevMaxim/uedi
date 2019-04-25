namespace DAL.DomainEntities.DocOrder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DAL.DomainEntities.Spr;

    public class Order : IDoc
    {
        public int IdentityColumn { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Counteragent Supplier { get; set; }
        public IEnumerable<Warehouse> WarehouseList { get; set; }
        public Organization Organization { get; set; }
        public IEnumerable<IDocRow> Positions { get; set; }
    }
}
