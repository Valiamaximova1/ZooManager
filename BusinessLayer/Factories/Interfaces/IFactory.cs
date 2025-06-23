using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Factories.Interfaces
{
    public interface IFactory<T>
    {
        T Create();
    }

}
