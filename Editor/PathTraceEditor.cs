using UnityEditor;
using UnityEngine;

namespace PathCarbo.Editor
{
	[CustomEditor(typeof(PathTrace))]
	public class PathTraceEditor : UnityEditor.Editor
	{
		bool bVisuals = true;

		bool bPathTracing = true ; 

		public override void OnInspectorGUI()
		{
			PathTrace pathTrace = target as PathTrace;


			bVisuals = EditorGUILayout.Foldout(bVisuals, "Visuals");

			if (bVisuals)
			{
				EditorGUI.indentLevel = 2;

				EditorGUILayout.PropertyField(serializedObject.FindProperty("endMarkerPrefab"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("height"));

				EditorGUI.indentLevel = 0;
			}

			bPathTracing = EditorGUILayout.Foldout(bPathTracing, "Path Tracing");

			if (bPathTracing)
			{
				EditorGUI.indentLevel = 1;
				pathTrace.pathTraceType = (PathTrace.EPathTraceType)EditorGUILayout.EnumPopup(pathTrace.pathTraceType);
			
				EditorGUI.indentLevel = 2;
				switch (pathTrace.pathTraceType)
				{
					case PathTrace.EPathTraceType.Agent:
						EditorGUILayout.PropertyField(serializedObject.FindProperty("agent"));
						break;
					case PathTrace.EPathTraceType.Positions:
						EditorGUILayout.PropertyField(serializedObject.FindProperty("targets"));

						EditorGUILayout.BeginHorizontal();

						if (GUILayout.Button("Show Path")) {
							pathTrace.DrawPath();
						}
						if (GUILayout.Button("Hide Path"))
						{
							pathTrace.RemovePath();
						}
						EditorGUILayout.EndHorizontal();

						pathTrace.showPathOnStart = GUILayout.Toggle(pathTrace.showPathOnStart, "Show path on game start");

						break;
					default:
						break;
				}
				EditorGUI.indentLevel = 1;

				EditorGUILayout.PropertyField(serializedObject.FindProperty("previewPathInEditor"));

				EditorGUI.indentLevel = 0;

				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}
