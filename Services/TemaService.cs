using DevExpress.Xpo;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XpoTutorial;

namespace BlazorServerSideApplication.Services
{

    public class TemaService : BaseService {
        public TemaService(IDataLayer dataLayer, UnitOfWork modificationUnitOfWork)
            : base(dataLayer, modificationUnitOfWork) { }
        public Task<IQueryable<Tema>> Get() {
            var query = (IQueryable<Tema>)readUnitOfWork.Query<Tema>();
            return Task.FromResult(query);
        }
        public async Task<Tema> Add(Dictionary<string, object> values) {
            string json = JsonConvert.SerializeObject(values);
            using(UnitOfWork uow = CreateModificationUnitOfWork()) {
                var newCustomer = JsonPopulateObjectHelper.PopulateObject<Tema>(json, uow);
                await uow.CommitChangesAsync();
                return await readUnitOfWork.GetObjectByKeyAsync<Tema>(newCustomer.Oid, true);
            }
        }
        public async Task<Tema> Update(int oid, Dictionary<string, object> values) {
            string json = JsonConvert.SerializeObject(values);
            using(UnitOfWork uow = CreateModificationUnitOfWork()) {
                var customer = await uow.GetObjectByKeyAsync<Tema>(oid);
                JsonPopulateObjectHelper.PopulateObject(json, uow, customer);
                await uow.CommitChangesAsync();
            }
            return await readUnitOfWork.GetObjectByKeyAsync<Tema>(oid, true);
        }
        public async Task Delete(int oid) {
            using(UnitOfWork uow = CreateModificationUnitOfWork()) {
                var customer = await uow.GetObjectByKeyAsync<Tema>(oid);
                customer.Delete();
                await uow.CommitChangesAsync();
            }
        }
    }
}
