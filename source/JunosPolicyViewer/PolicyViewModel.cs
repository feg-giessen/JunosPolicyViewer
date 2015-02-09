using System.Collections.Generic;
using System.Dynamic;

namespace JunosPolicyViewer
{
    public class PolicyRowViewModel : DynamicObject
    {
        private Dictionary<string, object> data = new Dictionary<string, object>();

        public string FromZone { get; set; }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (data.ContainsKey(binder.Name))
            {
                data[binder.Name] = value;
            }
            else
            {
                data.Add(binder.Name, value);
            }

            return true;
        }

        /// <summary>
        /// Stellt die Implementierung für Vorgänge bereit, die Memberwerte abrufen.Von der <see cref="T:System.Dynamic.DynamicObject"/>-Klasse abgeleitete Klassen können diese Methode überschreiben, um dynamisches Verhalten für Vorgänge wie das Abrufen eines Werts für eine Eigenschaft anzugeben.
        /// </summary>
        /// <returns>
        /// true, wenn der Vorgang erfolgreich ist, andernfalls false.Wenn die Methode false zurückgibt, wird das Verhalten vom Laufzeitbinder der Sprache bestimmt.(In den meisten Fällen wird eine Laufzeitausnahme ausgelöst.)
        /// </returns>
        /// <param name="binder">Stellt Informationen zum Objekt bereit, das den dynamischen Vorgang aufgerufen hat.Die binder.Name-Eigenschaft gibt den Namen des Members an, für den der dynamische Vorgang ausgeführt wird.Für die Console.WriteLine(sampleObject.SampleProperty)-Anweisung, in der sampleObject eine von der <see cref="T:System.Dynamic.DynamicObject"/>-Klasse abgeleitete Instanz der Klasse ist, gibt binder.Name z. B. "SampleProperty" zurück.Die binder.IgnoreCase-Eigenschaft gibt an, ob der Membername die Groß-/Kleinschreibung berücksichtigt.</param><param name="result">Das Ergebnis des get-Vorgangs.Wenn die Methode z. B. für eine Eigenschaft aufgerufen wird, können Sie <paramref name="result"/> den Eigenschaftswert zuweisen.</param>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return this.data.TryGetValue(binder.Name, out result);
        }
    }

    public class PolicyViewModel
    {
        public string Policies { get; set; }
    }
}
