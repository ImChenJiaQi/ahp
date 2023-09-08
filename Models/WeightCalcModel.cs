using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using AHPTest.Commons;
using System.Windows;

namespace AHPTest.Models
{
    internal class WeightCalcModel : BindableBase
    {
        public string Name { get; set; }

        public ObservableCollection<MikiModel> Models { get; set; } = new ObservableCollection<MikiModel>();

        public Dictionary<string, RuleModel> ExplainList { get; set; } = new Dictionary<string, RuleModel>();

        public double[] Values { get; set; }
        public string Result { get; set; }

        /// <summary>
        /// 填充判断矩阵
        /// </summary>
        /// <param name="weights"></param>
        public WeightCalcModel FillData(List<WeightModel> weights)
        {
            if (weights == null || weights.Count == 0) return this;


            var properties = typeof(MikiModel).GetProperties();

            var list = weights.SelectMany(x => new List<Guid>() { x.LeftId, x.RightId }).Distinct().ToList();
            int n = list.Count;
            if (n > AppData.Names.Length)
            {
                MessageBox.Show($"指标个数不得超过{AppData.Names.Length}");
                return this;
            }
            for (int i = 0; i < n; i++)
            {
                MikiModel model = new MikiModel()
                {
                    Name = AppData.Names[i],
                };
                ExplainList.Add(AppData.Names[i], AppData.Rule.FindChild(list[i]));
                Models.Add(model);
            }
            ///
            /// 填充数值
            ///
            foreach (var weight in weights)
            {
                var i = list.FindIndex(x => x == weight.LeftId);
                var j = list.FindIndex(x => x == weight.RightId);
                var p = properties.First(x => x.Name == $"Value{j + 1}");
                p.SetValue(Models[i], weight.Value);

                var p2 = properties.First(x => x.Name == $"Value{i + 1}");

                p2.SetValue(Models[j], (1 / weight.Value));
            }
            for (int i = 0; i < n; i++)
            {
                var p = properties.First(x => x.Name == $"Value{i + 1}");
                p.SetValue(Models[i], 1.0);
            }
            return this;
        }
    }
}
