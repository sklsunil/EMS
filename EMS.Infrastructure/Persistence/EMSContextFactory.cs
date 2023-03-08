namespace EMS.Infrastructure.Persistence;

public class EMSContextFactory<TContext> : IDbContextFactory<TContext> where TContext : DbContext
{
    private readonly IServiceProvider provider;

    public EMSContextFactory(IServiceProvider provider)
    {
        this.provider = provider;
    }

    public TContext CreateDbContext()
    {
        if (provider == null)
        {
            throw new InvalidOperationException(
                $"You must configure an instance of IServiceProvider");
        }

        return ActivatorUtilities.CreateInstance<TContext>(provider);
    }
}

