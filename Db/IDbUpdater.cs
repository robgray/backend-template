namespace Vald.TeleHab.Library.DatabaseUpdater;

public interface IDbUpdater
{
    Task<int> PerformUpdate();
}