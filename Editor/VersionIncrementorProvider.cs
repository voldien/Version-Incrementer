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
		public static GUIContent settingLabel = new GUIContent("Settings");
		public static GUIContent incrementOnPlay = new GUIContent("Increment on Play", "The patch will be incremented after each play session in the editor.");
		public static GUIContent incrementOnBuild = new GUIContent("Increment on Build", "The patch will incremented after each compilation in the editor.");
		public static GUIContent version = new GUIContent("Version", "The current version of the project.");
		public static GUIContent ReleaseState = new GUIContent("ReleaseState", "The current release state of the project.");
	}


	public VersionIncrementorSettingsProvider(string path, SettingsScope scope = SettingsScope.User)
		: base(path, scope) { }

	public static bool IsSettingsAvailable()
	{
		return File.Exists(VersionIncrementorSettings.GetSettingFilePath());
	}

	public override void OnActivate(string searchContext, VisualElement rootElement)
	{
		m_VersionIncrementorSettings = VersionIncrementorSettings.GetSerializedSettings();
	}

	public override void OnGUI(string searchContext)
	{
		m_VersionIncrementorSettings.Update();
		// Use IMGUI to display UI:
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel++;		
		EditorGUILayout.LabelField(Styles.settingLabel, EditorStyles.boldLabel);
		EditorGUI.indentLevel++;
		EditorGUILayout.PropertyField(m_VersionIncrementorSettings.FindProperty("increment_on_play"), Styles.incrementOnPlay);
		EditorGUILayout.PropertyField(m_VersionIncrementorSettings.FindProperty("increment_on_build"), Styles.incrementOnBuild);

		EditorGUI.indentLevel = indent + 1;
		EditorGUILayout.LabelField(Styles.version,EditorStyles.boldLabel);
		EditorGUI.indentLevel++;
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(m_VersionIncrementorSettings.FindProperty("m_major"));
		EditorGUILayout.PropertyField(m_VersionIncrementorSettings.FindProperty("m_minor"));
		EditorGUILayout.PropertyField(m_VersionIncrementorSettings.FindProperty("m_patch"));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.PropertyField(m_VersionIncrementorSettings.FindProperty("m_releaseStates"), Styles.ReleaseState);

		EditorGUI.indentLevel = indent;

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
	public static SettingsProvider CreateVersionIncrementorSettingsProvider()
	{
		if (!IsSettingsAvailable())
		{
			VersionIncrementorSettings.GetOrCreateSettings();
		}
		SettingsProvider provider = new VersionIncrementorSettingsProvider("Project/Version Incrementer Settings", SettingsScope.Project);

		provider.keywords = GetSearchKeywordsFromGUIContentProperties<Styles>();
		return provider;
	}
}
