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
    class V2DataOnGrid : V2Data, IEnumerable<DataItem> {
        public Grid1D[] GridData { get; set; }
        public Complex[,] NodeValue { get; set; }

        IEnumerator IEnumerable.GetEnumerator() {
            return new DataEnumerator(NodeValue, GridData[0], GridData[1]);
        }

        public override IEnumerator<DataItem> GetEnumerator() {
            return new DataEnumerator(NodeValue, GridData[0], GridData[1]);
        }

        private int firstDimLen = 0;
        private int secDimLen = 0;

        public V2DataOnGrid(string x, double y, Grid1D param1, Grid1D param2) : base(x, y) {
            GridData = new Grid1D[2];
            GridData[0] = param1;
            GridData[1] = param2;
            NodeValue = new Complex[param1.NodeNum, param2.NodeNum];
            firstDimLen = NodeValue.GetLength(0);
            secDimLen = NodeValue.GetLength(1);
        }


        private class DataEnumerator : IEnumerator<DataItem> {
            private Grid1D grid1, grid2;
            private Complex[,] NodeVal;

            public DataEnumerator(Complex[,] NodeValues, Grid1D grid_1, Grid1D grid_2) {
                NodeVal = NodeValues;
                grid1 = grid_1;
                grid2 = grid_2;
            }

            private int x = -1;
            private int y = 0;

            object IEnumerator.Current => Current;

            public DataItem Current {
                get {
                    Vector2 coord = new Vector2(grid1.Step + x, grid2.Step + y);
                    Complex EM_field = NodeVal[x, y];
                    DataItem obj = new DataItem(coord, EM_field);
                    return obj;
                }
            }

            public bool MoveNext() {
                if (x == NodeVal.GetLength(0) - 1) {
                    x = 0;
                    ++y;
                } else {
                    ++x;
                }

                if (y < NodeVal.GetLength(1)) {
                    return true;
                } else {
                    return false;
                }
            }

            public void Reset() {
                x = -1;
                y = 0;
            }

            public void Dispose() {
                NodeVal = null;
            }
        }

        public void InitRandom(double minValue, double maxValue) {
            Random rnd = new Random();

            for (int i = 0; i < firstDimLen; i++) {
                for (int j = 0; j < secDimLen; j++) {
                    double real = rnd.NextDouble() * (maxValue - minValue) + minValue;
                    double imag = rnd.NextDouble() * (maxValue - minValue) + minValue;
                    NodeValue[i, j] = new Complex(real, imag);
                }
            }
        }

        public override Complex[] NearAverage(float eps) {
            double average = 0;
            int num = 0;

            for (int i = 0; i < firstDimLen; i++) {
                for (int j = 0; j < secDimLen; j++) {
                    num++;
                    average += NodeValue[i, j].Real;
                }
            }
            average /= num;

            int counter = 0;
            int capacity = 16;
            Complex[] result = new Complex[capacity];

            for (int i = 0; i < firstDimLen; i++) {
                for (int j = 0; j < secDimLen; j++) {
                    if (Math.Abs(NodeValue[i, j].Real - average) <= eps) {
                        result[counter] = NodeValue[i, j];
                        counter++;
                    }

                    if (counter == capacity - 2) {
                        capacity *= 2;
                        Array.Resize(ref result, capacity);
                    }
                }

            }
            Array.Resize(ref result, counter);

            return result;
        }

        public override string ToString() {
            return $"Type = V2DataOnGrid\nInfo = {Info}\nElectromagnetic field frequency = {EM_freq}\n" +
                   $"OX GridData: Step = {GridData[0].Step}, Number of Nodes = {GridData[0].NodeNum}\n" +
                   $"OY GridData: Step = {GridData[1].Step}, Number of Nodes = {GridData[1].NodeNum}\n";
        }

        public override string ToLongString() {
            string res = this.ToString();

            for (int i = 0; i < firstDimLen; i++) {
                for (int j = 0; j < secDimLen; j++) {
                    res += $"coord = ({i}, {j}), value = {NodeValue[i, j]}\n";
                }
            }

            return res;
        }

        public static explicit operator V2DataCollection(V2DataOnGrid obj) {
            V2DataCollection res = new V2DataCollection(obj.Info, obj.EM_freq);

            for (int i = 0; i < obj.firstDimLen; i++) {
                for (int j = 0; j < obj.secDimLen; j++) {
                    Vector2 vec = new Vector2((float)i * obj.GridData[0].Step, (float)j * obj.GridData[1].Step);
                    Complex em_field = obj.NodeValue[i, j];
                    res.ListData.Add(new DataItem(vec, em_field));
                }
            }

            return res;
        }

        public override string ToLongString(string format) {
            string res = $"Type = V2DataOnGrid\nInfo = {Info}\nElectromagnetic field frequency = {EM_freq.ToString(format)}\n" +
                         $"OX GridData: Step = {GridData[0].Step.ToString(format)}, Number of Nodes = {GridData[0].NodeNum}\n" +
                         $"OY GridData: Step = {GridData[1].Step.ToString(format)}, Number of Nodes = {GridData[1].NodeNum}\n";

            for (int i = 0; i < firstDimLen; i++) {
                for (int j = 0; j < secDimLen; j++) {
                    res += $"coord = ({i}, {j}), value = {NodeValue[i, j].ToString(format)}, abs = {NodeValue[i, j].Magnitude.ToString(format)}\n";
                }
            }

            return res;

        }
    }
}
