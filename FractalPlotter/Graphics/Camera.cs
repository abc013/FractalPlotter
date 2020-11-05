using OpenTK.Mathematics;

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
		public static Matrix4 IdentityMatrix = Matrix4.Identity;
		public static Matrix4 CameraMatrix;
		public static Matrix4 ScaleMatrix;
		public static Matrix4 InverseScaleMatrix;

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
		public static float Scale { get; private set; }
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
			Scale = Settings.Scale;
			Rotation = new Vector3();
			Changed = true;

			ResizeViewport(Settings.GraphWidth, Settings.GraphHeight);
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
		public static void SetScale(float s)
		{
			Scale = s;
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
		public static void Translate(int x, int y, int z)
		{
			var speed = Settings.CameraSpeed / Scale;
			var exactSpeed = (double)Settings.CameraSpeed / Scale;
			Location += new Vector3(x * speed, y * speed, z * speed);
			ExactLocation += new Vector3d(x * exactSpeed, y * exactSpeed, z * exactSpeed);
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

			var scale = Scale / 2f;
			ScaleMatrix = Matrix4.CreateScale(scale / Ratio, scale, 0);

			InverseScaleMatrix = Matrix4.CreateScale(1 / scale);
			CameraMatrix = locMatrix * ScaleMatrix;
		}
	}
}
