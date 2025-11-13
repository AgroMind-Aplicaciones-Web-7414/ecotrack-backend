namespace EcotrackPlatform.API.Iam.Domain.Model.Aggregates;

public class AuthSession
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public int ProfileId { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; private set; }
    public bool Revoked { get; private set; }
    public string? UserAgent { get; private set; }
    public string? IpAddress { get; private set; }

    protected AuthSession() { }

    public AuthSession(int profileId, TimeSpan ttl, string? userAgent, string? ip)
    {
        ProfileId = profileId;
        ExpiresAt = DateTime.UtcNow.Add(ttl);
        UserAgent = userAgent;
        IpAddress = ip;
    }

    public bool IsActive() => !Revoked && DateTime.UtcNow < ExpiresAt;

    public void Revoke() => Revoked = true;
}