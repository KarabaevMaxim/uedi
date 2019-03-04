namespace Bridge1C.Itida
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Bridge1C.DomainEntities;

	public class ItidaRepositoryService : IRepositoryService
	{
		public ItidaRepositoryService(string connectionString)
		{
			this.Repository = new ItidaRepository(connectionString);
		}

		/// <summary>
		/// Записать в базу данных объект номенклатуры.
		/// </summary>
		/// <param name="ware">Объект для записи.</param>
		public bool AddNewWare(Ware ware)
		{
			return this.Repository.AddNewWare(ware);
		}

		/// <summary>
		/// Возвращает объект номенклатуры по указанному реквизиту.
		/// </summary>
		/// <param name="prop">Реквизит для поиска.</param>
		/// <param name="propValue">Значение реквизита для поиска.</param>
		/// <param name="counteragentGln">ГЛН контрагента (необходимо в случае поиска по внешнему коду номенклатуры).</param>
		public Ware GetWare(Requisites prop, string propValue, string counteragentGln = "")
		{
			return this.Repository.GetWare(prop, propValue, counteragentGln);
		}

		/// <summary>
		/// Получить список товаров.
		/// </summary>
		public List<Ware> GetAllWares()
		{
			return this.Repository.GetAllWares();
		}

		/// <summary>
		/// Получить контрагента.
		/// </summary>
		/// <param name="prop">Реквизит для поиска.</param>
		/// <param name="propValue">Значение реквизита для поиска.</param>
		public Counteragent GetCounteragent(Requisites prop, string propValue)
		{
			return this.Repository.GetCounteragent(prop, propValue);
		}

		/// <summary>
		/// Инициализирует значением gln поле ГЛН контрагента с кодом counteragentCode, если перед этим в базе найден контрагент с 
		/// ГЛН gln, после будет переинициализировано пустой строкой.
		/// </summary>
		/// <param name="counteragentCode">Код контрагента для переициализации.</param>
		/// <param name="gln">ГНЛ.</param>
		/// <returns>true, если операция завершена успешно, иначе false.</returns>
		public bool RematchingCounteragent(Counteragent counteragent, string gln)
		{
			if (counteragent == null || string.IsNullOrWhiteSpace(gln))
				return false;

			return this.Repository.RematchingCounteragent(counteragent.Code, gln);
		}

		/// <summary>
		/// Получить ЕИ.
		/// </summary>
		/// <param name="prop">Реквизит для поиска.</param>
		/// <param name="propValue">Значение реквизита для поиска.</param>
		public Unit GetUnit(Requisites prop, string propValue)
		{
			return this.Repository.GetUnit(prop, propValue);
		}

		/// <summary>
		/// Получить склад.
		/// </summary>
		/// <param name="prop">Реквизит для поиска.</param>
		/// <param name="propValue">Значение реквизита для поиска.</param>
		public Warehouse GetWarehouse(Requisites prop, string propValue)
		{
			return this.Repository.GetWarehouse(prop, propValue);
		}

		/// <summary>
		/// Получить список складов.
		/// </summary>
		public List<Warehouse> GetAllWarehouses()
		{
			return this.Repository.GetAllWarehouses();
		}

		/// <summary>
		/// Инициализирует поле ex_code склада warehouse значением gln, если в базе есть контрагент с ex_code gln 
		/// </summary>
		/// <param name="warehouseCode">Код склада для инициализации.</param>
		/// <param name="gln">ГЛН.</param>
		public bool RematchingWarehouse(string warehouseCode, string gln)
		{
			if (string.IsNullOrWhiteSpace(warehouseCode) || string.IsNullOrWhiteSpace(gln))
				return false;

			return this.Repository.RematchingWarehouse(warehouseCode, gln);
		}

		/// <summary>
		/// Нереализуемый метод.
		/// </summary>
		/// <param name="warehouseCode"></param>
		public Shop GetShop(string warehouseCode)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Нереализуемый метод.
		/// </summary>
		/// <param name="warehouseCode"></param>
		/// <returns></returns>
		//public Organization GetOrganization(string warehouseCode)
		//{
		//	throw new NotImplementedException();
		//}

		/// <summary>
		/// Получить организацию.
		/// </summary>
		/// <param name="prop">Реквизит для поиска.</param>
		/// <param name="propValue">Значение реквизита для поиска.</param>
		public Organization GetOrganization(Requisites prop, string propValue)
		{
			return this.Repository.GetOrganization(prop, propValue);
		}

		/// <summary>
		/// Добавить новую накладную в базу данных.
		/// </summary>
		/// <param name="waybill">Накладная для записи.</param>
		public bool AddNewWaybill(Waybill waybill)
		{
			if (waybill == null)
				return false;

			return this.Repository.AddNewWaybill(waybill);
		}

		/// <summary>
		/// Получить список всех контрагентов из базы.
		/// </summary>
		public List<Counteragent> GetAllCounteragents()
		{
			return this.Repository.GetAllCounteragents();
		}

		/// <summary>
		/// Добавляет внешний код номенклатуре и удаляет этот внешний код из предыдушей номенклатуры.
		/// </summary>
		/// <param name="ware">Номенклатура.</param>
		/// <param name="exCode">Внешний код.</param>
		public bool AddNewExCodeToWare(Ware ware, WareExCode exCode)
		{
			// todo: эти 2 сроки перенести в модуль сопоставленя
			this.Repository.GetWareByExCode(exCode.Value, exCode.Counteragent.Code);
			


			this.Repository.AddNewExCode(ware.Code, exCode);
			ware.ExCodes.Add(exCode);
			return true;
		}


		public bool RemoveExCode(WareExCode exCode)
		{
			if (exCode == null)
				throw new ArgumentNullException();

			Ware ware = this.Repository.GetWareByExCode(exCode.Value, exCode.Counteragent.Code);

			if (ware == null)
				return false;

			this.Repository.RemoveExCode(ware.Code, exCode);
			return true;
		}

		public Task<List<Counteragent>> GetAllCounteragentsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<bool> RematchingCounteragentAsync(Counteragent counteragent, string gln)
		{
			throw new NotImplementedException();
		}

		private ItidaRepository Repository { get; set; }
	}
}
