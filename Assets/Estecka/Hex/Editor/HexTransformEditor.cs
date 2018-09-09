#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Estecka.Hex.HexEditor {
	[CustomEditor(typeof(HexTransform))]
	public class HexTransformEditor : Editor {

		public override void OnInspectorGUI () {
			HexTransform hex = target as HexTransform;
			VectorRGB value;

			/*SerializedProperty _quad = serializedObject.FindProperty ("_cachedLocalQuadPos");
			SerializedProperty _hex = serializedObject.FindProperty ("_cachedLocalHexPos");

			if (_quad.vector2Value != (Vector2)hex.transform.position) {
				_quad.vector2Value = (Vector2)hex.transform.position;
				VectorRGBDrawer.ToProperty(_hex, (VectorRGB)hex.transform.position);
			}

			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (_hex, new GUIContent ("Position"), true);
			if (EditorGUI.EndChangeCheck()){
				var quadPos = (Vector2)VectorRGBDrawer.FromProperty (_hex);
				_quad.vector2Value = quadPos;
				hex.transform.position = new Vector3 (
					quadPos.x,
					quadPos.y,
					hex.transform.position.z
				);
			}/**/


			EditorGUI.BeginChangeCheck ();
			value = VectorRGBDrawer.VectorRGBFieldLayout (hex.localPositionCached.balanced, new GUIContent("Position"));
			if (EditorGUI.EndChangeCheck ()) {
				Undo.RecordObject (hex, "Hex Position change");
				hex.localPositionRaw = value;
			}
			/**/

		}
		
	} // END Editor
} // END Namespace
#endif