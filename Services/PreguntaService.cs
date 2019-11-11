using DevExpress.Xpo;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XpoTutorial;

namespace BlazorServerSideApplication.Services
{

    public class PreguntaService : BaseService {
        public PreguntaService(IDataLayer dataLayer, UnitOfWork modificationUnitOfWork)
            : base(dataLayer, modificationUnitOfWork) { }
        public Task<IQueryable<Pregunta>> Get() {
            var query = (IQueryable<Pregunta>)readUnitOfWork.Query<Pregunta>();
            return Task.FromResult(query);
        }
        public async Task<Pregunta> Add(Dictionary<string, object> values) {
            string json = JsonConvert.SerializeObject(values);
            using(UnitOfWork uow = CreateModificationUnitOfWork()) {
                var newCustomer = JsonPopulateObjectHelper.PopulateObject<Pregunta>(json, uow);
                await uow.CommitChangesAsync();
                return await readUnitOfWork.GetObjectByKeyAsync<Pregunta>(newCustomer.Oid, true);
            }
        }
        public async Task<Pregunta> Update(int oid, Dictionary<string, object> values) {
            string json = JsonConvert.SerializeObject(values);
            using(UnitOfWork uow = CreateModificationUnitOfWork()) {
                var customer = await uow.GetObjectByKeyAsync<Pregunta>(oid);
                JsonPopulateObjectHelper.PopulateObject(json, uow, customer);
                await uow.CommitChangesAsync();
            }
            return await readUnitOfWork.GetObjectByKeyAsync<Pregunta>(oid, true);
        }
        public async Task Delete(int oid) {
            using(UnitOfWork uow = CreateModificationUnitOfWork()) {
                var customer = await uow.GetObjectByKeyAsync<Pregunta>(oid);
                customer.Delete();
                await uow.CommitChangesAsync();
            }
        }
    }
}
