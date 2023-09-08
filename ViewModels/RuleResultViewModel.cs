using AHPTest.Commons;
using AHPTest.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHPTest.ViewModels
{
    internal class RuleResultViewModel : BindableBase
    {
        public ObservableCollection<RuleModel> Models { get; set; } = new ObservableCollection<RuleModel>();

        public DelegateCommand RefreshCmd { get; set; }
        public DelegateCommand SaveCmd { get; set; }
        readonly string key = AppData.content;
        public RuleResultViewModel()
        {
            RefreshCmd = new DelegateCommand(Load);
            SaveCmd = new DelegateCommand(Save);
            Load();
        }

        private void Save()
        {

        }

        private void Load()
        {
            var models = Config.GetValue<List<RuleModel>>(key);
            Models = new ObservableCollection<RuleModel>(models ?? new List<RuleModel>());
        }
    }
}
