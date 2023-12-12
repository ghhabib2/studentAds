namespace Classified.Data.Infrastructure
{
public class DatabaseFactory : Disposable, IDatabaseFactory
{
    private ClassifiedContext _dataContext;
    public ClassifiedContext Get()
    {
        return _dataContext ?? (_dataContext = new ClassifiedContext());
    }
    protected override void DisposeCore()
    {
        if (_dataContext != null)
            _dataContext.Dispose();
    }
}
}
