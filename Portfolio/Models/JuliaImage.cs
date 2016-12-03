using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Web;

namespace Portfolio.Models
{
    public class JuliaImage
    {
        //Takes this to page
        public string FilePath { get; set; }



        //Returns this to Controller on Form submit

        public float ConstantReal { get; set; } = -0.221f;
        public float ConstantImaginary { get; set; } = -0.713f;
        public float Threshold { get; set; } = 2f;
        public float GreenDivider { get; set; } = 1;
        public float BlueDivider { get; set; } = 1;
        public float RedDivider { get; set; } = 1;
        public float AlphaDivider { get; set; } = 1;
        public int Steps { get; set; } = 255;
        public int Width { get; set; } = 500;
        public int Height { get; set; } = 500;


        public Complex Constant { get; set; }
    }
}