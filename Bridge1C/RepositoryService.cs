namespace Bridge1C
{
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DomainEntities;
	using NLog;

    public class RepositoryService : IRepositoryService
    {
        public RepositoryService(string dataBaseFile, string login, string pass)
        {
			this.logger.Info("Инициализация объекта сервиса 1С");

			if (string.IsNullOrWhiteSpace(dataBaseFile) || string.IsNullOrWhiteSpace(login))
				throw new ArgumentNullException("Передан пустой параметр");

			this.Repository = new Repository(new Connector(dataBaseFile, login, pass));
			this.logger.Info("Инициализация объекта сервиса 1С завершена");
		}

        public RepositoryService(string connectionString)
        {
			this.logger.Info("Инициализация объекта сервиса 1С");

			if (string.IsNullOrWhiteSpace(connectionString))
				throw new ArgumentNullException("Передан пустой параметр");

			this.Repository = new Repository(new Connector(connectionString));
			this.logger.Info("Инициализация объекта сервиса 1С завершена");
		}

        /// <summary>
        /// Добавить новый товар.
        /// </summary>
        /// <param name="ware">Товар для добавления.</param>
        /// <returns>true, если успешно, иначе false.</returns>
        public bool AddNewWare(Ware ware)
        {
			this.logger.Info("Добавление номенклатуры наименование {0}", ware?.Name);

			if(ware == null)
				throw new ArgumentNullException("Передан пустой параметр");

			ware.Code = this.Repository.AddNewWare(ware.Name, ware.FullName, ware.Unit.International, ware.BarCodes);

            if(!string.IsNullOrWhiteSpace(ware.Code))
            {
                if (ware.BarCodes != null && ware.BarCodes.Any())
                    this.Repository.AddNewBarcodes(ware.Code, ware.BarCodes);

                if (ware.ExCodes != null && ware.ExCodes.Any())
                    this.Repository.AddNewExCodes(ware.Code, ware.ExCodes.Select(ec => ec.Counteragent.GLN).ToList(), ware.ExCodes.Select(ec => ec.Value).ToList());

				this.logger.Info("Новая номенклатура добавлена");
				return true;
            }
			else
			{
				this.logger.Warn("Не удалось добавить номенклатуру");
				return false;
			}
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
			this.logger.Info("Получение номенклатуры по реквизиту {0} = {1}", prop, propValue);

			var ware = this.Repository.GetWare(prop, propValue, counteragentGln);

            if (ware == null)
			{
				this.logger.Warn("Номенклатура не найдена");
				return null;
			}
			else
			{
				this.logger.Info("Номенклатура найдена {0}", ware.Наменование);
				return this.GetDomainWareFromDbWare(ware);
			}
        }

        public List<Ware> GetAllWares()
        {
			this.logger.Info("Получение всей номенклатуры");

			List<Ware> result = new List<Ware>();

            foreach (var item in Repository.GetAllWares())
                result.Add(this.GetDomainWareFromDbWare(item));

			this.logger.Info("Номенклатура получена Количество {0}", result.Count);
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
			this.logger.Info("Получение контрагента по реквизиту {0} = {1}", prop, propValue);

			if (string.IsNullOrWhiteSpace(propValue))
				throw new ArgumentNullException("Передан пустой параметр");

			Counteragent result = new Counteragent();
            var counteragent = this.Repository.GetCounteragent(prop, propValue);

            if (counteragent == null)
			{
				this.logger.Warn("Контрагент не найден");
				return null;
			}
			else
			{
				result.Code = counteragent.Код;
				result.Name = counteragent.Наименование;
				result.FullName = counteragent.НаименованиеПолное;
				result.GLN = counteragent.ГЛН;
				this.logger.Info("Контрагент получен Наименование {0}", result.Name);
				return result;
			}
        }

		public List<Counteragent> GetAllCounteragents()
		{
			this.logger.Info("Получение всех контрагентов");

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

			this.logger.Info("Контрагенты получены Количество {0}", result.Count);
			return result;
		}

		/// <summary>
		/// Инициализирует поле ГЛН контрагента counteagent значением gln, если в базе есть контрагент с ГЛН gln 
		/// </summary>
		/// <param name="counteragent">Контрагент для инициализации.</param>
		/// <param name="gln">ГЛН.</param>
		public bool RematchingCounteragent(Counteragent counteragent, string gln)
		{
			this.logger.Info("Пересопоставление контрагента код {0} с ГЛН {1}", counteragent?.Code, gln);

			if (counteragent == null || string.IsNullOrWhiteSpace(gln))
				throw new ArgumentNullException("Передан пустой параметр");

			bool result = this.Repository.RematchingCounteragent(counteragent.Code, gln);

			if (result)
				this.logger.Info("Пересопоставление контрагента завершено");
			else
				this.logger.Warn("Пересопоставление не выполнено");

			return result;
		}

		/// <summary>
		/// Получить единицу измерения.
		/// </summary>
		/// <param name="prop">Свойство, по которому будет осуществлен поиск.</param>
		/// <param name="propValue">Значение свойства для поиска.</param>
		/// <returns>ЕИ.</returns>
		public Unit GetUnit(Requisites prop, string propValue)
        {
			this.logger.Info("Получение единицы измерения по реквизиту {0} = {1}", prop, propValue);

			if (string.IsNullOrWhiteSpace(propValue))
				throw new ArgumentNullException("Передан пустой параметр");

			Unit result = new Unit();
            var unit = this.Repository.GetUnit(prop, propValue);

            if (unit == null)
			{
				this.logger.Warn("Единица измерения не найдена");
				return null;
			}
			else
			{
				result.Code = unit.Код;
				result.Name = unit.Наименование;
				result.FullName = unit.НаименованиеПолное;
				result.International = unit.МеждународноеСокращение;
				this.logger.Info("Единица измерения получена");
				return result;
			}
        }

        public Warehouse GetWarehouse(Requisites prop, string propValue)
        {
			this.logger.Info("Получение склада по реквизиту {0} {1}", prop, propValue);

			if (string.IsNullOrWhiteSpace(propValue))
				throw new ArgumentNullException("Передан пустой параметр");

            Warehouse result = new Warehouse();
            var warehouse = this.Repository.GetWareHouse(prop, propValue);

            if (warehouse == null)
			{
				this.logger.Warn("Склад не найден");
				return null;
			}
			else
			{
				result.Code = warehouse.Код;
				result.Name = warehouse.Наименование;
				result.Shop = new Shop { Code = warehouse.Магазин.Код, Name = warehouse.Магазин.Наименование };
				result.User = new User
				{
					Code = this.Repository.GuidToString(warehouse.Ответственный.ИдентификаторПользователяИБ),
					Name = warehouse.Ответственный.Наименование
				};
				this.logger.Info("Склад получен Наименование {0}", result.Name);
				return result;
			}
        }

		public List<Warehouse> GetAllWarehouses()
		{
			this.logger.Info("Получение всех складов");
			List<Warehouse> result = new List<Warehouse>();

			foreach (var item in Repository.GetAllWarehouses())
			{
				result.Add(new Warehouse
				{
					Code = item.Код,
					Name = item.Наименование,
					Shop = new Shop
					{
						Code = item.Магазин.Код,
						Name = item.Магазин.Наименование
					},
					User = new User
					{
						Code = this.Repository.GuidToString(item.Ответственный.ИдентификаторПользователяИБ),
						Name = item.Ответственный.Наименование
					}
				});
			}

			this.logger.Info("Склады получены Количество {0}", result.Count);
			return result;
		}

		public List<Warehouse> GetWarehousesByActiveUser() // todo: надо реализовать
		{
			this.logger.Info("Получение активного пользователя");
			var warehouses = this.Repository.GetWarehousesByActiveUser();
			List<Warehouse> result = new List<Warehouse>();

			if (warehouses == null)
			{
				this.logger.Warn("Склады активного пользователя не найдены");
				return null;
			}
			else
			{
				foreach (var item in warehouses)
				{
					result.Add(new Warehouse
					{
						Code = item.Код,
						Name = item.Наименование,
						Shop = new Shop
						{
							Code = item.Магазин.Код,
							Name = item.Магазин.Наименование
						},
						User = new User
						{
							Code = this.Repository.GuidToString(item.Ответственный.ИдентификаторПользователяИБ),
							Name = item.Ответственный.Наименование
						}
					});
				}

				this.logger.Info("Склады активного получены Количество {0}", result.Count);
				return result;
			}
		}

		public bool RematchingWarehouse(string warehouseCode, string gln)
		{
			this.logger.Info("Пересопоставление склада {0} с ГЛН {1}", warehouseCode, gln);

			if (string.IsNullOrWhiteSpace(warehouseCode) || string.IsNullOrWhiteSpace(gln))
				throw new ArgumentNullException("Передан пустой параметр");

			bool result = this.Repository.UpdateWarehouseGLN(warehouseCode, gln);

			if (result)
				this.logger.Info("Пересопоставление выполнено");
			else
				this.logger.Warn("Пересопоставление не выполнено");

			return result;
		}

		public Shop GetShop(string warehouseCode)
        {
			this.logger.Info("Поиск магазина по коду склада {0}", warehouseCode);

			if (string.IsNullOrWhiteSpace(warehouseCode))
				throw new ArgumentNullException("Передан пустой параметр");

            Shop result = new Shop();
            var shop = this.Repository.GetShop(warehouseCode);

			if (shop == null)
			{
				this.logger.Warn("Магазин не найден");
				return null;
			}
			else
			{
				result.Code = shop.Код;
				result.Name = shop.Наименование;
				this.logger.Info("Магазин найден Наименование {0}", result.Name);
				return result;
			}
        }

		public Organization GetOrganization(Requisites prop, string propValue)
		{
			this.logger.Info("Поиск организации по реквизиту {0} {1}", prop, propValue);

			if (string.IsNullOrWhiteSpace(propValue))
				throw new ArgumentNullException("Передан пустой параметр");

			Organization result = new Organization();
			var organization = this.Repository.GetOrganization(prop, propValue);

			if (organization == null)
			{
				this.logger.Warn("Организация не найдена");
				return null;
			}
			else
			{
				result.Code = organization.Код;
				result.Name = organization.Наименование;
				result.GLN = organization.ГЛН;
				this.logger.Info("Организация найдена Наименование {0}", result.Name);
				return result;
			}
		}

		public User GetCurrentUser()
		{
			this.logger.Info("Получение активного пользователя");
			var user = this.Repository.GetCurrentUser();

			if (user == null)
			{
				this.logger.Warn("Активный пользователь не получен");
				return null;
			}
			else
			{
				User result = new User
				{
					Code = this.Repository.GuidToString(user.ИдентификаторПользователяИБ),
					Name = user.Наименование
				};
				this.logger.Info("Активный пользователь получен Код {0}", result.Name);
				return result;
			}
		}

		public bool AddNewWaybill(Waybill waybill)
        {
			this.logger.Info("Добавление новой накладной Номер {0} Дата {1}", waybill?.Number, waybill?.Date);

			if (waybill == null)
				throw new ArgumentNullException("Передан пустой параметр");

            var supplier = this.Repository.GetCounteragent(Requisites.Code, waybill.Supplier.Code);

			if(supplier == null)
			{
				this.logger.Warn("Не найден поставщик Код {0}", waybill.Supplier.Code);
				return false;
			}

            var warehouse = this.Repository.GetWareHouse(Requisites.Code, waybill.Warehouse.Code);

			if(warehouse == null)
			{
				this.logger.Warn("Не найден склад Код {0}", waybill.Warehouse.Code);
				return false;
			}

            var shop = this.Repository.GetShop(warehouse);

			if (shop == null)
			{
				this.logger.Warn("Не найден магазин склада Код {0}", warehouse.Code);
				return false;
			}

			bool result = this.Repository.AddNewWaybill(waybill.Number, waybill.Date, supplier, warehouse, shop, waybill.Positions);

			if (result)
				this.logger.Info("Накладная добавлена");
			else
				this.logger.Warn("Накладная не добавлена");

			return result;

		}

        public bool AddNewExCodeToWare(Ware ware, WareExCode exCode)
        {
			this.logger.Info("Добавление нового внешнего кода {0} к номенклатуре Код {1}", exCode?.Value, ware?.Code);

			if (ware == null || exCode == null)
				throw new ArgumentNullException("Передан пустой параметр");

            ware.ExCodes.Add(exCode);
			bool result = this.Repository.AddNewExCode(ware.Code, exCode.Counteragent.GLN, exCode.Value);

			if (result)
				this.logger.Info("Внешний код добавлен");
			else
				this.logger.Warn("Не удалось добавить внешний код");

			return result;
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
			this.logger.Info("Получение внешних кодов номенклатуры {0}", ware?.Код);

			if (ware == null)
				throw new ArgumentNullException("Передан пустой параметр");

            var exCodes = this.Repository.GetWareExCodes(ware);

            if (exCodes == null)
			{
				this.logger.Warn("Внешние коды не найдены");
				return null;
			}
			else
			{
				List<WareExCode> result = new List<WareExCode>();

				foreach (var item in exCodes)
				{
					WareExCode wec = new WareExCode();
					wec.Value = item.ВнешнийКод;
					wec.Counteragent = this.GetCounteragent(Requisites.GLN, item.ГЛНКонтрагента);
					result.Add(wec);
				}

				this.logger.Info("Внешние коды найдены количество {0}", result.Count);
				return result;
			}
        }

        /// <summary>
        /// Получить штрихкода товара.
        /// </summary>
        /// <param name="ware">Товар.</param>
        /// <returns>Коллекция штрихкодов.</returns>
        private List<string> GetWareBarcodes(dynamic ware)
        {
			this.logger.Info("Получение штрихкодов номенклатуры {0}", ware?.Код);

			if (ware == null)
				throw new ArgumentNullException("Передан пустой параметр");

			List<string> result = this.Repository.GetWareBarcodes(ware);

			if (result == null)
				this.logger.Warn("Штрихкода не найдены");
			else
				this.logger.Info("Штрихкода найдены Количество {0}", result.Count);

			return result;
		}

        private Repository Repository { get; set; }
		private readonly Logger logger = LogManager.GetCurrentClassLogger();
	}
}
