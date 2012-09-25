using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CodeViewExaminer.CodeView;
using DDebugger.Win32;

namespace DDebugger.TargetControlling
{
	public class DebugProcessModule
	{
		public readonly IntPtr StartAddress;
		public readonly IntPtr ImageBase;
		public readonly string ImageFile;

		public readonly CodeViewData SymbolInformation;
		public readonly bool ContainsSymbolData;

		public DebugProcessModule(IntPtr imageBase, IntPtr startAddr, string imageFile, CodeViewData cvData = null)
		{
			this.StartAddress = startAddr;
			this.ImageBase = imageBase;
			this.ImageFile = imageFile;
			this.SymbolInformation = cvData;
			ContainsSymbolData = cvData != null;
		}

		internal static string GetModuleFileName(IntPtr hProcess,IntPtr lpImageName, bool isUnicode)
		{
			if (lpImageName != IntPtr.Zero)
			{
				// lpImageName is points to a pointer which points to the first character
				var pp = Marshal.ReadIntPtr(lpImageName);

				if (pp != IntPtr.Zero)
				{
					if (isUnicode)
						return APIIntermediate.ReadString(hProcess, pp, System.Text.Encoding.Unicode, 512);
					else
						return APIIntermediate.ReadString(hProcess, pp, System.Text.Encoding.ASCII, 256);
				}
			}
			
			return "<Unknown module>";
		}
	}
}
