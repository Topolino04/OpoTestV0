using DevExpress.Xpo;
using System.Linq;

namespace BlazorServerSideApplication
{
    public class ExamenPregunta : XPObject
    {
        public ExamenPregunta(Session session) : base(session) { }


        private Examen examen;
        [Association]
        [Aggregated]
        public Examen Examen
        {
            get => examen;
            set => SetPropertyValue(nameof(Examen), ref examen, value);
        }

        private PlantillaPregunta plantilla;
        public PlantillaPregunta Plantilla
        {
            get => plantilla;
            set => SetPropertyValue(nameof(Plantilla), ref plantilla, value);
        }

        private string enunciado;
        public string Enunciado
        {
            get => enunciado;
            set => SetPropertyValue(nameof(Enunciado), ref enunciado, value);
        }

        private string explicacion;
        public string Explicacion
        {
            get => explicacion;
            set => SetPropertyValue(nameof(Explicacion), ref explicacion, value);
        }

        public bool? Estado => Respuestas.Any(x => x.Seleccionada == null) ? (bool?)null : Respuestas.All(x => x.Seleccionada == x.Correcta);

        [Association]
        public XPCollection<ExamenRespuesta> Respuestas => GetCollection<ExamenRespuesta>(nameof(Respuestas));

    }
}
