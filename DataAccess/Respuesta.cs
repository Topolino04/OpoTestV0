using DevExpress.Xpo;

namespace XpoTutorial
{
    public class Respuesta : XPObject
    {
        public Respuesta(Session session) : base(session) { }

        private Pregunta pregunta;
        [Association]
        [Aggregated]
        public Pregunta Pregunta
        {
            get => pregunta;
            set => SetPropertyValue(nameof(Pregunta), ref pregunta, value);
        }

        private decimal texto;
        public decimal Texto
        {
            get => texto;
            set => SetPropertyValue(nameof(Texto), ref texto, value);
        }

        private bool correcta;
        public bool Correcta
        {
            get => correcta;
            set => SetPropertyValue(nameof(Correcta), ref correcta, value);
        }

        private string explicacion;
        [Size(200)]
        public string Explicacion
        {
            get => explicacion;
            set => SetPropertyValue(nameof(Explicacion), ref explicacion, value);
        }
    }
}
