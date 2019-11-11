using DevExpress.Xpo;

namespace XpoTutorial
{
    public class Pregunta : XPObject
    {
        public Pregunta(Session session) : base(session) { }

        private Tema tema;
        [Association]
        public Tema Tema
        {
            get => tema;
            set => SetPropertyValue(nameof(Tema), ref tema, value);
        }

        private string enunciado;
        public string Enunciado
        {
            get { return enunciado; }
            set { SetPropertyValue(nameof(Enunciado), ref enunciado, value); }
        }

        private string explicacion;
        public string Explicacion
        {
            get { return explicacion; }
            set { SetPropertyValue(nameof(Explicacion), ref explicacion, value); }
        }

        [Association()]
        public XPCollection<Respuesta> Respuestas => GetCollection<Respuesta>(nameof(Respuestas));
    }
}
