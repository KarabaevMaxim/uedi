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
			this.InitWarehouseReference();

		}

        public void InitProductReference()
        {
            this.ProductReference = CoreInit.RepositoryService.GetAllWares();
        }

		public void InitWarehouseReference()
		{
			this.WarehouseReference = CoreInit.RepositoryService.GetAllWarehouses();
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
			this.AddWarehouse(waybill.Warehouse);
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

		public List<Bridge1C.DomainEntities.Warehouse> GetWarehouses()
		{
			return this.Warehouses;
		}

		public void AddWarehouse(Bridge1C.DomainEntities.Warehouse warehouse)
		{
			if (!this.Warehouses.Contains(warehouse))
				this.Warehouses.Add(warehouse);
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

		/// <summary>
		/// Список складов из небоработанных накладных.
		/// </summary>
		private List<Bridge1C.DomainEntities.Warehouse> Warehouses { get; set; } = new List<Bridge1C.DomainEntities.Warehouse>();

		/// <summary>
		/// Справочник складов из базы.
		/// </summary>
		public List<Bridge1C.DomainEntities.Warehouse> WarehouseReference { get; private set; } = new List<Bridge1C.DomainEntities.Warehouse>();

		/// <summary>
		/// Справочник товаров из базы.
		/// </summary>
		public List<Bridge1C.DomainEntities.Ware> ProductReference { get; private set; } = new List<Bridge1C.DomainEntities.Ware>();
    }
}
