using AHPTest.Commons;
using AHPTest.Models;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PropertyChanged;
using Prism.Mvvm;
namespace AHPTest.ViewModels
{
    internal class RuleListViewModel : BindableBase
    {
        public ObservableCollection<RuleModel> Models { get; set; }
        public DelegateCommand<(RuleModel, RuleModel)?> AddCmd { get; set; }
        public DelegateCommand SaveCmd { get; set; }
        public DelegateCommand ResetCmd { get; set; }
        public DelegateCommand ImportCmd { get; set; }
        public DelegateCommand SaveAsCmd { get; set; }
        public DelegateCommand RefreshCmd { get; set; }
        public DelegateCommand<RuleModel> DeleteCmd { get; set; }
        readonly string key = AppData.content;
        static RuleModel data = new RuleModel()
        {
            Content = "XXX绿色施工评价指标体系",
            Children = new ObservableCollection<RuleModel>(){
                new RuleModel(){Content="周边环境", Children = new ObservableCollection<RuleModel>(){
                    new RuleModel(){Content="沉降"}
                } },
            }
        };
        public RuleListViewModel()
        {
            Load();
            AddCmd = new DelegateCommand<(RuleModel, RuleModel)?>(Add);
            SaveCmd = new DelegateCommand(Save);
            SaveAsCmd = new DelegateCommand(SaveAs);
            ImportCmd = new DelegateCommand(Import);
            DeleteCmd = new DelegateCommand<RuleModel>(Delete);
            RefreshCmd = new DelegateCommand(Load);
            ResetCmd = new DelegateCommand(Reset);
        }
        #region 配置文件

        private void Import()
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Title = "导入配置文件",
                Filter = "(ahp.json文件)|*.ahp.json",

            };
            if (dialog.ShowDialog() == true)
            {
                // File.Copy(dialog.FileName, Config.ConfigFile, true);
                // 
                using (var fs = File.Open(dialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    StreamReader sr = new StreamReader(fs);
                    var s = sr.ReadToEnd();
                    File.WriteAllText(Config.ConfigFile, s);
                }
                Load();
                AppData.MyEA.GetEvent<UpdateEvent>().Publish();
            }
        }

        private void SaveAs()
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Title = "另存为",
                FileName = $"{Models.First().Content}.ahp.json",
                Filter = "(ahpJson文件)|*.ahp.json",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            if (dialog.ShowDialog() == true)
            {
                Save();
                File.Copy(Config.ConfigFile, dialog.FileName, true);
                MessageBox.Show("导出成功!");
            }
        }


        private void Reset()
        {
            var res = MessageBox.Show("确认清理所有数据?", "重置", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (res == MessageBoxResult.Yes)
            {
                File.Delete(Config.ConfigFile);
                AppData.MyEA.GetEvent<UpdateEvent>().Publish();
            }
        }

        #endregion
        private void Delete(RuleModel model)
        {
            if (Models.Contains(model) && Models.Count == 1) return;
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
            AppData.Rule = Models.First();
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
            }

            Models = new ObservableCollection<RuleModel>(list ?? new List<RuleModel>() { data });
        }
    }
}
