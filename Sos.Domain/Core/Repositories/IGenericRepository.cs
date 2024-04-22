using Sos.Domain.Core.Commons.Bases;

namespace Sos.Domain.Core.Repositories
{
    /// <summary>
    /// Represents the interface for generic repository with the most common repository methods.
    /// </summary>
    public interface IGenericRepository<TEntity>
        where TEntity : Entity
    {
        /// <summary>
        /// Inserts the specified entity into the database.
        /// </summary>
        /// <param name="entity">The entity to be inserted into the database.</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Inserts the specified entities to the database.
        /// </summary>
        /// <param name="entities">The entities to be inserted into the database.</param>
        void InsertRange(IReadOnlyCollection<TEntity> entities);

        /// <summary>
        /// Updates the specified entity in the database.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Updates the specified entities to the database.
        /// </summary>
        /// <param name="entities">The entities to be inserted into the database.</param>
        public void UpdateRange(IReadOnlyCollection<TEntity> entities);

        /// <summary>
        /// Removes the specified entity from the database.
        /// </summary>
        /// <param name="entity">The entity to be removed from the database.</param>
        void Remove(TEntity entity);
    }
}
