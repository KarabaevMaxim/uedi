namespace EdiModuleCore
{
    using DAL;
	using DAL.Itida;

    public static class CoreInit
    {
        public static void Connect(string login, string pass)
        {
            CoreInit.RepositoryService = new RepositoryService(@"C:\Users\Максим\Documents\InfoBase7", login, pass);
        }

		public static void Connect(string login, string pass, string db)
		{
			CoreInit.RepositoryService = new RepositoryService(db, login, pass);
		}

		public static void ConnectToItida(string connectionString)
		{
			CoreInit.RepositoryService = new ItidaRepositoryService(connectionString);
		}

        public static void Init()
        {
            CoreInit.ModuleRepository = new ModuleRepository();
        }

        public static IRepositoryService RepositoryService { get; private set; }
        public static ModuleRepository ModuleRepository { get; private set; }
    }
}
