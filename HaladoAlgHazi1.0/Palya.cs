using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace HaladoAlgHazi1._0
{

    public class Palya : FrameworkElement
    {
       public static Random rand = new Random();
        DispatcherTimer dt;
        List<RandomPont> randomPonts = new List<RandomPont>();
        List<Kor> korok = new List<Kor>();
        DrawingContext d;

        public Palya()
        {
            Vector a = new Vector(100, 100);
            Loaded += Palya_Loaded;
        }
        void KoroketLetrehoz(int db)
        {
            do
            {
                List<int> iranyszgek = new List<int>();
                while (iranyszgek.Count < db)
                {
                    int tmp = rand.Next(0, 360);
                    iranyszgek.Add(tmp);
                    iranyszgek = iranyszgek.Distinct().ToList();
                }
                iranyszgek.Sort();
                for (int i = 0; i < db; i++)
                {
                    Kor k = new Kor();
                    k.X = 350 + Math.Sin((iranyszgek[i]) * Math.PI / 180.0) * 400;
                    k.Y = 350 + Math.Cos((iranyszgek[i]) * Math.PI / 180.0) * 400;
                    korok.Add(k);
                }
                for (int i = 0; i < db - 1; i++)
                {
                    korok[i].Szomszed = korok[i + 1];
                }
                korok[db - 1].Szomszed = korok[0];
            } while (!JolVannakAPontok(korok));
        }

        private void Palya_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 15; i++)
            {
                randomPonts.Add(new RandomPont());
            }
            KoroketLetrehoz(15);
            dt = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(2) };
            dt.Tick += Dt_Tick;
            dt.Start();
           Task task1 = Task.Run(() => Megoldasok.HillClimbingRandom(korok, Fitnes , 5 , Vegevan , JolVannakAPontok  ));
           //    Task task1 = Task.Run(() => Megoldasok.HillClimbingFull(korok, Fitnes, 1, Vegevan, JolVannakAPontok));
        //    Task task1 = Task.Run(() => Megoldasok.SimulatedAnnealing(korok, Fitnes, 5, Vegevan, Temperature ,JolVannakAPontok));
        }

        double Temperature (int t)
        {
            return 5000.0 * Math.Pow(1.0 - (double)t / 5000.0 , 2);
           
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            InvalidateVisual();
        }

        bool JolVannakAPontok(List<Kor> k)
        {
            for (int i = 0; i < k.Count; i++)
            {
                double x0 = k[i].X;
                double y0 = k[i].Y;
                double x1 = k[i].Szomszed.X;
                double y1 = k[i].Szomszed.Y;
             //   Vector iranyvektor = new Vector((x1 - x0), (y1 - y0));
                Vector normalvektor = new Vector((y1 - y0), -1 * (x1 - x0));
                double c = normalvektor.X * k[i].X + normalvektor.Y * k[i].Y;
                for (int j = 0; j < randomPonts.Count; j++)
                {
                    double szamlalo = normalvektor.X * randomPonts[j].X + normalvektor.Y * randomPonts[j].Y - c;
                    if (szamlalo <= 0)
                        return false;
                }
            }
            return true;
        }

        double Fitnes(List<Kor> k)
        {
            double osszhossz = 0;
            for (int i = 0; i < k.Count; i++)
            {
                double x = Math.Abs(k[i].X - korok[i].Szomszed.X);
                double y = Math.Abs(k[i].Y - korok[i].Szomszed.Y);
                osszhossz += Math.Sqrt(x * x + y * y);
            }
            return osszhossz;
        }
        bool Vegevan (int szam)
        {
            return (szam > 350);
        }

        double Fitnes(List<Kor> k ,int szam, double ujx, double ujy)
        {
            double osszhossz = 0;
            if (szam >= 0)
            {
              
                for (int i = 0; i < korok.Count; i++)
                {
                    if (i == szam)
                    {
                        double x = Math.Abs(ujx - k[i].Szomszed.X);
                        double y = Math.Abs(ujy - k[i].Szomszed.Y);
                        osszhossz += Math.Sqrt(x * x + y * y);
                    }

                    else if (i == szam - 1)
                    {
                        double x = Math.Abs(ujx - k[i].X);
                        double y = Math.Abs(ujy - k[i].Y);
                        osszhossz += Math.Sqrt(x * x + y * y);
                    }
                    else if (szam - 1 == -1)
                    {
                        double x = Math.Abs(ujx - k[k.Count - 1].X);
                        double y = Math.Abs(ujy - k[k.Count - 1].Y);
                        osszhossz += Math.Sqrt(x * x + y * y);
                    }
                    else
                    {
                        double x = Math.Abs(k[i].X - k[i].Szomszed.X);
                        double y = Math.Abs(k[i].Y - k[i].Szomszed.Y);
                        osszhossz += Math.Sqrt(x * x + y * y);
                    }

                }
            }
            else
            {
                for (int i = 0; i < k.Count; i++)
                {
                    double x = Math.Abs(k[i].X - korok[i].Szomszed.X);
                    double y = Math.Abs(k[i].Y - korok[i].Szomszed.Y);
                    osszhossz += Math.Sqrt(x * x + y * y);
                }
            }
            return osszhossz;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            lock (korok)
            {
                d = drawingContext;
                foreach (RandomPont p in randomPonts)
                {
                    drawingContext.DrawGeometry(p.Szin, null, p.Alak);
                }
                foreach (Kor k in korok)
                {

                    drawingContext.DrawGeometry(k.Szin, null, k.Alak);
                }

                foreach (Kor k in korok)
                {
                    drawingContext.DrawGeometry(Brushes.Red, new Pen(Brushes.Red, 1), new LineGeometry(k.Pontja, k.Szomszed.Pontja));
                }
            }




        }

        void HillClimbingRandom() // s = korok , ds = tavolsagszamitas , e = 10  , stop)
        {
            int nemtortentsemmi = 0;
            double e = 10;
            while (nemtortentsemmi < 50 )
            {
                int szam = rand.Next(0, korok.Count);
                int randszog = rand.Next(0, 360);
                double xszam = korok[szam].X + Math.Sin((randszog) * Math.PI / 180.0) * e;
                double yszam = korok[szam].Y + Math.Sin((randszog) * Math.PI / 180.0) * e;
                Thread.Sleep(10);
                if (Fitnes(korok, szam, xszam, yszam) < Fitnes(korok))
                {
                    double szam1 = korok[szam].X;
                    double szam2 = korok[szam].Y;
                    lock (korok)
                    {
                        korok[szam].X = xszam;
                        korok[szam].Y = yszam;
                        if (!JolVannakAPontok(korok))
                        {
                            korok[szam].X = szam1;
                            korok[szam].Y = szam2;
                            nemtortentsemmi++;
                        }
                        else
                        {
                            Console.WriteLine(Fitnes(korok));
                            nemtortentsemmi = 0;
                        }
                    }
                }
            }
            Console.WriteLine("Megtalalt optimalis index:" + Fitnes(korok));
        }

    }
}
