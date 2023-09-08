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
using MoreLinq.Extensions;
using System.Windows;

namespace AHPTest.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {

        public ObservableCollection<RuleModel> Models { get; set; } = new ObservableCollection<RuleModel>()
        {
            new RuleModel(){ Content="Test",Children = new ObservableCollection<RuleModel>(){
                new RuleModel(){Content="12312"}
            } },
            new RuleModel(){ Content="Test"},
            new RuleModel(){ Content="Test"},
            new RuleModel(){ Content="Test"},
            new RuleModel(){ Content="Test"},
        };
        public DelegateCommand<(RuleModel, RuleModel)?> AddCmd { get; set; }
        public DelegateCommand SaveCmd { get; set; }
        public DelegateCommand<RuleModel> DeleteCmd { get; set; }
        readonly string key = "content";
        public MainWindowViewModel()
        {
            Load();
            AddCmd = new DelegateCommand<(RuleModel, RuleModel)?>(Add);
            SaveCmd = new DelegateCommand(Save);
            DeleteCmd = new DelegateCommand<RuleModel>(Delete);
        }

        private void Delete(RuleModel model)
        {
            if (model.Children.Count > 0)
            {
                var res = MessageBox.Show("确定删除", "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (res == MessageBoxResult.OK) return;
            }
            model.Parent?.Children.Remove(model);
            if (model.Parent == null && Models.Contains(model) && Models.Count > 1)
            {
                Models.Remove(model);
            }
        }

        private void Save()
        {
            Config.SetValue(key, Models);
            MessageBox.Show("保存成功!");
        }

        private void Add((RuleModel, RuleModel)? tuple)
        {
            if (tuple == null) return;
            var parent = tuple.Value.Item1;
            var child = tuple.Value.Item2;
            parent.Children.Add(child);
            child.Parent = parent;
        }

        private void Load()
        {
            List<RuleModel> list = Config.GetValue<List<RuleModel>>(key);
            if (list != null)
            {
                list = RuleModel.Format(list);
                Models.Clear();
                Models = new ObservableCollection<RuleModel>(list);
            }
        }

        private void Update()
        {

        }
    }
}
