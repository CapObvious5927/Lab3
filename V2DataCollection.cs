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
    class V2DataCollection : V2Data, IEnumerable<DataItem> {
        public List<DataItem> ListData { get; set; }

        IEnumerator IEnumerable.GetEnumerator() {
            return new DataEnumerator(ListData);
        }

        public override IEnumerator<DataItem> GetEnumerator() {
            return ListData.GetEnumerator();
        }

        public V2DataCollection(string x, double y) : base(x, y) {
            ListData = new List<DataItem>();
        }

        public V2DataCollection(string filename) : base("string", 0) {
            //данные хранятся следующим образом: на каждой строчке координаты узла и значения ЭМ поля в точке
            FileStream fs = null;
            List<DataItem> file_data = new List<DataItem>();

            try {
                if (File.Exists(filename) == false) {
                    throw new Exception("Couldn't find file with this name\n");
                }

                fs = new FileStream(filename, FileMode.Open);
                StreamReader streamReader = new StreamReader(fs);

                string str_data = streamReader.ReadLine();
                while (str_data != null) {
                    string[] sep_data = str_data.Split(' ');

                    if (sep_data.Length != 4) {
                        throw new Exception("Incorrect form of data\n");
                    }

                    Vector2 coord = new Vector2(float.Parse(sep_data[0]), float.Parse(sep_data[1]));
                    Complex EM_field = new Complex(Convert.ToDouble(sep_data[2]), Convert.ToDouble(sep_data[3]));
                    DataItem new_obj = new DataItem(coord, EM_field);

                    file_data.Add(new_obj);

                    str_data = streamReader.ReadLine();
                }

                ListData = new List<DataItem>();
                ListData = file_data;

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
                if (fs != null) {
                    fs.Close();
                }
            }
        }

        private class DataEnumerator : IEnumerator<DataItem> {
            private List<DataItem> DataList;

            private int position = -1;

            object IEnumerator.Current => Current;

            public DataItem Current {
                get {
                    DataItem cur = DataList.ElementAt(position);
                    return cur;
                }
            }

            public DataEnumerator(List<DataItem> new_data) {
                DataList = new_data;
            }

            public bool MoveNext() {
                position++;
                if (position < DataList.Count) {
                    return true;
                } else {
                    return false;
                }
            }

            public void Reset() {
                position = -1;
            }

            public void Dispose() {
                DataList = null;
            }
        }

        public void InitRandom(int nItems, float xmax, float ymax, double minValue, double maxValue) {
            Random rnd = new Random();

            for (int i = 0; i < nItems; i++) {
                float x = (float)rnd.NextDouble() * xmax;
                float y = (float)rnd.NextDouble() * (float)ymax;
                double real = rnd.NextDouble() * (maxValue - minValue) + minValue;
                double imag = rnd.NextDouble() * (maxValue - minValue) + minValue;
                Vector2 vec = new Vector2(x, y);
                Complex comp = new Complex(real, imag);
                DataItem tmp = new DataItem(vec, comp);
                ListData.Add(tmp);
            }
        }

        public override Complex[] NearAverage(float eps) {
            double average = 0;
            int num = 0;

            foreach (var item in ListData) {
                num++;
                average += item.EM_field.Real;
            }

            average /= num;

            int counter = 0;
            int capacity = 16;
            Complex[] result = new Complex[capacity];
            foreach (var item in ListData) {
                if (Math.Abs(item.EM_field.Real - average) <= eps) {
                    result[counter] = item.EM_field;
                    counter++;
                }

                if (counter == capacity - 2) {
                    capacity *= 2;
                    Array.Resize(ref result, capacity);
                }
            }

            Array.Resize(ref result, counter);

            return result;
        }


        public override string ToString() {
            return $"Type = V2DataCollection\nInfo = {Info}\nElectromagnetic field frequency = {EM_freq}\n" +
                   $"Number of elems in the List<DataItem> = {ListData.Count}\n";
        }

        public override string ToLongString() {
            string res = this.ToString();

            foreach (var item in ListData) {
                res += $"coord = ({item.Coord.X}, {item.Coord.Y}), value = {item.EM_field}\n";
            }

            return res;
        }

        public override string ToLongString(string format) {
            string res = $"Type = V2DataCollection\nInfo = {Info}\nElectromagnetic field frequency = {EM_freq.ToString(format)}\n" +
                         $"Number of elems in the List<DataItem> = {ListData.Count}\n";
            foreach (var item in ListData) {
                res += item.ToString(format);
            }

            return res;
        }
    }

}
