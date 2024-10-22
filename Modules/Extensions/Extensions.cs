using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVirtualization.Modules
{
    public static class Extensions
    {
        public static T Nvl<T>(this object obj, T defaultValue)
        {
            return obj != null ? (T)Convert.ChangeType(obj, typeof(T)) : defaultValue;
        }
    }

}
