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
    /// RuleResultView.xaml 的交互逻辑
    /// </summary>
    public partial class RuleResultView : UserControl, IUserControl
    {
        RuleResultViewModel vm;
        public RuleResultView()
        {
            InitializeComponent();
            vm = new RuleResultViewModel();
            this.DataContext = vm;
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
