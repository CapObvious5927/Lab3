using System;
using System.Data;
using System.Numerics;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;


namespace Lab3 {
    class V2MainCollection : IEnumerable<V2Data> {
        private List<V2Data> ListV2Data;
        private int elem_num;

        public IEnumerator GetEnumerator() {
            return ListV2Data.GetEnumerator();
        }

        IEnumerator<V2Data> IEnumerable<V2Data>.GetEnumerator() {
            return ListV2Data.GetEnumerator();
        }

        public V2MainCollection() {
            ListV2Data = new List<V2Data>();
            elem_num = 0;
        }

        public void OnDataChanged(object source, DataChangedEventArgs args) {
            if (DataChanged != null) {
                DataChanged(source, args);
            }
        }

        public void OnPropertyChanged(object source, PropertyChangedEventArgs args) {
            if (DataChanged != null) {
                DataChanged(source, new DataChangedEventArgs(ChangeInfo.ItemChanged, ListV2Data[elem_num - 1].EM_Freq));
            }
        }

        public V2Data this[int idx] {
            get {
                return ListV2Data[idx];
            }
            set {
                value.PropertyChanged += OnPropertyChanged;
                ListV2Data[idx] = value;
                OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Replace, ListV2Data[idx].EM_Freq));
            }
        }

        public event DataChangedEventHandler DataChanged;

        public int Count {
            get { return ListV2Data.Count; }
        }

        public double AverageAbsValue {
            get {
                var EM_fields = from V2Data_obj in ListV2Data from data in V2Data_obj select data.EM_field;
                var res = from field in EM_fields select field.Magnitude;
                return res.Average();
            }
        }

        public IEnumerable<DataItem> MaxDiffWithAverage {
            get {
                double average = this.AverageAbsValue;

                var all = ListV2Data.SelectMany(x => x);
                double max_diff = all.Max(x => Math.Abs(average - x.EM_field.Magnitude));

                IEnumerable<DataItem> res = from item in all where Math.Abs(average - item.EM_field.Magnitude) == max_diff select item;
                
                return res;
            }
        }

        public IEnumerable<Vector2> AppearTwoTimesOrMore {
            get {
                var res = from x in ListV2Data from item in x group item by item.Coord into g where g.Count() > 1 select g.Key;
                return res;
            }
        }

        public void Add(V2Data item) {
            item.PropertyChanged += OnPropertyChanged;
            ListV2Data.Add(item);
            OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Add, ListV2Data[elem_num].EM_Freq));
            elem_num++;
        }

        public bool Remove(string id, double w) {
            int numRemovedItems = ListV2Data.RemoveAll(item => item.Info == id && item.EM_Freq == w);
            if (numRemovedItems > 0) {
                OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Remove, ListV2Data[elem_num - numRemovedItems - 1].EM_Freq));
                elem_num -= numRemovedItems;
                return true;
            }
            return false;
        }

        public void AddDefaults() {
            Random rnd = new Random();
            int num = 3 + rnd.Next(0, 5); // количество добавленных элементов от 3 до 8 выбирается рандомно 
            int numOnGrid = rnd.Next(1, num - 1); // количество элементов типа V2DataOnGrid выбирается рандомно в диапозоне от 1 до n-1
            int numCol = num - numOnGrid;
            double minVal = 1.0;
            double maxVal = 32.5;
            float maxValFloat = (float)100.0;

            for (int i = 0; i < numOnGrid; i++) {
                Grid1D tmp1 = new Grid1D((float)rnd.NextDouble() * maxValFloat, rnd.Next(0, 32));
                Grid1D tmp2 = new Grid1D((float)rnd.NextDouble() * maxValFloat, rnd.Next(0, 32));
                V2DataOnGrid objOnGrid = new V2DataOnGrid("new default V2DataOnGrid object", (float)rnd.NextDouble() * maxValFloat, tmp1, tmp2);
                objOnGrid.InitRandom(minVal, maxVal);
                ListV2Data.Add(objOnGrid);
            }

            for (int i = 0; i < numCol; i++) {
                V2DataCollection objCol = new V2DataCollection("new default V2DataCollection object", rnd.NextDouble() * 100.0);
                objCol.InitRandom(rnd.Next(1, 10), (float)rnd.NextDouble() * maxValFloat, (float)rnd.NextDouble() * maxValFloat, minVal, maxVal);
                ListV2Data.Add(objCol);
            }
        }


        public override string ToString() {
            string res = "";
            foreach (var item in ListV2Data) {
                res += item.ToString() + "\n";
            }

            return res;
        }

        public string ToLongString() {
            string res = "";
            foreach (var item in ListV2Data) {
                res += item.ToLongString();
            }
            return res;
        }

        public string ToLongString(string format) {
            string res = "";
            foreach (var item in ListV2Data) {
                res += item.ToLongString(format);
            }
            return res;
        }
    }

}
