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

		public Task<List<Counteragent>> GetAllCounteragentsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<bool> RematchingCounteragentAsync(Counteragent counteragent, string gln)
		{
			throw new NotImplementedException();
		}

		public bool RematchingCounteragent(Counteragent counteragent, string gln)
		{
			throw new NotImplementedException();
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

		public bool RematchingWarehouse(string warehouseCode, string gln)
		{
			throw new NotImplementedException();
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
		public Organization GetOrganization(string warehouseCode)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Получить организацию.
		/// </summary>
		/// <param name="prop">Реквизит для поиска.</param>
		/// <param name="propValue">Значение реквизита для поиска.</param>
		public Organization GetOrganization(Requisites prop, string propValue)
		{
			return this.Repository.GetOrganization(prop, propValue);
		}


		public bool AddNewWaybill(Waybill waybill)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Добавляет внешний код номенклатуре.
		/// </summary>
		/// <param name="ware">Номенклатура.</param>
		/// <param name="exCode">Внешний код.</param>
		public bool AddNewExCodeToWare(Ware ware, WareExCode exCode)
		{
			this.Repository.AddNewExCode(ware.Code, exCode);
			ware.ExCodes.Add(exCode);
			return true;
		}

		private ItidaRepository Repository { get; set; }
	}
}
