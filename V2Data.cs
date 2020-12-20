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
    abstract class V2Data : IEnumerable<DataItem>, INotifyPropertyChanged {
        private string info;
        private double EM_freq;

        public V2Data(string x, double y) {
            Info = x;
            EM_freq = y;
        }

        public abstract IEnumerator<DataItem> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(object source, string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(source, PropertyChangedEventArgs(propertyName));
            }
        }

        public string Info {
            get {
                return info;
            }
            set {
                info = value;
                OnPropertyChanged(this, "Info");
            }
        }

        public double EM_Freq {
            get {
                return EM_freq;
            }
            set {
                EM_freq = value;
                OnPropertyChanged(this, "EM_Freq");
            }
        }

        public abstract Complex[] NearAverage(float eps);
        public abstract string ToLongString();

        public override string ToString() {
            return $"Info\nElectromagnetic field frequency = {EM_freq}\n";
        }

        public abstract string ToLongString(string format);
    }
}
