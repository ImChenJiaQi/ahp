using AHPTest.Commons;
using AHPTest.Models;
using AHPTest.ViewModels;
using AHPTest.Views;
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

namespace AHPTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new MainWindowViewModel();
            AppData.MyEA.GetEvent<UpdateEvent>().Subscribe(Update);
            this.DataContext = vm;

        }
        void Init()
        {
            var list = Config.GetValue<List<RuleModel>>(AppData.content);
            if (list != null && list.Count > 0)
                AppData.Rule = list[0];
        }

        private void Update()
        {
            Init();
            foreach (TabItem item in tab.Items)
            {       
                if (item.Content is IUserControl control)
                {
                    control.Update();
                }
            }
        }
    }
}
