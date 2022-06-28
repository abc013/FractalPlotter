using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.Serialization;

namespace FractalPlotter
{
	/// <summary>
	/// Exception type to use when a graphic error happens.
	/// </summary>
	[Serializable]
	public class GraphicsException : Exception
	{
		public GraphicsException(string position, ErrorCode error) : base($"Uncaught OpenGL error at {position}: {error}") { }

		protected GraphicsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	/// <summary>
	/// Exception type to use when the default shader could not be loaded.
	/// </summary>
	[Serializable]
	public class DefaultShaderException : Exception
	{
		public DefaultShaderException() : base("The default shader could not be loaded. Check the information.log for more details.") { }

		protected DefaultShaderException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	/// <summary>
	/// Exception type to use when the settings file could not be loaded.
	/// </summary>
	[Serializable]
	public class InvalidSettingsException : Exception
	{
		public InvalidSettingsException(string message) : base(message) { }

		protected InvalidSettingsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
