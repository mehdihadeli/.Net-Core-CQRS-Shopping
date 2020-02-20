using System;
using System.Threading.Tasks;
using Shopping.Infrastructure.Persistence;

namespace Shopping.Tests.UnitTests.Common
{
    public abstract class UnitTestBase: IDisposable
    {
        private ShoppingDbContext _shoppingDbContext;
        private ApplicationIdentityDbContext _applicationIdentityDbContext;

        protected  ShoppingDbContext ShoppingDbContext
        {
            get 
            {
                if (_shoppingDbContext == null)
                {
                    _shoppingDbContext = ShoppingContextFactory.Create();
                    LoadShoppingTestDataAsync().Wait();
                }

                return _shoppingDbContext;
            }
        }

        protected  ApplicationIdentityDbContext ApplicationIdentityDbContext
        {
            get 
            {
                if (_applicationIdentityDbContext == null)
                {
                    _applicationIdentityDbContext = ApplicationIdentityContextFactory.Create();
                    LoadIdentityTestDataAsync().Wait();
                }

                return _applicationIdentityDbContext;
            }
        }

        /// <summary>
        /// Override this method to load test data
        /// into the in-memory database context prior
        /// to any tests being executed in your
        /// test class.
        /// FRAGILE: this method can't be called from the constructor so you must call it manually
        /// </summary>
        protected virtual async Task LoadShoppingTestDataAsync()
        {
            await Task.Delay(0);
        }
        
        
        /// <summary>
        /// Override this method to load test data
        /// into the in-memory database context prior
        /// to any tests being executed in your
        /// test class.
        /// FRAGILE: this method can't be called from the constructor so you must call it manually
        /// </summary>
        protected virtual async Task LoadIdentityTestDataAsync()
        {
            await Task.Delay(0);
        }
        

        public void Dispose()
        {
            if (_shoppingDbContext != null)
                ShoppingContextFactory.Destroy(_shoppingDbContext);
            
            if (_applicationIdentityDbContext != null)
                ApplicationIdentityContextFactory.Destroy(_applicationIdentityDbContext);
        }
    }
}