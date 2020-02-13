using DevExpress.Xpo;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerSideApplication;
using System;

namespace BlazorServerSideApplication.Services
{

    public class ExamenService : BaseService<Examen>
    {
        public ExamenService(IDataLayer dataLayer, UnitOfWork modificationUnitOfWork) : base(dataLayer, modificationUnitOfWork) { }


        public async Task<Examen> GenerarExamen(int numperoPreguntas, params int[] keyTemas)
        {
            using (UnitOfWork uow = CreateModificationUnitOfWork())
            {
                var temas = keyTemas.Select(x => uow.GetObjectByKey<Tema>(x)).ToList();
                var result = new Examen(uow);
                result.FechaInicio = DateTime.Now;
                result.Temas.AddRange(temas);
                result.Preguntas.AddRange(
                    uow.Query<PlantillaPregunta>()
                    .Where(x => temas.Contains(x.Tema))
                    .Take(numperoPreguntas)
                    .Select(x => x.GenerarExamenPregunta())
                );
                await uow.CommitChangesAsync();
                return await GetByKey(result.Oid);
            }
        }
        public async Task<Examen> RepetirExamen(int oid)
        {
            using (UnitOfWork uow = CreateModificationUnitOfWork())
            {
                Examen old = uow.GetObjectByKey<Examen>(oid);
                var result = new Examen(uow);
                result.Temas.AddRange(old.Temas);
                result.FechaInicio = DateTime.Now;
                result.Preguntas.AddRange(old.Preguntas.Select(x=> x.Plantilla.GenerarExamenPregunta()));
                await uow.CommitChangesAsync();
                return await GetByKey(result.Oid);
            }
        }

        public async Task<ExamenPregunta> ResolverPregunta(int preguntaOid, params int[] repuestasSelecciondas)
        {
            using (UnitOfWork uow = CreateModificationUnitOfWork())
            {
                var pregunta = uow.GetObjectByKey<ExamenPregunta>(preguntaOid);

                foreach (ExamenRespuesta respuesta in pregunta.Respuestas)
                    respuesta.Seleccionada = repuestasSelecciondas.Contains(respuesta.Oid);
                await uow.CommitChangesAsync();
                return await uow.GetObjectByKeyAsync<ExamenPregunta>(preguntaOid);
            }
        }
    }
}
