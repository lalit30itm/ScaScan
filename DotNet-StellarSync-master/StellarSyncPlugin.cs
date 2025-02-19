using NLog;
using Sandbox;
using Sandbox.Game.World;
using StellarSync.Synchronization;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using Torch;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Plugins;
using Torch.API.Session;
using Torch.Managers.PatchManager;
using Torch.Managers;
using Torch.Server.Managers;
using Torch.Session;
using Torch.Views;

namespace StellarSync
{
    public class StellarSyncPlugin : TorchPluginBase, IWpfPlugin
    {
        public static StellarSyncPlugin Instance;
        public StellarSyncConfig Config => _config.Data;
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public static InstanceManager? InstanceManager;
        public static NexusAPI? NexusAPI { get; private set; }
        private static readonly Guid NexusGUID = new("28a12184-0422-43ba-a6e6-2e228611cca5");

        private Persistent<StellarSyncConfig> _config = null!;
        private TorchSessionManager? _sessionManager;
        private bool InitPlugins = false;

        public override void Init(ITorchBase torch) {
            base.Init(torch);
            Instance = this;
            _config = Persistent<StellarSyncConfig>.Load(Path.Combine(StoragePath, "StellarSync.cfg"));

            _sessionManager = Torch.Managers.GetManager<TorchSessionManager>();
            if (_sessionManager != null)
            {
                Torch.GameStateChanged += Torch_GameStateChanged;
                _sessionManager.SessionStateChanged += SessionChanged;
            }
            
            InstanceManager = torch.Managers.GetManager<InstanceManager>();
            
        }

        private void SessionChanged(ITorchSession session, TorchSessionState newState) {
            if (newState == TorchSessionState.Unloading || newState == TorchSessionState.Unloaded) {
                Synchronizer.DisposeTimer();
            }
        }

        public UserControl GetControl() => new PropertyGrid {
            Margin = new(3),
            DataContext = _config.Data
        };

        private void Torch_GameStateChanged(MySandboxGame game, TorchGameState newState) {
            if (newState == TorchGameState.Loaded) {
                Synchronizer.SetupTimer();
            } else {
                Synchronizer.DisposeTimer();
                Synchronizer.ResetOnceFlags();
            }
        }

        public void InitPluginDependencies(PluginManager Plugins, PatchManager Patches) {
            if (Plugins.Plugins.TryGetValue(NexusGUID, out ITorchPlugin torchPlugin)) {
                Type type = torchPlugin.GetType();
                Type? type2 = type?.Assembly.GetType("Nexus.API.PluginAPISync");
                if (type2 != null) {
                    type2.GetMethod("ApplyPatching", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null,
                    [
                        typeof(NexusAPI),
                        "Distress"
                    ]);
                    NexusAPI = new NexusAPI(4399);
                }
            }
        }
        
        public override void Update() {
            if (!InitPlugins) {
                InitPluginDependencies(Torch.Managers.GetManager<PluginManager>(), Torch.Managers.GetManager<PatchManager>());
                InitPlugins = true;
            }
        }
    }
}