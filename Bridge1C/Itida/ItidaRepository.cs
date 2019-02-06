namespace Bridge1C.Itida
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
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
		/// <returns></returns>
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
					while(reader.Read())
					{
						Ware ware = new Ware
						{
							Code = ((string)reader.GetValue(0)).Trim(),
							Name = ((string)reader.GetValue(1)).Trim(),
							FullName = ((string)reader.GetValue(2)).Trim(),
							Unit = this.GetUnit(Requisites.Code, ((string)reader.GetValue(3)).Trim()), // todo: можно в запросе использовать Join Для объединения с таблицей единиц измерения
							ExCodes = this.GetExCodes(((string)reader.GetValue(0)).Trim())
						};
						result.Add(ware);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Получить ЕИ.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public Unit GetUnit(Requisites propertyName, string value)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				Unit result = null;

				switch (propertyName)
				{
					case Requisites.Code:
						SqlCommand command = new SqlCommand("SELECT ex_Code, name FROM spredn WHERE code = @code", conn);
						command.Parameters.Add(new SqlParameter("@code", value));
						SqlDataReader reader = command.ExecuteReader();

						if (reader.HasRows)
						{
							reader.Read();
							result = new Unit
							{
								Code = value.Trim(),
								International = ((string)reader.GetValue(0)).Trim(),
								FullName = ((string)reader.GetValue(1)).Trim(),
								Name = value.Trim()
							};
						}

						break;
					case Requisites.Name:
						break;
					case Requisites.InternationalReduction_Unit:
						break;
					default:
						break;
				}

				return result;
			}
		}

		/// <summary>
		/// Получить список внешних кодов номенклатуры.
		/// </summary>
		/// <param name="wareCode"></param>
		/// <returns></returns>
		public List<WareExCode> GetExCodes(string wareCode) // todo: тут какой то касяк, надо будет разобраться.
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				List<WareExCode> result = new List<WareExCode>();
				SqlCommand command = new SqlCommand("SELECT client, ex_code FROM sprres_clients WHERE code = @code", conn);
				command.Parameters.Add(new SqlParameter("@code", wareCode));
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						WareExCode exCode = new WareExCode
						{
							Counteragent = this.GetCounteragent(Requisites.Code, ((string)reader.GetValue(0)).Trim()),
							Value = ((string)reader.GetValue(1)).Trim()
						};

						result.Add(exCode);
					}
				}

				return result;
			}
		}



		/// <summary>
		/// Получить контрагента.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public Counteragent GetCounteragent(Requisites propertyName, string value)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				Counteragent result = null;

				switch(propertyName)
				{
					case Requisites.Code:
						SqlCommand command = new SqlCommand("SELECT name, shortname, ex_code FROM sprclient WHERE code = @code", conn);
						command.Parameters.Add(new SqlParameter("@code", value));
						SqlDataReader reader = command.ExecuteReader();

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
						break;
					default:
						break;
				}

				return result;
			}
		}

		private readonly string connectionString;
	}
}
