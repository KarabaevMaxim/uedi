namespace DAL._1C.Roznica
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
	using NLog;
    using DomainEntities.DocWaybill;

    public class Repository
    {
        /// <summary>
        /// Инициализирует объект в памяти.
        /// </summary>
        /// <param name="connector">Подключение к базе 1С.</param>
        public Repository(Connector connector)
        {
			this.logger.Info("Инициализация объекта репозитория 1С Розница");
			this.Connector = connector;
			this.logger.Info("Инициализация объекта репозитория 1С Розница завершена");
		}

        /// <summary>
        /// Получить контрагента.
        /// </summary>
        /// <param name="propertyName">Имя свойства, по которому будет производиться поиск.</param>
        /// <param name="propertyValue">Значение свойства.</param>
        /// <returns>Контрагент.</returns>
        public dynamic GetCounteragent(Requisites propertyName, string propertyValue)
        {
			this.logger.Info("Запрос контрагента");

			if (string.IsNullOrWhiteSpace(propertyValue))
				throw new ArgumentNullException("Передан пустой параметр propertyValue");

            try
            {
                dynamic counteragent = null;

                switch (propertyName)
                {
                    case Requisites.Code:
						this.logger.Info("по коду {0}", propertyValue);
						counteragent = this.Connector.Connection.Справочники.Контрагенты.НайтиПоКоду(propertyValue);
                        break;
                    case Requisites.Name:
						this.logger.Info("по наименованию {0}", propertyValue);
						counteragent = this.Connector.Connection.Справочники.Контрагенты.НайтиПоНаименованию(propertyValue);
                        break;
                    case Requisites.GLN:
						this.logger.Info("по ГЛН {0}", propertyValue);
						counteragent = this.Connector.Connection.Справочники.Контрагенты.НайтиПоРеквизиту(RequisiteBindingConfig.RequisiteBingings[propertyName], propertyValue);
						break;
                    default:
                        break;
                }

				if(string.IsNullOrWhiteSpace(counteragent.Код))
				{
					this.logger.Warn("Контрагент не найден");
					return null;
				}

				this.logger.Info("Контрагент получен Код {0}", counteragent.Код);
				return counteragent;
            }
            catch(Exception ex)
            {
				this.logger.Error(ex, "Не удалось получить контрагента");
				return null;
            }
        }

        /// <summary>
        /// Получить справочник контрагентов.
        /// </summary>
        public List<dynamic> GetAllCounteragents()
        {
			this.logger.Info("Запрос всех контрагентов");

			try
            {
                List<dynamic> result = new List<dynamic>();
                dynamic выборка = this.Connector.Connection.Справочники.Контрагенты.Выбрать();

                while (выборка.Следующий())
                {
                    if (!выборка.ЭтоГруппа)
                    {
                        result.Add(выборка.Ссылка);
                    }
                }

				this.logger.Info("Контрагенты получены Количество {0}", result.Count);
				return result;
            }
            catch (Exception ex)
            {
				this.logger.Error(ex, "Не удалось получить контрагентов");
				return null;
            }
        }

		/// <summary>
		/// Инициализирует значением gln поле ГЛН контрагента с кодом counteragentCode, если перед этим в базе найден контрагент с 
		/// ГЛН gln, после будет переинициализировано пустой строкой.
		/// </summary>
		/// <param name="counteragentCode">Код контрагента для переициализации.</param>
		/// <param name="gln">ГНЛ.</param>
		/// <returns>true, если операция завершена успешно, иначе false.</returns>
		public bool RematchingCounteragent(string counteragentCode, string gln)
		{
			this.logger.Info("Сопоставление контрагента код {0} ГЛН {1}", counteragentCode, gln);

			if (string.IsNullOrWhiteSpace(counteragentCode) || string.IsNullOrWhiteSpace(gln))
				throw new ArgumentNullException("Передан пустой параметр counteragentCode или gln");

			try
			{
				dynamic oldCounteragent = this.GetCounteragent(Requisites.GLN, gln);

				if (oldCounteragent != null && !string.IsNullOrWhiteSpace(oldCounteragent.Код))
				{
					this.logger.Info("Удаление ГНЛ у старого контрагента Код {0}", oldCounteragent.Код);
					dynamic oldCounteragentObj = oldCounteragent.ПолучитьОбъект();
					oldCounteragentObj.ГЛН = string.Empty;
					oldCounteragentObj.Записать();
					this.logger.Info("ГНЛ у старого контрагента удален");
				}
					
				dynamic newCounteragent = this.GetCounteragent(Requisites.Code, counteragentCode);

				if (newCounteragent == null || string.IsNullOrWhiteSpace(newCounteragent.Код))
				{
					this.logger.Warn("Не найден новый контрагент, сопоставление не выполнено");
					return false;
				}

				dynamic newCounteragentObj = newCounteragent.ПолучитьОбъект();
				newCounteragentObj.ГЛН = gln;
				newCounteragentObj.Записать();
				this.logger.Info("Cопоставление выполнено");
				return true;
			}
			catch(Exception ex)
			{
				this.logger.Error(ex, "Ошибка при сопоставлении, сопоставление не выполнено");
				return false;
			}
		}

		public dynamic GetWareHouse(Requisites propertyName, string propertyValue)
        {
			this.logger.Info("Получение склада");

			try
            {
                dynamic warehouse = null;

                switch (propertyName)
                {
                    case Requisites.Code:
						this.logger.Info("по коду {0}", propertyValue);
						warehouse = this.Connector.Connection.Справочники.Склады.НайтиПоКоду(propertyValue);
                        break;
                    case Requisites.Name:
						this.logger.Info("по наименованию {0}", propertyValue);
						warehouse = this.Connector.Connection.Справочники.Склады.НайтиПоНаименованию(propertyValue);
                        break;
                    default:
						this.logger.Info("по реквизиту {0} {1}",propertyName, propertyValue);
						warehouse = this.Connector.Connection.Справочники.Склады.НайтиПоРеквизиту(RequisiteBindingConfig.RequisiteBingings[propertyName], propertyValue);
                        break;
                }

				if(string.IsNullOrWhiteSpace(warehouse.Код))
				{
					this.logger.Warn("Склад не найден");
					return null;
				}

				this.logger.Info("Склад получен Код {0}", warehouse.Код);
				return warehouse;
            }
            catch(Exception ex)
            {
				this.logger.Error(ex, "Не удалось получить склад");
                return null;
            }
        }

		/// <summary>
		/// Получить справочник складов.
		/// </summary>
		/// <returns>Справочник складов.</returns>
		public List<dynamic> GetAllWarehouses()
		{
			this.logger.Info("Получение всех складов");

			try
			{
				List<dynamic> result = new List<dynamic>();
				dynamic выборка = this.Connector.Connection.Справочники.Склады.Выбрать();

				while (выборка.Следующий())
				{
					if (!выборка.ЭтоГруппа)
					{
						result.Add(выборка.Ссылка);
					}
				}

				this.logger.Info("Склады получены Количество {0}", result.Count);
				return result;
			}
			catch (Exception ex)
			{
				this.logger.Error(ex, "Не удалось получить список всех складов");
				return null;
			}
		}

		/// <summary>
		/// Получить склады, на которых активный пользователь является ответственным.
		/// </summary>
		public List<dynamic> GetWarehousesByActiveUser() // todo: надо реализовать метод
		{
			this.logger.Info("Получение складов активного пользователя");

			try
			{
				List<dynamic> result = new List<dynamic>();
				var пользователь = this.GetCurrentUser();

				if(пользователь == null)
				{
					this.logger.Warn("Не найден активный пользователь");
					return null;
				}
				else
				{
					dynamic запрос = this.Connector.Connection.NewObject("Запрос");
					запрос.Текст = @"ВЫБРАТЬ
										Склады.Ссылка Как Ссылка
                                     ИЗ
										Справочник.Склады Как Склады
                                     ГДЕ
                                        Склады.Ответственный = &СсылкаПользователь";

					запрос.УстановитьПараметр("СсылкаПользователь", пользователь);
					dynamic выборка = запрос.Выполнить().Выбрать();

					while(выборка.Следующий())
					{
						result.Add(выборка.Ссылка);
					}
				}

				this.logger.Info("Склады получены Количество {0}", result.Count);
				return result;
			}
			catch(Exception ex)
			{
				this.logger.Error(ex, "Не удалось получить склады активного пользователя");
				return null;
			}
			
		}

		//public dynamic GetUserByWarehouse(string warehouseCode)
		//{
		//	if (string.IsNullOrWhiteSpace(warehouseCode))
		//		throw new ArgumentNullException("Передан пустой параметр");


		//	dynamic warehouse = this.GetWareHouse(Requisites.Code, warehouseCode);

		//	if(warehouse == null)
		//	{
		//		this.logger.Warn("Не найден склад");
		//		return null;
		//	}
		//	else
		//	{
		//		return warehouse.Ответственный;
		//	}
		//}

		/// <summary>
		/// Изменить ГЛН склада. Если с ГЛН gln уже был склад в базе, то ГНЛ того склада удаляется.
		/// </summary>
		/// <param name="warehouseCode">Код склада.</param>
		/// <param name="gln">Новый ГЛН.</param>
		/// <returns>true в случае успеха, иначе false.</returns>
		public bool UpdateWarehouseGLN(string warehouseCode, string gln)
		{
			this.logger.Info("Сопоставление склада код {0} ГЛН {1}", warehouseCode, gln);

			if (string.IsNullOrWhiteSpace(warehouseCode) || string.IsNullOrWhiteSpace(gln))
				throw new ArgumentNullException("Передан пустой параметр");

			var склад = this.GetWareHouse(Requisites.Code, warehouseCode);

			if (склад == null)
				return false;

			var старыйСклад = this.GetWareHouse(Requisites.GLN, gln);
			try
			{
				if (старыйСклад != null && !string.IsNullOrWhiteSpace(старыйСклад.Код))
				{
					this.logger.Info("Удаление ГНЛ у старого склада Код {0}", старыйСклад.Код);
					var объектСтарогоСклада = старыйСклад.ПолучитьОбъект();
					объектСтарогоСклада.ГЛН = string.Empty;
					объектСтарогоСклада.Записать();
					this.logger.Info("ГНЛ у старого склада удален");
				}

				var объектСклада = склад.ПолучитьОбъект();
				объектСклада.ГЛН = gln;
				объектСклада.Записать();
				this.logger.Info("Cопоставление выполнено");
				return true;	
			}
			catch(Exception ex)
			{
				this.logger.Error(ex, "Ошибка при сопоставлении, сопоставление не выполнено");
				return false;
			}
		}

        public dynamic GetShop(dynamic warehouse)
        {
			this.logger.Info("Получение магазина склада Код {0}", warehouse.Код);

			if (warehouse == null)
				throw new ArgumentNullException("Передан пустой аргумент");

			this.logger.Info("Магазин получен Код {0}", warehouse.Магазин.Код);
			return warehouse.Магазин;
        }

        public dynamic GetShop(string warehouseCode)
        {
			this.logger.Info("Получение магазина по коду склада Код {0}", warehouseCode);
			var result = this.GetWareHouse(Requisites.Code, warehouseCode);

            if (result == null)
			{
				this.logger.Warn("Склад не найден");
				return null;
			}

			this.logger.Info("Получен магазин Код {0}", result.Магазин.Код);
			return result.Магазин;
        }

        public dynamic GetOrganization(string warehouseCode)
        {
			this.logger.Info("Получение организации по коду склада Код {0}", warehouseCode);
			var result = this.GetWareHouse(Requisites.Code, warehouseCode);

            if (result == null)
			{
				this.logger.Warn("Склад не найден");
				return null;
			}

			this.logger.Info("Получена организация Код {0}", result.Магазин.Код);
			return result.Организация;
        }

		public dynamic GetOrganization(Requisites propertyName, string propertyValue)
		{
			this.logger.Info("Получение организации");

			try
			{
				dynamic organization = null;

				switch (propertyName)
				{
					case Requisites.Code:
						this.logger.Info("по коду {0}", propertyValue);
						organization = this.Connector.Connection.Справочники.Организации.НайтиПоКоду(propertyValue);
						break;
					case Requisites.Name:
						this.logger.Info("по наименованию {0}", propertyValue);
						organization = this.Connector.Connection.Справочники.Организации.НайтиПоНаименованию(propertyValue);
						break;
					default:
						this.logger.Info("по реквизиту {0} {1}",propertyName, propertyValue);
						organization = this.Connector.Connection.Справочники.Организации.НайтиПоРеквизиту(RequisiteBindingConfig.RequisiteBingings[propertyName], propertyValue);
						break;
				}

				if(string.IsNullOrWhiteSpace(organization.Код))
				{
					this.logger.Warn("Организация не найдена");
					return null;
				}

				this.logger.Info("Получена организация Код {0}", organization.Код);
				return organization;
			}
			catch (Exception ex)
			{
				this.logger.Error(ex, "Не удалось получить организацию");
				return null;
			}
		}

        public dynamic GetTaxRate(TaxRates rate)
        {
			this.logger.Info("Получение налоговой ставки по ее величине {0}", rate);

			try
            {
				dynamic result = null;

                switch (rate)
                {
                    case TaxRates.Tax_0:
						result = this.Connector.Connection.Перечисления.СтавкиНДС.НДС0;
						break;
                    case TaxRates.Tax_10:
						result = this.Connector.Connection.Перечисления.СтавкиНДС.НДС10;
						break;
                    case TaxRates.Tax_20:
						result = this.Connector.Connection.Перечисления.СтавкиНДС.НДС20;
						break;
                    case TaxRates.Tax_None:
						result = this.Connector.Connection.Перечисления.СтавкиНДС.БезНДС;
						break;
                    default:
						throw new ArgumentOutOfRangeException("Передана неверная налоговая ставка");
                }

				this.logger.Info("Налоговая ставка получена");
				return result;
            } 
            catch(Exception ex)
            {
				this.logger.Error(ex, "Не удалось получить налоговую ставку");
                return null;
            }
        }

        public dynamic GetUnit(Requisites propertyName, string propertyValue)
        {
			this.logger.Info("Получение единицы измерения");

			try
            {
				dynamic result = null;

                switch (propertyName)
                {
                    case Requisites.Code:
						this.logger.Info("по коду {0}", propertyValue);
						result = this.Connector.Connection.Справочники.БазовыеЕдиницыИзмерения.НайтиПоКоду(propertyValue);
						break;
                    case Requisites.InternationalReduction_Unit:
						this.logger.Info("по международному сокращению {0}", propertyValue);
						result = this.Connector.Connection.Справочники.БазовыеЕдиницыИзмерения.НайтиПоРеквизиту(RequisiteBindingConfig.RequisiteBingings[propertyName], propertyValue);
						break;
					default:
                        throw new ArgumentOutOfRangeException("Передано неверное имя реквизита");
                }

				if(string.IsNullOrWhiteSpace(result.Код))
				{
					this.logger.Warn("Не найдена единица измерения");
					return null;
				}

				this.logger.Info("Получена единица измерения Код {0}", result.Код);
				return result;
            }
            catch (Exception ex)
            {
				this.logger.Error(ex, "Не удалось получить единицу измерения");
                return null;
            }
        }

        public dynamic GetWare(Requisites propertyName, string propertyValue, string counteragentGln = "")
        {
			this.logger.Info("Получение номенклатуры");

            try
            {
                dynamic ware = null;

                switch (propertyName)
                {
                    case Requisites.Code:
						this.logger.Info("по коду {0}", propertyValue);
                        ware = this.Connector.Connection.Справочники.Номенклатура.НайтиПоКоду(propertyValue);
                        break;
                    case Requisites.Name:
						this.logger.Info("по наименованию {0}", propertyValue);
						ware = this.Connector.Connection.Справочники.Номенклатура.НайтиПоНаименованию(propertyValue);
                        break;
                    case Requisites.ExCode_Ware:
						this.logger.Info("по внешнему коду {0} глн контрагента {1}", propertyValue, counteragentGln);
						dynamic запрос = this.Connector.Connection.NewObject("Запрос");
                        запрос.Текст = @"   ВЫБРАТЬ
                                                ЕДИ_СопоставлениеНоменклатуры.Номенклатура, 
												ЕДИ_СопоставлениеНоменклатуры.ГЛНКонтрагента
                                            ИЗ
                                                РегистрСведений.ЕДИ_СопоставлениеНоменклатуры как ЕДИ_СопоставлениеНоменклатуры
                                            ГДЕ
                                                ЕДИ_СопоставлениеНоменклатуры.ГЛНКонтрагента = &ГЛНКонтрагента
                                            И
                                                ЕДИ_СопоставлениеНоменклатуры.ВнешнийКод = &ВнешнийКод";
                        запрос.УстановитьПараметр("ГЛНКонтрагента", counteragentGln);
                        запрос.УстановитьПараметр("ВнешнийКод", propertyValue);
                        dynamic выборка = запрос.Выполнить().Выбрать();

                        if (выборка.Следующий())
                            ware = выборка.Номенклатура;
                        else
                            ware = null;

                        break;
                    default:
						throw new ArgumentOutOfRangeException("Передано неверное имя реквизита");
                }

				if(ware == null || string.IsNullOrWhiteSpace(ware.Код))
				{
					this.logger.Warn("Номенклатура не найдена");
					return null;
				}

				this.logger.Info("Получена номенклатура Код {0}", ware.Код);
				return ware;
            }
            catch(Exception ex)
            {
				this.logger.Error(ex, "Не удалось получить номенклатуру");
                return null;
            }
        }

		public dynamic GetBarCodeType(BarcodeTypes type)
        {
			this.logger.Info("Получение типа штрихкода {0}", type);

            try
            {
				dynamic result = null;

                switch (type)
                {
                    case BarcodeTypes.Ean_8:
						result = this.Connector.Connection.ПланыВидовХарактеристик.ТипыШтрихкодов.EAN8;
						break;
                    case BarcodeTypes.Ean_13:
						result = this.Connector.Connection.ПланыВидовХарактеристик.ТипыШтрихкодов.EAN13;
						break;
                    case BarcodeTypes.Ean_128:
						result = this.Connector.Connection.ПланыВидовХарактеристик.ТипыШтрихкодов.EAN128;
						break;
                    default:
						throw new ArgumentOutOfRangeException("Передан певерный тип штрихкода");
                }

				if(string.IsNullOrWhiteSpace(result.Ссылка))
				{
					this.logger.Warn("Тип штрихкода не найден");
					return null;
				}

				this.logger.Info("Получен типа штрихкод Ссылка {0}", result.Ссылка);
				return result;
            }
            catch(Exception ex)
            {
				this.logger.Error(ex, "Не удалось получить тип штрихкода");
                return null;
            }
        }

        public List<dynamic> GetWareExCodes(dynamic ware)
        {
			this.logger.Info("Получение внешних кодов номенклатуры {0}", ware?.Код);

			if (ware == null)
				throw new ArgumentNullException("Передан пустой параметр");

			try
			{
				List<dynamic> result = new List<dynamic>();

				var отборТовар = this.Connector.Connection.NewObject("Структура");
				отборТовар.Вставить("Номенклатура", ware.Ссылка);
				var выборка = this.Connector.Connection.РегистрыСведений.ЕДИ_СопоставлениеНоменклатуры.Выбрать(отборТовар);

				while (выборка.Следующий())
				{
					var temp = this.Connector.Connection.NewObject("Структура");
					temp.Вставить("ВнешнийКод", выборка.ВнешнийКод);
					temp.Вставить("ГЛНКонтрагента", выборка.ГЛНКонтрагента);
					result.Add(temp);
				}

				this.logger.Info("Внешние коды получены Количество {0}", result.Count);
				return result;
			}
			catch(Exception ex)
			{
				this.logger.Error(ex, "Не удалось получить внешние коды");
				return null;
			}
        }

        public List<dynamic> GetAllWares()
        {
			this.logger.Info("Получение всей номенклатуры");

            try
            {
                List<dynamic> result = new List<dynamic>();
                dynamic выборка = this.Connector.Connection.Справочники.Номенклатура.Выбрать();

                while (выборка.Следующий())
                {
                    if (!выборка.ЭтоГруппа)
                    {
                        result.Add(выборка.Ссылка);
                    }
                }

				this.logger.Info("Вся номенклатура получена Количество {0}", result.Count);
				return result;
            }
            catch (Exception ex)
            {
				this.logger.Error(ex, "Не удалось получить всю номенклатуру");
                return null;
            }
        }

        public List<string> GetWareBarcodes(dynamic ware)
        {
			this.logger.Info("Получение штрихкодов номенклатуры Код {0}", ware?.Код);

			if (ware == null)
				throw new ArgumentOutOfRangeException("Передан пустой параметр");

			try
			{
				List<string> result = new List<string>();

				var отборВладелец = this.Connector.Connection.NewObject("Структура");
				отборВладелец.Вставить("Владелец", ware.Ссылка);
				var выборка = this.Connector.Connection.РегистрыСведений.Штрихкоды.Выбрать(отборВладелец);

				while (выборка.Следующий())
					result.Add(выборка.Штрихкод);

				this.logger.Info("Штрихкоды получены Количество {0}", result.Count);
				return result;
			}
			catch (Exception ex)
			{
				this.logger.Error(ex, "Не удалось получить штрихкоды номенклатуры");
				return null;
			}
           
        }

		public string GuidToString(dynamic guid)
		{
			this.logger.Info("Конвертация guid в строку", guid);

			try
			{
				string result = this.Connector.Connection.КонвертерГУИДВСтроку.ПолучитьСтрокуИзГУИД(guid);

				if (!string.IsNullOrWhiteSpace(result))
				{
					this.logger.Info("guid конвертирован в строку {0}", result);
					return result;
				}
				else
				{
					this.logger.Error("Не удалось конвертировать guid в строку");
					return null;
				}

				
			}
			catch(Exception ex)
			{
				this.logger.Error(ex, "Ошибка конвертации guid в строку");
				return null;
			}
		}

		public dynamic GetCurrentUser()
		{
			this.logger.Info("Получение активного пользователя");

			try
			{
				dynamic result = null;
				dynamic guid = this.Connector.Connection.ПользователиИнформационнойБазы.ТекущийПользователь().УникальныйИдентификатор;
				dynamic запрос = this.Connector.Connection.NewObject("Запрос");
				запрос.Текст = @"	ВЫБРАТЬ
									Пользователи.Ссылка КАК Ссылка	
								ИЗ
									Справочник.Пользователи КАК Пользователи
								ГДЕ
									Пользователи.ИдентификаторПользователяИБ = &ИдентификаторПользователяИБ";
				запрос.УстановитьПараметр("ИдентификаторПользователяИБ", guid);
				dynamic выборка = запрос.Выполнить().Выбрать();

				if (выборка.Следующий())
					result = выборка.Ссылка;
				else
					result = null;

				this.logger.Info("Активный пользователь получен {0}", this.GuidToString(result.ИдентификаторПользователяИБ));
				return result;
			}
			catch(Exception ex)
			{
				this.logger.Error(ex, "Не удалось получить активного пользователя");
				return null;
			}

		}

        public bool UpdateWareExCode(string innerCode, string exCode, string supplierCode)
        {
			this.logger.Info("Добавление внешнего кода {0} поставщика Код {1} к номенклатуре Код {2}", exCode, supplierCode, innerCode);
            var товар = this.GetWare(Requisites.Code, innerCode);
            var поставщик = this.GetCounteragent(Requisites.Code, supplierCode);

            if (товар == null || поставщик == null)
			{
				this.logger.Warn("Не найден поставщик или номенклатура");
				return false;
			}
                
            try
            {
                var менеджерЗаписи = this.Connector.Connection.РегистрыСведений.ЕДИ_СопоставлениеНоменклатуры.СоздатьМенеджерЗаписи();

                менеджерЗаписи.Контрагент = поставщик;
                менеджерЗаписи.ВнешнийКод = exCode;
                менеджерЗаписи.Прочитать();

                if (!менеджерЗаписи.Выбран())
                {
                    менеджерЗаписи.Контрагент = поставщик;
                    менеджерЗаписи.ВнешнийКод = exCode;
                }

                менеджерЗаписи.Номенклатура = товар;
                менеджерЗаписи.Записать();
				this.logger.Info("Внешний код записан");
				return true;
            }
            catch (Exception ex)
            {
				this.logger.Error(ex, "Не удалось добавить внешний код");
                return false;
            }
        }

        public string AddNewWare(string name, string fullName, string unit, List<string> barCodes)
        {
            return this.AddNewWare("", name, fullName, unit, barCodes);
        }

        public string AddNewWare(string code, string name, string fullName, string unit, List<string> barCodes)
        {
			this.logger.Info("Добавление новой номенклатуры {0}", name);

            try
            {
                var товар = this.Connector.Connection.Справочники.Номенклатура.СоздатьЭлемент();
                товар.Код = code;
                товар.Наименование = name;
                товар.НаименованиеПолное = name;
                товар.СтавкаНДС = this.GetTaxRate(TaxRates.Tax_None);
                товар.ЕдиницаИзмерения = this.GetUnit(Requisites.InternationalReduction_Unit, unit);
                товар.Весовой = unit.ToLower() == "kgm" || unit.ToLower() == "grm";
                товар.Записать();

                foreach (var item in barCodes)
                    this.AddNewBarcode(товар, item);

				this.logger.Info("Номенклатура добавлена {0}", name);
				return товар.Код;
            }
            catch (Exception ex)
            {
				this.logger.Error(ex, "Не удалось добавить новую номенклатуру");
                return null;
            }
        }

        public bool AddNewBarcode(dynamic ware, string value)
        {
			this.logger.Info("Добавление нового штрихкода {0} номенклатуры Код {1}", value, ware.Код);

			try
            {
				if (ware == null || string.IsNullOrWhiteSpace(value) || value.Length < 4)
					throw new ArgumentOutOfRangeException("Передан пустой параметр или длина штрихкода меньше 4х символов");

                var менеджер = this.Connector.Connection.РегистрыСведений.Штрихкоды.СоздатьМенеджерЗаписи();
                менеджер.Владелец = ware.Ссылка;
                менеджер.Штрихкод = value;
                менеджер.ТипШтрихкода = this.GetBarCodeType(value.Length <= 8 ? BarcodeTypes.Ean_8 : 
                                                                                value.Length <= 13 ? BarcodeTypes.Ean_13 : 
                                                                                BarcodeTypes.Ean_128);
                менеджер.Записать(true);
				this.logger.Info("Новый штрихкод добавлен");
				return true;
            }
            catch(Exception ex)
            {
				this.logger.Error(ex, "Не удалось добавить штрихкод");
                return false;
            }
        }

        public bool AddNewBarcode(string wareCode, string value)
		{
			this.logger.Info("Добавление нового штрихкода {0} ко коду номенклатуры {1}", value, wareCode);

			var ware = this.GetWare(Requisites.Code, wareCode);

			if (ware == null)
			{
				this.logger.Warn("Номенклатура не найдена");
				return false;
			}

			return this.AddNewBarcode(ware, value);
		}

		public bool AddNewBarcodes(string wareCode, List<string> values)
		{
			this.logger.Info("Добавление штрихкодов Количество {0} номенклатуре Код {1}", values.Count, wareCode);

            try
            {
				if (values == null || !values.Any() || string.IsNullOrWhiteSpace(wareCode))
					throw new ArgumentNullException("Передан пустой параметр");

                var ware = this.GetWare(Requisites.Code, wareCode);

                if (ware == null)
				{
					this.logger.Warn("Номенклатура не найдена");
					return false;
				}
                   

                foreach (var item in values)
                    this.AddNewBarcode(ware, item);

                return true;
            }
            catch (Exception ex)
            {
				this.logger.Error(ex, "Не удалось добавить штрихкоды");
                return false;
            }
        }

        public bool AddNewExCode(string wareCode, string counteragentGln, string value)
        {
			this.logger.Info("Добавление внешнего кода {0} ГЛН контрагента {1} для номенклатуры Код {2}", value, counteragentGln, wareCode);

			try
            {
				if (string.IsNullOrWhiteSpace(wareCode) || string.IsNullOrWhiteSpace(counteragentGln) || string.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("Передан пустой параметр");

				var номенклатура = this.GetWare(Requisites.Code, wareCode);

				if (номенклатура == null || string.IsNullOrWhiteSpace(номенклатура.Код))
				{
					this.logger.Warn("Номенклатура не найдена");
					return false;
				}

				var менеджер = this.Connector.Connection.РегистрыСведений.ЕДИ_СопоставлениеНоменклатуры.СоздатьМенеджерЗаписи();
                менеджер.Номенклатура = номенклатура;
                менеджер.ГЛНКонтрагента = counteragentGln;
                менеджер.ВнешнийКод = value;
                менеджер.Записать(true);
				this.logger.Info("Внешний код добавлен");
				return true;
            }
            catch (Exception ex)
            {
				this.logger.Error(ex, "Не удалось добавить внешний код");
                return false;
            }
        }

        public bool AddNewExCodes(string wareCode, List<string> counteragentGlns, List<string> values)
        {
			this.logger.Info("Добавление внешних кодов Количество {0} номенклатуры {1}", values.Count, wareCode);

            try
            {
				if (counteragentGlns == null || !counteragentGlns.Any() || values == null || !values.Any() || counteragentGlns.Count != values.Count)
					throw new ArgumentOutOfRangeException("Переданы неверные параметры");

                var ware = this.GetWare(Requisites.Code, wareCode);

                if (ware == null)
				{
					this.logger.Warn("Номенклатура не найдена");
					return false;
				}
                    
                for (int i = 0; i < values.Count; i++)
                    this.AddNewExCode(ware, counteragentGlns[i], values[i]);

				this.logger.Info("Внешние коды добавлены");
                return true;
            }
            catch (Exception ex)
            {
				this.logger.Error(ex, "Не удалось добавить внешние коды");
                return false;
            }
        }

        public bool AddNewWaybill(string number, DateTime date, dynamic counteragent, dynamic warehouse, dynamic shop, List<WaybillRow> rows)
        {
			this.logger.Info("Добавление новой накладной Номер {0} Дата {1}", number, date.ToString("dd.MM.yyyy hh:mm:ss"));

            try
            {
                if(counteragent == null || warehouse == null || shop == null || rows == null || !rows.Any() || string.IsNullOrWhiteSpace(number))
					throw new ArgumentNullException("Передан пустой параметр");

                var поступление = this.Connector.Connection.Документы.ПоступлениеТоваров.СоздатьДокумент();
                поступление.Контрагент = counteragent;
                поступление.Магазин = shop;
                поступление.Склад = warehouse;
               
                поступление.Дата = DateTime.Now;
                поступление.ДатаВходящегоДокумента = date;
                поступление.НомерВходящегоДокумента = number;

                foreach (var row in rows)
                {
                    var строкаТабличнойЧасти = поступление.Товары.Добавить();
                    строкаТабличнойЧасти.Номенклатура = this.GetWare(Requisites.Code, row.Ware.Code);
                    строкаТабличнойЧасти.КоличествоУпаковок = row.Count;
                    строкаТабличнойЧасти.Цена = row.Price;
                    строкаТабличнойЧасти.Сумма = row.Count * (float)row.Price;
                    строкаТабличнойЧасти.СтавкаНДС = row.TaxRate;
                    строкаТабличнойЧасти.СуммаНДС = row.TaxAmount;
                }

                поступление.Записать();
				this.logger.Info("Накладная успешно записана");
                return true;
            }
            catch(Exception ex)
            {
				this.logger.Error(ex, "Не удалось добавить новую накладную");
                return false;
            }
        }

        public List<dynamic> GetAllOrders()
        {
            try
            {
                List<dynamic> result = new List<dynamic>();
                dynamic запрос = this.Connector.Connection.NewObject("Запрос");
                запрос.Текст = @"ВЫБРАТЬ
									Заказ.Ссылка КАК Ссылка	
								ИЗ
									Документ.ЗаказПоставщику КАК Заказ
								ГДЕ
									Заказ.ПометкаУдаления = Ложь";
                dynamic выборка = запрос.Выполнить().Выбрать();

                while(выборка.Следующий())
                    result.Add(выборка.Ссылка);

                return result;
            }
            catch(Exception ex)
            {
                this.logger.Error(ex, "Не удалось получить заказы");
                return null;
            }
        }

        private Connector Connector { get; set; }
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
