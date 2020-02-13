using DevExpress.Xpo;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerSideApplication.Services
{
    public abstract class BaseService<T> where T : PersistentBase

    {
        readonly IDataLayer dataLayer;
        protected readonly UnitOfWork readUnitOfWork;
        public BaseService(IDataLayer dataLayer, UnitOfWork modificationUnitOfWork)
        {
            this.dataLayer = dataLayer;
            readUnitOfWork = modificationUnitOfWork;
        }
        protected UnitOfWork CreateModificationUnitOfWork()
        {
            return new UnitOfWork(dataLayer);
        }

        public Task<IQueryable<T>> Get() => Task.FromResult((IQueryable<T>)readUnitOfWork.Query<T>());
        public Task<T> GetByKey(object key) => readUnitOfWork.GetObjectByKeyAsync<T>(key);

        public async Task<T> Add(Dictionary<string, object> values)
        {
            string json = JsonConvert.SerializeObject(values);
            using (UnitOfWork uow = CreateModificationUnitOfWork())
            {
                var newCustomer = JsonPopulateObjectHelper.PopulateObject<T>(json, uow);
                await uow.CommitChangesAsync();

                return await readUnitOfWork.GetObjectByKeyAsync<T>(uow.GetKeyValue(newCustomer), true);
            }
        }
        public async Task<T> Update(int oid, Dictionary<string, object> values)
        {
            string json = JsonConvert.SerializeObject(values);
            using (UnitOfWork uow = CreateModificationUnitOfWork())
            {
                var customer = await uow.GetObjectByKeyAsync<T>(oid);
                JsonPopulateObjectHelper.PopulateObject(json, uow, customer);
                await uow.CommitChangesAsync();
            }
            return await readUnitOfWork.GetObjectByKeyAsync<T>(oid, true);
        }
        public async Task Delete(int oid)
        {
            using (UnitOfWork uow = CreateModificationUnitOfWork())
            {
                var customer = await uow.GetObjectByKeyAsync<T>(oid);
                uow.Delete(customer);
                await uow.CommitChangesAsync();
            }
        }
    }
}
