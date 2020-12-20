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
    struct Grid1D {
        public float Step { get; set; }
        public int NodeNum { get; set; }

        public Grid1D(float x, int y) {
            Step = x;
            NodeNum = y;
        }

        public override string ToString() {
            return $"Step = {Step}\nNumber of nodes = {NodeNum}\n";
        }

        public string ToString(string format) {
            return $"Step = {Step.ToString(format)}\nNumber of nodes = {NodeNum}\n"; // NodeNum int, поэтому форматирования не требуется
        }
    }
}
