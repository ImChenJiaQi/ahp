using AHPTest.Commons;
using AHPTest.Models;
using AHPTest.Views;
using MathNet.Numerics.LinearAlgebra;
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
    internal class WeightCaclViewModel : BindableBase
    {
        public ObservableCollection<WeightCalcModel> Models { get; set; } = new ObservableCollection<WeightCalcModel>();
        readonly string key = AppData.judgeMatrix;
        public DelegateCommand SaveCmd { get; set; }
        public DelegateCommand RefreshCmd { get; set; }
        public DelegateCommand CalcCmd { get; set; }
        public WeightCaclViewModel()
        {
            SaveCmd = new DelegateCommand(Save);
            RefreshCmd = new DelegateCommand(Load);
            CalcCmd = new DelegateCommand(Calc);
            Load();
        }

        private void Calc()
        {
            var properties = typeof(MikiModel).GetProperties();
            foreach (var m in Models)
            {
                var list = m.Models;
                int n = list.Count;
                List<double[]> values = list.Select(model =>
                {
                    var res = Enumerable.Range(1, n).Select(i => (double)properties.First(x => x.Name == $"Value{i}").GetValue(model)).ToArray();
                    return res;
                }).ToList();
                Matrix<double> matrix = Matrix<double>.Build.DenseOfColumns(values);
                var v2 = values.Select(x =>
                {
                    double v = 1;
                    foreach (var item in x)
                    {
                        v = v * item;
                    }
                    Math.Pow(v, 1 / n);
                    return v;
                });
                double sum = v2.Sum();

                m.Values = v2.Select(x => (x / sum)).ToArray();
                for (int i = 0; i < m.Values.Length; i++)
                {
                    var key = AppData.Names[i];
                    m.ExplainList[key].Weight = m.Values[i];
                }
                m.Result = string.Join(",", v2.Select(x => (x / sum).ToString("F4")));

            }
            AppData.Rule.Weight = 1;
            AppData.Rule.FinalWeight = 1;
            AppData.Rule.CalcFinalWeight();
            Config.SetValue(AppData.content, new List<RuleModel>() { AppData.Rule });
        }

        private void Save()
        {

        }

        private void Load()
        {
            List<WeightModel> list = Config.GetValue<List<WeightModel>>(key);
            List<RuleModel> rule = Config.GetValue<List<RuleModel>>(AppData.content);

            if (list != null && rule != null && rule.Count > 0)
            {
                AppData.Rule = rule.First();
                // 填充判断矩阵
                var g = list.GroupBy(x => x.ParentId).ToList();

                var models = g.Select(x =>
                new WeightCalcModel { Name = rule.First().FindChild(x.Key)?.Content }.FillData(x.ToList()))
                    .ToList();
                Models = new ObservableCollection<WeightCalcModel>(models);
            }
            else
            {
                Models = new ObservableCollection<WeightCalcModel>();
            }
        }
    }

}
