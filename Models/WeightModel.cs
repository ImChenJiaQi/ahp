using AHPTest.Commons;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AHPTest.Models
{

    public class WeightModel : BindableBase
    {
        public Guid ParentId { get; set; }
        public double Value { get; set; } = 1;
        [JsonIgnore]
        public RuleModel Left { get; set; }
        [JsonIgnore]
        public RuleModel Right { get; set; }
        [JsonIgnore]
        public RuleModel Top { get; set; }
        public Guid LeftId { get; set; }
        public Guid RightId { get; set; }

        public WeightModel FillData()
        {
            if (AppData.Rule == null) return this;
            Left = AppData.Rule.FindChild(LeftId);
            Right = AppData.Rule.FindChild(RightId);
            Top = AppData.Rule.FindChild(ParentId);
            return this;
        }

        public void Save()
        {
            LeftId = Left.Id; RightId = Right.Id;
        }
    }
}
