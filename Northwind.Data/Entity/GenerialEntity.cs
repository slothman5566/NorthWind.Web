using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Data.Entity
{
    public abstract class GenerialEntity <T>:BaseEntity, IEntity<T>
    {
        public virtual T ID { get; set; }
    
    }
}
