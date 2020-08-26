using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace HaladoAlgHazi1._0
{
  public  class RandomPont
    {
        static Random rand = new Random();
        double x;
        double y;

        public RandomPont()
        {
            x = rand.Next(100, 601);
            y = rand.Next(100, 601);
        }

        public Brush Szin { get => Brushes.Blue;  }
        public Geometry Alak { get {
                 return new EllipseGeometry(new Point(x,y), 5, 5);
            } }

        public double Y { get => y;  }
        public double X { get => x;  }
    }
}
