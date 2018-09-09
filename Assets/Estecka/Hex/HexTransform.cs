using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Estecka.Hex;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Estecka.Hex {
	/// <summary>
	/// Interface for manipulating a Transform's position as in an hexagonal tile board.
	/// </summary>
	[ExecuteInEditMode]
	public sealed class HexTransform : MonoBehaviour {
		public Vector3 red  	{ get {return transform.TransformVector (VectorRGB.RedXY); 		} }
		public Vector3 yellow  	{ get {return transform.TransformVector (VectorRGB.YellowXY); 	} }
		public Vector3 green 	{ get {return transform.TransformVector (VectorRGB.GreenXY); 	} }
		public Vector3 cyan  	{ get {return transform.TransformVector (VectorRGB.CyanXY); 	} }
		public Vector3 blue 	{ get {return transform.TransformVector (VectorRGB.BlueXY); 	} }
		public Vector3 magenta 	{ get {return transform.TransformVector (VectorRGB.MagentaXY); 	} }


		[SerializeField] VectorRGB _cachedLocalHexPos;
		[SerializeField] Vector2   _cachedLocalQuadPos;

		/// <summary>
		/// Caches the value you set and the resulting XY position, thus preventing precision loss when doing back-and-forth conversion between RGB and XY. 
		/// The value you get is the exact same value you set (given that the actual position doesn't change in between).
		/// Setting the Hex position does not affect the depth (Z axis) of the transform.
		/// </summary>
		/// <seealso cref="localPositionRaw"/>
		public VectorRGB localPositionCached {
			get {
				if (_cachedLocalQuadPos == (Vector2)transform.localPosition)
					return _cachedLocalHexPos;
				else {
					_cachedLocalQuadPos = transform.localPosition;
					return (_cachedLocalHexPos = (VectorRGB)transform.localPosition);
				}
			}
			set {
				_cachedLocalHexPos = value;
				_cachedLocalQuadPos = transform.localPosition = value.ToVector3 (localDepth); 
			}
		}

		/// <summary>
		/// Get the Hex position by converting the XY position. The value you get might not be the exact value you set du to the imprecision of conversions.
		/// The value you set here is still cached and retrievable with <c>localPositionCached</c>
		/// Setting the Hex position does not affect the depth (Z axis) of the transform.
		/// </summary>
		/// <seealso cref="localPositionCached"/>
		public VectorRGB localPositionRaw {
			get { return (VectorRGB)transform.localPosition; }
			set {
				_cachedLocalHexPos = value;
				_cachedLocalQuadPos = transform.localPosition = value.ToVector3 (localDepth); 
			}
		}

		/// <summary>
		/// The object's Hex position in world space.
		/// Setting the Hex position does not affect the depth (Z axis) of the transform.
		/// </summary>
		public VectorRGB position {
			get { return (VectorRGB)transform.position; }
			set { transform.position = value.ToVector3 (depth); }
		}
			

		public float localDepth { 
			get { return transform.localPosition.z; }
			set {
				Vector3 pos = transform.localPosition;
				pos.z = value;
				transform.localPosition = pos;
			}
		}
		public float depth {
			get { return transform.position.z; }
			set { 
				Vector3 pos = transform.position;
				pos.z = value;
				transform.position = pos;
			}
		}


		Vector3 ParentTransformVector (Vector3 vector){
			return transform.parent ?
				transform.parent.TransformVector (vector) :
				vector;
		}
		void OnDrawGizmosSelected(){
			Gizmos.color = Color.red;	Gizmos.DrawRay (transform.position, ParentTransformVector(VectorRGB.RedXY));
			Gizmos.color = Color.green;	Gizmos.DrawRay (transform.position, ParentTransformVector(VectorRGB.GreenXY));
			Gizmos.color = Color.blue;	Gizmos.DrawRay (transform.position, ParentTransformVector(VectorRGB.BlueXY));
		}

	}//END Behaviour
}//END namespace
	