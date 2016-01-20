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

	[MenuItem ("AutoBuilder/PrefabFromVox")]
	static void DoCreateSimplePrefab()
	{
		Object[] objs = Resources.LoadAll("Vox");
		int newCount = 0;
		int updateCount = 0;
		foreach (Object obj in objs) {
			GameObject src = obj as GameObject;
			if( src )
			{
				bool hasMesh = src.GetComponent<MeshFilter>() != null;
				bool hasChildMesh = src.GetComponentInChildren<MeshFilter>() != null;
				//Debug.Log(src +" "+hasMesh+" "+hasChildMesh);
				if( !hasMesh && hasChildMesh )
				{
					string prefabName = "Imported/"+obj.name;
					GameObject prefab = Resources.Load(prefabName, typeof(GameObject)) as GameObject;
					GameObject go = null;
					if( prefab == null )
					{
						//Debug.Log("Created "+prefabName);
						newCount++;
						go = GameObject.Instantiate(src);
						if( src.name.StartsWith("actor") )
						{
							go.transform.localScale = Vector3.one*5f;
							Actor a = go.AddComponent<Actor>();
							Engine e = go.AddComponent<Engine>();
							e.actor = a;
							Hero h = go.AddComponent<Hero>();
							h.actor = a;
							a.engine = e;
							a.hero = h;
							GameObject hb = new GameObject("HealthBar");
							hb.AddComponent<HealthBar>();
							hb.AddComponent<SpriteRenderer>();
							hb.transform.parent = go.transform;
							Transform bodyT = go.transform.Find("default");
							bodyT.gameObject.name = "actorbody";
							a.actorBody = bodyT.gameObject.AddComponent<ActorBody>();
						}
						if( src.name.StartsWith("terrain") )
						{
							go.transform.localScale = Vector3.one*10f;
							go.AddComponent<Tile>();
							Transform bodyT = go.transform.Find("default");
							bodyT.gameObject.name = "terrainbody";
						}
						if( src.name.StartsWith("prop") )
						{
							if( src.name.Contains("house") )
							{
								go.AddComponent<Kingdom>();
							}
							else
							{
								go.AddComponent<TileContents>();
							}
							Transform bodyT = go.transform.Find("default");
							bodyT.gameObject.name = "propbody";
						}
					}
					else
					{
						//Debug.Log("Updated"+prefabName);
						updateCount++;
						go = GameObject.Instantiate(prefab);
						GameObject other = GameObject.Instantiate(src);
						int children = go.transform.childCount;
				        for (int i = children-1; i >= 0; --i)
				        {
				        	Transform child = go.transform.GetChild(i);
				        	if( child.gameObject.name.EndsWith("body"))
				        	{
								GameObject.DestroyImmediate(go.transform.GetChild(i).gameObject);
							}
						}
						children = other.transform.childCount;
						for (int i = children-1; i >= 0; --i)
				        {
				        	Transform child = other.transform.GetChild(i);
				        	child.SetParent(go.transform);
							child.localScale = Vector3.one;
						}
						if( src.name.StartsWith("actor") )
						{
							Actor a = go.GetComponent<Actor>();
							Transform bodyT = go.transform.Find("default");
							bodyT.gameObject.name = "actorbody";
							a.actorBody = bodyT.gameObject.AddComponent<ActorBody>();
						}
						if( src.name.StartsWith("terrain") )
						{
							Transform bodyT = go.transform.Find("default");
							bodyT.gameObject.name = "terrainbody";
						}
						if( src.name.StartsWith("prop") )
						{
							Transform bodyT = go.transform.Find("default");
							bodyT.gameObject.name = "propbody";
						}
						GameObject.DestroyImmediate(other);

					}
					PrefabUtility.CreatePrefab("Assets/Resources/"+prefabName+".prefab", go, ReplacePrefabOptions.Default);
					GameObject.DestroyImmediate(go);
			    	
			    }
			}
			
	    }
	    Debug.Log("Crunched "+newCount+" New, "+updateCount+" Updated");
	}

	[MenuItem("AutoBuilder/GetVox")]
	static void GetVox ()
	{
		try
		{
			System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
			myProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
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
		catch (System.Exception e)
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