using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;


namespace Lab3 {
    class ClassForMain {
        public static void MainCollDataChanged(object sourse, DataChangedEventArgs args) {
            Console.WriteLine(args.ToString());
        }

        static void Main(string[] args) {
            /*Grid1D tmp1 = new Grid1D((float)1.4, 5);
            Grid1D tmp2 = new Grid1D((float)2.4, 7);
            V2DataOnGrid objOnGrid = new V2DataOnGrid("testing object", 32.9, tmp1, tmp2);
            objOnGrid.InitRandom(1.0, 32.5);
            Console.Write(objOnGrid.ToLongString());
            Console.Write("\n");

            V2DataCollection objCol = (V2DataCollection) objOnGrid;
            Console.Write(objCol.ToLongString());
            Console.Write("\n");
            
            V2MainCollection objMainCol = new V2MainCollection();
            objMainCol.AddDefaults();
            Console.Write(objMainCol.ToString());
            Console.Write("\n");

            Console.Write("Items in V2MainCollection object NearAverage(eps):\n");
            foreach (var item in objMainCol) {
                float eps = (float) 0.1;
                Complex[] res = item.NearAverage(eps);
                foreach (var complex in res) {
                    Console.Write(complex.ToString());
                    Console.Write('\n');
                }
            }*/

            /*try {
                V2DataCollection DataCollection = new V2DataCollection("testing.txt");
                Console.WriteLine(DataCollection.ToLongString());

                V2MainCollection MainCollection = new V2MainCollection();
                MainCollection.AddDefaults();
                string res = MainCollection.ToLongString();
                Console.WriteLine(res);

                Console.WriteLine($"\n\nAverage Abs Value = {MainCollection.AverageAbsValue}");

                Console.WriteLine("\n\nMaxDiffWithAverage: ");
                var list_max_diff = MainCollection.MaxDiffWithAverage;
                foreach (var item in list_max_diff) {
                    Console.WriteLine(item.ToString());
                }

                Console.WriteLine("\n\nAppear Two Times Or More:");
                var list_appear = MainCollection.AppearTwoTimesOrMore;
                foreach (var item in list_appear) {
                    Console.WriteLine(item.ToString());
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }*/

            V2MainCollection MainCollection = new V2MainCollection();
            MainCollection.DataChanged += MainCollDataChanged;

            V2DataCollection tmp1 = new V2DataCollection("abc", 0.6);
            V2DataCollection tmp2 = new V2DataCollection("bcd", 1.6);
            V2DataCollection tmp3 = new V2DataCollection("cde", 2.6);
            MainCollection.Add(tmp1);
            MainCollection.Add(tmp2);
            Console.WriteLine(MainCollection.ToString());

            MainCollection[0] = tmp3;
            MainCollection[0].Info = "efg";
            Console.WriteLine(MainCollection.ToString());

            MainCollection.Remove("efg", 2.6);
            Console.WriteLine(MainCollection.ToString());
        }
    }
}



