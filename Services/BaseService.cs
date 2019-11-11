using DevExpress.Xpo;

namespace BlazorServerSideApplication.Services
{
    public abstract class BaseService {
        readonly IDataLayer dataLayer;
        protected readonly UnitOfWork readUnitOfWork;
        public BaseService(IDataLayer dataLayer, UnitOfWork modificationUnitOfWork) {
            this.dataLayer = dataLayer;
            readUnitOfWork = modificationUnitOfWork;
        }
        protected UnitOfWork CreateModificationUnitOfWork() {
            return new UnitOfWork(dataLayer);
        }
    }
}
