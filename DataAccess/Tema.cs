using DevExpress.Xpo;

namespace XpoTutorial
{
    public class Tema : XPObject
    {
        public Tema(Session session) : base(session) { }


        private string nombre;
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        private string explicacion;
        [Size(200)]
        public string Explicacion
        {
            get => explicacion;
            set => SetPropertyValue(nameof(Explicacion), ref explicacion, value);
        }

        [Association]
        public XPCollection<Pregunta> Preguntas => GetCollection<Pregunta>(nameof(Preguntas));
    }
}
