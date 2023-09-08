using AHPTest.Models;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PropertyChanged;
namespace AHPTest.ViewModels
{
    internal class MikiViewModel : BindableBase
    {
        public List<int> Ints { get; set; } = Enumerable.Range(1, 9).ToList();
        public int Index { get; set; } = 4;
        public string Content { get; set; } = "1，0.5,3\r\n2，4\r\n2";
        public string OutPut { get; set; }
        public bool IsCalcEnabled { get; set; } = false;
        public ObservableCollection<MikiModel> Models { get; set; } = new ObservableCollection<MikiModel>();
        public DelegateCommand GenerateCmd { get; set; }
        public DelegateCommand CalcCmd { get; set; }
        readonly List<PropertyInfo> properties = typeof(MikiModel).GetProperties().Where(x => x.Name.Contains("Value")).ToList();
        public MikiViewModel()
        {
            GenerateCmd = new DelegateCommand(Generate);
            CalcCmd = new DelegateCommand(Calc);
        }

        private void Generate()
        {
            Models.Clear();
            if (string.IsNullOrEmpty(Content)) return;
            string[] names = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I" };
            string content = Content
                .Replace(" ", "")
                .Replace("，", ",")
                .Replace(",,", ",")
                .Replace("\r", "")
                .Replace(",\n", "\n");
            //.Replace("\"", "/");
            try
            {

                List<string[]> vs = content.Split('\n')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => x.Split(',')).ToList();
                List<double[]> values = vs.Select(x => x.Select(y =>
                {
                    double value;
                    if (y.Contains("/"))
                    {
                        var aa = y.Split('/').Select(v => double.Parse(v)).ToList();
                        value = aa[0] / aa[1];
                    }
                    else value = double.Parse(y);
                    return value;
                }).Reverse().ToArray()).ToList();

                for (int i = 0; i < Index; i++)
                {
                    MikiModel model = new MikiModel()
                    {
                        Name = names[i],
                    };
                    for (int j = Index; j > 0; j--)
                    {
                        // j列=>x, i行=>y 
                        var p = properties.First(x => x.Name == "Value" + j);

                        if (i + 1 == j) { p.SetValue(model, 1.0); }
                        else if (i + 1 < j) // 右上
                        {
                            int x = Index - j;
                            double v = 1;
                            if (i < values.Count)
                            {
                                var row = values[i];
                                if (x < row.Length)
                                    v = row[x];
                            }

                            p.SetValue(model, v);
                        }
                        else // 左下
                        {
                            var p1 = properties.First(x => x.Name == "Value" + (i + 1));
                            double v = (double)p1.GetValue(Models[j - 1]);
                            p.SetValue(model, 1 / v);
                        }
                    }
                    Models.Add(model);
                }
                IsCalcEnabled = true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        // 方根法
        private void Calc()
        {
            if (Models.Count < Index)
            {
                MessageBox.Show("判断矩阵过小");
                return;
            }
            try
            {
                List<double[]> values = Models.Select(model =>
                {
                    var res = Enumerable.Range(1, Index).Select(i => (double)properties.First(x => x.Name == "Value" + i).GetValue(model)).ToArray();
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
                    Math.Pow(v, 1 / Index);
                    return v;
                });
                double sum = v2.Sum();
                OutPut = string.Join(",", v2.Select(x => (x / sum).ToString("F4")));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
