using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpoTest
{
    public class Examen : XPObject
    {
        public Examen(Session session) : base(session) { }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (!FechaFin.HasValue && !IsDeleted && Preguntas.All(x => x.Estado.HasValue))
                FechaFin = DateTime.Now;
        }


        private DateTime fechaInicio;
        public DateTime FechaInicio
        {

            get => fechaInicio;
            set => SetPropertyValue(nameof(FechaInicio), ref fechaInicio, value);
        }

        private DateTime? fechaFin;
        public DateTime? FechaFin
        {

            get => fechaFin;
            set => SetPropertyValue(nameof(FechaFin), ref fechaFin, value);
        }

        public int PreguntasCorrectas => Preguntas.Count(x => x.Estado ?? false);
        public int PreguntasIncorrectas => Preguntas.Count(x => !x.Estado ?? false);
        public int PregntasRespondidas => Preguntas.Count(x => x.Estado.HasValue);
        public int PreguntasPendientes => Preguntas.Count(x => !x.Estado.HasValue);
        public int TotalPreguntas => Preguntas.Count;

        [Association]
        public XPCollection<Tema> Temas => GetCollection<Tema>(nameof(Temas));

        [Association]
        public XPCollection<ExamenPregunta> Preguntas => GetCollection<ExamenPregunta>(nameof(Preguntas));

    }
}
