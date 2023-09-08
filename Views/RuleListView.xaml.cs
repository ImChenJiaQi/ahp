using AHPTest.Models;
using AHPTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AHPTest.Views
{
    /// <summary>
    /// RuleListView.xaml 的交互逻辑
    /// </summary>
    public partial class RuleListView : UserControl, IUserControl
    {
        RuleListViewModel vm;
        public RuleListView()
        {
            InitializeComponent();
            vm = new RuleListViewModel();
            this.DataContext = vm;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string content = ((FrameworkElement)sender).Tag as string;
            var old = ((FrameworkElement)sender).DataContext as RuleModel;
            if (!string.IsNullOrEmpty(content) && old != null)
            {
                vm.AddCmd.Execute((old, new RuleModel() { Content = content }));
                ((FrameworkElement)sender).Tag = null;
                if (((FrameworkElement)sender).Parent is Grid grid
                    && grid.Parent is Grid grid2
                    && grid2.Children.Cast<FrameworkElement>().First(x => x.Name == "tbn") is ToggleButton tbn)
                {
                    tbn.IsChecked = false;
                    //grid.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender) is Grid grid
                   && grid.Parent is Grid grid2
                   && grid2.Children.Cast<FrameworkElement>().First(x => x.Name == "tbn") is ToggleButton tbn)
            {
                var btn = grid.Children.OfType<FrameworkElement>().Last();
                Add_Click(btn, e);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var model = ((FrameworkElement)sender).DataContext as RuleModel;
            if (model != null)
            {
                vm.DeleteCmd.Execute(model);
            }
        }

        public void Save()
        {
            vm.SaveCmd.Execute();
        }

        public void Update()
        {
            vm.RefreshCmd.Execute();
        }
    }
}
