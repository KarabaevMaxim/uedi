namespace Bridge1C
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DomainEntities;

    public class RepositoryService : IRepositoryService
    {
        public RepositoryService(string dataBaseFile, string login, string pass)
        {
            this.Repository = new Repository(new Connector(dataBaseFile, login, pass));
        }

        public RepositoryService(string connectionString)
        {
            this.Repository = new Repository(new Connector(connectionString));
        }

        /// <summary>
        /// Добавить новый товар.
        /// </summary>
        /// <param name="ware">Товар для добавления.</param>
        /// <returns>true, если успешно, иначе false.</returns>
        public bool AddNewWare(Ware ware)
        {
            ware.Code = this.Repository.AddNewWare(ware.Name, ware.FullName, ware.Unit.International, ware.BarCodes);

            if(!string.IsNullOrWhiteSpace(ware.Code))
            {
                if (ware.BarCodes != null && ware.BarCodes.Any())
                    this.Repository.AddNewBarcodes(ware.Code, ware.BarCodes);

                if (ware.ExCodes != null && ware.ExCodes.Any())
                    this.Repository.AddNewExCodes(ware.Code, ware.ExCodes.Select(ec => ec.Counteragent.GLN).ToList(), ware.ExCodes.Select(ec => ec.Value).ToList());

                return true;
            }

            return false;
        }

        /// <summary>
        /// Получить товар.
        /// </summary>
        /// <param name="prop">Свойство, по которому будет осуществлен поиск.</param>
        /// <param name="propValue">Значение свойства для поиска.</param>
        /// <param name="counteragentCode">Код контрагента, если поиск будет по внешнему коду.</param>
        /// <returns>Объект товара.</returns>
        public Ware GetWare(Requisites prop, string propValue, string counteragentGln = "")
        {
            var ware = this.Repository.GetWare(prop, propValue, counteragentGln);

            if (ware == null)
                return null;

            Ware result = this.GetDomainWareFromDbWare(ware);
            return result;
        }

        public List<Ware> GetAllWares()
        {
            List<Ware> result = new List<Ware>();

            foreach (var item in Repository.GetAllWares())
                result.Add(this.GetDomainWareFromDbWare(item));

            return result;
        }

        /// <summary>
        /// Получить контрагента по указанному реквизиту.
        /// </summary>
        /// <param name="prop">Свойство, по которому будет осуществляться поиск.</param>
        /// <param name="propValue">Значение свойства для поиска.</param>
        /// <returns>Контрагент.</returns>
        public Counteragent GetCounteragent(Requisites prop, string propValue)
        {
            Counteragent result = new Counteragent();
            var counteragent = this.Repository.GetCounteragent(prop, propValue);

            if (counteragent == null)
                return null;

            result.Code = counteragent.Код;
            result.Name = counteragent.Наименование;
            result.FullName = counteragent.НаименованиеПолное;
            result.GLN = counteragent.ГЛН;
            return result;
        }

		public List<Counteragent> GetAllCounteragents()
		{
			List<Counteragent> result = new List<Counteragent>();
			var list = this.Repository.GetAllCounteragents();

			foreach (var item in list)
			{
				Counteragent counteragent = new Counteragent
				{
					Code = item.Код,
					Name = item.Наименование,
					FullName = item.НаименованиеПолное,
					GLN = item.ГЛН
				};
				result.Add(counteragent);
			}

			return result;
		}

		public async Task<List<Counteragent>> GetAllCounteragentsAsync()
        {
            List<Counteragent> result = new List<Counteragent>();
			var list = await Task.Run(() => this.Repository.GetAllCounteragents());

            foreach (var item in list)
            {
                Counteragent counteragent = new Counteragent
                {
                    Code = item.Код,
                    Name = item.Наименование,
                    FullName = item.НаименованиеПолное,
                    GLN = item.ГЛН
                };
                result.Add(counteragent);
            }

            return result;
        }

		/// <summary>
		/// Инициализирует поле ГЛН контрагента counteagent значением gln, если в базе есть контрагент с ГЛН gln 
		/// </summary>
		/// <param name="counteragent">Контрагент для инициализации.</param>
		/// <param name="gln">ГЛН.</param>
		public async Task<bool> RematchingCounteragentAsync(Counteragent counteragent, string gln)
		{
			if (counteragent == null || string.IsNullOrWhiteSpace(gln))
				return false;

			bool result = await Task.Run(() => this.RematchingCounteragent(counteragent, gln));
			return result;
		}

		/// <summary>
		/// Инициализирует поле ГЛН контрагента counteagent значением gln, если в базе есть контрагент с ГЛН gln 
		/// </summary>
		/// <param name="counteragent">Контрагент для инициализации.</param>
		/// <param name="gln">ГЛН.</param>
		public bool RematchingCounteragent(Counteragent counteragent, string gln)
		{
			if (counteragent == null || string.IsNullOrWhiteSpace(gln))
				return false;

			return this.Repository.RematchingCounteragent(counteragent.Code, gln);
		}

		/// <summary>
		/// Получить единицу измерения.
		/// </summary>
		/// <param name="prop">Свойство, по которому будет осуществлен поиск.</param>
		/// <param name="propValue">Значение свойства для поиска.</param>
		/// <returns>ЕИ.</returns>
		public Unit GetUnit(Requisites prop, string propValue)
        {
            Unit result = new Unit();
            var unit = this.Repository.GetUnit(prop, propValue);

            if (unit == null)
                return null;

            result.Code = unit.Код;
            result.Name = unit.Наименование;
            result.FullName = unit.НаименованиеПолное;
            result.International = unit.МеждународноеСокращение;
            return result;
        }

        public Warehouse GetWarehouse(Requisites prop, string propValue)
        {
            Warehouse result = new Warehouse();
            var warehouse = this.Repository.GetWareHouse(prop, propValue);

            if (warehouse == null)
                return null;

            result.Code = warehouse.Код;
            result.Name = warehouse.Наименование;
            result.Shop = new Shop { Code = warehouse.Магазин.Код, Name = warehouse.Магазин.Наименование };
            return result;
        }

		public List<Warehouse> GetAllWarehouses()
		{
			List<Warehouse> result = new List<Warehouse>();

			foreach (var item in Repository.GetAllWarehouses())
				result.Add(new Warehouse
				{
					Code = item.Код,
					Name = item.Наименование,
					Shop = new Shop
					{
						Code = item.Магазин.Код,
						Name = item.Магазин.Наименование
					}
				});

			return result;
		}

		public List<Warehouse> GetWarehousesByActiveUser()
		{
			throw new System.NotImplementedException();
		}

		public bool RematchingWarehouse(string warehouseCode, string gln)
		{
			return this.Repository.UpdateWarehouseGLN(warehouseCode, gln);
		}

		public Shop GetShop(string warehouseCode)
        {
            Shop result = new Shop();
            var shop = this.Repository.GetShop(warehouseCode);

            if (shop == null)
                return null;

            result.Code = shop.Код;
            result.Name = shop.Наименование;
            return result;
        }

		public Organization GetOrganization(Requisites prop, string propValue)
		{
			Organization result = new Organization();
			var organization = this.Repository.GetOrganization(prop, propValue);

			if (organization == null || string.IsNullOrWhiteSpace(organization.Код))
				return null;

			result.Code = organization.Код;
			result.Name = organization.Наименование;
			result.GLN = organization.ГЛН; // todo: В 1С я еще не добавил реквизит ГЛН организации
			return result;
		}

		public User GetCurrentUser()
		{
			throw new System.NotImplementedException();
		}

		public bool AddNewWaybill(Waybill waybill)
        {
            var supplier = this.Repository.GetCounteragent(Requisites.Code, waybill.Supplier.Code);
            var warehouse = this.Repository.GetWareHouse(Requisites.Code, waybill.Warehouse.Code);
            var shop = this.Repository.GetShop(warehouse);
            return this.Repository.AddNewWaybill(waybill.Number, waybill.Date, supplier, warehouse, shop, waybill.Positions);
        }

        public bool AddNewExCodeToWare(Ware ware, WareExCode exCode)
        {
            ware.ExCodes.Add(exCode);
            return this.Repository.AddNewExCode(ware.Code, exCode.Counteragent.GLN, exCode.Value);
        }

		public bool RemoveExCode(WareExCode exCode) // todo: Не забыть реализовать метод
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Маппит объект номенклатуры базы данных в доменную номенклатуру и возвращает ее объект.
		/// </summary>
		/// <param name="ware">Объект номенклатуры базы данных.</param>
		/// <returns>Инициализированный объект домееной номенклатуры.</returns>
		private Ware GetDomainWareFromDbWare(dynamic ware)
        {
            if (ware == null)
                return null;

            Ware result = new Ware();
            result.Code = ware.Код;
            result.Name = ware.Наименование;
            result.FullName = ware.НаименованиеПолное;
            result.Unit = this.GetUnit(Requisites.Code, ware.ЕдиницаИзмерения.Код);
            result.ExCodes = this.GetWareExCodes(ware);
            result.BarCodes = this.GetWareBarcodes(ware);
            return result;
        }

        /// <summary>
        /// Получить список внешних кодов для товара.
        /// </summary>
        /// <param name="ware">Товар.</param>
        /// <returns>Список внешних кодов.</returns>
        private List<WareExCode> GetWareExCodes(dynamic ware)
        {
            var exCodes = this.Repository.GetWareExCodes(ware);

            if (exCodes == null)
                return null;

            List<WareExCode> result = new List<WareExCode>();

            foreach (var item in exCodes)
            {
                WareExCode wec = new WareExCode();
                wec.Value = item.ВнешнийКод;
                wec.Counteragent = this.GetCounteragent(Requisites.GLN, item.ГЛНКонтрагента);
                result.Add(wec);
            }

            return result;
        }

        /// <summary>
        /// Получить штрихкода товара.
        /// </summary>
        /// <param name="ware">Товар.</param>
        /// <returns>Коллекция штрихкодов.</returns>
        private List<string> GetWareBarcodes(dynamic ware)
        {
            if (ware == null)
                return null;

            return this.Repository.GetWareBarcodes(ware);
        }

        private Repository Repository { get; set; }
	}
}
