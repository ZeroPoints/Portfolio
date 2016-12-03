using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Numerics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    public class FractalController : Controller
    {



        public ActionResult Index(JuliaImage myModel)
        {

            Dictionary<Tuple<int, int>, int> xypos = new Dictionary<Tuple<int, int>, int>();

            //Clean up folder if its size is greater then a 1gb
            var filesInPath = Directory.GetFiles(System.Configuration.ConfigurationManager.AppSettings["ImageDirectory"], "*.*");
            long directorySize = 0;
            foreach (string name in filesInPath)
            {
                FileInfo info = new FileInfo(name);
                directorySize += info.Length;
            }
            //Rough estimate cant remember if its bytes or bits atm
            if (directorySize > 1 * 1000 * 1000 * 1000)
            {

                foreach (string name in filesInPath)
                {
                    try
                    {
                        System.IO.File.Delete(name);
                    }
                    catch//(Exception ex) //Supress and pass on
                    {

                    }
                }
            }



            //These shouldnt happen
            if (myModel == null)
            {
                myModel = new JuliaImage();
            }
            if (myModel.AlphaDivider <= 0)
            {
                myModel.AlphaDivider = 1;
            }
            if (myModel.RedDivider <= 0)
            {
                myModel.RedDivider = 1;
            }
            if (myModel.GreenDivider <= 0)
            {
                myModel.GreenDivider = 1;
            }
            if (myModel.BlueDivider <= 0)
            {
                myModel.BlueDivider = 1;
            }
            if (myModel.Steps <= 0 || myModel.Steps > 255)
            {
                myModel.Steps = 255;
            }
            if (myModel.Height <= 0)
            {
                myModel.Height = 500;
            }
            if (myModel.Width <= 0)
            {
                myModel.Width = 500;
            }

            //Init Constant
            myModel.Constant = new Complex(myModel.ConstantReal, myModel.ConstantImaginary);



            float height = myModel.Height;
            float width = myModel.Width;

            for (int i = 0; i <= width; i++)
            {
                for (int j = 0; j <= height; j++)
                {
                    var result = CalculateVals(new Complex(i * 2 / width - 1, j * 2 / height - 1), ref myModel);
                    xypos.Add(new Tuple<int, int>(i, j), result);
                }
            }


            Guid fileName = Guid.NewGuid();
            using (Bitmap bmp = new Bitmap(Convert.ToInt32(width), Convert.ToInt32(height)))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    for (int x = 0; x <= width; x++)
                    {
                        for (int y = 0; y <= height; y++)
                        {
                            int value = xypos[new Tuple<int, int>(x, y)];
                            int av = Convert.ToInt32(value / myModel.AlphaDivider);
                            int rv = Convert.ToInt32(value / myModel.RedDivider);
                            int gv = Convert.ToInt32(value / myModel.GreenDivider);
                            int bv = Convert.ToInt32(value / myModel.BlueDivider);
                            if (av < 0 || av > 255)
                            {
                                av = 255;
                            }
                            if (rv < 0 || rv > 255)
                            {
                                rv = 255;
                            }
                            if (gv < 0 || gv > 255)
                            {
                                gv = 255;
                            }
                            if (bv < 0 || bv > 255)
                            {
                                bv = 255;
                            }
                            g.FillRectangle(new SolidBrush(
                                Color.FromArgb(
                                    av, rv, gv, bv
                                    )), new Rectangle(x, y, 1, 1));
                        }
                    }
                }
                bmp.Save(System.Configuration.ConfigurationManager.AppSettings["ImageDirectory"] + fileName.ToString() + ".png", ImageFormat.Png);
            }
            myModel.FilePath = "/Fractal/ImageResource/" + fileName.ToString();
            return View(myModel);
        }




        public int CalculateVals(Complex val, ref JuliaImage myModel)
        {
            int steps = 0;
            Complex newCmp = val;
            double magnitude = 0;
            while (magnitude < myModel.Threshold && steps < myModel.Steps)
            {
                steps++;
                var tempReal = (newCmp * newCmp) + myModel.Constant;
                magnitude = tempReal.Magnitude;
                newCmp = tempReal;
            }

            return steps;
        }





        public ActionResult ImageResource(string id)
        {
            try
            {
                //id has to = a guid
                var testGuidParse = Guid.Parse(id);
                var dir = System.Configuration.ConfigurationManager.AppSettings["ImageDirectory"];
                var path = Path.Combine(dir, id + ".png"); //validate the path for security or use other means to generate the path.
                return base.File(path, "image/png");
            }
            catch
            {
                return View("");
            }
        }








    }
}