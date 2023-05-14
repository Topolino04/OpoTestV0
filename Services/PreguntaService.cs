using DevExpress.Blazor.Internal;
using DevExpress.Xpo;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OpoTest.Services
{
    public class PreguntaService : XpoService<PlantillaPregunta>
    {
        public PreguntaService(IDataLayer dataLayer) : base(dataLayer)
        {
        }

        public void DuplicarPreguntas(IEnumerable<PlantillaPregunta> preguntas, Tema newTema)
        {
            using (UnitOfWork uow = GetUnitOfWork())
            {
                newTema = uow.GetObjectByKey<Tema>(newTema.Oid);

                foreach (var pregunta in preguntas)
                {
                    var duplicado = new PlantillaPregunta(uow)
                    {
                        Enunciado = pregunta.Enunciado,
                        Explicacion = pregunta.Explicacion,
                        Tema = newTema,
                    };

                    duplicado.Respuestas.AddRange(pregunta.Respuestas.Select(x => new PlantillaRespuesta(uow)
                    {
                        Pregunta = duplicado,
                        Correcta = x.Correcta,
                        Texto = x.Texto,
                        Explicacion = x.Explicacion,
                    }));

                }

                uow.CommitChanges();
            }
        }
    }
}
