using AHPTest.Commons;
using AHPTest.Models;
using AHPTest.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using System.Windows;

namespace AHPTest.ViewModels
{

    internal class ExporterViewModel : BindableBase
    {
        public List<Property> Properties { get; set; }
        public List<Property> Methods { get; set; }
        public bool IsLeftShowed { get; set; } = true;
        public bool IsEvenShowed { get; set; } = true;
        static Dictionary<string, List<Property>> dict = new Dictionary<string, List<Property>>()
        {
            {"三级标度",new List<Property>(){
                new Property("同等重要"){Value = "1"},
                new Property("稍重要"){Value = "2"},
                new Property("非常重要"){Value = "3"},
                new Property("稍不重要"){Value = "1/2"},
                new Property("非常不重要"){Value = "1/3"},
            }},
            {"五级标度",new List<Property>(){
                new Property("同等重要"){Value = "1"},
                new Property("稍重要"){Value = "2"},
                new Property("明显重要"){Value = "3"},
                new Property("强烈重要"){Value = "4"},
                new Property("绝对重要"){Value = "5"},
                new Property("稍不重要"){Value = "1/2"},
                new Property("明显不重要"){Value ="1/3"},
                new Property("强烈不重要"){Value ="1/4"},
                new Property("绝对不重要"){Value ="1/5"},
            }},
            {"七级标度",new List<Property>(){
                new Property("同等重要"){Value = "1"},
                new Property("稍重要"){Value = "2"},
                new Property("明显重要"){Value = "3"},
                new Property("更明显重要"){Value = "4"},
                new Property("强烈重要"){Value = "5"},
                new Property("更强烈重要"){Value = "6"},
                new Property("绝对重要"){Value = "7"},
                new Property("稍不重要"){Value = "1/2"},
                new Property("明显不重要"){Value ="1/3"},
                new Property("更明显不重要"){Value ="1/4"},
                new Property("强烈不重要"){Value ="1/5"},
                new Property("更强烈不重要"){Value ="1/6"},
                new Property("绝对不重要"){Value ="1/7"},
            }},
            {"九级标度",new List<Property>(){
                new Property("同等重要"){Value = "1"},
                new Property("略重要"){Value = "2"},
                new Property("稍重要"){Value = "3"},
                new Property("更稍重要"){Value = "4"},
                new Property("明显重要"){Value = "5"},
                new Property("更明显重要"){Value = "6"},
                new Property("强烈重要"){Value = "7"},
                new Property("更强烈重要"){Value = "8"},
                new Property("绝对重要"){Value = "9"},
                new Property("略不重要"){Value = "1/2"},
                new Property("稍不重要"){Value = "1/3"},
                new Property("更不重要"){Value = "1/4"},
                new Property("明显不重要"){Value = "1/5"},
                new Property("明显更不重要"){Value = "1/6"},
                new Property("强烈重要"){Value = "1/7"},
                new Property("强烈更不重要"){Value = "1/8"},
                new Property("绝对不重要"){Value = "1/9"},
            }},
        };
        public List<WeightModel> Models { get; set; }


        List<Property> list;
        public DelegateCommand CalcCmd { get; set; }
        public DelegateCommand<string> SelectedCmd { get; set; }
        public DelegateCommand FormatCmd { get; set; }
        public DelegateCommand RefreshCmd { get; set; }
        public DelegateCommand SaveCmd { get; set; }
        readonly string key = AppData.scales;
        readonly string weightkey = AppData.judgeMatrix;
        public ExporterViewModel()
        {
            CalcCmd = new DelegateCommand(Calc);
            SelectedCmd = new DelegateCommand<string>(Selected);
            FormatCmd = new DelegateCommand(format);
            RefreshCmd = new DelegateCommand(Load);
            SaveCmd = new DelegateCommand(Save);
            Load();
        }

        private void Save()
        {
            Config.SetValue(key, list);
        }

        private void Selected(string obj)
        {
            list = dict[obj];
            format();
        }

        private void Load()
        {
            List<Property> properties = Config.GetValue<List<Property>>(key);
            if (properties != null && properties.Count > 0) list = properties;
            else list = dict.First().Value;
            Methods = dict.Select(m => new Property(m.Key) { IsChecked = list.Count == m.Value.Count }).ToList();

            format();

            List<WeightModel> weights = Config.GetValue<List<WeightModel>>(weightkey);
            if (weights != null && weights.Count > 0) Models = weights.Select(x => x.FillData()).ToList();
        }

        void format()
        {
            Properties = list.Where(x =>
                    (IsEvenShowed || (!IsEvenShowed && x.RealValue % 2 != 0))
                    && (IsLeftShowed || (!IsLeftShowed && x.RealValue >= 1))
                    ).ToList();
        }

        private void Calc()
        {
            Config.SetValue(key, list);
            QuestionNaireView view = new QuestionNaireView();
            if (view.ShowDialog() == true)
            {
                Load();
            }
        }
    }
}
