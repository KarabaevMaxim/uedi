using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdiModule
{
    using System.Runtime.InteropServices;

    [Guid("6A2B049B-7FEE-4F89-BE73-A59A06C7E07C")]
    public interface IEntryPoint
    {
        void MainWindowShow();
    }
}
