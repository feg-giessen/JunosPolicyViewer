using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace JunosPolicyViewer.Junos
{
    [DebuggerDisplay("{FromZone} -> {ToZone}")]
    public class SecurityPolicyContainer
    {

        public string FromZone { get; set; }

        public string ToZone { get; set; }

        public IEnumerable<SecurityPoliciesGroup> AppliedGroups { get; set; }

        public IEnumerable<SecurityPolicy> Policies { get; set; }

        public static SecurityPolicyContainer Parse(XElement xml, Func<string, SecurityPoliciesGroup> groupResolver)
        {
            if (xml == null || xml.Name.LocalName != "policy")
                return null;

            return new SecurityPolicyContainer
            {
                FromZone = xml.Element(Ns.Xnm + "from-zone-name").Try(x => x.Value.Trim('<', '>')),
                ToZone = xml.Element(Ns.Xnm + "to-zone-name").Try(x => x.Value.Trim('<', '>')),
                AppliedGroups = xml.Elements(Ns.Xnm + "apply-groups").Select(e => groupResolver(e.Value)).ToList(),
                Policies = xml.Elements(Ns.Xnm + "policy").Select(SecurityPolicy.Parse).ToList()
            };
        }
    }
}