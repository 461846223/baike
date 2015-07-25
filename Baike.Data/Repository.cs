using System.Data.Entity.Infrastructure;

namespace Baike.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    using Baike.Repository.Contract;
    using Baike.Repository.Impl;


    /// <summary>
    /// Repository
    /// </summary>
    /// <typeparam name="T">泛型实体</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// CmsContext
        /// </summary>
        private BaikeContext context;

        /// <summary>
        /// DbSet
        /// </summary>
        private DbSet<T> dbset;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">传入CmsContext</param>
        public Repository(BaikeContext context)
        {
            this.context = context;
            this.dbset = context.Set<T>();
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public virtual int Count
        {
            get
            {
                return this.dbset.Count();
            }
        }

        public int GetCount(Expression<Func<T, bool>> filter)
        {
            return this.dbset.Count(filter);
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter)
        {
            return this.dbset.Where(filter).AsQueryable<T>();
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="total">
        /// The total.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50)
        {
            var skipCount = index * size;
            var resetSet = filter != null ? this.dbset.Where(filter).AsQueryable() : this.dbset.AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="total">
        /// The total.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <param name="orderBy">
        /// The order by.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            int skipCount = index * size;
            var resetSet = filter != null ? this.dbset.Where(filter).AsQueryable() : this.dbset.AsQueryable();

            if (orderBy != null)
            {
                resetSet = orderBy(resetSet);
            }

            total = resetSet.Count();

            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
           
            return resetSet.AsQueryable();


            ////int skipCount = index * size;
            ////var resetSet = filter != null ? this.dbset.Where(filter).AsQueryable() : this.dbset.AsQueryable();
            ////resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            ////total = resetSet.Count();
            ////if (orderBy != null)
            ////{
            ////    return orderBy(resetSet).AsQueryable();
            ////}
            ////else
            ////{
            ////    return resetSet.AsQueryable();
            ////}
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="filter">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="includeProperties">包含属性</param>
        /// <returns></returns>
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = this.dbset;

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>dbset</returns>
        public IQueryable<T> GetAll()
        {
            return this.dbset.AsQueryable();
        }

        /// <summary>
        /// 查看是否存在某种条件下记录
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        public bool Contains(Expression<Func<T, bool>> filter)
        {
            return this.dbset.Count(filter) > 0;
        }

        /// <summary>
        /// 根据ID获取一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Find(object id)
        {
            return this.dbset.Find(id);
        }

        /// <summary>
        /// Find object by keys
        /// </summary>
        /// <param name="keys">Specified the search keys</param>
        /// <returns>DbSet</returns>
        public virtual T Find(params object[] keys)
        {
            return this.dbset.Find(keys);
        }

        /// <summary>
        /// 根据条件查找
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns>T</returns>
        public virtual T Find(Expression<Func<T, bool>> filter)
        {
            return this.dbset.FirstOrDefault(filter);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">要插入的实体</param>
        /// <returns>T</returns>
        public virtual T Insert(T entity)
        {
            T newentity = this.dbset.Add(entity);
            this.context.SaveChanges();
            return newentity;
        }

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id">根据ID来删除</param>
        public virtual void Delete(object id)
        {
            T entityToDelete = this.dbset.Find(id);
            this.Delete(entityToDelete);
            this.context.SaveChanges();
        }

        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="entityToDelete">要删除的实体</param>
        public virtual void Delete(T entityToDelete)
        {
            this.dbset.Remove(entityToDelete);
            this.context.SaveChanges();
        }

        /// <summary>        
        /// 根据条件删除.        
        /// </summary>        
        /// <param name="filter">删除条件</param>
        /// <returns>影响行数</returns>
        public virtual int Delete(Expression<Func<T, bool>> filter)
        {
            var objects = this.Get(filter);
            foreach (var obj in objects)
                this.dbset.Remove(obj);
            return this.context.SaveChanges();
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entityToUpdate">要更新的实体</param>
        public virtual void Update(T entityToUpdate)
        {
            try
            {
                this.dbset.Attach(entityToUpdate);
                this.context.Entry(entityToUpdate).State = EntityState.Modified;
                this.context.SaveChanges();
            }
            catch (InvalidOperationException ex)
            {
                var objContext = ((IObjectContextAdapter)this.context).ObjectContext;
                var objSet = objContext.CreateObjectSet<T>();
                var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entityToUpdate);
                Object foundEntity;
                var exits = objContext.TryGetObjectByKey(entityKey, out foundEntity);
                if (exits && this.dbset.Local != null && this.dbset.Local.Contains(foundEntity) &&
                    this.dbset.Local.Any())
                {
                    if (entityKey.EntityKeyValues != null && entityKey.EntityKeyValues.Any())
                    {
                        DbEntityEntry<T> entry =
                            this.context.Entry(this.dbset.Find(entityKey.EntityKeyValues.FirstOrDefault().Value));
                        entry.CurrentValues.SetValues(entityToUpdate);
                    }
                }

                this.context.SaveChanges();
            }
        }
    }
}