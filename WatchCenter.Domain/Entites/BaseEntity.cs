using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchCenter.Domain.Entites
{
    public abstract class BaseEntity
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
    }

}
