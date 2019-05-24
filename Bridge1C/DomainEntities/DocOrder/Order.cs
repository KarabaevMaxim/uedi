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
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Counteragent Supplier { get; set; }
        public IEnumerable<Warehouse> WarehouseList { get; set; }
        public Organization Organization { get; set; }
        public IEnumerable<IDocRow> Positions { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is Order order)
            {
                return this.Number == order.Number &&
                        this.Date == order.Date &&
                        this.DeliveryDate == order.DeliveryDate &&
                        this.Supplier.Equals(order.Supplier) &&
                        this.Organization.Equals(order.Organization) &&
                        ((this.WarehouseList == null && order.WarehouseList == null) || (!this.WarehouseList.Any() && !order.WarehouseList.Any()) || this.WarehouseList.Any(order.WarehouseList.Contains)) &&
                        ((this.Positions == null && order.Positions == null) || (!this.Positions.Any() && !order.Positions.Any()) || this.Positions.Any(order.Positions.Contains));
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
