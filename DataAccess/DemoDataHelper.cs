using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorServerSideApplication
{
    public static class DemoDataHelper
    {
        private static readonly string[] firstNames = new string[] {
            "Peter", "Ryan", "Richard", "Tom", "Mark", "Steve",
            "Jimmy", "Jeffrey", "Andrew", "Dave", "Bert", "Mike",
            "Ray", "Paul", "Brad", "Carl", "Jerry" };
        private static readonly string[] lastNames = new string[] {
            "Dolan", "Fischer", "Hamlett", "Hamilton", "Lee",
            "Lewis", "McClain", "Miller", "Murrel", "Parkins",
            "Roller", "Shipman", "Bailey", "Barnes", "Lucas", "Campbell" };
        private static readonly string[] productNames = new string[] {
            "Chai", "Chang", "Aniseed Syrup", "Chef Anton's Cajun Seasoning",
            "Chef Anton's Gumbo Mix", "Grandma's Boysenberry Spread",
            "Uncle Bob's Organic Dried Pears", "Northwoods Cranberry Sauce",
            "Mishi Kobe Niku", "Ikura", "Queso Cabrales", "Queso Manchego La Pastora",
            "Konbu", "Tofu", "Genen Shouyu", "Pavlova", "Alice Mutton",
            "Carnarvon Tigers", "Teatime Chocolate Biscuits",
            "Sir Rodney's Marmalade", "Sir Rodney's Scones",
            "Gustaf's Knäckebröd", "Tunnbröd", "Guaraná Fantástica",
            "NuNuCa Nuß-Nougat-Creme", "Gumbär Gummibärchen",
            "Schoggi Schokolade", "Rössle Sauerkraut", "Thüringer Rostbratwurst",
            "Nord-Ost Matjeshering", "Gorgonzola Telino", "Mascarpone Fabioli",
            "Geitost", "Sasquatch Ale", "Steeleye Stout", "Inlagd Sill",
            "Gravad lax", "Côte de Blaye", "Chartreuse verte",
            "Boston Crab Meat", "Jack's New England Clam Chowder",
            "Singaporean Hokkien Fried Mee", "Ipoh Coffee",
            "Gula Malacca", "Rogede sild", "Spegesild", "Zaanse koeken",
            "Chocolade", "Maxilaku", "Valkoinen suklaa", "Manjimup Dried Apples",
            "Filo Mix", "Perth Pasties", "Tourtière", "Pâté chinois",
            "Gnocchi di nonna Alice", "Ravioli Angelo", "Escargots de Bourgogne",
            "Raclette Courdavault", "Camembert Pierrot", "Sirop d'érable",
            "Tarte au sucre", "Vegie-spread", "Wimmers gute Semmelknödel",
            "Louisiana Fiery Hot Pepper Sauce", "Louisiana Hot Spiced Okra",
            "Laughing Lumberjack Lager", "Scottish Longbreads",
            "Gudbrandsdalsost", "Outback Lager", "Flotemysost",
            "Mozzarella di Giovanni", "Röd Kaviar", "Longlife Tofu",
            "Rhönbräu Klosterbier", "Lakkalikööri", "Original Frankfurter grüne Soße" };
        private static readonly Random Random = new Random(0);

        public static void Seed(UnitOfWork uow)
        {
            int ordersCnt = uow.Query<PlantillaRespuesta>().Count();
            if (ordersCnt > 0) return;

            var temas = new Tema[]
            {
                new Tema(uow) { Nombre = "Tema 1" },
                new Tema(uow) { Nombre = "Tema 2" },
                new Tema(uow) { Nombre = "Tema 3" },
                new Tema(uow) { Nombre = "Tema 4" }
            };

            var names = new KeyValuePair<string, string>[firstNames.Length * lastNames.Length];
            for (int i = 0; i < firstNames.Length * lastNames.Length; i++)
            {
                int j = Random.Next(i + 1);
                names[i] = names[j];
                names[j] = new KeyValuePair<string, string>(firstNames[i / lastNames.Length], lastNames[i % lastNames.Length]);
            }
            foreach (var t in names)
            {
                CreateCustomer(uow, t.Key, t.Value, temas[Random.Next(4)]);
            }

            for (int i = 0; i < 20; i++)
            {
                var result = new Examen(uow);
                result.Temas.Add(temas[i % 4]);
                result.Preguntas.AddRange(
                    uow.Query<PlantillaPregunta>()
                    .InTransaction()
                    .Where(x => x.Tema == temas[i % 4])
                    .Take(5)
                    .Select(x => x.GenerarExamenPregunta()));
                result.FechaInicio = DateTime.Now;
            }
            uow.CommitChanges();
        }

        private static void CreateCustomer(UnitOfWork uow, string firstName, string lastName, Tema tema)
        {
            PlantillaPregunta pregunta = new PlantillaPregunta(uow);
            pregunta.Enunciado = firstName;
            pregunta.Explicacion = lastName;
            pregunta.Tema = tema;

            for (int i = 0; i < 10; i++)
            {
                PlantillaRespuesta order = new PlantillaRespuesta(uow);
                order.Explicacion = productNames[Random.Next(productNames.Length)];
                order.Texto = $"Respuesta  {Random.Next(1000)}";
                order.Correcta = i == 5;
                order.Pregunta = pregunta;
            }
            pregunta.Enunciado = "La respuesta correcta es" + pregunta.Respuestas.First(x => x.Correcta).Texto;
        }
    }
}
