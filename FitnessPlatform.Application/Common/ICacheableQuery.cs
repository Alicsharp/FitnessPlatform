using System;
using System.Collections.Generic;

namespace FitnessPlatform.Application.Common
{
    public interface ICacheableQuery
    {
        string CacheKey { get; }
        TimeSpan? Expiration { get; }
    }
}
