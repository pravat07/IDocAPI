using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDocAPI.Models
{
    public class StringTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }
    public class Symtops
    {
        public string Age { get; set; }
        public string Weight { get; set; }
        public string NormalTemperature { get; set; }
        public string CurrentTemperature { get; set; }
        public string Gender { get; set; }
    }
}
