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
    delegate void DataChangedEventHandler(object source, DataChangedEventArgs args);

    public enum ChangeInfo {
        ItemChanged, Add, Remove, Replace
    }

    class DataChangedEventArgs {
        public ChangeInfo changedInfo { get; set; }

        public double value { get; set; }

        DataChangedEventArgs(ChangeInfo a, double b) {
            changedInfo = a;
            value = b;
        }

        public override string ToString() {
            return $"Changed info: {changedInfo.ToString()}\nValue = {value}\n";
        }
    }
}
