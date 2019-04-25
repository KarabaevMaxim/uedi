namespace DAL.DomainEntities.DocOrder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DAL.DomainEntities.Spr;

    public class OrderRow : IDocRow
    {
        public Ware Ware { get; set; }
        public Unit Unit { get; set; }
        public float Count { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
