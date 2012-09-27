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
	}
}
