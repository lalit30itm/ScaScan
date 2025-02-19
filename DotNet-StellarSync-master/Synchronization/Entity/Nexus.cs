namespace StellarSync.Synchronization.Entity {
    internal class Nexus {        
        public required string SectorName { get; set; }
        public required string IPAddress { get; set; }
        public required int Port { get; set; }
        public required bool IsGeneralSpace { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public required double Radius { get; set; }
        public required int ServerID { get; set; }
    }
}
