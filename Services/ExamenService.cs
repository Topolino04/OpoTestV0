using DevExpress.Xpo;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpoTest.Services
{
    public class ExamenService : XpoService<Examen>
    {
        public ExamenService(IDataLayer dataLayer) : base(dataLayer) { }

        public async Task<Examen> GenerarExamenFallos(int numeroPreguntas)
        {
            using (UnitOfWork uow = GetUnitOfWork())
            {
                var claves = uow.Query<ExamenRespuesta>()
                       .Where(x => x.Seleccionada.HasValue && x.Seleccionada != x.Correcta)
                       .Select(x => x.Pregunta.Oid)
                       .Distinct()
                       .ToList();
                claves.Shuffle();

                var preguntas = uow.GetObjectsByKey(
                    uow.GetClassInfo<PlantillaPregunta>(),
                    claves.Take(numeroPreguntas).ToList(), true)
                    .OfType<PlantillaPregunta>();

                var result = new Examen(uow);
                result.FechaInicio = DateTime.Now;
                result.Preguntas.AddRange(preguntas.Select(x => x.GenerarExamenPregunta()));
                result.Temas.AddRange(preguntas.Select(x => x.Tema).Distinct());

                await uow.CommitChangesAsync();
                return await GetByKey(result.Oid);
            }
        }
        public async Task<Examen> GenerarExamen(int numeroPreguntas, params int[] keyTemas)
        {
            using (UnitOfWork uow = GetUnitOfWork())
            {
                var temas = keyTemas.Select(x => uow.GetObjectByKey<Tema>(x)).ToList();
                var result = new Examen(uow);
                result.FechaInicio = DateTime.Now;
                result.Temas.AddRange(temas);

                var claves = uow.Query<PlantillaPregunta>()
                    .Where(x => temas.Contains(x.Tema))
                    .Select(x => x.Oid)
                    .ToList();
                claves.Shuffle();


                var preguntas = uow.GetObjectsByKey(
                    uow.GetClassInfo<PlantillaPregunta>(),
                    claves.Take(numeroPreguntas).ToList(), true)
                    .OfType<PlantillaPregunta>();

                result.Preguntas.AddRange(preguntas.Select(x => x.GenerarExamenPregunta()));

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
