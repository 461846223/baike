namespace Baike.Repository
{
    using System;

    using Baike.Entity;
    using Baike.Entity.DBModel;
    using Baike.Repository;
    using Baike.Repository.Contract;
    using Baike.Repository.Impl;

    /// <summary>
    /// UnitOfWork
    /// </summary>
    public class UnitOfWork :  IDisposable
    {

        public UnitOfWork()
        {

        }


        public UnitOfWork(string ConnString)
        {
            context = new BaikeContext(ConnString);
        }

        /// <summary>
        /// The disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The context.
        /// </summary>
        private readonly BaikeContext context = new BaikeContext("name=ConnString");

        /// <summary>
        /// Gets the  web site repository.
        /// </summary>
        public IRepository<WebSite> WebSiteRepository
        {
            get
            {
                return new Repository<WebSite>(this.context);
            }
        }

        /// <summary>
        /// Gets the node repository.
        /// </summary>
        public IRepository<Node> NodeRepository
        {
            get
            {
                return new Repository<Node>(this.context);
            }
        }

        /// <summary>
        /// Gets the content repository.
        /// </summary>
        public IRepository<Content> ContentRepository
        {
            get
            {
                return new Repository<Content>(this.context);
            }
        }

        /// <summary>
        /// Gets the data url history repository.
        /// </summary>
        public IRepository<DataUrlHistory> DataUrlHistoryRepository
        {
            get
            {
                return new Repository<DataUrlHistory>(this.context);
            }
        }


        /// <summary>
        /// Gets the MingxingRepository.
        /// </summary>
        public IRepository<Mingxing> MingxingRepository
        {
            get
            {
                return new Repository<Mingxing>(this.context);
            }
        }


        /// <summary>
        /// Gets the KeywordImgRepository.
        /// </summary>
        public IRepository<KeywordImg> KeywordImgRepository
        {
            get
            {
                return new Repository<KeywordImg>(this.context);
            }
        }

        /// <summary>
        /// 保存数据，用于多表操作操作时，单表操作不需要此方法
        /// </summary>
        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        /// <summary>
        /// 资源回收
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 资源回收
        /// </summary>
        /// <param name="disposing">disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}