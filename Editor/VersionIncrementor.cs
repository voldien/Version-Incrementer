using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Callbacks;

[InitializeOnLoad]
public class VersionIncrementor
{
	static VersionIncrementor()
	{
		/*	Update when editor is updated.	*/
		EditorApplication.update += RunOnce;
	}

	static void RunOnce()
	{
		/*	*/
		EditorApplication.update -= RunOnce;
		/*	*/
		if (VersionIncrementorSettings.GetOrCreateSettings().updatedOnPlay)
			ReadVersionAndIncrement();
	}

	[PostProcessBuildAttribute(1)]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
	{
		/*	Updated patch by default whe updating.	*/
		if (VersionIncrementorSettings.GetOrCreateSettings().updateOnBuild)
			IncreaseBuild();

		/*	*/
		Debug.Log("Build v:" + PlayerSettings.bundleVersion + " (" + PlayerSettings.Android.bundleVersionCode + ")");
	}

	static void ReadVersionAndIncrement()
	{

		IncrementVersion(0, 0, 1);
		VersionIncrementorSettings settings = VersionIncrementorSettings.GetOrCreateSettings();
		/*	Display the information.	*/
		if (settings != null)
		{
			Debug.Log("Major, Minor, SubMinor, SubVerLetter: " + settings.MajorVersion + " " + settings.MinorVersion + " " + settings.PatchVersion + " " + settings.State.ToString());
		}
	}

	static void IncrementVersion(int majorIncr, int minorIncr, int buildIncr)
	{

		VersionIncrementorSettings settings = null;
		try{
			settings = VersionIncrementorSettings.GetOrCreateSettings();

			/*	Increment the the version.	*/
			settings.MajorVersion += majorIncr;
			settings.MinorVersion += minorIncr;
			settings.PatchVersion += buildIncr;

			/*	Update project version settings.	*/
			PlayerSettings.bundleVersion = settings.Version;
			PlayerSettings.Android.bundleVersionCode = settings.MajorVersion * 10000 + settings.MinorVersion * 1000 + settings.PatchVersion;
		}finally{

			/*	Update version settings object.	*/
			if(settings != null){
				EditorUtility.SetDirty(settings);
			}
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}

	private static void SetReleaseState(VersionIncrementorSettings.ReleaseState state)
	{

		VersionIncrementorSettings settings = null;
		try {
			settings = VersionIncrementorSettings.GetOrCreateSettings();
			settings.setState(state);

			PlayerSettings.bundleVersion = settings.Version;
			PlayerSettings.Android.bundleVersionCode = settings.MajorVersion * 10000 + settings.MinorVersion * 1000 + settings.PatchVersion;
		}
		finally
		{
			/*	Update version settings object.	*/
			if (settings != null)
			{
				EditorUtility.SetDirty(settings);
			}
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}

	[MenuItem("Build/Increase Minor Version", false, 1)]
	private static void IncreaseMinor()
	{
		IncrementVersion(0, 1, 0);
	}

	[MenuItem("Build/Increase Major Version", false, 0)]
	private static void IncreaseMajor()
	{
		IncrementVersion(1, 0, 0);
	}

	[MenuItem("Build/Increase Patch Version", false, 2)]
	private static void IncreaseBuild()
	{
		IncrementVersion(0, 0, 1);
	}

	[MenuItem("Build/Change Release State/PreAlpha", false, 15)]
	private static void SetPreAlpha()
	{
		SetReleaseState(VersionIncrementorSettings.ReleaseState.PreAlpha);
	}

	[MenuItem("Build/Change Release State/Alpha", false, 14)]
	private static void SetAlpha()
	{
		SetReleaseState(VersionIncrementorSettings.ReleaseState.Alpha);
	}

	[MenuItem("Build/Change Release State/PreBeta", false, 13)]
	private static void SetPreBeta()
	{
		SetReleaseState(VersionIncrementorSettings.ReleaseState.PreBeta);
	}

	[MenuItem("Build/Change Release State/Beta", false, 12)]
	private static void SetBeta()
	{
		SetReleaseState(VersionIncrementorSettings.ReleaseState.Beta);
	}

	[MenuItem("Build/Change Release State/ReleaseCandiate", false, 11)]
	private static void SetReleaseCandiate()
	{
		SetReleaseState(VersionIncrementorSettings.ReleaseState.ReleaseCandidate);
	}

	[MenuItem("Build/Change Release State/Release", false, 10)]
	private static void SetRelease()
	{
		SetReleaseState(VersionIncrementorSettings.ReleaseState.Release);
	}
}