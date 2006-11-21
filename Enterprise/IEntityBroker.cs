using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ClearCanvas.Enterprise
{
    /// <summary>
    /// Used by <see cref="IEntityBroker.Load"/> to provide fine control over the loading of entities.
    /// </summary>
    [Flags]
    public enum EntityLoadFlags
    {
        /// <summary>
        /// Default value
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// Forces a version check, causing an exception to be thrown if the version does not match
        /// </summary>
        CheckVersion = 0x0001,

        /// <summary>
        /// Asks for a proxy to the entity, rather than loading the full entity.  There is no guarantee
        /// that this flag will be obeyed, because the underlying implementation may not support proxies,
        /// or the entity may not be proxiable.
        /// </summary>
        Proxy = 0x0002,
    }


    /// <summary>
    /// Base interface for all entity broker interfaces.  An entity broker allows entities to be retrieved
    /// from persistent storage. This interface should not be implemented directly.
    /// Instead, a sub-interface should be defined that extends this interface for a given entity.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="Entity"/> sub-class on which this broker acts</typeparam>
    /// <typeparam name="TSearchCriteria">The <see cref="SearchCriteria"/> subclass corresponding to the entity</typeparam>
    public interface IEntityBroker<TEntity, TSearchCriteria> : IPersistenceBroker
        where TEntity: Entity
        where TSearchCriteria : SearchCriteria
    {
        /// <summary>
        /// Loads the entity referred to by the specified entity reference.
        /// </summary>
        /// <param name="entityRef"></param>
        /// <returns></returns>
        TEntity Load(EntityRef<TEntity> entityRef);

        /// <summary>
        /// Loades the entity referred to by the specified reference, obeying the specified flags
        /// where possible.
        /// </summary>
        /// <param name="entityRef"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        TEntity Load(EntityRef<TEntity> entityRef, EntityLoadFlags flags);

        /// <summary>
        /// Retrieves all entities matching the specified criteria.  Caution: this method may return an arbitrarily large
        /// result set.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IList<TEntity> Find(TSearchCriteria criteria);

        /// <summary>
        /// Retrieves all entities matching the specified criteria,
        /// constrained by the specified page constraint.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IList<TEntity> Find(TSearchCriteria criteria, SearchResultPage page);

        /// <summary>
        /// Retrieves the entire set of entities of this class.  Caution: this method may return an arbitrarily large
        /// result set.
        /// </summary>
        /// <returns></returns>
        IList<TEntity> FindAll();


        /// <summary>
        /// Returns the number of entities matching the specified criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        long Count(TSearchCriteria criteria);

        /// <summary>
        /// Makes the specified entity transient (removes it from the persistent store).
        /// </summary>
        /// <param name="entity">The entity instance to remove</param>
        void Delete(TEntity entity);
    }
}
