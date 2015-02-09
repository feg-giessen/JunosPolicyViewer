using System.Xml;
using System.Xml.Linq;

namespace JunosPolicyViewer.Junos
{
    internal static class Ns
    {
        public static readonly XNamespace Nc = "urn:ietf:params:xml:ns:netconf:base:1.0";
        
        public static readonly XNamespace Xnm = "http://xml.juniper.net/xnm/1.1/xnm";

        public static XmlNamespaceManager CreateManager()
        {
            var nsManager = new XmlNamespaceManager(new NameTable());
            
            nsManager.AddNamespace("nc", Nc.NamespaceName);
            nsManager.AddNamespace("xnm", Xnm.NamespaceName);

            return nsManager;
        }
    }
}
