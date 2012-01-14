namespace AD.ORM.EntityFramework
{
    public interface IHiLoGenerator<out TId>
    {
        TId GetIdentifier();
    }
}