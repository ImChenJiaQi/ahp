using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
namespace AHPTest.Models
{
    internal class Property : BindableBase
    {
        public Property(string name)
        {
            Name = name;
        }
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public double? RealValue
        {
            get
            {

                if (string.IsNullOrEmpty(Value)) return null;
                if (!Value.Contains("/")) return double.Parse(Value);
                else
                {
                    var vs = Value.Replace(" ", "").Split('/');
                    return double.Parse(vs[0]) / double.Parse(vs[1]);
                }
            }
        }
    }
}
