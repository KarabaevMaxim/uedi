namespace DAL.DomainEntities.DocWaybill
{
    using System;
    using System.Collections.Generic;
    using Spr;

    public class Waybill : IDoc
    {
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public Counteragent Supplier { get; set; }
        public Warehouse Warehouse { get; set; }
        public Organization Organization { get; set; }
        public Shop Shop { get; set; }
        public IEnumerable<IDocRow> Positions { get; set; }

		public override string ToString()
		{
			return string.Format("№ {0} от {1}", this.Number, this.Date);
		}
	}
}
