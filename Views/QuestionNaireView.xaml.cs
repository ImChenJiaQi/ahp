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
using System.Windows.Shapes;

namespace AHPTest.Views
{
    /// <summary>
    /// QuestionNaireView.xaml 的交互逻辑
    /// </summary>
    public partial class QuestionNaireView : Window
    {
        QuestionNaireViewModel vm;
        public QuestionNaireView()
        {
            InitializeComponent();
            vm = new QuestionNaireViewModel();
            this.DataContext = vm;
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            vm.PreviousCmd.Execute();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            string content = ((Button)sender).Content as string;
            if (content == "完成")
            {
                vm.NextCmd.Execute();
                vm.SaveCmd.Execute();
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                vm.NextCmd.Execute();
            }
        }
    }
}
