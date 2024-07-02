namespace Vald.TeleHab.Library.DatabaseUpdater;

using DbUp.Engine;

public interface IDbUpgradeEngineFactory
{
    UpgradeEngine Get();
}
