using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gest.Interface_SQLiteAccess
{
    public interface ISQLiteAccess
    {
         Task<String> GetAsyncConnection();
    }
}
