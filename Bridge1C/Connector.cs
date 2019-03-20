namespace Bridge1C
{
	using V83;
	using NLog;

    public class Connector
    {
        /// <summary>
        /// Инициализирует объект в памяти.
        /// </summary>
        /// <param name="dataBaseFile">Полное имя файла базы данных.</param>
        /// <param name="login">Логин для входа в 1С.</param>
        /// <param name="password">Пароль для входа в 1С.</param>
        /// <param name="poolCapacity">Вместительность пула подключений.</param>
        /// <param name="poolTimeout">Таймаут до отключения при простое.</param>
        /// <param name="maxConnections">Макситмальное количество одновременных подключений.</param>
        public Connector(string dataBaseFile, string login, string password, uint poolCapacity = 10, uint poolTimeout = 60, uint maxConnections = 2)
        {
			this.logger.Info("Инициализация объекта коннектора");
            COMConnector comConnector = new COMConnector();
            string connectionString = string.Format("File = '{0}'; Usr = '{1}'; pwd = '{2}';", dataBaseFile, login, password);
            comConnector.PoolCapacity = poolCapacity;
            comConnector.PoolTimeout = poolTimeout;
            comConnector.MaxConnections = maxConnections;
            this.Connection = comConnector.Connect(connectionString);
			this.logger.Info("Инициализация объекта коннектора завершена");
		}

        public Connector(string connectionString)
        {
			this.logger.Info("Инициализация объекта коннектора");
			COMConnector comConnector = new COMConnector();
            comConnector.PoolCapacity = 10;
            comConnector.PoolTimeout = 60;
            comConnector.MaxConnections = 2;
            this.Connection = comConnector.Connect(connectionString);
			this.logger.Info("Инициализация объекта коннектора завершена");
		}

        /// <summary>
        /// Подключение к базе.
        /// </summary>
        public dynamic Connection { get; private set; }
		private readonly Logger logger = LogManager.GetCurrentClassLogger();
    }


}
