using System.Windows.Media;
using JunosPolicyViewer.Junos;

namespace JunosPolicyViewer
{
    internal static class UiHelpers
    {
        public static Color GetPolicyColor(SecurityPolicy policy)
        {
            switch (policy.Action)
            {
                case PolicyAction.Deny:
                    return Color.FromArgb(50, 255, 0, 0);

                case PolicyAction.Reject:
                    return Color.FromArgb(50, 255, 120, 0);

                case PolicyAction.Permit:
                    return Color.FromArgb(80, 80, 255, 0);

                case PolicyAction.PermitIpsec:
                    return Color.FromArgb(40, 100, 200, 0);
            }

            return Color.FromArgb(0, 0, 0, 0);
        }
    }
}
