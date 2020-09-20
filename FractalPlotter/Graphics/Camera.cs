using OpenToolkit.Mathematics;

namespace ComplexNumberGrapher.Graphics
{
	/// <summary>
	/// Class to keep track of movement in virtual space.
	/// </summary>
	public static class Camera
	{
		/// <summary>
		/// Camera matrix used for points and other geometry.
		/// </summary>
		public static Matrix4 CameraMatrix;

		/// <summary>
		/// Window ratio.
		/// </summary>
		public static float Ratio { get; private set; }

		/// <summary>
		/// Location. This location parameter has 32-bit depth. It is accessible in shaders.
		/// </summary>
		public static Vector3 Location { get; private set; }
		/// <summary>
		/// Location. This location parameter has 64-bit depth. It is accessible in shaders.
		/// </summary>
		public static Vector3d ExactLocation { get; private set; }
		/// <summary>
		/// Scale. This parameter is accessible in shaders.
		/// </summary>
		public static Vector3 Scale { get; private set; }
		/// <summary>
		/// Rotation. Unused.
		/// </summary>
		public static Vector3 Rotation { get; private set; }

		/// <summary>
		/// Keeps track of whether the matrix needs to be updated.
		/// </summary>
		public static bool Changed { get; private set; }

		public static void Load()
		{
			Location = new Vector3(Settings.LocationX, Settings.LocationY, Settings.LocationZ);
			ExactLocation = new Vector3d(Settings.LocationX, Settings.LocationY, Settings.LocationZ);
			Scale = new Vector3(Settings.Scale);
			Rotation = new Vector3();
			Changed = true;
		}

		/// <summary>
		/// Set new translation.
		/// </summary>
		public static void SetTranslation(double x, double y, double z)
		{
			ExactLocation = new Vector3d(x, y, z);
			Location = new Vector3((float)x, (float)y, (float)z);
			Changed = true;
		}

		/// <summary>
		/// Set new scale.
		/// </summary>
		public static void SetScale(float sx, float sy, float sz)
		{
			Scale = new Vector3(sx, sy, sz);
			Changed = true;
		}

		/// <summary>
		/// Set new rotation.
		/// </summary>
		public static void SetRotation(float rx, float ry, float rz)
		{
			Rotation = new Vector3(rx, ry, rz);
			Changed = true;
		}

		/// <summary>
		/// Move in the specified directions. The values will be multiplied by the camera speed as well.
		/// In order to allow movement in deeper regions, moving is also being divided by the current scale.
		/// </summary>
		public static void Translate(float x, float y, float z)
		{
			var speed = Settings.CameraSpeed;
			Location += new Vector3(x * speed / Scale.X, y * speed / Scale.Y, z * speed / Scale.Z);
			ExactLocation += new Vector3d(x * (double)speed / Scale.X, y * (double)speed / Scale.Y, z * (double)speed / Scale.Z);
			Changed = true;
		}

		/// <summary>
		/// Add scale with the specified factor.
		/// In order to allow movement in deeper regions, scaling is being multiplied with the current scale.
		/// </summary>
		public static void Scaling(float s)
		{
			Scale += s * Scale;
			Changed = true;
		}

		/// <summary>
		/// Add rotation to the current one.
		/// </summary>
		public static void Rotate(float x, float y, float z)
		{
			Rotation += new Vector3(x, y, z);
			Changed = true;
		}

		/// <summary>
		/// Update the window ratio if the window was resized.
		/// </summary>
		public static void ResizeViewport(int width, int height)
		{
			Ratio = width / (float)height;
			Changed = true;
		}

		/// <summary>
		/// Calculates the camera matrix.
		/// </summary>
		public static void CalculateMatrix()
		{
			if (!Changed)
				return;

			Changed = false;

			var locMatrix = Matrix4.CreateTranslation(new Vector3(-Location.X, -Location.Y, Location.Z));

			// TODO for 3D space
			//var rotMatrixX = Matrix4.CreateRotationX(Rotation.X);
			//var rotMatrixY = Matrix4.CreateRotationY(Rotation.Y);
			//var rotMatrixZ = Matrix4.CreateRotationY(Rotation.Z);

			// TODO: currently only x-scale is used.
			var scaleMatrix = Matrix4.CreateScale(Scale.X / (2f * Ratio), Scale.Y / 2f, Scale.Z / 2f);

			CameraMatrix = locMatrix * scaleMatrix;
		}
	}
}
