namespace EdiModuleCore
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Model;

    public class ModuleRepository
    {
        public ModuleRepository()
        {
            this.InitProductReference();
        }

        public void InitProductReference()
        {
            this.ProductReference = CoreInit.RepositoryService.GetAllWares();
        }

        /// <summary>
        /// Добавить накладную в общий список.
        /// </summary>
        private void AddWaybillToGeneralList(Waybill waybill)
        {
            if (!this.GeneralWaybillList.Contains(waybill))
            {
                Waybill instance = new Waybill();

                foreach (var item in typeof(Waybill).GetProperties())
                    item.SetValue(instance, item.GetValue(waybill));

                this.GeneralWaybillList.Add(instance);
            }
        }
        
        /// <summary>
        /// Добавить необработанную накладную.
        /// </summary>
        private void AddUnprocessedWaybill(Waybill waybill)
        {
            if (!this.UnprocessedWaybills.Contains(waybill))
                this.UnprocessedWaybills.Add(waybill);
        }

        /// <summary>
        /// Добавить новую накладную.
        /// </summary>
        public void AddWaybill(Waybill waybill)
        {
            this.AddWaybillToGeneralList(waybill);
            this.AddUnprocessedWaybill(this.GeneralWaybillList.Last());
        }

        /// <summary>
        /// Удалить накладную из списка необработанных накладных.
        /// </summary>
        /// <param name="waybill"></param>
        /// <returns></returns>
        public bool RemoveUnprocessedWaybill(Waybill waybill)
        {
            return this.UnprocessedWaybills.Remove(waybill);
        }

        public List<Waybill> GetGeneralWaybillList()
        {
            return this.GeneralWaybillList;
        }

        public void AddMatchedWare(MatchedWare ware)
        {
            if (!this.MatchedWares.Contains(ware))
                this.MatchedWares.Add(ware);
        }

        public List<MatchedWare> GetMatchedWares()
        {
            return this.MatchedWares;
        }

        public List<Waybill> GetUnprocessedWaybills()
        {
            return this.UnprocessedWaybills;
        }

        /// <summary>
        /// Список необработанных накладных.
        /// </summary>
        private List<Waybill> UnprocessedWaybills { get; set; } = new List<Waybill>();

        /// <summary>
        /// Список накладных за все время.
        /// </summary>
        private List<Waybill> GeneralWaybillList { get; set; } = new List<Waybill>(); // todo: надо куда-то сохранять этот массив

        private List<MatchedWare> MatchedWares { get; set; } = new List<MatchedWare>();

        public List<Bridge1C.DomainEntities.Ware> ProductReference { get; private set; } = new List<Bridge1C.DomainEntities.Ware>();
    }
}
