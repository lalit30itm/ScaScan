using Sandbox.Game.Entities;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using VRage.Game.ModAPI;
using Sandbox.Game.World;
using StellarSync.Synchronization.Entity;
using System.Diagnostics;

namespace StellarSync.Synchronization {
    internal class Synchronizer {

        private static System.Timers.Timer? _syncTimer;
        private static readonly Stopwatch _timer = new();
        private static readonly HttpClient _httpClient = new();

        private static bool _planetsSynced = false;
        private static bool _serverDetailsSynced = false;

        private static bool SendToServer(Payload payload) {
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = _httpClient.PostAsync(StellarSyncPlugin.Instance.Config.SyncURL, content).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK) {
                if (payload.Planets != null && payload.Planets.Count > 0) _planetsSynced = true;
                if (payload.Server != null) _serverDetailsSynced = true;
                return true;
            }

            StellarSyncPlugin.Log.Warn($"Data not synced, response = {result.StatusCode}");
            return false;
            
        }

        public static void SetupTimer() {
            DisposeTimer();
            _syncTimer = new System.Timers.Timer(StellarSyncPlugin.Instance.Config.SyncInterval * 1000);
            _syncTimer.Elapsed += (sender, e) => Synchronize();
            _syncTimer.AutoReset = true;
            _syncTimer.Enabled = true;
        }

        public static void DisposeTimer() {
            if (_syncTimer == null) return;
            _syncTimer.Stop();
            _syncTimer.Dispose();
        }

        public static void ResetOnceFlags() {
            _planetsSynced = false;
            _serverDetailsSynced = false;
        }

        private static void Synchronize() {
            _timer.Restart();
            Payload payload = new();
            if (StellarSyncPlugin.Instance.Config.SynchronizeGrids) CollectGrids(payload);
            if (StellarSyncPlugin.Instance.Config.SynchronizePlayers) CollectPlayers(payload);
            if (StellarSyncPlugin.Instance.Config.SynchronizePlanets) CollectPlanets(payload);
            if (StellarSyncPlugin.Instance.Config.SynchronizeServerDetails) CollectServerDetails(payload);
            StellarSyncPlugin.Log.Info($"Data collected in {_timer.Elapsed}");

            if (StellarSyncPlugin.Instance.Config.ApiSynchronizationEnabled && SendToServer(payload)) {
                StellarSyncPlugin.Instance.Config.LastSynchronizationDate = DateTime.Now.ToString();
            }            
        }

        private static void CollectGrids(Payload payload) {
            foreach (var entity in MyEntities.GetEntities()) {
                if (entity is not MyCubeGrid grid || grid.Projector != null)
                    continue;

                Grid trackedGrid = new() {
                    Name = grid.DisplayName,
                    EntityId = grid.EntityId,
                    Owners = grid.SmallOwners,
                    BlockCount = grid.BlocksCount,
                    PCU = grid.BlocksPCU,
                    GridSize = grid.GridSize,
                    IsParked = grid.IsParked,
                    IsPowered = grid.IsPowered,
                    IsStatic = grid.IsStatic,
                    Mass = grid.Mass,
                    X = grid.PositionComp.WorldVolume.Center.X,
                    Y = grid.PositionComp.WorldVolume.Center.Y,
                    Z = grid.PositionComp.WorldVolume.Center.Z
                };
                payload.Grids.Add(trackedGrid);
            }
        }

        private static void CollectPlayers(Payload payload) {
            var players = new List<IMyPlayer>(MySession.Static.Players.GetOnlinePlayers());
            foreach (var player in players) {
                Player trackedPlayer = new() {
                    IdentityId = player.IdentityId,
                    Rank = player.PromoteLevel.ToString(),
                    SteamId = player.SteamUserId,
                    Name = player.DisplayName,
                    X = player.GetPosition().X,
                    Y = player.GetPosition().Y,
                    Z = player.GetPosition().Z
                };
                IMyFaction faction = MySession.Static.Factions.TryGetPlayerFaction(player.IdentityId);
                if (faction != null) {
                    trackedPlayer.Faction = faction.Name;
                }
                payload.Players.Add(trackedPlayer);
            }
        }

        private static void CollectPlanets(Payload payload) {
            if (_planetsSynced && StellarSyncPlugin.Instance.Config.SynchronizePlanetsOnlyOnce) return;
            var planets = MyEntities.GetEntities().OfType<MyPlanet>().ToList();            
            foreach (var planet in planets) {                
                Planet trackedPlanet = new() {
                    Name = planet.Name,
                    Radius = planet.AverageRadius,
                    AtmosphereRadius = planet.AtmosphereRadius,
                    HasAtmosphere = planet.HasAtmosphere,
                    X = planet.LocationForHudMarker.X,
                    Y = planet.LocationForHudMarker.Y,
                    Z = planet.LocationForHudMarker.Z
                };
                payload.Planets.Add(trackedPlanet);
            }
        }

        private static void CollectServerDetails(Payload payload) {
            if (_serverDetailsSynced && StellarSyncPlugin.Instance.Config.SynchronizeServerDetailsOnlyOnce) return;
            var im = StellarSyncPlugin.InstanceManager;
            if (im == null) return;

            payload.Server = new() {
                Ip = im.DedicatedConfig.IP,
                Port = im.DedicatedConfig.Port,
                WorldName = im.DedicatedConfig.WorldName,
                ServerName = im.DedicatedConfig.ServerName,
                ServerDescription = im.DedicatedConfig.ServerDescription
            };
                
            foreach (var mod in MySession.Static.Mods) {
                payload.Server.Mods.Add(new Mod {
                    Name = mod.FriendlyName,
                    WorkshopId = mod.GetWorkshopId().Id
                });
            }

            var NexusAPI = StellarSyncPlugin.NexusAPI;
            if (NexusAPI == null || !NexusAPI.IsRunningNexus()) return;

            var currentSector = NexusAPI.GetSectors().Find(sector => sector.ServerID == NexusAPI.GetThisServer().ServerID);
            if (currentSector == null) return;

            payload.Server.Nexus = new() {
                SectorName = NexusAPI.GetThisServer().Name,
                IPAddress = currentSector.IPAddress,
                Port = currentSector.Port,
                IsGeneralSpace = currentSector.IsGeneralSpace,
                X = currentSector.Center.X,
                Y = currentSector.Center.Y,
                Z = currentSector.Center.Z,
                Radius = currentSector.Radius,
                ServerID = NexusAPI.GetThisServer().ServerID
            };

        }
    }
}
