namespace StellarSync.Synchronization.Entity {
    internal class Grid {
        public required string Name { get; set; }
        public long EntityId { get; set; }
        public string? CustomData { get; set; }
        public int BlockCount { get; set; }
        public int PCU { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public required List<long> Owners { get; set; }
        public bool IsPowered { get; set; }
        public bool IsParked { get; set; }
        public bool IsStatic { get; set; }
        public float GridSize { get; set; }
        public float Mass { get; set; }
    }
}
