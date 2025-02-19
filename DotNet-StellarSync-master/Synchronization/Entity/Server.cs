namespace StellarSync.Synchronization.Entity {
    internal class Server {
        public required string Ip { get; set; }

        public required int Port { get; set; }
        public required string WorldName { get; set; }
        public required string ServerName { get; set; }
        public required string ServerDescription { get; set; }
        public List<Mod> Mods = [];
        public Nexus? Nexus;
        
    }
}
