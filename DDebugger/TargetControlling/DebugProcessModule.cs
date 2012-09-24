using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CodeViewExaminer.CodeView;

namespace DDebugger.TargetControlling
{
	public class DebugProcessModule
	{
		public readonly IntPtr ImageBase;
		public readonly string ImageFile;

		public readonly CodeViewData SymbolInformation;
		public readonly bool ContainsSymbolData;

		public DebugProcessModule(IntPtr imageBase, string imageFile, CodeViewData cvData = null)
		{
			this.ImageBase = imageBase;
			this.ImageFile = imageFile;
			this.SymbolInformation = cvData;
			ContainsSymbolData = cvData != null;
		}

		internal static string GetModuleFileName(IntPtr lpImageName, bool isUnicode, uint procId)
		{
			if (lpImageName != IntPtr.Zero)
			{
				if (isUnicode)
					return Marshal.PtrToStringUni(lpImageName);
				else
					return Marshal.PtrToStringAnsi(lpImageName);
			}
			else if (procId == 0)
				return "<Unknown module>";

			return Process.GetProcessById((int)procId).MainModule.FileName;
		}
	}
}
