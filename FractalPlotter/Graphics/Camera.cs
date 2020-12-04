using OpenTK.Mathematics;

namespace FractalPlotter.Graphics
{
	/// <summary>
	/// Class to keep track of movement in virtual space.
	/// </summary>
	public static class Camera
	{
		public static float RelativeSpeed => Settings.CameraSpeed / Scale;

		/// <summary>
		/// Default matrix.
		/// </summary>
		public static Matrix4 IdentityMatrix = Matrix4.Identity;
		/// <summary>
		/// Camera matrix used for points and other geometry.
		/// </summary>
		public static Matrix4 CameraMatrix;
		/// <summary>
		/// Matrix that saves the scale.
		/// </summary>
		public static Matrix4 ScaleMatrix;
		/// <summary>
		/// Matrix that saves the inverted scale.
		/// </summary>
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
		/// Keeps track of whether the matrix needs to be updated.
		/// </summary>
		public static bool Changed { get; private set; }

		/// <summary>
		/// Initialize standard camera settings.
		/// </summary>
		public static void Load()
		{
			Location = new Vector3((float)Settings.LocationX, (float)Settings.LocationY, (float)Settings.LocationZ);
			ExactLocation = new Vector3d(Settings.LocationX, Settings.LocationY, Settings.LocationZ);
			Scale = Settings.Scale;
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
		/// Move in the specified directions. The values will be multiplied by the camera speed as well.
		/// In order to allow movement in deeper regions, moving is also being divided by the current scale.
		/// </summary>
		public static void Translate(int x, int y, int z)
		{
			var speed = RelativeSpeed;
			Location += new Vector3(x * speed, y * speed, z * speed);

			var exactSpeed = (double)RelativeSpeed;
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

			var scale = Scale / 2f;
			ScaleMatrix = Matrix4.CreateScale(scale / Ratio, scale, 0);
			InverseScaleMatrix = Matrix4.CreateScale(1 / scale);

			CameraMatrix = locMatrix * ScaleMatrix;
		}
	}
}
