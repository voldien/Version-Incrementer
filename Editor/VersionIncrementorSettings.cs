using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class VersionIncrementorSettings : ScriptableObject
{

	[SerializeField]
	private bool increment_on_play;
	[SerializeField]
	private bool increment_on_build;

	[SerializeField]
	private int m_major;
	[SerializeField]
	private int m_minor;
	[SerializeField]
	private int m_patch;
	[Serializable]
	public enum ReleaseState
	{
		PreAlpha,
		Alpha,
		PreBeta,
		Beta,
		ReleaseCandidate,
		Release,
	}
	[SerializeField]
	private ReleaseState m_releaseStates;

	public static bool IsSettingsAvailable()
	{
		return File.Exists(GetSettingFilePath());
	}

	public static string GetSettingFilePath(){
		//TOOD handle the path for the asset file.
		return "Assets/Editor/VersionIncrementorSettings.asset";
	}

	internal static VersionIncrementorSettings GetOrCreateSettings()
	{

		VersionIncrementorSettings settings = AssetDatabase.LoadAssetAtPath<VersionIncrementorSettings>(GetSettingFilePath());
		if (settings == null)
		{
			/*	Create default setting object.	*/
			settings = ScriptableObject.CreateInstance<VersionIncrementorSettings>();
			settings.increment_on_play = true;
			settings.increment_on_build = true;
			settings.m_major = 0;
			settings.m_minor = 0;
			settings.m_patch = 0;
			settings.m_releaseStates = ReleaseState.PreAlpha;
			AssetDatabase.CreateAsset(settings, GetSettingFilePath());
			AssetDatabase.SaveAssets();
		}
		return settings;
	}

	public string Version { get { return this.GetBundledVersion(); } set { ParsePartVersion(value); } }
	public bool updatedOnPlay { get { return increment_on_play; } }
	public bool updateOnBuild { get { return this.increment_on_build; } }
	public int MajorVersion { get { return this.m_major; } set { this.m_major = value; } }
	public int MinorVersion { get { return this.m_minor; } set { this.m_minor = value; } }
	public int PatchVersion { get { return this.m_patch; } set { this.m_patch = value; } }

	public ReleaseState State { get { return this.m_releaseStates; } }
	public string StateAbbr { get { return GetReleaseAbbr(State); } }
	private string GetReleaseAbbr(ReleaseState state)
	{
		switch (state)
		{
			case ReleaseState.PreAlpha:
				return "pa";
			case ReleaseState.Alpha:
				return "a";
			case ReleaseState.PreBeta:
				return "pb";
			case ReleaseState.Beta:
				return "b";
			case ReleaseState.ReleaseCandidate:
				return "rc";
			case ReleaseState.Release:
				return "r";
			default:
				throw new ArgumentException("Invalid release state.");
		}
	}

	private string GetBundledVersion()
	{
		/*	Recompute version string.	*/
		return m_major.ToString("0") + "." +
			  m_minor.ToString("0") + "." +
			  m_patch.ToString("000") + "." +
			  this.StateAbbr;
	}

	private int[] ParsePartVersion(string version)
	{
		int[] part = new int[4];

		/*	*/
		string[] lines = version.Split('.');
		int MajorVersion = int.Parse(lines[0]);
		int MinorVersion = int.Parse(lines[1]);
		int SubMinorVersion = int.Parse(lines[2]);
		int SubVersionText = 0;//int.Parse(lines[3].Trim());

		/*	*/
		part[0] = MajorVersion;
		part[1] = MinorVersion;
		part[2] = SubMinorVersion;
		part[3] = SubVersionText;
		return part;
	}

	internal static SerializedObject GetSerializedSettings()
	{
		return new SerializedObject(GetOrCreateSettings());
	}

	public bool isValidVersion()
	{
		return true;
	}

	internal void setState(ReleaseState state)
	{
		this.m_releaseStates = state;
	}
}