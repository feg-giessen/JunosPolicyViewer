using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace JunosPolicyViewer.Junos
{
    [DebuggerDisplay("{Name}, Zones: {FromZone} -> {ToZone}")]
    public class SecurityPoliciesGroup
    {
        public string Name { get; set; }

        public string FromZone { get; set; }

        public string ToZone { get; set; }

        public IEnumerable<SecurityPolicy> Policies { get; set; }

        public static SecurityPoliciesGroup Parse(XElement xml)
        {
            if (xml == null || xml.Name.LocalName != "groups")
                return null;

            var policy = xml.Descendants(Ns.Xnm + "policies").Single().Element(Ns.Xnm + "policy");

            return new SecurityPoliciesGroup
            {
                Name = xml.Element(Ns.Xnm + "name").Try(x => x.Value),
                FromZone = policy.Element(Ns.Xnm + "from-zone-name").Try(x => x.Value.Trim('<', '>')),
                ToZone = policy.Element(Ns.Xnm + "to-zone-name").Try(x => x.Value.Trim('<', '>')),
                Policies = policy.Elements(Ns.Xnm + "policy").Select(SecurityPolicy.Parse).ToList()
            };
        }
    }
}