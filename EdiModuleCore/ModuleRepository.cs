namespace EdiModuleCore
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Model;
	using System.Threading.Tasks;
	using Comparators;
	using Newtonsoft.Json;
	using NLog;

    public class ModuleRepository
    {
		public ModuleRepository()
        {
			this.logger.Info("Инициализация объекта ModuleRepository..");
            this.InitProductReference();
			this.InitWarehouseReference();
            this.InitCounteragentReference();
			this.logger.Info("Инициализация объекта ModuleRepository завершена");
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

        /// <summary>
        /// Добавить накладную в общий список.
        /// </summary>
        private void AddWaybillToGeneralList(Waybill waybill)
        {
			if (waybill == null)
				throw new ArgumentNullException("waybill");

			this.logger.Info("Проверка необходимости загрузки накладной {0} в главный список", JsonConvert.SerializeObject(waybill));

			if (!this.GeneralWaybillList.Contains(waybill))
            {
                Waybill instance = new Waybill();

                foreach (var item in typeof(Waybill).GetProperties())
                    item.SetValue(instance, item.GetValue(waybill));

                this.GeneralWaybillList.Add(instance);
				this.logger.Info("Накладная добавлена в главный список.");
			}
        }
        
        /// <summary>
        /// Добавить необработанную накладную.
        /// </summary>
        private void AddUnprocessedWaybill(Waybill waybill)
        {
			if (waybill == null)
				throw new ArgumentNullException("waybill");

			this.logger.Info("Проверка необходимости загрузки накладной {0} в список необработанных накладных", JsonConvert.SerializeObject(waybill));

			if (!this.UnprocessedWaybills.Contains(waybill))
			{
				this.UnprocessedWaybills.Add(waybill);
				this.logger.Info("Накладная добавлена в главный список");
			}
		}

        /// <summary>
        /// Добавить новую накладную.
        /// </summary>
        public void AddWaybill(Waybill waybill)
        {
			if (waybill == null)
				throw new ArgumentNullException("waybill");

			this.AddWaybillToGeneralList(waybill);
            this.AddUnprocessedWaybill(this.GeneralWaybillList.Last());
			this.AddWarehouse(waybill.Warehouse);
			this.AddCounteragent(waybill.Supplier);
        }

		/// <summary>
		/// Удалить накладную из списка необработанных накладных.
		/// </summary>
		/// <param name="waybill"></param>
		/// <returns></returns>
		public bool RemoveUnprocessedWaybill(Waybill waybill)
        {
			if (waybill == null)
				throw new ArgumentNullException("waybill");

			bool result = this.UnprocessedWaybills.Remove(waybill);

			if(result)
				this.logger.Info("Накладная удалена из списка необработанных накладных");
			else
				this.logger.Warn("Накладная не удалена из списка необработанных накладных");

			return result;
        }

        public List<Waybill> GetGeneralWaybillList()
        {
            return this.GeneralWaybillList;
        }

		public void UpdateWarehouses()
		{
			this.logger.Info("Обновление справочника складов");
			this.InitWarehouseReference();
			MatchingModule.UpdateWHMatching(this.Warehouses);

			var warehouse = this.Warehouses.FirstOrDefault(wh => wh.InnerWarehouse == null);

			if (warehouse == null)
				this.logger.Warn("Не удалось найти сопоставленный склад. Результат: {0}", JsonConvert.SerializeObject(warehouse));
			else
				this.logger.Info("Найден сопоставленный склад. Результат: {0}", JsonConvert.SerializeObject(warehouse));

			this.logger.Info("Справочник складов обновлен. Список: {0}", JsonConvert.SerializeObject(this.Warehouses));
		}

		public void UpdateCounteragents()
		{
			this.logger.Info("Обновление справочника контрагентов");
			this.InitCounteragentReference();
			MatchingModule.UpdateSupMatching(this.Counteragents);
			this.logger.Info("Справочник контрагентов обновлен. Список: {0}", JsonConvert.SerializeObject(this.Counteragents));
		}

		public void AddMatchedWare(MatchedWare ware)
        {
			if (ware == null)
				throw new ArgumentNullException("ware");

			this.logger.Info("Проверка необходимости загрузки номенклатуры {0} в список номенклатуры", JsonConvert.SerializeObject(ware));

			if (!this.MatchedWares.Contains(ware, new MatchedWareComparator()))
			{
				this.MatchedWares.Add(ware);
				this.logger.Info("Номенклатура добавлена в список");
			}
        }

        public List<MatchedWare> GetMatchedWares()
        {
            return this.MatchedWares;
        }

        public List<Waybill> GetUnprocessedWaybills()
        {
            return this.UnprocessedWaybills;
        }

		public void ClearUnprocessedWaybills()
		{
			this.UnprocessedWaybills.Clear();
		}

		public List<MatchedWarehouse> GetWarehouses()
		{
			return this.Warehouses;
		}

		public void AddWarehouse(MatchedWarehouse warehouse)
		{
			if (warehouse == null)
				throw new ArgumentNullException("warehouse");

			this.logger.Info("Проверка необходимости загрузки склада {0} в список", JsonConvert.SerializeObject(warehouse));

			if (!this.Warehouses.Contains(warehouse, new MatchedWarehouseComparator()))
			{
				this.Warehouses.Add(warehouse);
				this.logger.Info("Склад добавлен в список");
			}
		}

		public List<MatchedCounteragent> GetCounteragents()
		{
			return this.Counteragents;
		}

		public void AddCounteragent(MatchedCounteragent counteragent)
		{
			if (counteragent == null)
				throw new ArgumentNullException("counteragent");

			this.logger.Info("Проверка необходимости загрузки контрагента {0} в список", JsonConvert.SerializeObject(counteragent));

			if (!this.Counteragents.Contains(counteragent, new MatchedCounteragentComparator()))
			{
				this.Counteragents.Add(counteragent);
				this.logger.Info("Контрагент добавлен в список");
			}
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

		private readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
