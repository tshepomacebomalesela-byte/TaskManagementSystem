using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskApplication.Common.Interfaces
{
    public interface ICachableQuery
    {
        string CacheKey { get; }
        TimeSpan? Expiration { get; }
    }
}
