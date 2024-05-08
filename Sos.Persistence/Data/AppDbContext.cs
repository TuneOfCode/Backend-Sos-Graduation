using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Sos.Application.Core.Abstractions.Common;
using Sos.Application.Core.Abstractions.Data;
using Sos.Domain.Core.Abstractions;
using Sos.Domain.Core.Commons.Bases;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Events;
using Sos.Persistence.Extensions;
using System.Reflection;

namespace Sos.Persistence.Data
{
    /// <summary>
    /// Represents the application database context.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </remarks>
    /// <param name="options">The database context options.</param>
    /// <param name="dateTime">The date time.</param>
    /// <param name="mediator">The mediator.</param>
    public sealed class AppDbContext(
        DbContextOptions options,
        IDateTime dateTime,
        IMediator mediator
    ) : DbContext(options), IDbContext, IUnitOfWork
    {
        private readonly IDateTime _dateTime = dateTime;
        private readonly IMediator _mediator = mediator;


        /// <inheritdoc />
        public new DbSet<TEntity> Set<TEntity>()
            where TEntity : Entity
            => base.Set<TEntity>();

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplyUtcDateTimeConverter();

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Saves all of the pending changes in the unit of work.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities that have been saved.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DateTime localNow = _dateTime.LocalNow;

            UpdateAuditableEntities(localNow);

            UpdateSoftDeletableEntities(localNow);

            await PublishDomainEvents(cancellationToken);

            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task<IDbContextTransaction> BeginTransactionAsync(
            CancellationToken cancellationToken = default
        )
            => Database.BeginTransactionAsync(cancellationToken);

        /// <inheritdoc />
        public Task<int> ExecuteSqlAsync(
            string sql,
            IEnumerable<SqlParameter> parameters,
            CancellationToken cancellationToken = default
        )
            => Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);

        /// <inheritdoc />
        public async Task<Maybe<TEntity>> GetBydIdAsync<TEntity>(Guid id) where TEntity : Entity
            => id == Guid.Empty
                ? Maybe<TEntity>.None
                : Maybe<TEntity>.From((await Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id))!);

        /// <inheritdoc />
        public void Insert<TEntity>(TEntity entity) where TEntity : Entity
            => Set<TEntity>().Add(entity);

        /// <inheritdoc />
        public void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities) where TEntity : Entity
            => Set<TEntity>().AddRange(entities);

        /// <inheritdoc />
        void IDbContext.Remove<TEntity>(TEntity entity)
            => Set<TEntity>().Remove(entity);

        /// <summary>
        /// Updates the entities implementing <see cref="IAuditableEntity"/> interface.
        /// </summary>
        /// <param name="localNow">The current date and time in UTC format.</param>
        private void UpdateAuditableEntities(DateTime localNow)
        {
            foreach (EntityEntry<IAuditableEntity> entityEntry in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(nameof(IAuditableEntity.CreatedAt)).CurrentValue = localNow;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(nameof(IAuditableEntity.ModifiedAt)).CurrentValue = localNow;
                }
            }
        }

        /// <summary>
        /// Updates the entities implementing <see cref="ISoftDeletableEntity"/> interface.
        /// </summary>
        /// <param name="localNow">The current date and time in UTC format.</param>
        private void UpdateSoftDeletableEntities(DateTime localNow)
        {
            foreach (EntityEntry<ISoftDeletableEntity> entityEntry
                in ChangeTracker.Entries<ISoftDeletableEntity>())
            {
                if (entityEntry.State != EntityState.Deleted)
                {
                    continue;
                }

                entityEntry.Property(nameof(ISoftDeletableEntity.DeletedAt)).CurrentValue = localNow;

                entityEntry.Property(nameof(ISoftDeletableEntity.Deleted)).CurrentValue = true;

                entityEntry.State = EntityState.Modified;

                UpdateDeletedEntityEntryReferencesToUnchanged(entityEntry);
            }
        }

        /// <summary>
        /// Updates the specified entity entry's referenced entries in the deleted state to the modified state.
        /// This method is recursive.
        /// </summary>
        /// <param name="entityEntry">The entity entry.</param>
        private static void UpdateDeletedEntityEntryReferencesToUnchanged(EntityEntry entityEntry)
        {
            if (!entityEntry.References.Any())
            {
                return;
            }

            foreach (ReferenceEntry referenceEntry
                in entityEntry.References.Where(r => r.TargetEntry!.State == EntityState.Deleted))
            {
                referenceEntry.TargetEntry!.State = EntityState.Unchanged;

                UpdateDeletedEntityEntryReferencesToUnchanged(referenceEntry.TargetEntry);
            }
        }

        /// <summary>
        /// Publishes and then clears all the domain events that exist within the current transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task PublishDomainEvents(CancellationToken cancellationToken)
        {
            List<EntityEntry<AggregateRoot>> aggregateRoots = ChangeTracker
                .Entries<AggregateRoot>()
                .Where(entityEntry => entityEntry.Entity.DomainEvents.Any())
                .ToList();

            List<IDomainEvent> domainEvents = aggregateRoots.SelectMany(entityEntry => entityEntry.Entity.DomainEvents).ToList();

            aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

            IEnumerable<Task> tasks = domainEvents
                .Select(domainEvent => _mediator.Publish(domainEvent, cancellationToken));

            await Task.WhenAll(tasks);
        }
    }
}
