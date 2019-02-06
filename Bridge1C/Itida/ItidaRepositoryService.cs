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

		public bool AddNewWare(Ware ware)
		{
			throw new NotImplementedException();
		}

		public Ware GetWare(Requisites prop, string propValue, string counteragentGln = "")
		{
			throw new NotImplementedException();
		}

		public List<Ware> GetAllWares()
		{
			throw new NotImplementedException();
		}

		public Counteragent GetCounteragent(Requisites prop, string propValue)
		{
			throw new NotImplementedException();
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

		public Unit GetUnit(Requisites prop, string propValue)
		{
			throw new NotImplementedException();
		}

		public Warehouse GetWarehouse(Requisites prop, string propValue)
		{
			throw new NotImplementedException();
		}

		public List<Warehouse> GetAllWarehouses()
		{
			throw new NotImplementedException();
		}

		public bool RematchingWarehouse(string warehouseCode, string gln)
		{
			throw new NotImplementedException();
		}

		public Shop GetShop(string warehouseCode)
		{
			throw new NotImplementedException();
		}

		public Organization GetOrganization(string warehouseCode)
		{
			throw new NotImplementedException();
		}

		public bool AddNewWaybill(Waybill waybill)
		{
			throw new NotImplementedException();
		}

		public bool AddNewExCodeToWare(Ware ware, WareExCode exCode)
		{
			throw new NotImplementedException();
		}

		private ItidaRepository Repository { get; set; }
	}
}
