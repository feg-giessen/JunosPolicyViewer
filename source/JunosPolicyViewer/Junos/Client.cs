using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Renci.SshNet;

namespace JunosPolicyViewer.Junos
{
    public class Client
    {
        private const string FilterGroups = "<configuration><groups /></configuration>";

        private const string FilterSecurityZones = "<configuration><security><zones /></security></configuration>";

        private const string FilterSecurityPolicies = "<configuration><security><policies /></security></configuration>";

        public void Load(string host, int port, string user, string password)
        {
            using (var client = new NetConfClient(host, port, user, password))
            {
                client.AutomaticMessageIdHandling = false;
                client.OperationTimeout = new TimeSpan(0, 0, 60);

                client.Connect();

                var nsManager = Ns.CreateManager();
                var rpc = new XmlDocument();

                //
                // Security Zones
                rpc.LoadXml("<rpc><get-config><source><running /></source><filter>" + FilterSecurityZones + "</filter></get-config></rpc>");

                XmlDocument result = client.SendReceiveRpc(rpc);
                XDocument xmlZones = XDocument.Parse(result.OuterXml);

                this.Zones = xmlZones.Root.XPathSelectElements("./nc:data/xnm:configuration/xnm:security/xnm:zones/xnm:security-zone", nsManager).Select(SecurityZone.Parse).ToList();

                //
                // Security Policy Groups
                rpc.LoadXml("<rpc><get-config><source><running /></source><filter>" + FilterGroups + "</filter></get-config></rpc>");

                result = client.SendReceiveRpc(rpc);
                XDocument xmlGroups = XDocument.Parse(result.OuterXml);

                this.Groups = xmlGroups.Root.Descendants(Ns.Xnm + "groups")
                    .Where(x => x.Descendants(Ns.Xnm + "security").SelectMany(y => y.Descendants(Ns.Xnm + "policies")).Any())
                    .Select(SecurityPoliciesGroup.Parse)
                    .ToList();

                //
                // Security Policies
                rpc.LoadXml("<rpc><get-config><source><running /></source><filter>" + FilterSecurityPolicies + "</filter></get-config></rpc>");

                result = client.SendReceiveRpc(rpc);
                XDocument xmlPolicies = XDocument.Parse(result.OuterXml);

                this.Policies = xmlPolicies.Root.XPathSelectElements("./nc:data/xnm:configuration/xnm:security/xnm:policies/xnm:policy", nsManager)
                    .Select(xml => SecurityPolicyContainer.Parse(xml, s => this.Groups.FirstOrDefault(g => g.Name == s) ?? new SecurityPoliciesGroup { Name = s })).ToList();

                // Close
                client.SendCloseRpc();
            }
        }

        public IList<SecurityZone> Zones { get; private set; }

        public IList<SecurityPoliciesGroup> Groups { get; private set; }

        public IList<SecurityPolicyContainer> Policies { get; private set; }
    }
}
