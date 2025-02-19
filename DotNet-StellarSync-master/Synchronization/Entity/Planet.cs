using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarSync.Synchronization.Entity {
    internal class Planet {
        public required string Name { get; set; }
        public float Radius { get; set; }
        public float AtmosphereRadius { get; set; }
        public bool HasAtmosphere { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
