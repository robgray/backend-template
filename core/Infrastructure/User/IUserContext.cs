namespace Core.Infrastructure.User;

public interface IUserContext
{
    public string Name { get; }
    
    public string Id { get; }

    public void Reset();
}