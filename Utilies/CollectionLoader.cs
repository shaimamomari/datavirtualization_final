using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVirtualization.Utilies
{
    public static class CollectionLoader
    {
        public static ObservableCollection<T> LoadCollection<T>(Func<IEnumerable<T>> loadFunc)
        {
            return new ObservableCollection<T>(loadFunc().ToList());
        }
    }
}
