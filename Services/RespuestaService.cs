using DevExpress.Xpo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorServerSideApplication;

namespace BlazorServerSideApplication.Services {
    public class RespuestaService : BaseService<PlantillaRespuesta> {
        public RespuestaService(IDataLayer dataLayer, UnitOfWork readUnitOfWork)
            : base(dataLayer, readUnitOfWork) { }
        /*
        public Task<IQueryable<Respuesta>> Get() {
            var query = (IQueryable<Respuesta>)readUnitOfWork.Query<Respuesta>();
            return Task.FromResult(query);
        }
        */
        public Task<IQueryable<PlantillaRespuesta>> GetCustomerOrders(int customerOid) {
            var query = readUnitOfWork.Query<PlantillaRespuesta>().Where(order => order.Pregunta.Oid == customerOid);
            return Task.FromResult(query);
        }
        public async Task<PlantillaRespuesta> Add(Dictionary<string, object> values, int customerOid) {
            string json = JsonConvert.SerializeObject(values);
            using(UnitOfWork uow = CreateModificationUnitOfWork()) {
                var customer = await uow.GetObjectByKeyAsync<PlantillaPregunta>(customerOid);
                var newOrder = JsonPopulateObjectHelper.PopulateObject<PlantillaRespuesta>(json, uow);
                newOrder.Pregunta = customer;
                await uow.CommitChangesAsync();
                return await readUnitOfWork.GetObjectByKeyAsync<PlantillaRespuesta>(newOrder.Oid, true);
            }
        }
        /*
        public async Task<Respuesta> Update(int oid, Dictionary<string, object> values) {
            string json = JsonConvert.SerializeObject(values);
            using(UnitOfWork uow = CreateModificationUnitOfWork()) {
                var order = await uow.GetObjectByKeyAsync<Respuesta>(oid);
                JsonPopulateObjectHelper.PopulateObject(json, uow, order);
                await uow.CommitChangesAsync();
            }
            return await readUnitOfWork.GetObjectByKeyAsync<Respuesta>(oid, true);
        }
        public async Task Delete(int oid) {
            using(UnitOfWork uow = CreateModificationUnitOfWork()) {
                var order = await uow.GetObjectByKeyAsync<Respuesta>(oid);
                order.Delete();
                await uow.CommitChangesAsync();
            }
        }
        */
    }
}
