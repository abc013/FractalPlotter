using System;
using System.IO;

namespace ComplexNumberGrapher
{
	public static class Log
	{
		static StreamWriter information;
		static StreamWriter exception;

		public static void Initialize()
		{
			information = new StreamWriter(FileManager.Current + "information.log");
			exception = new StreamWriter(FileManager.Current + "exception.log");
		}

		public static void WriteInfo(object info)
		{
			information.WriteLine(DateTime.Now + ": " + info);
			information.Flush();
		}

		public static void WriteException(object e)
		{
			exception.WriteLine(e);
			exception.Flush();
		}

		public static void Exit()
		{
			information.Close();
			exception.Close();
		}
	}
}
