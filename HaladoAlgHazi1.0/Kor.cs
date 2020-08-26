using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace HaladoAlgHazi1._0
{
 public   class Kor
    {
      //  static Random rand = new Random();
        double x;
        double y;
        Kor szomszed;
        public Kor()
        {            
        }
        public Kor(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Brush Szin { get => Brushes.Red; }
        public Geometry Alak
        {
            get
            {
                return new EllipseGeometry(new Point(x, y), 5, 5);
            }
        }

        public double Y { get => y; set => y = value ; }
        public double X { get => x; set => x = value; }
        public Point Pontja { get {
                return new Point(x, y);
            } }
        internal Kor Szomszed { get => szomszed; set => szomszed = value; }

        public void Egyenlo (Kor k)
        {
              this.x = k.X;
            this.y = k.Y;
            this.szomszed = k.Szomszed;
        }
    }
}
