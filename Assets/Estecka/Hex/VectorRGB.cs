using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Estecka.Hex {
	/// <summary>
	/// Represents a position on an hexagonal tileboard.
	/// </summary>
	[System.Serializable]
	public struct VectorRGB : System.IEquatable<VectorRGB>{
		public float r, g, b;
		public VectorRGB(float red, float green, float blue){
			this.r = red;
			this.g = green;
			this.b = blue;
		}

		#region Statics
		static public readonly VectorRGB 
			Red 	= new VectorRGB(1,0,0), // Red => X+
			Yellow	= new VectorRGB(1,1,0),
			Green 	= new VectorRGB(0,1,0), //Green => y+ x-
			Cyan 	= new VectorRGB(0,1,1),
			Blue 	= new VectorRGB(0,0,1), //Blue => y- x-
			Magenta	= new VectorRGB(1,0,1);

		// Cos == X; Sin == Y
		public const float 
			Cos60 = 0.5f,
			Sin60 = 0.86602540378443864676372317075294f; 

		/// <summary>
		/// The XY coordinates of common RGB vectors
		/// </summary>
		static public readonly Vector2 
			RedXY 	= new Vector2 (1,0),
			GreenXY	= new Vector2 (-Cos60,  Sin60),
			BlueXY	= new Vector2 (-Cos60, -Sin60),
			YellowXY	= -BlueXY,
			CyanXY		= -RedXY,
			MagentaXY	= -GreenXY;

		/// <summary>
		/// The RGB coordinates of common XY vectors
		/// </summary>
		static public readonly VectorRGB
			RightRGB	= new VectorRGB(1,0,0),
			UpRGB		= new VectorRGB(-GreenXY.x/Sin60, 1/Sin60, 0), // gotta double check this one just in case
			LeftRGB = -RightRGB,
			DownRGB = -UpRGB;
		#endregion


		/// <summary>
		/// Balances out this vector. A balanced VectorRGB has no negative component, and at least one Zero-ed component.
		/// Because R,G and B combined cancel each other out, multiple different VectorRGBs can point to the same position; however only one of them is balanced. Use balanced vectors in order to compare positions.
		/// </summary>
		public void Balance(){
			float smallest;
			if (r>g)
				smallest = (g>b) ? b : g;
			else
				smallest = (r>b) ? b : r;

			this.r -= smallest;
			this.g -= smallest;
			this.b -= smallest;
		}//
		/// <summary>
		/// Returns a balanced copy of this Vector. A balanced VectorRGB has no negative component, and at least one Zero-ed component.
		/// Because R,G and B combined cancel each other out, multiple different VectorRGBs can point to the same position; however only one of them is balanced. Use balanced vectors in order to compare positions.
		/// </summary>
		public VectorRGB balanced {
			get{ 
				float smallest;
				if (r>g)
					smallest = (g>b) ? b : g;
				else
					smallest = (r>b) ? b : r;
				
				return new VectorRGB (r-smallest, g-smallest, b-smallest);
			}
		}//

		/// <summary>
		/// Checks whether one is the result of the other's back-and-forth RGB<=>XY conversion.
		/// </summary>
		public bool Approximates(VectorRGB other){
			return ((VectorRGB)(Vector2)this) == other
				|| ((VectorRGB)(Vector2)other) == this;
		}

		#region Conversion
		public override string ToString () {
			return string.Format ("({0}, {1}, {2})", r, g, b);
		}
		public Vector3 ToVector3(float z = 0)
		{ return new Vector3 (0,0,z) + (Vector3)( (r*RedXY)+(g*GreenXY)+(b*BlueXY) ); }

		static public explicit operator Vector3 (VectorRGB hex) 
		{ return (hex.r*RedXY) + (hex.g*GreenXY) + (hex.b*BlueXY); }

		static public explicit operator Vector2 (VectorRGB hex) 
		{ return (hex.r*RedXY) + (hex.g*GreenXY) + (hex.b*BlueXY); }

		static public explicit operator VectorRGB (Vector3 quad) 
		{ return ((quad.x*RightRGB) + (quad.y*UpRGB)).balanced; }

		static public explicit operator VectorRGB (Vector2 quad) 
		{ return ((quad.x*RightRGB) + (quad.y*UpRGB)).balanced; }
		#endregion

		#region Equality
		const int HASHFACTOR = 16777619;
		public override int GetHashCode () {
			unchecked {
				int hash = (int)2166136261;
				hash = (hash * HASHFACTOR) ^ r.GetHashCode ();
				hash = (hash * HASHFACTOR) ^ g.GetHashCode ();
				hash = (hash * HASHFACTOR) ^ b.GetHashCode ();
				return hash;
			}
		}
		public override bool Equals (object obj) {
			return obj is VectorRGB && this.Equals ((VectorRGB)obj);
		}
		public bool Equals (VectorRGB other){
			return this.r == other.r
				&& this.g == other.g
				&& this.b == other.b;
		}
		static public bool operator ==(VectorRGB a, VectorRGB b){
			return a.r == b.r
				&& a.g == b.g
				&& a.b == b.b;
		}
		static public bool operator !=(VectorRGB a, VectorRGB b){
			return a.r != b.r
				|| a.g != b.g
				|| a.b != b.b;
		}
		#endregion

		#region Arithmetic
		static public VectorRGB operator +(VectorRGB v) { return v; }
		static public VectorRGB operator -(VectorRGB v) {
			v.r = -v.r;
			v.g = -v.g;
			v.b = -v.b;
			return v;
		}

		static public VectorRGB operator +(VectorRGB a, VectorRGB b){
			a.r += b.r;
			a.g += b.g;
			a.b += b.b;
			return a;
		}
		static public VectorRGB operator -(VectorRGB a, VectorRGB b){
			a.r -= b.r;
			a.g -= b.g;
			a.b -= b.b;
			return a;
		}

		static public VectorRGB operator *(float f, VectorRGB v){
			v.r *= f;
			v.g *= f;
			v.b *= f;
			return v;
		}
		static public VectorRGB operator *(VectorRGB v, float f){
			v.r *= f;
			v.g *= f;
			v.b *= f;
			return v;
		}
		static public VectorRGB operator /(VectorRGB v, float f){
			f = 1 /f;
			v.r *= f;
			v.g *= f;
			v.b *= f;
			return v;
		}
		#endregion

	} //END Struct
} // END Namespace