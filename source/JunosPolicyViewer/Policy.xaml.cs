using System.Windows.Controls;
using System.Windows.Media;
using JunosPolicyViewer.Junos;

namespace JunosPolicyViewer
{
    /// <summary>
    /// Interaktionslogik für Policy.xaml
    /// </summary>
    public partial class Policy : UserControl
    {
        public Policy(SecurityPolicy policy)
        {
            this.InitializeComponent();

            this.labelName.Content = policy.Name;

            this.Background = new SolidColorBrush(UiHelpers.GetPolicyColor(policy));
        }
    }
}
