namespace StellarSync.Synchronization.Entity {
    internal class Payload {
        public List<Grid> Grids = [];
        public List<Player> Players = [];
        public List<Planet> Planets = [];
        public Server? Server;
    }
}
