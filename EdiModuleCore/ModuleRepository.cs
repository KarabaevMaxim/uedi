namespace EdiModuleCore
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Model;
	using System.Threading.Tasks;
	using Comparators;

    public class ModuleRepository
    {
		public ModuleRepository()
        {
            this.InitProductReference();
			this.InitWarehouseReference();
            this.InitCounteragentReference();
        }

        public void InitProductReference()
        {
            this.ProductReference = CoreInit.RepositoryService.GetAllWares();
        }

		public void InitWarehouseReference()
		{
			this.WarehouseReference = CoreInit.RepositoryService.GetAllWarehouses();
		}

		public void InitCounteragentReference()
		{
			this.CounteragentReference = CoreInit.RepositoryService.GetAllCounteragents();
		}

		public async void InitCounteragentReferenceAsync()
        {
            this.CounteragentReference = await CoreInit.RepositoryService.GetAllCounteragentsAsync();
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
        public bool AddWaybill(Waybill waybill)
        {
            this.AddWaybillToGeneralList(waybill);
            this.AddUnprocessedWaybill(this.GeneralWaybillList.Last());
			this.AddWarehouse(waybill.Warehouse);
			this.AddCounteragent(waybill.Supplier);
			return true;
        }

		public async Task<bool> AddWaybillAsync(Waybill waybill)
		{
			return await Task.Run(() => this.AddWaybill(waybill));
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
            if (!this.MatchedWares.Contains(ware, new MatchedWareComparator()))
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

		public List<MatchedWarehouse> GetWarehouses()
		{
			return this.Warehouses;
		}

		public void AddWarehouse(MatchedWarehouse warehouse)
		{
			if (!this.Warehouses.Contains(warehouse, new MatchedWarehouseComparator()))
				this.Warehouses.Add(warehouse);
		}

		public List<MatchedCounteragent> GetCounteragents()
		{
			return this.Counteragents;
		}

		public void AddCounteragent(MatchedCounteragent counteragent)
		{
			if (!this.Counteragents.Contains(counteragent, new MatchedCounteragentComparator()))
				this.Counteragents.Add(counteragent);
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
		private List<MatchedWarehouse> Warehouses { get; set; } = new List<MatchedWarehouse>();

		/// <summary>
		/// Справочник складов из базы.
		/// </summary>
		public List<Bridge1C.DomainEntities.Warehouse> WarehouseReference { get; private set; } = new List<Bridge1C.DomainEntities.Warehouse>();

		/// <summary>
		/// Список контрагентов из необработанных накладных.
		/// </summary>
		private List<MatchedCounteragent> Counteragents { get; set; } = new List<MatchedCounteragent>();

		/// <summary>
		/// Справочник контрагентов из базы.
		/// </summary>
		public List<Bridge1C.DomainEntities.Counteragent> CounteragentReference { get; private set; } = new List<Bridge1C.DomainEntities.Counteragent>();

        /// <summary>
        /// Справочник товаров из базы.
        /// </summary>
        public List<Bridge1C.DomainEntities.Ware> ProductReference { get; private set; } = new List<Bridge1C.DomainEntities.Ware>();
    }
}
