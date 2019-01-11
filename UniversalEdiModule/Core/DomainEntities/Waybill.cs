namespace UniversalEdiModule.Core.DomainEntities
{
    using System;
    using System.Collections.Generic;

    public class Waybill
    {
        /// <summary>
        /// Идентификатор в системе.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Дата/время загрузки.
        /// </summary>
        public DateTime DownloadDate { get; set; }
        /// <summary>
        /// Номер накладной.
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Дата накладной.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Заголовок накладной.
        /// </summary>
        public Header Header { get; set; }

        public Waybill() { }

        public Waybill(string number, DateTime date, Header header)
        {
            this.Number = number;
            this.Date = date;
            this.Header = header;
        }

        public override string ToString()
        {
            return string.Format("Номер: {0}, Дата: {1}\n Хедер: {2}", this.Number, this.Date, this.Header);
        }
    }
}
