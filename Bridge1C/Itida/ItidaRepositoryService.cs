namespace DAL.Itida
{
	using System;
	using System.Collections.Generic;
    using DomainEntities.DocWaybill;
    using DomainEntities.Spr;
	using NLog;
    using DomainEntities.DocOrder;

	public class ItidaRepositoryService : IRepositoryService
	{
		public ItidaRepositoryService(string connectionString)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
				throw new ArgumentNullException("Передан пустой параметр");

			this.logger.Info("Инициализация объекта сервиса Айтиды");
			this.Repository = new ItidaRepository(connectionString);
			this.logger.Info("Инициализация объекта сервиса Айтиды завершена");
		}

		/// <summary>
		/// Записать в базу данных объект номенклатуры.
		/// </summary>
		/// <param name="ware">Объект для записи.</param>
		public bool AddNewWare(Ware ware)
		{
			this.logger.Info("Добаввление новой номенклатуры Наименование {0}", ware.Name);

			if (ware == null)
				throw new ArgumentNullException("Передан пустой параметр");

			bool result = this.Repository.AddNewWare(ware);

			if (result)
				this.logger.Info("Номенклатура добавлена");
			else
				this.logger.Warn("Номенклатура не найдена");

			return result;
		}

		/// <summary>
		/// Возвращает объект номенклатуры по указанному реквизиту.
		/// </summary>
		/// <param name="prop">Реквизит для поиска.</param>
		/// <param name="propValue">Значение реквизита для поиска.</param>
		/// <param name="counteragentGln">ГЛН контрагента (необходимо в случае поиска по внешнему коду номенклатуры).</param>
		public Ware GetWare(Requisites prop, string propValue, string counteragentGln = "")
		{
			this.logger.Info("Получение номенклатуры по реквизиту {0} = {1}", prop, propValue);

			if (string.IsNullOrWhiteSpace(propValue))
				throw new ArgumentNullException("Передан пустой параметр");

			var result = this.Repository.GetWare(prop, propValue, counteragentGln);

			if (result == null)
			{
				this.logger.Warn("Номенклатура не найдена");
				return null;
			}
			else
			{
				this.logger.Info("Номенклатура найдена {0}", result.Name);
				return result;
			}
		}

		/// <summary>
		/// Получить список товаров.
		/// </summary>
		public List<Ware> GetAllWares()
		{
			this.logger.Info("Получение всей номенклатуры");
			var result = this.Repository.GetAllWares();
			this.logger.Info("Номенклатура получена Количество {0}", result.Count);
			return result;
		}

		/// <summary>
		/// Получить контрагента.
		/// </summary>
		/// <param name="prop">Реквизит для поиска.</param>
		/// <param name="propValue">Значение реквизита для поиска.</param>
		public Counteragent GetCounteragent(Requisites prop, string propValue)
		{
			this.logger.Info("Получение контрагента по реквизиту {0} = {1}", prop, propValue);

			if (string.IsNullOrWhiteSpace(propValue))
				throw new ArgumentNullException("Передан пустой параметр");

			var result = this.Repository.GetCounteragent(prop, propValue);

			if (result == null)
			{
				this.logger.Warn("Контрагент не найден");
				return null;
			}
			else
			{
				this.logger.Info("Контрагент получен Наименование {0}", result.Name);
				return result;
			}
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
			this.logger.Info("Пересопоставление контрагента код {0} с ГЛН {1}", counteragent?.Code, gln);

			if (counteragent == null || string.IsNullOrWhiteSpace(gln))
				throw new ArgumentNullException("Передан пустой параметр");

			var result = this.Repository.RematchingCounteragent(counteragent.Code, gln);

			if (result)
				this.logger.Info("Пересопоставление контрагента завершено");
			else
				this.logger.Warn("Пересопоставление не выполнено");

			return result;
		}

		/// <summary>
		/// Получить ЕИ.
		/// </summary>
		/// <param name="prop">Реквизит для поиска.</param>
		/// <param name="propValue">Значение реквизита для поиска.</param>
		public Unit GetUnit(Requisites prop, string propValue)
		{
			this.logger.Info("Получение единицы измерения по реквизиту {0} = {1}", prop, propValue);

			if (string.IsNullOrWhiteSpace(propValue))
				throw new ArgumentNullException("Передан пустой параметр");

			var result = this.Repository.GetUnit(prop, propValue);

			if (result == null)
			{
				this.logger.Warn("Единица измерения не найдена");
				return null;
			}
			else
			{
				this.logger.Info("Единица измерения получена");
				return result;
			}
		}

		/// <summary>
		/// Получить склад.
		/// </summary>
		/// <param name="prop">Реквизит для поиска.</param>
		/// <param name="propValue">Значение реквизита для поиска.</param>
		public Warehouse GetWarehouse(Requisites prop, string propValue)
		{
			this.logger.Info("Получение склада по реквизиту {0} {1}", prop, propValue);

			if (string.IsNullOrWhiteSpace(propValue))
				throw new ArgumentNullException("Передан пустой параметр");

			var result = this.Repository.GetWarehouse(prop, propValue);

			if (result == null)
			{
				this.logger.Warn("Склад не найден");
				return null;
			}
			else
			{
				this.logger.Info("Склад получен Наименование {0}", result.Name);
				return result;
			}
		}

		/// <summary>
		/// Получить список складов.
		/// </summary>
		public List<Warehouse> GetAllWarehouses()
		{
			this.logger.Info("Получение всех складов");
			var result = this.Repository.GetAllWarehouses();
			this.logger.Info("Склады получены Количество {0}", result.Count);
			return result;
		}

		/// <summary>
		/// Получить склады, на которых активный пользователь является ответственным.
		/// </summary>
		public List<Warehouse> GetWarehousesByActiveUser()
		{
			this.logger.Info("Получение складов активного пользователя");
			var result = this.Repository.GetWarehousesByActiveUser();

			if(result == null)
			{
				this.logger.Warn("Склады активного пользователя не найдены");
				return null;
			}
			else
			{
				this.logger.Info("Склады активного пользователя получены Количество {0}", result);
				return result;
			}
		}

        /// <summary>
        /// Инициализирует поле ex_code склада warehouse значением gln, если в базе есть склад с ex_code gln, то он будет перезаписан.
        /// </summary>
        /// <param name="warehouseCode">Код склада для инициализации.</param>
        /// <param name="gln">ГЛН.</param>
        public bool RematchingWarehouse(string warehouseCode, string gln)
		{
			this.logger.Info("Пересопоставление склада {0} с ГЛН {1}", warehouseCode, gln);

			if (string.IsNullOrWhiteSpace(warehouseCode) || string.IsNullOrWhiteSpace(gln))
				throw new ArgumentNullException("Передан пустой параметр");

			var result = this.Repository.RematchingWarehouse(warehouseCode, gln);

			if (result)
				this.logger.Info("Пересопоставление выполнено");
			else
				this.logger.Warn("Пересопоставление не выполнено");

			return result;
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
		/// Получить организацию.
		/// </summary>
		/// <param name="prop">Реквизит для поиска.</param>
		/// <param name="propValue">Значение реквизита для поиска.</param>
		public Organization GetOrganization(Requisites prop, string propValue)
		{
			this.logger.Info("Поиск организации по реквизиту {0} {1}", prop, propValue);

			if (string.IsNullOrWhiteSpace(propValue))
				throw new ArgumentNullException("Передан пустой параметр");

			var result = this.Repository.GetOrganization(prop, propValue);


			if (result == null)
			{
				this.logger.Warn("Организация не найдена");
				return null;
			}
			else
			{
				this.logger.Info("Организация найдена Наименование {0}", result.Name);
				return result;
			}
		}

		/// <summary>
		/// Добавить новую накладную в базу данных.
		/// </summary>
		/// <param name="waybill">Накладная для записи.</param>
		public bool AddNewWaybill(Waybill waybill)
		{
			this.logger.Info("Добавление новой накладной Номер {0} Дата {1}", waybill?.Number, waybill?.Date);

			if (waybill == null)
				throw new ArgumentNullException("Передан пустой параметр");

			var result = this.Repository.AddNewWaybill(waybill);

			if (result)
				this.logger.Info("Накладная добавлена");
			else
				this.logger.Warn("Накладная не добавлена");

			return result;
		}

		/// <summary>
		/// Получить список всех контрагентов из базы.
		/// </summary>
		public List<Counteragent> GetAllCounteragents()
		{
			this.logger.Info("Получение всех контрагентов");
			var result = this.Repository.GetAllCounteragents();
			this.logger.Info("Контрагенты получены Количество {0}", result.Count);
			return result;
		}

		/// <summary>
		/// Получить авторизованного ползователя.
		/// </summary>
		public User GetCurrentUser()
		{
			this.logger.Info("Получение активного пользователя");
			var result = this.Repository.GetCurrentUser();

			if (result == null)
			{
				this.logger.Info("Активный пользователь получен Код {0}", result.Name);
				return null;
			}
			else
			{
				this.logger.Warn("Активный пользователь не получен");
				return result;
			}
		}

		/// <summary>
		/// Добавляет внешний код номенклатуре и удаляет этот внешний код из предыдушей номенклатуры.
		/// </summary>
		/// <param name="ware">Номенклатура.</param>
		/// <param name="exCode">Внешний код.</param>
		public bool AddNewExCodeToWare(Ware ware, WareExCode exCode)
		{
			this.logger.Info("Добавление нового внешнего кода {0} к номенклатуре Код {1}", exCode?.Value, ware?.Code);

			if (ware == null || exCode == null)
				throw new ArgumentNullException("Передан пустой параметр");

			//this.Repository.GetWareByExCode(exCode.Value, exCode.Counteragent.Code);
			this.Repository.AddNewExCode(ware.Code, exCode);
			ware.ExCodes.Add(exCode);
			this.logger.Info("Внешний код добавлен");
			return true;
		}

		public bool RemoveExCode(WareExCode exCode)
		{
			this.logger.Info("Удаление внешнего кода {0} поставщика {1}", exCode?.Value, exCode.Counteragent?.Name);

			if (exCode == null)
				throw new ArgumentNullException("Передан пустой параметр");

			Ware ware = this.Repository.GetWareByExCode(exCode.Value, exCode.Counteragent.Code);

			if (ware == null)
			{
				this.logger.Info("Номенклатура по внешнему коду не найдена");
				return false;
			}
			else
			{
				var result = this.Repository.RemoveExCode(ware.Code, exCode);
				
				if(result)
					this.logger.Info("Внешний код удален");
				else
					this.logger.Info("Внешний код не удален");

				return result;
			}
		}

        public Order GetOrder(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentNullException("number");

            Order result = this.Repository.GetOrder(number);
            return result;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return this.Repository.GetAllOrders();
        }

		private ItidaRepository Repository { get; set; }
		private readonly Logger logger = LogManager.GetCurrentClassLogger();
	}
}
