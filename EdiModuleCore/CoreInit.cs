namespace EdiModuleCore
{
    using Bridge1C;

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

        public static void Init()
        {
            CoreInit.ModuleRepository = new ModuleRepository();
        }

        public static RepositoryService RepositoryService { get; private set; }
        public static ModuleRepository ModuleRepository { get; private set; }
    }
}
