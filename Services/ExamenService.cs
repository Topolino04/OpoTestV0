using DevExpress.Xpo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpoTest.Services
{
    public class ExamenService : XpoService<Examen>
    {
        public ExamenService(IDataLayer dataLayer) : base(dataLayer) { }

        public async Task<Examen> GenerarExamen(int numperoPreguntas, params int[] keyTemas)
        {
            using (UnitOfWork uow = GetUnitOfWork())
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
            using (UnitOfWork uow = GetUnitOfWork())
            {
                Examen old = uow.GetObjectByKey<Examen>(oid);
                var result = new Examen(uow);
                result.Temas.AddRange(old.Temas);
                result.FechaInicio = DateTime.Now;
                result.Preguntas.AddRange(old.Preguntas.Select(x => x.Plantilla.GenerarExamenPregunta()));
                await uow.CommitChangesAsync();
                return await GetByKey(result.Oid);
            }
        }
        public async Task<ExamenPregunta> ResolverPregunta(int preguntaOid, params int[] repuestasSelecciondas)
        {
            using (UnitOfWork uow = GetUnitOfWork())
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
