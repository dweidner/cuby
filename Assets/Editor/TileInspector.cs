//using UnityEngine;
//using System.Collections;
//using UnityEditor;
//
//[CustomEditor(typeof(Tile))]
//public class TileInspector : Editor 
//{
//	private int oldIndex = 0;
//
//    public override void OnInspectorGUI()
//    {
//		EditorGUI.BeginChangeCheck ();
//		int dimensionId = oldIndex;
//		Tile tile = (Tile)target;
//
//        DrawDefaultInspector();
//
//        GameObject[] dimensions = GameObject.FindGameObjectsWithTag("Dimension");
//        string[] dimensionLabels = new string[6];
//
//        for( int i = 0; i < dimensions.Length; ++i ){
//          dimensionLabels[i] = dimensions[i].name;
//			if(tile.TargetDimension.name == dimensions[i].name){
//				dimensionId = i;
//			}
//		}
//
//		dimensionId = EditorGUILayout.Popup("Zieldimension", oldIndex, dimensionLabels);
//
//		if (EditorGUI.EndChangeCheck ()) {
//			Debug.Log (dimensions[dimensionId].name);
//			tile.TargetDimension = dimensions[dimensionId];
//
//			if(oldIndex != dimensionId){
//				oldIndex = dimensionId;
//			}
//		}
//        //EditorUtility.SetDirty(target);
//    }
//}