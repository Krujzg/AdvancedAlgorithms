using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HaladoAlgHazi1._0
{
    public class Megoldasok
    {
        static public void HillClimbingRandom(List<Kor> S, Func<List<Kor>, int, double, double, double> ds, double e, Func<int, bool> StopCondition, Func<List<Kor>, bool> IsItGood)
        {
            int hanyadik = 0;
            while (!StopCondition(hanyadik/10))
            {
                hanyadik++;
                int szam = Palya.rand.Next(0, S.Count);
                int randszog = Palya.rand.Next(0, 360);
                double xszam = S[szam].X + Math.Sin((randszog) * Math.PI / 180.0) * e;
                double yszam = S[szam].Y + Math.Sin((randszog) * Math.PI / 180.0) * e;
                Thread.Sleep(10);
                if (ds(S, szam, xszam, yszam) < ds(S, -10, 0, 0))
                {
                    double szam1 = S[szam].X;
                    double szam2 = S[szam].Y;
                    lock (S)
                    {
                        S[szam].X = xszam;
                        S[szam].Y = yszam;
                        if (!IsItGood(S))
                        {
                            S[szam].X = szam1;
                            S[szam].Y = szam2;
                          
                        }
                    }
                }
            }
            MessageBox.Show("Vege az algoritmusnak a minimalis Kerulet: " + ds(S, -10, 0, 0), "VEGE", MessageBoxButton.OK, MessageBoxImage.Information);
        }

      /*  static public void HillClimbingFull2(List<Kor> S, Func<List<Kor>,  double> ds, double e, Func<int, bool> StopCondition, Func<List<Kor>, bool> IsItGood)
        {
            int nemtortentsemmi = 0;
            while (!StopCondition(nemtortentsemmi))
            {
                int szam = Palya.rand.Next(0, S.Count);

                //MinimumKivalasztas
                double minFitnes = ds(S);
                double minX = S[szam].X;
                double minY = S[szam].Y;
              //  Thread.Sleep(10);
                lock (S)
                {
                    for (int i = 0; i < 360; i++)
                    {
                        S[szam].X = S[szam].X + Math.Sin((i) * Math.PI / 180.0) * e;
                        S[szam].Y = S[szam].Y + Math.Sin((i) * Math.PI / 180.0) * e;
                        if (IsItGood(S) && ds(S) < minFitnes)
                        {
                            minFitnes = ds(S);
                            minX = S[szam].X;
                            minY = S[szam].Y;
                        }
                        else
                        {
                            S[szam].X = minX;
                            S[szam].Y = minY;
                        }
                    }
                   
                }
                Thread.Sleep(10);

            }
            MessageBox.Show("Vege az algoritmusnak a minimalis Kerulet: " + ds(S), "VEGE", MessageBoxButton.OK, MessageBoxImage.Information);
        }*/

        static public void HillClimbingFull(List<Kor> S, Func<List<Kor>, double> ds, double e, Func<int, bool> StopCondition, Func<List<Kor>, bool> IsItGood)
        {
            int hanyadik = 0;
            while (!StopCondition(hanyadik))
            {
                hanyadik++;

                int szam = Palya.rand.Next(0, S.Count);

                //MinimumKivalasztas
                double minFitnes = ds(S);
                double eredetiX = S[szam].X;
                double eredetiY = S[szam].Y;
                double minX = S[szam].X;
                double minY = S[szam].Y;
                Thread.Sleep(100);
                
                    bool b = true;
                
                    while (b) {
                     Thread.Sleep(10);
                    eredetiX = S[szam].X;
                     eredetiY = S[szam].Y;

                    lock (S)
                    {
                        b = false;
                        for (int i = 0; i < 360; i++)
                        {
                            S[szam].X = S[szam].X + Math.Sin((i) * Math.PI / 180.0) * e;
                            S[szam].Y = S[szam].Y + Math.Sin((i) * Math.PI / 180.0) * e;
                            if (IsItGood(S) && ds(S) < minFitnes)
                            {
                                minFitnes = ds(S);
                                minX = S[szam].X;
                                minY = S[szam].Y;
                                S[szam].X = eredetiX;
                                S[szam].Y = eredetiY;
                                b = true;
                            }
                            else
                            {
                                S[szam].X = eredetiX;
                                S[szam].Y = eredetiY;
                            }
                        }
                        S[szam].X = minX;
                        S[szam].Y = minY;
                       // Console.WriteLine(S[szam].GetHashCode());
                    }

                }
             //   Thread.Sleep(10);

            }
            MessageBox.Show("Vege az algoritmusnak a minimalis Kerulet: " + ds(S), "VEGE", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        static public void SimulatedAnnealing(List<Kor> p, Func<List<Kor>,  double> f, double e, Func<int, bool> StopCondition, Func<int, double >Temperature ,Func<List<Kor>, bool> IsItGood)
        {
            // p rand S
            int t = 0;     // t= 1
            List<Kor> popt = new List<Kor>();
            popt  = ListaMasolas( p); // popt = p
            List<Kor> q = new List<Kor>();
            q = ListaMasolas(p);

            for (int i = 0; i < 100; i++)
            {
               // Console.WriteLine(f(popt));
            }
            int szamlalo = 0;
            while (!StopCondition((int)(t/5000*350) ))
            {
                szamlalo++;
                t++;
                Thread.Sleep(10);
                lock (p)
                {
                    RandomLista(q, p, e); // q = random
                                          //  Console.WriteLine("P:"+f(p));
                                          //   Console.WriteLine("Q:" + f(q));
                    double deltae = f(q) / 2500 - f(p) / 2500;
                    if (IsItGood(q))    //egyaltalan helyes e a valasz
                    {
                        if (deltae < 0)
                        {
                            ListaMasolas(p, q);
                            if (f(p) < f(popt))
                            {
                                szamlalo = 0;
                                ListaMasolas(popt, p);
                            }
                        }
                         else
                         {
                             double T = Temperature(t);
                               double P = Math.Exp(-deltae*2500/(T*0.001));
                            if (Palya.rand.Next(0,100) < P*150)
                             {
                               
                                 ListaMasolas(p, q);
                             }
                         }       
                    }
                    else
                    {
                        szamlalo--;
                    }
                    if (szamlalo > 10)
                    {
                        ListaMasolas(p, popt);
                        szamlalo = 0;
                    }
                }
            }
            ListaMasolas(p, popt);
            Thread.Sleep(1000);
            MessageBox.Show("Vege az algoritmusnak a minimalis Kerulet: " + f(p), "VEGE", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void  ListaMasolas(List<Kor> uj ,List<Kor> regi)
        {
       
            for (int i = 0; i < regi.Count; i++)
            {
                uj[i].X = regi[i].X;
                uj[i].Y = regi[i].Y;
            }
        }


        public static List<Kor> ListaMasolas( List<Kor> regi)
        {
          List<Kor>  uj = new List<Kor>();
            for (int i = 0; i < regi.Count; i++)
            {
                Kor k = new Kor();
                k.X = regi[i].X;
                k.Y = regi[i].Y;
                uj.Add(k);
            }
            for (int i = 0; i < uj.Count - 1; i++)
            {
                uj[i].Szomszed = uj[i + 1];
            }
            uj[uj.Count - 1].Szomszed = uj[0];

            return uj;
        }

        public static List<Kor> RandomLista(List<Kor> uj , List<Kor> regi, double e)
        {
            for (int i = 0; i < regi.Count; i++)
            {
                int fok = Palya.rand.Next(0, 360);
                uj[i].X = regi[i].X + Math.Sin((fok) * Math.PI / 180.0) * e;
                uj[i].Y = regi[i].Y + Math.Sin((fok) * Math.PI / 180.0) * e;
            }
            for (int i = 0; i < uj.Count - 1; i++)
            {
                uj[i].Szomszed = uj[i + 1];
            }
            uj[uj.Count - 1].Szomszed = uj[0];
            return uj;
        }


    }
}
