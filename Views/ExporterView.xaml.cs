using AHPTest.Models;
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
    /// ExporterView.xaml 的交互逻辑
    /// </summary>
    public partial class ExporterView : UserControl, IUserControl
    {
        ExporterViewModel vm;
        public ExporterView()
        {
            InitializeComponent();
            vm = new ExporterViewModel();
            this.DataContext = vm;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Property tag = ((FrameworkElement)sender).DataContext as Property;
            if (tag != null)
            {
                vm.SelectedCmd.Execute(tag.Name);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            vm.FormatCmd.Execute();
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
