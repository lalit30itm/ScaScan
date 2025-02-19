using Torch;
using Torch.Views;

namespace StellarSync
{
    public class StellarSyncConfig : ViewModel {

        private bool _synchronizeGrids = true;
        [Display(Order = 0, GroupName = "Synchronization", Name = "Grids", Description = "Should grids be synchronized")]
        public bool SynchronizeGrids { get => _synchronizeGrids; set => SetValue(ref _synchronizeGrids, value); }

        private bool _synchronizePlayers = true;
        [Display(Order = 1, GroupName = "Synchronization", Name = "Players", Description = "Should players be synchronized")]
        public bool SynchronizePlayers { get => _synchronizePlayers; set => SetValue(ref _synchronizePlayers, value); }

        private bool _synchronizePlanets = true;
        [Display(Order = 2, GroupName = "Synchronization", Name = "Planets", Description = "Should planets be synchronized")]
        public bool SynchronizePlanets { get => _synchronizePlanets; set => SetValue(ref _synchronizePlanets, value); }

        private bool _synchronizePlanetsOnlyOnce = true;
        [Display(Order = 3, GroupName = "Synchronization", Name = "Planets Only Once", Description = "Should planets be synchronized only once")]
        public bool SynchronizePlanetsOnlyOnce { get => _synchronizePlanetsOnlyOnce; set => SetValue(ref _synchronizePlanetsOnlyOnce, value); }

        private bool _synchronizeServerDetails = true;
        [Display(Order = 4, GroupName = "Synchronization", Name = "Server Details", Description = "Should server details be synchronized")]
        public bool SynchronizeServerDetails { get => _synchronizeServerDetails; set => SetValue(ref _synchronizeServerDetails, value); }

        private bool _synchronizeServerDetailsOnlyOnce = true;
        [Display(Order = 5, GroupName = "Synchronization", Name = "Server Details Only Once", Description = "Should server details be synchronized only once")]
        public bool SynchronizeServerDetailsOnlyOnce { get => _synchronizeServerDetailsOnlyOnce; set => SetValue(ref _synchronizeServerDetailsOnlyOnce, value); }

        private bool _apiSynchronizationEnabled = false;
        [Display(Order = 0, GroupName = "API Synchronization", Name = "Enabled", Description = "Should synchronization via API be enabled")]
        public bool ApiSynchronizationEnabled { get => _apiSynchronizationEnabled; set => SetValue(ref _apiSynchronizationEnabled, value); }

        private string _syncUrl = "";
        [Display(Order = 1, GroupName = "API Synchronization", Name = "Synchronization URL", Description = "Where to yeet JSON data to")]
        public string SyncURL { get => _syncUrl; set => SetValue(ref _syncUrl, value); }

        private int _syncInterval = 60;
        [Display(Order = 2, GroupName = "API Synchronization", Name = "Synchronization Interval", Description = "How often to synchronize data (in seconds)")]
        public int SyncInterval { get => _syncInterval; set => SetValue(ref _syncInterval, value); }

        private string _lastSynchronizationDate = "";
        [Display(Order = 3, GroupName = "API Synchronization", Name = "Last Synchronization Date", Description = "When was the last time data was synchronized", ReadOnly = true)]
        public string LastSynchronizationDate { get => _lastSynchronizationDate; set => SetValue(ref _lastSynchronizationDate, value); }

        private bool _dbSynchronizationEnabled = false;
        [Display(Order = 0, GroupName = "Database Synchronization (NYI)", Name = "Enabled", Description = "Should synchronization directly to postgres database be enabled", Enabled = false)]
        public bool DbSynchronizationEnabled { get => _dbSynchronizationEnabled; set => SetValue(ref _dbSynchronizationEnabled, value); }
    }
}
