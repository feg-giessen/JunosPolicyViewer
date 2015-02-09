using System.Diagnostics;
using System.Xml.Linq;

namespace JunosPolicyViewer.Junos
{
    [DebuggerDisplay("{Name}")]
    public class SecurityZone
    {
        public string Name { get; set; }

        public static SecurityZone Parse(XElement xml)
        {
            if (xml == null)
                return null;

            if (xml.Name.LocalName == "security-zone")
            {
                return new SecurityZone
                {
                    Name = xml.Element(Ns.Xnm + "name").Try(x => x.Value)
                };
            }

            return null;
        }
    }
}