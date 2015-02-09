using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace JunosPolicyViewer
{
    /// <summary>
    /// Interaktionslogik für GroupContainer.xaml
    /// </summary>
    public partial class GroupContainer : UserControl
    {
        private bool expanded = false;

        public GroupContainer(Junos.SecurityPoliciesGroup group)
        {
            this.InitializeComponent();

            this.labelName.Content = group.Name;

            this.policyContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            for (int i = 0; i < group.Policies.Count(); i++)
            {
                this.policyContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(24) });
            }

            int index = 0;
            foreach (var item in group.Policies)
            {
                var element = new Policy(item)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 24
                };

                this.policyContainer.Children.Add(element);
                Grid.SetColumn(element, 0);
                Grid.SetRow(element, index++);
            }

            this.policyContainer.Height = group.Policies.Count() * 24;

            if (group.Policies.All(p => p.Action == group.Policies.First().Action))
            {
                this.Background = new SolidColorBrush(UiHelpers.GetPolicyColor(group.Policies.First()));
            }
        }

        public void Highlight(bool active)
        {
            ((DrawingGroup)((DrawingBrush)this.OuterBorder.BorderBrush).Drawing).Children.OfType<GeometryDrawing>().First().Brush = active
                ? new SolidColorBrush(Color.FromArgb(255, 0, 0, 255))
                : new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.expanded)
            {
                this.Height = this.Height - this.policyContainer.Height;
                this.arrow.Content = "\u25b6";
            }
            else
            {
                this.Height = this.Height + this.policyContainer.Height;
                this.arrow.Content = "\u25bc";
            }

            this.expanded = !this.expanded;
        }
    }
}
