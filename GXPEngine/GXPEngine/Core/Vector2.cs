using System;

namespace GXPEngine.Core
{
	public struct Vector2
	{
		public float x;
		public float y;
		
		public Vector2 (float x, float y)
		{
			this.x = x;
			this.y = y;
		}
		
		public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
		public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
		public static Vector2 operator *(Vector2 a, Vector2 b) => new Vector2(a.x * b.x, a.y * b.y);
		public static Vector2 operator /(Vector2 a, Vector2 b) => new Vector2(a.x / b.x, a.y / b.y);

		/// <summary>
		/// Get the distance between two vectors
		/// </summary>
		/// <param name="a">first vecor</param>
		/// <param name="b">second vector</param>
		/// <returns>Distance between two given vectors</returns>
        public static float Distance(Vector2 a, Vector2 b)
        {
			Vector2 diff = a - b;
			return Mathf.Sqrt((diff.x * diff.x) + (diff.y * diff.y));
		}

        public static Vector2 operator /(Vector2 a, float b) => new Vector2(a.x / b, a.y / b);

		/// <summary>
		/// Convert the current vector to one of length 1
		/// </summary>
		public void normalize()
        {
			this /= this.length();
        }

		/// <summary>
		/// Get the length of the current vector
		/// </summary>
		/// <returns>Length of the current vector</returns>
		public float length()
        {
			return Mathf.Sqrt((this.x * this.x) + (this.y * this.y));
		}

		override public string ToString() {
			return "[Vector2 " + x + ", " + y + "]";
		}
	}
}

