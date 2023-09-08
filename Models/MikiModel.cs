using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
namespace AHPTest.Models
{
    internal class MikiModel : BindableBase
    {
        public string Name { get; set; }
        public double? Value1 { get; set; }
        public double? Value2 { get; set; }
        public double? Value3 { get; set; }
        public double? Value4 { get; set; }
        public double? Value5 { get; set; }
        public double? Value6 { get; set; }
        public double? Value7 { get; set; }
        public double? Value8 { get; set; }
        public double? Value9 { get; set; }
    }
}
