namespace Bridge1C.Itida
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Data.SqlClient;
	using DomainEntities;

	public class ItidaRepository
	{
		public ItidaRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		/// <summary>
		/// Получить список товаров.
		/// </summary>
		public List<Ware> GetAllWares()
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<Ware> result = new List<Ware>();
				SqlCommand command = new SqlCommand("SELECT maincode, shortname, name, ed FROM sprnn", conn);
				SqlDataReader reader = command.ExecuteReader();

				if(reader.HasRows)
				{
					string wareCode = string.Empty;

					while (reader.Read())
					{
						wareCode = reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim();
						Ware ware = new Ware
						{
							Code = wareCode,
							Name = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
							FullName = reader.GetValue(2) == DBNull.Value ? string.Empty : ((string)reader.GetValue(2)).Trim(),
							Unit = this.GetUnit(Requisites.Code, reader.GetValue(3) == DBNull.Value ? string.Empty : ((string)reader.GetValue(3)).Trim()), // todo: можно в запросе использовать Join Для объединения с таблицей единиц измерения
							ExCodes = this.GetExCodes(wareCode),
							BarCodes = this.GetWareBarcodes(wareCode)
						};
						result.Add(ware);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Возвращает объект номенклатуры по указанному реквизиту.
		/// </summary>
		/// <param name="propertyName">Реквизит для поиска.</param>
		/// <param name="propertyValue">Значение реквизита для поиска.</param>
		/// <param name="counteragentGln">ГЛН контрагента (необходимо в случае поиска по внешнему коду номенклатуры).</param>
		public Ware GetWare(Requisites propertyName, string propertyValue, string counteragentGln = "")
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				Ware result = null;
				SqlCommand command = null;
				SqlDataReader reader = null;
				switch (propertyName)
				{
					case Requisites.Name:
						break;
					case Requisites.Code:
						command = new SqlCommand("SELECT maincode, shortname, name, ed FROM sprnn WHERE maincode = @code", conn);
						command.Parameters.Add(new SqlParameter("@code", propertyValue));
						reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							string wareCode = ((string)reader.GetValue(0)).Trim();
							result = new Ware
							{
								Code = wareCode,
								Name = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
								FullName = reader.GetValue(2) == DBNull.Value ? string.Empty : ((string)reader.GetValue(2)).Trim(),
								Unit = this.GetUnit(Requisites.Code, reader.GetValue(3) == DBNull.Value ? string.Empty : ((string)reader.GetValue(3)).Trim()), // todo: можно в запросе использовать Join Для объединения с таблицей единиц измерения
								ExCodes = this.GetExCodes(wareCode),
								BarCodes = this.GetWareBarcodes(wareCode)
							};
						}
						break;
					case Requisites.ExCode_Ware:
						if (string.IsNullOrWhiteSpace(counteragentGln))
							throw new ArgumentOutOfRangeException("Значение counteragentGln не корректно");

						command = new SqlCommand(@"	SELECT spr.maincode, spr.shortname, spr.name, spr.ed
													FROM sprnn AS spr 
														LEFT JOIN sprres_clients AS excode ON spr.nn = excode.code 
														LEFT JOIN sprclient AS client ON excode.client = client.code 
													WHERE excode.ex_code = @wareEx AND client.ex_code = @clientEx", conn);
						command.Parameters.Add(new SqlParameter("@wareEx", propertyValue));
						command.Parameters.Add(new SqlParameter("@clientEx", counteragentGln));
						reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							string wareCode = ((string)reader.GetValue(0)).Trim();
							result = new Ware
							{
								Code = wareCode,
								Name = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
								FullName = reader.GetValue(2) == DBNull.Value ? string.Empty : ((string)reader.GetValue(2)).Trim(),
								Unit = this.GetUnit(Requisites.Code, reader.GetValue(3) == DBNull.Value ? string.Empty : ((string)reader.GetValue(3)).Trim()), // todo: можно в запросе использовать Join Для объединения с таблицей единиц измерения
								ExCodes = this.GetExCodes(wareCode),
								BarCodes = this.GetWareBarcodes(wareCode)
							};
						}
						break;
					default:
						throw new ArgumentOutOfRangeException("Значение propertyName не допустимо");
				}

				return result;
			}
		}

		/// <summary>
		/// Возвращает объект номенклатуры по указанному реквизиту.
		/// </summary>
		/// <param name="exCode">Внешний код.</param>
		/// <param name="counteragentCode">Код контрагента.</param>
		public Ware GetWareByExCode(string exCode, string counteragentCode)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				Ware result = null;
				SqlCommand command = command = new SqlCommand(@"SELECT spr.maincode, spr.shortname, spr.name, spr.ed
																FROM sprnn AS spr 
																	LEFT JOIN sprres_clients AS excode ON spr.nn = excode.code 
																	LEFT JOIN sprclient AS client ON excode.client = client.code 
																WHERE excode.ex_code = @wareEx AND client.code = @clientCode", conn);
				command.Parameters.Add(new SqlParameter("@wareEx", exCode));
				command.Parameters.Add(new SqlParameter("@clientCode", counteragentCode));
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					reader.Read();
					string wareCode = ((string)reader.GetValue(0)).Trim();
					result = new Ware
					{
						Code = wareCode,
						Name = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
						FullName = reader.GetValue(2) == DBNull.Value ? string.Empty : ((string)reader.GetValue(2)).Trim(),
						Unit = this.GetUnit(Requisites.Code, reader.GetValue(3) == DBNull.Value ? string.Empty : ((string)reader.GetValue(3)).Trim()), // todo: можно в запросе использовать Join Для объединения с таблицей единиц измерения
						ExCodes = this.GetExCodes(wareCode),
						BarCodes = this.GetWareBarcodes(wareCode)
					};
				}

				return result;
			}
		}

		/// <summary>
		/// Получить ЕИ.
		/// </summary>
		/// <param name="propertyName">Реквизит для поиска.</param>
		/// <param name="value">Значение реквизита для поиска.</param>
		public Unit GetUnit(Requisites propertyName, string value)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				Unit result = null;
				SqlCommand command = null;
				SqlDataReader reader = null;

				switch (propertyName)
				{
					case Requisites.Code:
						command = new SqlCommand("SELECT ex_Code, name FROM spredn WHERE code = @code", conn);
						command.Parameters.Add(new SqlParameter("@code", value));
						reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							result = new Unit
							{
								Code = value.Trim(),
								International = reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim(),
								FullName = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
								Name = value.Trim()
							};
						}
						break;
					case Requisites.Name:
						break;
					case Requisites.InternationalReduction_Unit:
						command = new SqlCommand("SELECT code, name FROM spredn WHERE ex_Code = @exCode", conn);
						command.Parameters.Add(new SqlParameter("@exCode", value));
						reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							string code = reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim();
							result = new Unit
							{
								Code = code,
								International = value,
								FullName = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
								Name = code
							};
						}
						break;
					default:
						throw new ArgumentOutOfRangeException("Значение porpertyName не допустимо");
				}

				return result;
			}
		}

		/// <summary>
		/// Получить контрагента.
		/// </summary>
		/// <param name="propertyName">Реквизит для поиска.</param>
		/// <param name="value">Значение реквизита для поиска.</param>
		public Counteragent GetCounteragent(Requisites propertyName, string value)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				Counteragent result = null;
				SqlCommand command = null;
				SqlDataReader reader = null;

				switch (propertyName)
				{
					case Requisites.Code:
						command = new SqlCommand("SELECT name, shortname, ex_code FROM sprclient WHERE code = @code", conn);
						command.Parameters.Add(new SqlParameter("@code", value));
						reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							result = new Counteragent
							{
								Code = value,
								FullName = ((string)reader.GetValue(0)).Trim(),
								Name = ((string)reader.GetValue(1)).Trim(),
								GLN = ((string)reader.GetValue(2)).Trim(),
							};
						}
						break;
					case Requisites.Name:
						break;
					case Requisites.GLN:
						command = new SqlCommand("SELECT code, name, shortname FROM sprclient WHERE ex_code = @exCode", conn);
						command.Parameters.Add(new SqlParameter("@exCode", value));
						reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							result = new Counteragent
							{
								Code = ((string)reader.GetValue(0)).Trim(),
								FullName = ((string)reader.GetValue(1)).Trim(),
								Name = ((string)reader.GetValue(2)).Trim(),
								GLN = value
							};
						}
						break;
					default:
						throw new ArgumentOutOfRangeException("Значение propertyName не допустимо");
				}

				return result;
			}
		}

		public List<Counteragent> GetAllCounteragents()
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<Counteragent> result = new List<Counteragent>();
				SqlCommand command = new SqlCommand("SELECT code, name, shortname, ex_code FROM sprclient", conn);
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while(reader.Read())
					{
						result.Add(new Counteragent
						{
							Code = ((string)reader.GetValue(0)).Trim(),
							FullName = ((string)reader.GetValue(1)).Trim(),
							Name = ((string)reader.GetValue(2)).Trim(),
							GLN = ((string)reader.GetValue(3)).Trim(),
						});
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Получить организацию.
		/// </summary>
		/// <param name="propertyName">Реквизит для поиска.</param>
		/// <param name="value">Значение реквизита для поиска.</param>
		public Organization GetOrganization(Requisites propertyName, string value)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				Organization result = null;

				switch (propertyName)
				{
					case Requisites.Code:
						SqlCommand command = new SqlCommand("SELECT shortname, ex_code FROM sprfirm WHERE code = @code", conn);
						command.Parameters.Add(new SqlParameter("@code", value));
						SqlDataReader reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							result = new Organization
							{
								Code = value,
								Name = ((string)reader.GetValue(0)).Trim(),
								GLN = ((string)reader.GetValue(1)).Trim()
							};
						}
						break;
					case Requisites.Name:
						break;
					case Requisites.GLN:
						break;
					default:
						throw new ArgumentOutOfRangeException("Значение porpertyName не допустимо");
				}

				return result;
			}
		}

		/// <summary>
		/// Получить склад.
		/// </summary>
		/// <param name="propertyName">Реквизит для поиска.</param>
		/// <param name="value">Значение реквизита для поиска.</param>
		public Warehouse GetWarehouse(Requisites propertyName, string value)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				Warehouse result = null;
				SqlCommand command = null;
				SqlDataReader reader = null;

				switch (propertyName)
				{
					case Requisites.Code:
						command = new SqlCommand("SELECT name FROM sprskl WHERE code = @code", conn);
						command.Parameters.Add(new SqlParameter("@code", value));
						reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							string name = reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim();
							result = new Warehouse
							{
								Code = value,
								Name = name,
								Shop = new Shop { Code = value, Name = name }
							};
						}
						break;
					case Requisites.Name:
						break;
					case Requisites.GLN:
						command = new SqlCommand("SELECT code, name FROM sprskl WHERE ex_code = @gln", conn);
						command.Parameters.Add(new SqlParameter("@gln", value));
						reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							string name = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim();
							result = new Warehouse
							{
								Code = reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim(),
								Name = name,
								Shop = new Shop { Code = value, Name = name }
							};
						}
						break;
					default:
						throw new ArgumentOutOfRangeException("Значение porpertyName не допустимо");
				}

				return result;
			}
		}

		/// <summary>
		/// Получить список складов.
		/// </summary>
		public List<Warehouse> GetAllWarehouses()
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<Warehouse> result = new List<Warehouse>();
				SqlCommand command = new SqlCommand("SELECT code, name FROM sprskl", conn);
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					string wareCode = string.Empty;

					while (reader.Read())
					{
						string code = reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim();
						string name = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim();
						Warehouse warehouse = new Warehouse
						{
							Code = code,
							Name = name,
							Shop = new Shop { Code = code, Name = name }
						};
						result.Add(warehouse);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Возвращает список всех накладных.
		/// </summary>
		public List<Waybill> GetAllWaybills()
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<Waybill> result = new List<Waybill>();

				SqlCommand command = new SqlCommand(@"SELECT wbtb.ndok, wbtb.date, cntr.code, cntr.name, cntr.shortname, cntr.ex_code,
														org.code, org.shortname, org.ex_code, skl.code, skl.name
													FROM spr001 as wbtb 
													LEFT JOIN sprclient as cntr ON wbtb.client = cntr.code 
													LEFT JOIN sprfirm as org ON wbtb.firm = org.code 
													LEFT JOIN sprskl as skl ON wbtb.sklad = skl.code", conn);
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						Waybill wb = new Waybill
						{
							Number = ((string)reader.GetValue(0)).Trim(),
							Date = ((DateTime)reader.GetValue(1)),
							Supplier = new Counteragent
							{
								Code = ((string)reader.GetValue(2)).Trim(),
								FullName = ((string)reader.GetValue(3)).Trim(),
								Name = ((string)reader.GetValue(4)).Trim(),
								GLN = ((string)reader.GetValue(5)).Trim(),
							},
							Organization = new Organization
							{
								Code = ((string)reader.GetValue(6)).Trim(),
								Name = ((string)reader.GetValue(7)).Trim(),
								GLN = ((string)reader.GetValue(8)).Trim()
							},
							Warehouse = new Warehouse
							{
								Code = ((string)reader.GetValue(9)).Trim(),
								Name = ((string)reader.GetValue(10)).Trim(),
								Shop = null
							},
							Positions = new List<WaybillRow>(),
							Shop = null
						};
						result.Add(wb);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Возвращает список накладных по указанному номеру. В идеале должен быть возвращен список из одной накладной, но не обязательно.
		/// </summary>
		/// <param name="number">Номер накладной.</param>
		public List<Waybill> GetWaybillsByNumber(string number)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<Waybill> result = new List<Waybill>();

				SqlCommand command = new SqlCommand(@"SELECT wbtb.date, cntr.code, cntr.name, cntr.shortname, cntr.ex_code,
														org.code, org.shortname, org.ex_code, skl.code, skl.name
													FROM spr001 as wbtb 
													LEFT JOIN sprclient as cntr ON wbtb.client = cntr.code 
													LEFT JOIN sprfirm as org ON wbtb.firm = org.code 
													LEFT JOIN sprskl as skl ON wbtb.sklad = skl.code
													WHERE wbtb.ndok = @ndok", conn);
				command.Parameters.Add(new SqlParameter("@ndok", number));
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						Waybill wb = new Waybill
						{
							Number = number,
							Date = ((DateTime)reader.GetValue(0)),
							Shop = null
						};
						wb.Supplier = reader.GetValue(1) == DBNull.Value ? null : new Counteragent
						{
							Code = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
							FullName = reader.GetValue(2) == DBNull.Value ? string.Empty : ((string)reader.GetValue(2)).Trim(),
							Name = reader.GetValue(3) == DBNull.Value ? string.Empty : ((string)reader.GetValue(3)).Trim(),
							GLN = reader.GetValue(4) == DBNull.Value ? string.Empty : ((string)reader.GetValue(4)).Trim()
						};
						wb.Organization = reader.GetValue(5) == DBNull.Value ? null : new Organization
						{
							Code = reader.GetValue(5) == DBNull.Value ? string.Empty : ((string)reader.GetValue(5)).Trim(),
							Name = reader.GetValue(6) == DBNull.Value ? string.Empty : ((string)reader.GetValue(6)).Trim(),
							GLN = reader.GetValue(7) == DBNull.Value ? string.Empty : ((string)reader.GetValue(7)).Trim()
						};
						wb.Warehouse = reader.GetValue(8) == DBNull.Value ? null : new Warehouse
						{
							Code = reader.GetValue(8) == DBNull.Value ? string.Empty : ((string)reader.GetValue(8)).Trim(),
							Name = reader.GetValue(9) == DBNull.Value ? string.Empty : ((string)reader.GetValue(9)).Trim(),
							Shop = null
						};
						wb.Positions = new List<WaybillRow>();
						result.Add(wb);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Записать в базу данных объект номенклатуры.
		/// </summary>
		/// <param name="ware">Объект для записи.</param>
		public bool AddNewWare(Ware ware)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlCommand command = new SqlCommand("sp_addware", conn);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.Add(new SqlParameter("@name", ware.FullName));
				command.Parameters.Add(new SqlParameter("@submaincode", ""));
				int identityColumn = (int)command.ExecuteScalar();
				command = new SqlCommand(@"	UPDATE sprres 
											SET shortname = @sName, ed = @unit
											WHERE identity_column = @ic", conn);
				command.Parameters.Add(new SqlParameter("@sName", ware.Name));
				command.Parameters.Add(new SqlParameter("@unit", ware.Unit.Code));
				command.Parameters.Add(new SqlParameter("@ic", identityColumn));
				int result = command.ExecuteNonQuery(); // todo: касяк - возвращает 2, хотя должна быть задействована всего одна строка

				command = new SqlCommand("SELECT code FROM sprres WHERE identity_column = @ic", conn);
				command.Parameters.Add(new SqlParameter("@ic", identityColumn));

				SqlDataReader reader = command.ExecuteReader();
				
				if(reader.HasRows)
				{
					reader.Read();
					string wareIc = reader.GetValue(0) == DBNull.Value ? string.Empty : (string)reader.GetValue(0);
					this.AddNewBarcodes(wareIc, ware.BarCodes);
					this.AddNewExCodes(wareIc, ware.ExCodes);
				}

				return result > 0;
			}
		}

		public void AddNewExCodes(string wareIc, List<WareExCode> exCodes)
		{
			if (string.IsNullOrWhiteSpace(wareIc) || exCodes == null || exCodes.Count == 0)
				throw new ArgumentOutOfRangeException();

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlCommand command = new SqlCommand("", conn);
				SqlParameter parameterCode = new SqlParameter("@wareCode", wareIc);
				SqlParameter parameterClientCode = new SqlParameter("@clientCode", null);
				SqlParameter parameterExCode = new SqlParameter("@exCode", null);
				command.Parameters.Add(parameterCode);
				command.Parameters.Add(parameterClientCode);
				command.Parameters.Add(parameterExCode);

				foreach (var item in exCodes)
				{
					command.CommandText = "INSERT INTO sprres_clients (code, client, ex_code) VAlUES (@wareCode, @clientCode, @exCode)";
					parameterClientCode.Value = item.Counteragent.Code;
					parameterExCode.Value = item.Value;
					command.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Добавить новый внешний код к номенклатуре с кодом wareMainCode (sprnn.maincode).
		/// </summary>
		/// <param name="wareMainCode">Код номенклатуры(sprnn.maincode).</param>
		/// <param name="exCode">Внешний код для записи.</param>
		public void AddNewExCode(string wareMainCode, WareExCode exCode)
		{
			if (string.IsNullOrWhiteSpace(wareMainCode) || exCode == null)
				throw new ArgumentOutOfRangeException();

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlCommand command = new SqlCommand("SELECT nn FROM sprnn WHERE maincode = @maincode", conn);
				command.Parameters.Add(new SqlParameter("@maincode", wareMainCode));
				SqlDataReader reader = command.ExecuteReader();

				if(reader.HasRows)
				{
					reader.Read();
					string wareCode = reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim();
					reader.Close();

					if (string.IsNullOrWhiteSpace(wareCode))
						return;

					command = new SqlCommand("INSERT INTO sprres_clients (code, client, ex_code) VALUES (@wareCode, @clientCode, @exCode)", conn);
					command.Parameters.Add(new SqlParameter("@wareCode", wareCode));
					command.Parameters.Add(new SqlParameter("@clientCode", exCode.Counteragent.Code));
					command.Parameters.Add(new SqlParameter("@exCode", exCode.Value));
					command.ExecuteNonQuery();
				}
			}
		}

		public void RemoveExCode(string wareMainCode, WareExCode exCode)
		{
			if (string.IsNullOrWhiteSpace(wareMainCode) || exCode == null)
				throw new ArgumentOutOfRangeException();

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlCommand command = new SqlCommand("SELECT nn FROM sprnn WHERE maincode = @maincode", conn);
				command.Parameters.Add(new SqlParameter("@maincode", wareMainCode));
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					reader.Read();
					string wareCode = reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim();
					reader.Close();

					if (string.IsNullOrWhiteSpace(wareCode))
						return;

					command = new SqlCommand(@"	DELETE FROM sprres_clients 
												WHERE code = @wareCode AND client = @clientCode AND ex_code = @exCode", conn);
					command.Parameters.Add(new SqlParameter("@wareCode", wareCode));
					command.Parameters.Add(new SqlParameter("@clientCode", exCode.Counteragent.Code));
					command.Parameters.Add(new SqlParameter("@exCode", exCode.Value));
					command.ExecuteNonQuery();
				}
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
			if (string.IsNullOrWhiteSpace(counteragentCode) || string.IsNullOrWhiteSpace(gln))
				throw new ArgumentOutOfRangeException();

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlCommand command = new SqlCommand("UPDATE sprclient SET ex_code = '' WHERE ex_code = @gln", conn);
				command.Parameters.Add(new SqlParameter("@gln", gln));
				command.ExecuteNonQuery();

				command = new SqlCommand("UPDATE sprclient SET ex_code = @gln WHERE code = @code", conn);
				command.Parameters.Add(new SqlParameter("@gln", gln));
				command.Parameters.Add(new SqlParameter("@code", counteragentCode));
				int result = command.ExecuteNonQuery();
				return result > 0;
			}
		}

		/// <summary>
		/// Инициализирует поле ex_code склада warehouse значением gln, если в базе есть контрагент с ex_code gln 
		/// </summary>
		/// <param name="warehouseCode">Код склада для инициализации.</param>
		/// <param name="gln">ГЛН.</param>
		public bool RematchingWarehouse(string warehouseCode, string gln)
		{
			if (string.IsNullOrWhiteSpace(warehouseCode) || string.IsNullOrWhiteSpace(gln))
				throw new ArgumentOutOfRangeException();

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlCommand command = new SqlCommand("UPDATE sprskl SET ex_code = '' WHERE ex_code = @gln", conn);
				command.Parameters.Add(new SqlParameter("@gln", gln));
				command.ExecuteNonQuery();

				command = new SqlCommand("UPDATE sprskl SET ex_code = @gln WHERE code = @code", conn);
				command.Parameters.Add(new SqlParameter("@gln", gln));
				command.Parameters.Add(new SqlParameter("@code", warehouseCode));
				int result = command.ExecuteNonQuery();
				return result > 0;
			}
		}

		public bool AddNewWaybill(Waybill waybill)
		{
			if (waybill == null)
				throw new ArgumentNullException();

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlCommand command = new SqlCommand("sp_insertdoc", conn);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.Add(new SqlParameter("@docCode", "001"));
				command.Parameters.Add(new SqlParameter("@ndok", waybill.Number));
				command.Parameters.Add(new SqlParameter("@date", waybill.Date));
				command.Parameters.Add(new SqlParameter("@parentNDOK", DBNull.Value));
				command.Parameters.Add(new SqlParameter("@parentFIRM", DBNull.Value));
				command.Parameters.Add(new SqlParameter("@parentAUTHOR", DBNull.Value));
				command.Parameters.Add(new SqlParameter("@parentCUR", DBNull.Value));
				command.Parameters.Add(new SqlParameter("@bfn", "0000000002"));
				command.Parameters.Add(new SqlParameter("@accountList", "001"));
				command.Parameters.Add(new SqlParameter("@onlyNewNumber", DBNull.Value));
				command.Parameters.Add(new SqlParameter("@defaultAccount", DBNull.Value));
				command.Parameters.Add(new SqlParameter("@viewpoint", "0000000001"));
				string newNdoc = (string)command.ExecuteScalar();

				command = new SqlCommand("SELECT identity_column FROM spr001 WHERE ndok = @newNdoc ORDER BY identity_column DESC", conn);
				command.Parameters.Add(new SqlParameter("@newNdoc", waybill.Number));
				SqlDataReader reader = command.ExecuteReader();

				int newIc = -1;
				if(reader.HasRows)
				{
					reader.Read();
					newIc = (int)reader.GetValue(0);
					reader.Close();
					command = new SqlCommand("UPDATE spr001 SET firm = @orgCode, client = @counterCode, sklad = @whCode, summa = @amount WHERE identity_column = @wbIc", conn);
					command.Parameters.Add(new SqlParameter("@orgCode", waybill.Organization.Code));
					command.Parameters.Add(new SqlParameter("@counterCode", waybill.Supplier.Code));
					command.Parameters.Add(new SqlParameter("@whCode", waybill.Warehouse.Code));
					command.Parameters.Add(new SqlParameter("@amount", waybill.Positions.Sum(p => (float)p.Price * p.Count)));
					command.Parameters.Add(new SqlParameter("@wbIc", newIc));
					command.ExecuteNonQuery();

					foreach (var item in waybill.Positions)
					{
						// найти внутренний код товара по его главному коду
						command = new SqlCommand(@"SELECT nn FROM sprnn WHERE maincode = @mainCode", conn);
						command.Parameters.Add(new SqlParameter("@maincode", item.Ware.Code));
						reader = command.ExecuteReader();
						string wareCode = string.Empty;

						if (reader.HasRows)
						{
							reader.Read();
							wareCode = (string)reader.GetValue(0);
						}
						
						reader.Close();

						// найти код списка налогов по величине ставки налога
						command = new SqlCommand(@"	SELECT list.code 
													FROM sprkodn AS spr 
													INNER JOIN speclistkodn AS spec ON spr.code = spec.list 
													INNER JOIN sprlistkodn AS list ON spec.code = list.code
													WHERE proc_ = @taxRate", conn);
						command.Parameters.Add(new SqlParameter("@taxRate", item.TaxRate));
						reader = command.ExecuteReader();
						string taxCode = string.Empty;

						if(reader.HasRows)
						{
							reader.Read();
							taxCode = (string)reader.GetValue(0);
						}

						reader.Close();
						command = new SqlCommand(@"	INSERT INTO spec001 (ic, nn, ed, kolp, cena, summa, sumnds, kodn, nnname) 
													VALUES (@wbIc, @wareCode, @ed, @count, @price, @amount, @taxAmount, @taxCode, @wareName)", conn);
						command.Parameters.Add(new SqlParameter("@wbIc", newIc));
						command.Parameters.Add(new SqlParameter("@wareCode", wareCode));
						command.Parameters.Add(new SqlParameter("@ed", item.Unit.Code));
						command.Parameters.Add(new SqlParameter("@count", item.Count));
						command.Parameters.Add(new SqlParameter("@price", item.Price));
						command.Parameters.Add(new SqlParameter("@amount", (float)item.Price * item.Count));
						command.Parameters.Add(new SqlParameter("@taxAmount", (item.TaxRate == 0 ? 0 : 1.0f / (float)item.TaxRate) * (float)item.Price * item.Count));
						command.Parameters.Add(new SqlParameter("@taxCode", taxCode));
						command.Parameters.Add(new SqlParameter("@wareName", item.Ware.Name));
						int result = command.ExecuteNonQuery();
					}
				}
				else
				{
					return false;
				}
			}

			return true;
		}

		 

		#region Закрытые члены класса (потенциально закрытые)

		/// <summary>
		/// Сохраняет в базу данных штрихкода номенклатуры с кодом wareIc (sprnnbc.nn или sprres.code)
		/// </summary>
		/// <param name="wareIc">Код товара (sprnnbc.nn или sprres.code).</param>
		/// <param name="barcodes">Список штрихкодов.</param>
		private void AddNewBarcodes(string wareIc, List<string> barcodes)
		{
			if (string.IsNullOrWhiteSpace(wareIc) || barcodes == null || barcodes.Count == 0)
				throw new ArgumentOutOfRangeException();

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlCommand command = new SqlCommand("", conn);
				SqlParameter parameterCode = new SqlParameter("@code", wareIc);
				SqlParameter parameterBarcode = new SqlParameter("@barcode", null);
				command.Parameters.Add(parameterCode);
				command.Parameters.Add(parameterBarcode);

				foreach (var item in barcodes)
				{
					if (string.IsNullOrWhiteSpace(item))
						continue;

					command.CommandText = "INSERT INTO sprnnbc (nn, bc) VAlUES (@code, @barcode)";
					parameterBarcode.Value = item;
					command.ExecuteNonQuery();
				}

			}
		}

		/// <summary>
		/// Возвращает список ШК для товара с указанным кодом wareCode (sprres.maincode или sprnn.maincode).
		/// </summary>
		/// <param name="wareCode">Код товара (sprres.maincode или sprnn.maincode).</param>
		public List<string> GetWareBarcodes(string wareCode)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<string> result = new List<string>();
				SqlCommand command = new SqlCommand(@"SELECT bc
													FROM sprnn as spr left join sprnnbc as bc on spr.nn = bc.nn
													WHERE maincode = @mc", conn);
				command.Parameters.Add(new SqlParameter("@mc", wareCode));
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						result.Add(reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim());
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Возвращает строки из накладной с указанным идентификатором (identity_column).
		/// </summary>
		/// <param name="wbIc">Идентификатор(identity_column)</param>
		public List<WaybillRow> GetWaybillRows(string wbIc)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<WaybillRow> result = new List<WaybillRow>();
				SqlCommand command = new SqlCommand(@"	SELECT spec.kolp, spec.cena, ISNULL((
															SELECT kodn.proc_ 
															FROM sprkodn kodn
															WHERE CHARINDEX(kodn.ntype, '05 06' ) <> 0 AND CHARINDEX( kodn.code, (
																SELECT list 
																FROM speclistkodn 
																WHERE code= spec.kodn AND date= (
																	SELECT MAX( date ) 
																	FROM speclistkodn 
																	WHERE code = spec.kodn AND date <= spr.date))) <> 0), 0 
														) AS TaxRate, spec.sumnds, edn.code, edn.name, edn.ex_code,
														nn.maincode, nn.shortname, nn.name, nn.ed
														FROM spec001 AS spec 
															INNER JOIN spr001 AS spr ON spec.ic = spr.identity_column 
															LEFT JOIN spredn as edn ON spec.ed = edn.code 
															LEFT JOIN sprnn AS nn ON spec.nn = nn.nn
														WHERE spec.ic = @ndok", conn);
				command.Parameters.Add(new SqlParameter("@ndok", wbIc));
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						WaybillRow row = new WaybillRow();
						row.Count = reader.GetValue(0) == DBNull.Value ? 0 : (float)(double)reader.GetValue(0);
						row.Price = reader.GetValue(1) == DBNull.Value ? 0 : (decimal)(double)reader.GetValue(1);
						row.TaxRate = reader.GetValue(2) == DBNull.Value ? 0 : (int)Math.Round((double)reader.GetValue(2));
						row.TaxAmount = reader.GetValue(3) == DBNull.Value ? 0 : (decimal)(double)reader.GetValue(3);
						row.Unit = reader.GetValue(4) == DBNull.Value ? null : new Unit
						{
							Code = reader.GetValue(4) == DBNull.Value ? string.Empty : ((string)reader.GetValue(4)).Trim(),
							Name = reader.GetValue(4) == DBNull.Value ? string.Empty : ((string)reader.GetValue(4)).Trim(),
							FullName = reader.GetValue(5) == DBNull.Value ? string.Empty : ((string)reader.GetValue(5)).Trim(),
							International = reader.GetValue(6) == DBNull.Value ? string.Empty : ((string)reader.GetValue(6)).Trim()
						};
						string wareCode = reader.GetValue(7) == DBNull.Value ? string.Empty : ((string)reader.GetValue(7)).Trim();
						row.Ware = string.IsNullOrWhiteSpace(wareCode) ? null : new Ware
						{
							Code = wareCode,
							Name = reader.GetValue(8) == DBNull.Value ? string.Empty : ((string)reader.GetValue(8)).Trim(),
							FullName = reader.GetValue(9) == DBNull.Value ? string.Empty : ((string)reader.GetValue(9)).Trim(),
							Unit = this.GetUnit(Requisites.Code, reader.GetValue(9) == DBNull.Value ? string.Empty : ((string)reader.GetValue(10)).Trim()),
							ExCodes = this.GetExCodes(wareCode),
							BarCodes = this.GetWareBarcodes(wareCode)
						};

						result.Add(row);
					}
				}
				return result;
			}
		}

		/// <summary>
		/// Получить список внешних кодов номенклатуры по ее коду (sprnn.maincode или sprres.maincode).
		/// </summary>
		/// <param name="wareCode">Код номенклатуры (sprnn.maincode или sprres.maincode).</param>
		public List<WareExCode> GetExCodes(string wareCode)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<WareExCode> result = new List<WareExCode>();
				SqlCommand command = new SqlCommand(@"	SELECT client.code, client.name, client.shortname,client.ex_code, excode.ex_code 
														FROM sprnn AS spr 
															LEFT JOIN sprres_clients as excode on spr.nn = excode.code
															LEFT JOIN sprclient as client ON excode.client = client.code
														WHERE spr.maincode = @code",
														conn);
				command.Parameters.Add(new SqlParameter("@code", wareCode));
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						WareExCode exCode = new WareExCode
						{
							Counteragent = new Counteragent
							{
								Code = reader.GetValue(0) == DBNull.Value ? string.Empty : ((string)reader.GetValue(0)).Trim(),
								FullName = reader.GetValue(1) == DBNull.Value ? string.Empty : ((string)reader.GetValue(1)).Trim(),
								Name = reader.GetValue(2) == DBNull.Value ? string.Empty : ((string)reader.GetValue(2)).Trim(),
								GLN = reader.GetValue(3) == DBNull.Value ? string.Empty : ((string)reader.GetValue(3)).Trim(),
							},
							Value = reader.GetValue(4) == DBNull.Value ? string.Empty : ((string)reader.GetValue(4)).Trim()
						};

						result.Add(exCode);
					}
				}

				return result;
			}
		}

		private readonly string connectionString;

		#endregion
	}
}
