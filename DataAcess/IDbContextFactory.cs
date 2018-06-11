using DataAcess;

namespace DataAccess
{
    public interface IDbContextFactory
    {
        string TenantName { get; set; }

        CRMContext Create();
    }
}