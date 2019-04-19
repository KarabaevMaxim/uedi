namespace DAL
{
	using System.Collections.Generic;
    using DomainEntities.DocWaybill;
    using DomainEntities.Spr;

	/// <summary>
	/// Интерфейс слоя доступа данных, являющийся прослойкой между сущностями баз данных и сущностями доменными.
	/// </summary>
	public interface IRepositoryService
	{
		/// <summary>
		/// Сохранить новый товар в базу.
		/// </summary>
		/// <param name="ware">Объект номенклатуры для сохранения.</param>
		/// <returns>true в случае успеха, иначе false.</returns>
		bool AddNewWare(Ware ware);
		/// <summary>
		/// Получить товар из базы данных по реквизиту.
		/// </summary>
		/// <param name="prop">Реквизит, по которому выполнять поиск.</param>
		/// <param name="propValue">Значение поиска.</param>
		/// <param name="counteragentGln">ГНЛ контрагента (в случае поиска по внешнему коду).</param>
		/// <returns>Объект номенклатуры.</returns>
		Ware GetWare(Requisites prop, string propValue, string counteragentGln = "");
		/// <summary>
		/// Получить все товары из базы данных.
		/// </summary>
		/// <returns>Список номенклатуры.</returns>
		List<Ware> GetAllWares();
		/// <summary>
		/// Получить контрагента по реквизиту.
		/// </summary>
		/// <param name="prop">Реквизит, по которому осуществлять поиск.</param>
		/// <param name="propValue">Значение для поиска.</param>
		/// <returns>Объект контрагента.</returns>
		Counteragent GetCounteragent(Requisites prop, string propValue);
		/// <summary>
		/// Получить список контрагентов.
		/// </summary>
		List<Counteragent> GetAllCounteragents();
		/// <summary>
		/// Перезаписать у ГЛН контрагента
		/// </summary>
		/// <param name="counteragent">Контрагент, которому необходимо установить ГЛН.</param>
		/// <param name="gln">ГНЛ для записи.</param>
		/// <returns>true в случа успеха, иначе false.</returns>
		bool RematchingCounteragent(Counteragent counteragent, string gln);
		/// <summary>
		/// Получить единицу измерения по реквизиту.
		/// </summary>
		/// <param name="prop">Реквизит поиска.</param>
		/// <param name="propValue">Значение поиска.</param>
		/// <returns>Объект единицы измерения.</returns>
		Unit GetUnit(Requisites prop, string propValue);
		/// <summary>
		/// Получить авторизованного пользователя.
		/// </summary>
		User GetCurrentUser();
		/// <summary>
		/// Получить склад из базы данных по реквизиту.
		/// </summary>
		/// <param name="prop">Реквизит поиска.</param>
		/// <param name="propValue">Значение поиска.</param>
		/// <returns>Объект склада.</returns>
		Warehouse GetWarehouse(Requisites prop, string propValue);
		/// <summary>
		/// Получить список всех складов из базы данных.
		/// </summary>
		/// <returns>Список складов.</returns>
		List<Warehouse> GetAllWarehouses();
		/// <summary>
		/// Получить склады, на которых активный пользователь является ответственным.
		/// </summary>
		List<Warehouse> GetWarehousesByActiveUser();
		/// <summary>
		/// Перезаписать ГЛН склада.
		/// </summary>
		/// <param name="warehouseCode">Склад, у которого необходимо перезаписать ГНЛ.</param>
		/// <param name="gln">ГЛН.</param>
		/// <returns>true в случа успеха, иначе false.</returns>
		bool RematchingWarehouse(string warehouseCode, string gln);
		/// <summary>
		/// Получить магазин по коду склада.
		/// </summary>
		/// <param name="warehouseCode">Код склада.</param>
		/// <returns>Объект магазина.</returns>
		Shop GetShop(string warehouseCode);
		/// <summary>
		/// Получить организацию по реквизиту.
		/// </summary>
		/// <param name="propertyName">Реквизит для поиска.</param>
		/// <param name="value">Значение реквизита для поиска.</param>
		Organization GetOrganization(Requisites prop, string propValue);
		/// <summary>
		/// Добавить в базу данных новую накладную.
		/// </summary>
		/// <param name="waybill">Объект накладной.</param>
		/// <returns>true в случа успеха, иначе false.</returns>
		bool AddNewWaybill(Waybill waybill);
		/// <summary>
		/// Добавить новый внешний код товара в базу данных. 
		/// </summary>
		/// <param name="ware">Товар, к которому будет ппривязан внешний код.</param>
		/// <param name="exCode">Внешний код.</param>
		/// <returns>true в случа успеха, иначе false.</returns>
		bool AddNewExCodeToWare(Ware ware, WareExCode exCode);

		bool RemoveExCode(WareExCode exCode);
	}
}
