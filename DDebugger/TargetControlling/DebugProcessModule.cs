using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CodeViewExaminer;
using CodeViewExaminer.CodeView;
using DDebugger.Win32;

namespace DDebugger.TargetControlling
{
	public class DebugProcessModule
	{
		public readonly IntPtr StartAddress;
		public readonly IntPtr ImageBase;
		public readonly string ImageFile;

		public readonly ExecutableMetaInfo ModuleMetaInfo;
		public readonly bool ContainsSymbolData;

		public DebugProcessModule(IntPtr imageBase, string imageFile, ExecutableMetaInfo emi)
		{
			this.StartAddress = new IntPtr(emi.PEHeader.OptionalHeader32.AddressOfEntryPoint + (uint)imageBase.ToInt32());
			this.ImageBase = imageBase;
			this.ImageFile = imageFile;
			this.ModuleMetaInfo = emi;
			ContainsSymbolData = emi.CodeViewSection != null;
		}
	}
}
