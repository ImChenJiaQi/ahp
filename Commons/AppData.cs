using AHPTest.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHPTest.Commons
{
    internal static class AppData
    {
        public static string[] Names = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I" };


        public const string content = "content";

        public const string scales = "scales";
        public const string judgeMatrix = "judgeMatrix";


        public static RuleModel Rule { get; set; }

        public static EventAggregator MyEA { get; } = new EventAggregator();

    }
}
