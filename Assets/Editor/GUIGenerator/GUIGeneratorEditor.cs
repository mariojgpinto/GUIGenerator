using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GUIGenerator))]
public class GUIGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		GUIGenerator myScript = (GUIGenerator)target;
		if(GUILayout.Button("Generate Files"))
		{
			myScript.GenerateFiles();
		}
	}
}