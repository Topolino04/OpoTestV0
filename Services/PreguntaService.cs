using DevExpress.Xpo;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerSideApplication;

namespace BlazorServerSideApplication.Services
{

    public class PreguntaService : BaseService<PlantillaPregunta>
    {
        public PreguntaService(IDataLayer dataLayer, UnitOfWork modificationUnitOfWork) : base(dataLayer, modificationUnitOfWork) { }
    }
}
