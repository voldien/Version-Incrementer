using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

class VersionIncrementorSettingsProvider : SettingsProvider
{
	private SerializedObject m_VersionIncrementorSettings;

	class Styles
	{
		public static GUIContent incrementOnPlay = new GUIContent("Increment on Play", "The patch will be incremented after each play session in the editor.");
		public static GUIContent incrementOnBuild = new GUIContent("Increment on Build", "The patch will incremented after each compilation in the editor.");
		public static GUIContent version = new GUIContent("Version", "The current version of the project.");
		public static GUIContent ReleaseState = new GUIContent("ReleaseState", "The current release state of the project.");
	}

	const string k_VersionIncrementorSettingsPath = "Assets/Editor/VersionIncrementorSettings.asset";

	public VersionIncrementorSettingsProvider(string path, SettingsScope scope = SettingsScope.User)
		: base(path, scope) { }

	public static bool IsSettingsAvailable()
	{
		return File.Exists(k_VersionIncrementorSettingsPath);
	}

	public override void OnActivate(string searchContext, VisualElement rootElement)
	{
		m_VersionIncrementorSettings = VersionIncrementorSettings.GetSerializedSettings();
	}

	public override void OnGUI(string searchContext)
	{
		m_VersionIncrementorSettings.Update();
		// Use IMGUI to display UI:
		EditorGUILayout.PropertyField(m_VersionIncrementorSettings.FindProperty("increment_on_play"), Styles.incrementOnPlay);
		EditorGUILayout.PropertyField(m_VersionIncrementorSettings.FindProperty("increment_on_build"), Styles.incrementOnBuild);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(Styles.version);
		EditorGUILayout.PropertyField(m_VersionIncrementorSettings.FindProperty("m_major"));
		EditorGUILayout.PropertyField(m_VersionIncrementorSettings.FindProperty("m_minor"));
		EditorGUILayout.PropertyField(m_VersionIncrementorSettings.FindProperty("m_patch"));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.PropertyField(m_VersionIncrementorSettings.FindProperty("m_releaseStates"), Styles.ReleaseState);

		VersionIncrementorSettings settings = (VersionIncrementorSettings)m_VersionIncrementorSettings.targetObject;

		if (m_VersionIncrementorSettings.hasModifiedProperties)
		{
			if (!settings.isValidVersion())
			{
				//TODO add logic for informing.

			}
		}
		m_VersionIncrementorSettings.ApplyModifiedProperties();
	}

	[SettingsProvider]
	public static SettingsProvider CreateMyCustomSettingsProvider()
	{
		if (IsSettingsAvailable())
		{
			SettingsProvider provider = new VersionIncrementorSettingsProvider("Project/Version Incrementor Settings", SettingsScope.Project);

			provider.keywords = GetSearchKeywordsFromGUIContentProperties<Styles>();
			return provider;
		}

		return null;
	}
}