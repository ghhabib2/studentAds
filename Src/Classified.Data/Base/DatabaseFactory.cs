namespace Classified.Data.Base
{

/// <summary>
/// Class that make bas for make a connection and close connection to database.
/// </summary>
public class DatabaseFactory : Disposable
{
    /// <summary>
    /// Database Db Context object
    /// </summary>
    protected readonly ApplicationDbContext Context;

    //Open Connection to Database
    public DatabaseFactory()
    {
        //Connect to database as DbContext
        Context=new ApplicationDbContext();
    }

    //Close Connection to Database
    protected override void DisposeCore()
    {
            //Dispose Connection to Database
            Context.Dispose();
    }
}
}
