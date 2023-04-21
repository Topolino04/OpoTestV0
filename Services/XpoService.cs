using DevExpress.Xpo;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpoTest.Services
{
    public class XpoService<T> where T : PersistentBase
    {
        readonly IDataLayer dataLayer;
        public XpoService(IDataLayer dataLayer)
        {
            this.dataLayer = dataLayer;
        }
        protected UnitOfWork GetUnitOfWork() => new UnitOfWork(dataLayer);

        public Task<IEnumerable<T>> Get(CancellationToken token = default) => GetUnitOfWork().Query<T>().EnumerateAsync(token);

        public Task<IEnumerable<T>> Get(Func<T, bool> expression, CancellationToken token = default) => Task.Run(() => (IEnumerable<T>)GetUnitOfWork().Query<T>().Where(expression).ToArray(), token);
        public Task<T> GetByKey(object key, CancellationToken token = default) => GetUnitOfWork().GetObjectByKeyAsync<T>(key, token);

        public async Task<T> Add(object values, object aggregatedObject = null, CancellationToken token = default)
        {
            string json = JsonConvert.SerializeObject(values);
            using (UnitOfWork uow = GetUnitOfWork())
            {
                var newCustomer = JsonPopulateObjectHelper.PopulateObject<T>(json, uow);
                if (newCustomer is IAggregated aggregated)
                    aggregated.SetAgregation(aggregatedObject);
                await uow.CommitChangesAsync(token);
                return await GetUnitOfWork().GetObjectByKeyAsync<T>(uow.GetKeyValue(newCustomer), true, token);
            }
        }
        public async Task<T> Update(int oid, object values, CancellationToken token = default)
        {
            string json = JsonConvert.SerializeObject(values);
            using (UnitOfWork uow = GetUnitOfWork())
            {
                var customer = await uow.GetObjectByKeyAsync<T>(oid, token);
                JsonPopulateObjectHelper.PopulateObject(json, uow, customer);
                await uow.CommitChangesAsync(token);
            }
            return await GetUnitOfWork().GetObjectByKeyAsync<T>(oid, true, token);
        }
        public async Task Delete(int oid, CancellationToken token = default)
        {
            using (UnitOfWork uow = GetUnitOfWork())
            {
                var obj = await uow.GetObjectByKeyAsync<T>(oid, token);
                uow.Delete(obj);
                await uow.CommitChangesAsync(token);
            }
        }
    }

    internal interface IAggregated
    {
        void SetAgregation(object aggregatedObject);
    }
}
