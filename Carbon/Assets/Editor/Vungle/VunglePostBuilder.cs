using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.IO;
using System.Diagnostics;


public class VunglePostBuilder : MonoBehaviour
{
	[PostProcessBuild]
	private static void onPostProcessBuildPlayer( BuildTarget target, string pathToBuiltProject )
	{
		if( target == BuildTarget.iOS )
		{
			// grab the path to the postProcessor.py file
			var scriptPath = Path.Combine( Application.dataPath, "Editor/Vungle/VunglePostProcessor.py" );

			// sanity check
			if( !File.Exists( scriptPath ) )
			{
				UnityEngine.Debug.LogError( "Vungle post builder could not find the VunglePostProcessor.py file. Did you accidentally delete it?" );
				return;
			}

			var pathToNativeCodeFiles = Path.Combine( Application.dataPath, "Editor/Vungle/VungleSDK" );

			var args = string.Format( "\"{0}\" \"{1}\" \"{2}\"", scriptPath, pathToBuiltProject, pathToNativeCodeFiles );
			var proc = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "python2.6",
					Arguments = args,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				}
			};

			proc.Start();
		}
	}
}
