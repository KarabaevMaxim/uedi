namespace EdiModuleCore
{
    public class User
    {
		public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string DbFolder { get; set; }
        public string WaybillFolder { get; set; }
        public string ArchiveFolder { get; set; }

		public override string ToString()
		{
			return this.Login;
		}
	}
}
