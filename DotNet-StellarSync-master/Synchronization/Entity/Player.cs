namespace StellarSync.Synchronization.Entity {
    internal class Player {
        public long IdentityId { get; set; }
        public ulong SteamId { get; set; }
        public required string Name { get; set; }
        public string? Faction { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public required string Rank { get; set; }
    }
}
