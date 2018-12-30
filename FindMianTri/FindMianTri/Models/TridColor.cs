using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMianTri.Models
{
    class TridColor
    {
        public string Hex;
        public string Name;
        public TridColor(string hex, string name)
        {
            this.Hex = hex;
            this.Name = name;
        }
    }
}
