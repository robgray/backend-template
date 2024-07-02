namespace Core.Infrastructure.Database;

using System;

public interface IAuditable
{
    public string? CreatedBy { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public string? LastModifiedBy { get; set; }
    
    public DateTimeOffset LastModifiedAt { get; set; }
}