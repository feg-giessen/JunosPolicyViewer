using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace JunosPolicyViewer.Junos
{
    [DebuggerDisplay("{Name} ({Action})")]
    public class SecurityPolicy
    {
        public string Name { get; set; }

        public IEnumerable<string> SourceAddress { get; set; }

        public IEnumerable<string> DestinationAddress { get; set; }

        public IEnumerable<string> Application { get; set; }

        public PolicyAction Action { get; set; }

        public PolicyLog? Log { get; set; }

        public bool Count { get; set; }

        public static SecurityPolicy Parse(XElement xml)
        {
            if (xml == null || xml.Name.LocalName != "policy")
                return null;

            var match = xml.Element(Ns.Xnm + "match");
            var then = xml.Element(Ns.Xnm + "then");

            return new SecurityPolicy
            {
                Name = xml.Element(Ns.Xnm + "name").Try(x => x.Value),
                SourceAddress = match.Elements(Ns.Xnm + "source-address").Select(x => x.Value).ToList(),
                DestinationAddress = match.Elements(Ns.Xnm + "destination-address").Select(x => x.Value).ToList(),
                Application = match.Elements(Ns.Xnm + "application").Select(x => x.Value).ToList(),
                Count = then.Elements(Ns.Xnm + "count").Any(),
                Action = then.Elements(Ns.Xnm + "deny").Any() ? PolicyAction.Deny 
                             : (then.Elements(Ns.Xnm + "reject").Any() ? PolicyAction.Reject
                                    : (then.Elements(Ns.Xnm + "permit").Any() ? (then.Element(Ns.Xnm + "permit").Elements(Ns.Xnm + "tunnel").Any() ? PolicyAction.PermitIpsec : PolicyAction.Permit) 
                                        : default(PolicyAction))),
            };

        }
    }
}