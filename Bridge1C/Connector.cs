using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge1C
{
    using V83;

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
            COMConnector comConnector = new COMConnector();
            string connectionString = string.Format("File = '{0}'; Usr = '{1}'; pwd = '{2}';", dataBaseFile, login, password);
            comConnector.PoolCapacity = poolCapacity;
            comConnector.PoolTimeout = poolTimeout;
            comConnector.MaxConnections = maxConnections;
            this.Connection = comConnector.Connect(connectionString);
        }

        public Connector(string connectionString)
        {
            COMConnector comConnector = new COMConnector();
            comConnector.PoolCapacity = 10;
            comConnector.PoolTimeout = 60;
            comConnector.MaxConnections = 2;
            this.Connection = comConnector.Connect(connectionString);
        }

        /// <summary>
        /// Подключение к базе.
        /// </summary>
        public dynamic Connection { get; private set; }
    }


}
