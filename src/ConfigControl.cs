using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KroesTerminal
{
    internal class ConfigControl
    {
        private ConfigEntry<bool> QuotaUICfg;

		private ConfigEntry<bool> QuotaNotifCfg;

		private ConfigEntry<bool> KScanCfg;

		private ConfigEntry<bool> KItemsCfg;

		private ConfigEntry<bool> KEnemyCfg;

		private ConfigEntry<bool> KEnemyPeacefulCfg;

		internal bool enableQuotaUI { get => QuotaUICfg.Value; set => QuotaUICfg.Value = value; }

		internal bool QuotaNotif { get => QuotaNotifCfg.Value; set => QuotaNotifCfg.Value = value; }

		internal bool KScan { get => KScanCfg.Value; set =>	KScanCfg.Value = value; }

		internal bool KItems { get => KItemsCfg.Value; set => KItemsCfg.Value = value; }
		internal bool KEnemy { get => KEnemyCfg.Value; set => KEnemyCfg.Value = value; }

		internal bool EnemyPeaceful { get => KEnemyPeacefulCfg.Value; set => KEnemyPeacefulCfg.Value = value; }

		public ConfigControl(ConfigFile config)
		{
			QuotaUICfg = config.Bind<bool>("Features", "Enable Quota UI", true);
			QuotaNotifCfg = config.Bind<bool>("Features", "Enable Quota notification", true);
			KScanCfg = config.Bind<bool>("Terminal Commands", "Allow KSCAN command", true);
			KItemsCfg = config.Bind<bool>("Terminal Commands", "Allow KITEMS command", true);
			KEnemyCfg = config.Bind<bool>("Terminal Commands", "Allow KENEMY command", true);
			KEnemyPeacefulCfg = config.Bind<bool>("Terminal Commands", "List non-hostile enemies", true);
		}
    }
}
