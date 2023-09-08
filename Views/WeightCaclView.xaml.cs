using AHPTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// WeightCaclView.xaml 的交互逻辑
    /// </summary>
    public partial class WeightCaclView : UserControl, IUserControl
    {
        WeightCaclViewModel vm;
        public WeightCaclView()
        {
            InitializeComponent();
            vm = new WeightCaclViewModel();
            this.DataContext = vm;
            this.Loaded += WeightCaclView_Loaded;
        }

        private void WeightCaclView_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= WeightCaclView_Loaded;
            vm.RefreshCmd.Execute();
        }

        public void Save()
        {
            vm.SaveCmd.Execute();
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            int n = e.Row.GetIndex() + 1;
            for (int i = 0; i <= n; i++)
            {
                dg.Columns[i].Visibility = Visibility.Visible;
            }
        }

        public void Update()
        {
            vm.RefreshCmd.Execute();
        }
    }
}
