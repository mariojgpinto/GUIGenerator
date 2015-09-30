using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(GUIGenerator))]
public class GUIGeneratorEditor : Editor
{
	GUIGenerator myScript;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		myScript = (GUIGenerator)target;

		if(GUILayout.Button("Generate Files"))
		{
			List<string> files = myScript.ExistingFiles();

			if(files.Count > 0){
				string str = "Files in conflict:";
				for(int i = 0 ; i < files.Count ; ++i){
					str += "\n  - " + files[i];
				}
				str += "\n\nDo you wish to proceed?";

				if(EditorUtility.DisplayDialog("Files in Conflict", str, "Yes", "No")){
					Debug.Log("YES");
					GenerateFiles();
				}
				else{
					Debug.Log("NO");
				}
			}
			else{
				GenerateFiles();
			}
		}
	}

	void GenerateFiles(){
		myScript.GenerateFiles();
		
		EditorUtility.DisplayDialog("Generation Successful", "Files Generated!", "Ok");
	}
}