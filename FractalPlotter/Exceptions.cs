using OpenToolkit.Graphics.OpenGL;
using System;
using System.Runtime.Serialization;

namespace FractalPlotter.FractalPlotter
{
	[Serializable]
	public class GraphicsException : Exception
	{
		public GraphicsException(string position, ErrorCode error) : base($"Uncaught OpenGL error at {position}: {error}") { }

		protected GraphicsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	[Serializable]
	public class DefaultShaderException : Exception
	{
		public DefaultShaderException() : base("The default shader could not be loaded. Check the information.log for more details.") { }

		protected DefaultShaderException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
