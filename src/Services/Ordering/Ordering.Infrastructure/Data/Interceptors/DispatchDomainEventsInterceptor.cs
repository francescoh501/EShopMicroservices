using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Interceptors;

public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Cerco le entità del dbcontext che hanno implementato l'interfaccia aggregata,
    /// includendo l'elenco degli eventi di dominio e li aggiungo nella variabile 'aggregates'.
    /// Recupero gli eventi di dominio dagli aggregati in 'domainEvents'.
    /// Itero gli eventi del dominio aggregato e cancello tutti gli eventi del dominio.
    /// Infine distribuisco gli eventi tramite mediator.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private async Task DispatchDomainEvents(DbContext? context)
    {
        if (context is null) return;

        // Aggregati con eventi di dominio
        var aggregates = context.ChangeTracker
            .Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity);

        // Eventi di dominio
        var domainEvents = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        // Itero e cancello gli eventi
        aggregates.ToList().ForEach(a => a.ClearDomainEvents());

        // Distribuisco gli eventi tramite mediator
        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}
