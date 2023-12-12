using System;

namespace Classified.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        ClassifiedContext Get();
    }
}
