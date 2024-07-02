namespace Api.Infrastructure.User;

using Core.Infrastructure.User;

public class SystemContext : IUserContext
{
    public string Name => "System";
    public string Id => "System";
    
    public void Reset()
    {
    }
}
