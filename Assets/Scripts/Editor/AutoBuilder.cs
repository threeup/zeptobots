/* 
AutoBuilder.cs
Automatically changes the target platform and creates a build.
 
Installation
Place in an Editor folder.
 
Usage
Go to File > AutoBuilder and select a platform. These methods can also be run from the Unity command line using -executeMethod AutoBuilder.MethodName.
 
License
Copyright (C) 2011 by Thinksquirrel Software, LLC
 
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
 
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
 
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
 */
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System;

public static class AutoBuilder {
 
	static string GetProjectName()
	{
		string[] s = Application.dataPath.Split('/');
		return s[s.Length - 2];
	}
 
	static string[] GetScenePaths()
	{
		string[] scenes = new string[EditorBuildSettings.scenes.Length];
 
		for(int i = 0; i < scenes.Length; i++)
		{
			scenes[i] = EditorBuildSettings.scenes[i].path;
		}
 
		return scenes;
	}

	[MenuItem("AutoBuilder/GetVox")]
	static void GetVox ()
	{
		try
		{
			Process myProcess = new Process();
			myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
			myProcess.StartInfo.CreateNoWindow = false;
			myProcess.StartInfo.UseShellExecute = false;
			myProcess.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
			string path = "getvox.bat";
			myProcess.StartInfo.Arguments = "/c" + path;
			myProcess.EnableRaisingEvents = true;

			myProcess.Start();
			myProcess.WaitForExit();
			int exitCode = myProcess.ExitCode;
			UnityEngine.Debug.Log(exitCode);  
		} 
		catch (Exception e)
		{
		 	UnityEngine.Debug.Log(e);        
		}
	}
 
	[MenuItem("AutoBuilder/Windows/32-bit")]
	static void PerformWinBuild ()
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneWindows);
		BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Win/" + GetProjectName() + ".exe",BuildTarget.StandaloneWindows,BuildOptions.None);
	}
 
	[MenuItem("AutoBuilder/Windows/64-bit")]
	static void PerformWin64Build ()
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneWindows);
		BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Win64/" + GetProjectName() + ".exe",BuildTarget.StandaloneWindows64,BuildOptions.None);
	}
 
	[MenuItem("AutoBuilder/Web/Standard")]
	static void PerformWebBuild ()
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WebPlayer);
		BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Web",BuildTarget.WebPlayer,BuildOptions.None);
	}
	[MenuItem("AutoBuilder/Web/Streamed")]
	static void PerformWebStreamedBuild ()
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WebPlayerStreamed);
		BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Web-Streamed",BuildTarget.WebPlayerStreamed,BuildOptions.None);
	}
}