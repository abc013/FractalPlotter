using System;
using System.IO;

namespace FractalPlotter
{
	public static class Log
	{
		static StreamWriter information;
		static StreamWriter exception;

		/// <summary>
		/// Opens the log files. Please note that old logs will be deleted in the process.
		/// </summary>
		public static void Initialize()
		{
			information = new StreamWriter(FileManager.Current + "information.log");
			exception = new StreamWriter(FileManager.Current + "exception.log");
		}

		/// <summary>
		/// Write information to the information.log. Date and time are added automatically.
		/// </summary>
		/// <param name="info">Information to write.</param>
		public static void WriteInfo(object info)
		{
			if (!information.BaseStream.CanWrite)
				return;

			information.WriteLine(DateTime.Now + ": " + info);
			information.Flush();
		}

		/// <summary>
		/// Write an exception to the exception.log.
		/// </summary>
		/// <param name="e">Exception to write.</param>
		public static void WriteException(object e)
		{
			exception.WriteLine(e);
			exception.Flush();
		}

		/// <summary>
		/// Unlock the log files properly for other processes.
		/// </summary>
		public static void Exit()
		{
			information.Close();
			exception.Close();
		}
	}
}
