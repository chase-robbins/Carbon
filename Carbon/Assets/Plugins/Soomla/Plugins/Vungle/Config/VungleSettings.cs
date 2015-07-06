/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
using System.IO;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Soomla.Vungle
{

	#if UNITY_EDITOR
	[InitializeOnLoad]
	#endif
	/// <summary>
	/// This class holds the store's configurations.
	/// </summary>
	public class VungleSettings : ISoomlaSettings
	{

		#if UNITY_EDITOR
		static VungleSettings instance = new VungleSettings();
		static VungleSettings()
		{
			SoomlaEditorScript.addSettings(instance);
		}

		GUIContent vungleAppIdAndroidLabel = new GUIContent("Vungle App ID - Android [?]:", "Your Vungle app ID for Android (found on the Vungle dashboard).");
		GUIContent vungleAppIdiOSLabel = new GUIContent("Vungle App ID - iOS [?]:", "Your Vungle app ID for iOS (found on the Vungle dashboard).");

		public void OnEnable() {
			// No enabling, leave empty and let StoreManifestTools do the work
		}

		public void OnModuleGUI() {
		}

		public void OnInfoGUI() {
		}

		public void OnSoomlaGUI() {

			///
			/// Create Vungle App ID labels and text fields
			///
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(vungleAppIdAndroidLabel,  GUILayout.Width(150), SoomlaEditorScript.FieldHeight);
			VungleAppIdAndroid = EditorGUILayout.TextField(VungleAppIdAndroid, SoomlaEditorScript.FieldHeight);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(vungleAppIdiOSLabel,  GUILayout.Width(150), SoomlaEditorScript.FieldHeight);
			VungleAppIdiOS = EditorGUILayout.TextField(VungleAppIdiOS, SoomlaEditorScript.FieldHeight);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
		}
		#endif

		public static string VUNGLE_APP_ID_ANDROID_DEFAULT_MESSAGE = "[YOUR VUNGLE APP ID FOR ANDROID]";
		public static string VUNGLE_APP_ID_IOS_DEFAULT_MESSAGE = "[YOUR VUNGLE APP ID FOR IOS]";

		public static string VungleAppIdAndroid
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("VungleAppIdAndroid", out value) ? value : VUNGLE_APP_ID_ANDROID_DEFAULT_MESSAGE;
			}
			set
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("VungleAppIdAndroid", out v);
				if (v != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("VungleAppIdAndroid",value);
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static string VungleAppIdiOS
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("VungleAppIdiOS", out value) ? value : VUNGLE_APP_ID_IOS_DEFAULT_MESSAGE;
			}
			set
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("VungleAppIdiOS", out v);
				if (v != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("VungleAppIdiOS",value);
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

	}
}
