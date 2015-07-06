using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;

namespace Soomla.Vungle
{
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public class VungleManifestTools : ISoomlaManifestTools
    {
#if UNITY_EDITOR
		static VungleManifestTools instance = new VungleManifestTools();
		static VungleManifestTools()
		{
			SoomlaManifestTools.ManTools.Add(instance);
		}

		public void UpdateManifest() {
			SoomlaManifestTools.SetPermission("android.permission.INTERNET");
			SoomlaManifestTools.SetPermission("android.permission.WRITE_EXTERNAL_STORAGE");
			SoomlaManifestTools.SetPermission("android.permission.ACCESS_NETWORK_STATE");

			SoomlaManifestTools.AddActivity("com.vungle.publisher.FullScreenAdActivity", new Dictionary<string, string>() {
				{ "configChanges", "keyboardHidden|orientation|screenSize" },
				{ "theme", "@android:style/Theme.NoTitleBar.Fullscreen" }
			});

			SoomlaManifestTools.AppendApplicationElement("service", "com.vungle.publisher.VungleService", new Dictionary<string, string>() {
				{ "exported", "false" }
			});

			SoomlaManifestTools.AddMetaDataTag("com.google.android.gms.version", "@integer/google_play_services_version");
		}

#endif
	}
}
