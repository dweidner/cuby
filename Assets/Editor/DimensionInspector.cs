using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Dimension))]
public class DimensionInspector : Editor {
		
	
    public override void OnInspectorGUI()
    {
		EditorGUI.BeginChangeCheck ();
		Dimension dimension = (Dimension)target;

        DrawDefaultInspector();

		if (EditorGUI.EndChangeCheck ()) {
			dimension.SetLightning();
			SceneView.RepaintAll();
		}
        //EditorUtility.SetDirty(target);
    }
}
