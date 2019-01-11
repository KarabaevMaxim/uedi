namespace UniversalEdiModule.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Bridge1C;

    public class ConnectionManager
    {
        public ConnectionManager()
        {
            Connector = new Connector(@"C:\Users\Максим\Documents\InfoBase7", "", "");
        }

        public Connector Connector { get; private set; }
    }
}
