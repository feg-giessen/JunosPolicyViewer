using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using JunosPolicyViewer.Junos;

namespace JunosPolicyViewer
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class PolicyOverviewWindow : Window
    {
        private readonly Client client;

        private readonly Dictionary<string, List<GroupContainer>> groups = new Dictionary<string, List<GroupContainer>>();

        public PolicyOverviewWindow(Client client)
        {
            this.DataContext = this;

            this.InitializeComponent();

            this.client = client;
            this.Draw();
        }

        private void Draw()
        {
            for (int i = 0; i <= this.client.Zones.Count; i++)
            {
                this.mainGrid.ColumnDefinitions.Add(
                    new ColumnDefinition
                    {
                        Width = new GridLength(1, GridUnitType.Star),
                        MinWidth = 120
                    });
                this.mainGrid.RowDefinitions.Add(
                    new RowDefinition
                    {
                        Height = GridLength.Auto
                    });
            }

            var label = new Label();

            this.mainGrid.Children.Add(label);
            label.Content = "From \\ To";
            label.FontWeight = FontWeights.Bold;
            label.Background = new SolidColorBrush(Color.FromArgb(255, 220, 220, 220));
            Grid.SetColumn(label, 0);
            Grid.SetRow(label, 0);

            int index = 1;
            foreach (var zone in this.client.Zones)
            {
                label = new Label();

                this.mainGrid.Children.Add(label);
                label.Content = zone.Name;
                label.FontWeight = FontWeights.Bold;
                label.Background = new SolidColorBrush(Color.FromArgb(255, 220, 220, 220));
                Grid.SetColumn(label, index);
                Grid.SetRow(label, 0);

                label = new Label();

                this.mainGrid.Children.Add(label);
                label.Content = zone.Name;
                label.FontWeight = FontWeights.Bold;
                label.Background = new SolidColorBrush(Color.FromArgb(255, 220, 220, 220));
                Grid.SetColumn(label, 0);
                Grid.SetRow(label, index);

                int toIndex = 1;
                foreach (var toZone in this.client.Zones)
                {
                    var elementGrid = new Grid
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(2)
                    };

                    var policySelection = this.client.Policies.Where(p => p.FromZone == zone.Name && p.ToZone == toZone.Name).ToList();
                    int count = Math.Max(policySelection.Sum(p => p.Policies.Count() + p.AppliedGroups.Count()), 1);

                    elementGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    for (int i = 0; i < count; i++)
                    {
                        elementGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    }

                    int elementIndex = 0;
                    foreach (var policy in policySelection)
                    {
                        UserControl element;

                        foreach (var item in policy.Policies)
                        {
                            element = new Policy(item)
                            {
                                Height = 24,
                                VerticalAlignment = VerticalAlignment.Top,
                                Margin = new Thickness(1)
                            };

                            elementGrid.Children.Add(element);
                            Grid.SetColumn(element, 0);
                            Grid.SetRow(element, elementIndex++);
                        }

                        foreach (var item in policy.AppliedGroups)
                        {
                            string groupName = item.Name;

                            element = new GroupContainer(item)
                            {
                                Height = 24,
                                VerticalAlignment = VerticalAlignment.Top,
                                Margin = new Thickness(1)
                            };

                            element.MouseEnter += (o, args) => this.MouseOverGroup(groupName);
                            element.MouseLeave += (o, args) => this.MouseOutGroup(groupName);

                            elementGrid.Children.Add(element);
                            Grid.SetColumn(element, 0);
                            Grid.SetRow(element, elementIndex++);

                            List<GroupContainer> value;
                            if (this.groups.TryGetValue(groupName, out value))
                            {
                                value.Add((GroupContainer)element);
                            }
                            else
                            {
                                this.groups.Add(item.Name, new List<GroupContainer> { (GroupContainer)element });
                            }
                        }
                    }

                    if (elementGrid.Children.Count == 0)
                    {
                        var element = new Policy(new SecurityPolicy { Name = "-", Action = PolicyAction.Deny })
                        {
                            VerticalAlignment = VerticalAlignment.Stretch,
                            Margin = new Thickness(1)
                        };

                        elementGrid.Children.Add(element);
                        Grid.SetColumn(element, 0);
                        Grid.SetRow(element, elementIndex);
                    }

                    this.mainGrid.Children.Add(elementGrid);
                    Grid.SetColumn(elementGrid, toIndex++);
                    Grid.SetRow(elementGrid, index);
                }

                index++;
            }
        }

        private void MouseOverGroup(string name)
        {
            List<GroupContainer> list;
            if (this.groups.TryGetValue(name, out list))
            {
                foreach (var item in list)
                {
                    item.Highlight(true);
                }
            }
        }

        private void MouseOutGroup(string name)
        {
            List<GroupContainer> list;
            if (this.groups.TryGetValue(name, out list))
            {
                foreach (var item in list)
                {
                    item.Highlight(false);
                }
            }
        }
    }
}