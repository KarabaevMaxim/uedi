namespace UniversalEdiModule.Core
{
    public class Initializer
    {
        private Initializer(string dbFileName, string login, string password)
        {
            DbFileName1C = dbFileName;
            Login1C = login;
            Password1C = password;
        }

        internal static Initializer Instance(string dbFileName, string login, string password)
        {
            return new Initializer(dbFileName, login, password);
        }

        internal static string DbFileName1C { get; private set; }
        internal static string Login1C { get; private set; }
        internal static string Password1C { get; private set; }
    }
}
