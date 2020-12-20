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
    struct DataItem {
        public Vector2 Coord { get; set; }
        public Complex EM_field { get; set; }

        public DataItem(Vector2 x, Complex y) {
            Coord = x;
            EM_field = y;
        }

        public override string ToString() {
            return $"Coord = ({Coord.X}, {Coord.Y})\nEM_field = {EM_field}\n";
        }

        public string ToString(string format) {
            return $"Coord = {Coord.ToString(format)}\nEM_field = {EM_field.ToString(format)}\n" +
                   $"Abs of EM_field = {EM_field.Magnitude.ToString(format)}\n";
        }

    }
}
