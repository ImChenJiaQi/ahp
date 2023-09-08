using AHPTest.Commons;
using AHPTest.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PropertyChanged;
namespace AHPTest.ViewModels
{
    internal class QuestionNaireViewModel : BindableBase
    {
        public int TotalCount { get; set; }
        public int FinishedCount { get; set; }
        public bool IsCompleted { get => FinishedCount == TotalCount; }
        public List<Property> Propeties { get; set; }
        public WeightModel Weight { get; set; }
        public DelegateCommand SaveCmd { get; set; }
        public DelegateCommand NextCmd { get; set; }
        public DelegateCommand PreviousCmd { get; set; }
        public DelegateCommand CalcCmd { get; set; }

        List<RuleModel> list;
        readonly string dataKey = AppData.content;
        readonly string scalesKey = AppData.scales;
        readonly string result = AppData.judgeMatrix;
        List<WeightModel> weights;
        readonly double presion = Math.Pow(10, -3);
        public QuestionNaireViewModel()
        {
            Load();
            NextCmd = new DelegateCommand(Next);
            PreviousCmd = new DelegateCommand(Previous);
            SaveCmd = new DelegateCommand(Save);
        }

        private void Save()
        {
            foreach (var item in weights)
            {
                item.Save();
            }
            Config.SetValue(result, weights);
        }

        private void Previous()
        {
            weights[FinishedCount - 1].Value = Propeties.FirstOrDefault(x => x.IsChecked)?.RealValue ?? 1;
            if (FinishedCount > 1)
            {
                Weight = weights[FinishedCount - 2];
                Propeties = Propeties.Select(x =>
                {
                    x.IsChecked = Math.Abs((x.RealValue ?? 1) - Weight.Value) < presion;
                    return x;
                }).ToList();
                FinishedCount--;
            }
        }

        private void Next()
        {
            weights[FinishedCount - 1].Value = Propeties.FirstOrDefault(x => x.IsChecked)?.RealValue ?? 1;
            if (FinishedCount < weights.Count)
            {
                Weight = weights[FinishedCount];
                Propeties = Propeties.Select(x =>
                {
                    var v = Math.Abs((x.RealValue ?? 1) - Weight.Value);
                    x.IsChecked = v < presion;
                    return x;
                }).ToList();


                FinishedCount++;
            }
        }
        private void Load()
        {
            list = Config.GetValue<List<RuleModel>>(dataKey);
            if (list == null) { return; }
            var properties = Config.GetValue<List<Property>>(scalesKey);
            if (properties != null && properties.Count > 0)
            {
                Propeties = properties
                    .Select(p => { p.IsChecked = p.Value == "1"; return p; })
                    .OrderBy(x => x.RealValue).ToList();
            }
            weights = new List<WeightModel>();
            GetWeightList(ref weights, list);
            Weight = weights.FirstOrDefault();
            TotalCount = weights.Count;
            FinishedCount = TotalCount > 0 ? 1 : 0;
        }
        private void GetWeightList(ref List<WeightModel> list, RuleModel model)
        {

            int n = model.Children.Count;
            for (int i = 0; i < n; i++)
            {
                GetWeightList(ref list, model.Children[i]);
                for (int j = 0; j < i; j++)
                {
                    list.Add(new WeightModel() { Left = model.Children[i], Right = model.Children[j], ParentId = model.Id });
                }
            }
        }
        private void GetWeightList(ref List<WeightModel> list, List<RuleModel> models)
        {
            foreach (var item in models)
            {
                GetWeightList(ref list, item);
            }
        }
    }



}
