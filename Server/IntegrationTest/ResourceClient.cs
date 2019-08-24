namespace todoweb.Server.IntegrationTest
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using todoweb.Client.Models.Contract;

    public class ResourceClient<TClientResource>
                : IResourceClient<TClientResource>
                where TClientResource : IClientResource
    {
        private Type clientType_;
        protected object resourceClient_;

        public ResourceClient(object resourceClient)
        {
            this.resourceClient_ = resourceClient;
            this.clientType_ = resourceClient.GetType();
        }

        public virtual async Task<TClientResource> CreateAsync(TClientResource resource)
        {
            var methodInfo = this.clientType_.GetMethod("CreateAsync", new Type[] { typeof(TClientResource) });
            return await (Task<TClientResource>)methodInfo.Invoke(this.resourceClient_, new object[] { resource });
        }

        public async Task<TClientResource> CreateOrUpdateAsync(string id, TClientResource resource)
        {
            var methodInfo = this.clientType_.GetMethod("CreateOrUpdateAsync", new Type[] { typeof(string), typeof(TClientResource) });
            return await (Task<TClientResource>)methodInfo.Invoke(this.resourceClient_, new object[] { id, resource });
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var methodInfo = this.clientType_.GetMethod("DeleteAsync", new Type[] { typeof(string) });
            return await (Task<bool>)methodInfo.Invoke(this.resourceClient_, new object[] { id });
        }

        public async Task<TClientResource> GetAsync(string id)
        {
            var methodInfo = this.clientType_.GetMethod("GetAsync", new Type[] { typeof(string) });
            return await (Task<TClientResource>)methodInfo.Invoke(this.resourceClient_, new object[] { id });

        }

        public async Task<IEnumerable<TClientResource>> GetAllAsync()
        {
            var methodInfo = this.clientType_.GetMethod("GetAllAsync", new Type[] { });
            return await (Task<ICollection<TClientResource>>)methodInfo.Invoke(this.resourceClient_, null);
        }
    }
}
