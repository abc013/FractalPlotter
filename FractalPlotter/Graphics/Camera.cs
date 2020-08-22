using OpenToolkit.Mathematics;

namespace ComplexNumberGrapher.Graphics
{
	public static class Camera
	{
		public static Matrix4 CameraMatrix;

		public static float Ratio { get; private set; }

		public static Vector3 Location { get; private set; }
		public static Vector3d ExactLocation { get; private set; }
		public static Vector3 Scale { get; private set; }
		public static Vector3 Rotation { get; private set; }

		public static bool Changed { get; private set; }

		public static void Load()
		{
			Location = new Vector3(Settings.LocationX, Settings.LocationY, Settings.LocationZ);
			ExactLocation = new Vector3d(Settings.LocationX, Settings.LocationY, Settings.LocationZ);
			Scale = new Vector3(Settings.Scale);
			Rotation = new Vector3();
			Changed = true;
		}

		public static void SetTranslation(double x, double y, double z)
		{
			ExactLocation = new Vector3d(x, y, z);
			Location = new Vector3((float)x, (float)y, (float)z);
			Changed = true;
		}

		public static void SetScale(float sx, float sy, float sz)
		{
			Scale = new Vector3(sx, sy, sz);
			Changed = true;
		}

		public static void SetRotation(float rx, float ry, float rz)
		{
			Rotation = new Vector3(rx, ry, rz);
			Changed = true;
		}

		public static void Translate(float x, float y, float z)
		{
			var speed = Settings.CameraSpeed;
			Location += new Vector3(x * speed / Scale.X, y * speed / Scale.Y, z * speed / Scale.Z);
			ExactLocation += new Vector3d(x * (double)speed / Scale.X, y * (double)speed / Scale.Y, z * (double)speed / Scale.Z);
			Changed = true;
		}

		public static void Scaling(float s)
		{
			Scale += s * Scale;
			Changed = true;
		}

		public static void Rotate(float x, float y, float z)
		{
			Rotation += new Vector3(x, y, z);
			Changed = true;
		}

		public static void ResizeViewport(int width, int height)
		{
			Ratio = width / (float)height;
			Changed = true;
		}

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
