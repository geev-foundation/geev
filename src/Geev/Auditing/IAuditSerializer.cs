namespace Geev.Auditing
{
    public interface IAuditSerializer
    {
        string Serialize(object obj);
    }
}