namespace EdiModuleCore.XEntities.DocWaybill
{
    using System;

    public class Waybill : IDoc
    {
        /// <summary>
        /// NUMBER
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// DATE
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IDocHeader Header { get; set; }

        public override string ToString()
        {
            return string.Format("Номер: {0}, Дата: {1}\n Хедер: {2}", this.Number, this.Date, this.Header);
        }
    }
}
