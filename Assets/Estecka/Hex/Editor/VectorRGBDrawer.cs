#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Estecka.Hex.HexEditor {
	[CustomPropertyDrawer(typeof(VectorRGB))]
	public class VectorRGBDrawer : PropertyDrawer {

		static public GUIContent[] sublabels = new GUIContent[]{ 
			new GUIContent("R"),
			new GUIContent("G"),
			new GUIContent("B")
		};
		
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			property.Next (true);
			EditorGUI.MultiPropertyField(position, sublabels, property, label);
		}

		static float[] values = new float[3];
		/// <summary>
		/// Draws a layed out VectorRGB field in the inspector
		/// </summary>
		static public VectorRGB VectorRGBFieldLayout(VectorRGB value, GUIContent label){
			Rect position = EditorGUILayout.GetControlRect ();
			values [0] = value.r;
			values [1] = value.g;
			values [2] = value.b;

			EditorGUI.MultiFloatField(position, label, sublabels, values);

			value.r = values [0];
			value.g = values [1];
			value.b = values [2];
			return value;
		}

		/// <summary>
		/// Try retrieving a VectorRGB from a SerializedProperty
		/// </summary>
		static public VectorRGB FromProperty(SerializedProperty property){
			VectorRGB value;
			value.r = property.FindPropertyRelative ("r").floatValue;
			value.g = property.FindPropertyRelative ("g").floatValue;
			value.b = property.FindPropertyRelative ("b").floatValue;
			return value;
		}

		/// <summary>
		/// Try saving a VectorRGB into a SerializedProperty
		/// </summary>
		static public void ToProperty(SerializedProperty property, VectorRGB value){
			property.FindPropertyRelative ("r").floatValue = value.r;
			property.FindPropertyRelative ("g").floatValue = value.g;
			property.FindPropertyRelative ("b").floatValue = value.b;			
		}

	} // END Drawer
} // END Namespace
#endif