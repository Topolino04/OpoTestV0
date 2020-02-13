using DevExpress.Xpo;

namespace BlazorServerSideApplication
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
        public XPCollection<PlantillaPregunta> Preguntas => GetCollection<PlantillaPregunta>(nameof(Preguntas));

        [Association]
        public XPCollection<Examen> Examenes => GetCollection<Examen>(nameof(Examenes));

        Tema padre;
        [Association("TemasGerarquia")]
        public Tema Padre
        {
            get => padre;
            set => SetPropertyValue(nameof(Padre), ref padre, value);
        }

        [Association("TemasGerarquia")]
        public XPCollection<Tema> Hijos => GetCollection<Tema>(nameof(Hijos));

    }
}
