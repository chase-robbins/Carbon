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
using System;
using System.Runtime.InteropServices;
using Soomla;

namespace Soomla.Vungle
{
	/// <summary>
	/// This class provides the API for working with the SOOMLA Vungle plugin.
	/// Use it to initialize Vungle and play video ads in return for rewards.
	/// It must be initialized after <c>SoomlaStore</c>.
	/// </summary>
	public class SoomlaVungle : MonoBehaviour {
#if UNITY_IOS && !UNITY_EDITOR
		[DllImport ("__Internal")]
		private static extern void soomlaVungle_Init(string appId);
		[DllImport ("__Internal")]
		private static extern void soomlaVungle_PlayAd(bool enableBackButton);
		[DllImport ("__Internal")]
		[return:MarshalAs(UnmanagedType.Bool)]
		private static extern bool soomlaVungle_HasAds();
#endif

		private const string TAG = "SOOMLA SoomlaVungle";
		
		private static SoomlaVungle instance = null;
		
		/// <summary>
		/// Initializes game state before the game starts.
		/// </summary>
		void Awake(){
			if(instance == null){ 	// making sure we only initialize one instance.
				instance = this;
				GameObject.DontDestroyOnLoad(this.gameObject);
				Initialize();
			} else {				// Destroying unused instances.
				GameObject.Destroy(this.gameObject);
			}
		}

		/// <summary>
		/// Initializes the SOOMLA Vungle plugin.
		/// </summary>
		public static void Initialize() {

#if UNITY_ANDROID && !UNITY_EDITOR
			SoomlaUtils.LogDebug(TAG, "Starting Vungle for: " + VungleSettings.VungleAppIdAndroid);
			AndroidJNI.PushLocalFrame(100);
			// Initializing SoomlaEventHandler
			using(AndroidJavaClass jniEventHandler = new AndroidJavaClass("com.soomla.core.unity.SoomlaVungleEventHandler")) {
				jniEventHandler.CallStatic("initialize");
			}
			using(AndroidJavaClass jniSoomlaVungleClass = new AndroidJavaClass("com.soomla.plugins.ads.vungle.SoomlaVungle")) {
				using(AndroidJavaObject jniSoomlaVungle = jniSoomlaVungleClass.CallStatic<AndroidJavaObject>("getInstance")) {
					jniSoomlaVungle.Call("initialize", VungleSettings.VungleAppIdAndroid);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
			SoomlaUtils.LogDebug(TAG, "Starting Vungle for: " + VungleSettings.VungleAppIdiOS);
			soomlaVungle_Init(VungleSettings.VungleAppIdiOS);
#endif
		}

		/// <summary>
		/// Plays a video ad and grants the user a reward for watching it.
		/// </summary>
		/// <param name="reward">The reward that will be given to users for watching the video ad.</param>
		/// <param name="enableBackButton">Determines whether you would like to give the user the
		/// option to skip out of the video. <c>true</c> means a close button will be displayed.</param>
		public static void PlayAd(Reward reward, bool enableBackButton) {
			SoomlaUtils.LogDebug(TAG, "Playing Vungle Ad");

			savedReward = reward;

#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaVungleClass = new AndroidJavaClass("com.soomla.plugins.ads.vungle.SoomlaVungle")) {
				using(AndroidJavaObject jniSoomlaVungle = jniSoomlaVungleClass.CallStatic<AndroidJavaObject>("getInstance")) {
					jniSoomlaVungle.Call("playIncentivisedAd", enableBackButton, true, null);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
			soomlaVungle_PlayAd(enableBackButton);
#endif
		}

		/// <summary>
		/// Plays a video ad and grants the user a reward for watching it.
		/// </summary>
		/// <param name="reward">The reward that will be given to users for watching the video ad.</param>
		/// <param name="enableBackButton">Determines whether you would like to give the user the
		/// option to skip out of the video. <c>true</c> means a close button will be displayed.</param>
		public static bool HasAds() {
			SoomlaUtils.LogDebug(TAG, "Checking if Vungle has Ads to show.");
			
#if UNITY_ANDROID && !UNITY_EDITOR
			bool answer = false;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaVungleClass = new AndroidJavaClass("com.soomla.plugins.ads.vungle.SoomlaVungle")) {
				using(AndroidJavaObject jniSoomlaVungle = jniSoomlaVungleClass.CallStatic<AndroidJavaObject>("getInstance")) {
					answer = jniSoomlaVungle.Call<bool>("hasAds");
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);

			return answer;
#elif UNITY_IOS && !UNITY_EDITOR
			return soomlaVungle_HasAds();
#else
			return false;
#endif
		}




		public void onVungleAdEnd(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onVungleAdEnd:" + message);
			
			SoomlaVungle.OnVungleAdEnd();
		}


		public void onVungleAdStart(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onVungleAdStart:" + message);
			
			SoomlaVungle.OnVungleAdStart();
		}

		public void onVungleHasAds(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onVungleHasAds:" + message);
			
			SoomlaVungle.OnVungleHasAds();
		}

		public void onVungleAdViewed(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onVungleAdViewed:" + message);

			JSONObject eventJSON = new JSONObject(message);
			bool completed = eventJSON["completed"].b;
			double timeWatched = eventJSON["timeWatched"].n;

			if (completed && savedReward != null) {
				savedReward.Give();
				savedReward = null;
			}

			SoomlaVungle.OnVungleAdViewed(completed, timeWatched);
		}

		public delegate void Action();
		
		public static Action OnVungleAdEnd = delegate {};
		public static Action OnVungleAdStart = delegate {};
		public static Action OnVungleHasAds = delegate {};
		public static Action<bool, double> OnVungleAdViewed = delegate {};

		private static Reward savedReward;

	}
}
