using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AHPTest.Models
{
    public class RuleModel : BindableBase
    {
        [JsonIgnore]
        public bool IsExplanded { get; set; } = true;
        public string Content { get; set; }
        public double? Weight { get; set; }
        public double? FinalWeight { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonIgnore]
        public RuleModel Parent { get; set; }
        public ObservableCollection<RuleModel> Children { get; set; } = new ObservableCollection<RuleModel>();

        #region 设置Parent

        public static RuleModel Format(RuleModel model)
        {
            foreach (var item in model.Children)
            {
                item.Parent = model;
                Format(item);
            }
            return model;
        }
        public static List<RuleModel> Format(List<RuleModel> model)
        {
            for (int i = 0; i < model.Count; i++)
            {
                model[i] = Format(model[i]);
            }
            return model;
        }

        #endregion 
        public RuleModel FindChild(Guid id)
        {
            if (Id == id) return this;
            foreach (var item in Children)
            {
                var model = item.FindChild(id);
                if (model != null) return model;
            }
            return null;
        }
        /// <summary>
        /// 计算最终权重
        /// </summary>
        public void CalcFinalWeight()
        {
            if (Weight == null || FinalWeight == null) return;
            foreach (var item in Children)
            {
                if (item.Weight == null) { break; }
                item.FinalWeight = FinalWeight * item.Weight;
                item.CalcFinalWeight();
            }
        }
    }
}
