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
		public IntPtr StartAddress
		{
			get
			{
				return IntPtr.Add(ImageBase, (int)(ModuleMetaInfo.PEHeader.Is32BitHeader ?
					ModuleMetaInfo.PEHeader.OptionalHeader32.AddressOfEntryPoint:
					ModuleMetaInfo.PEHeader.OptionalHeader64.AddressOfEntryPoint));
			}
		}
		public IntPtr CodeBase
		{
			get
			{
				return IntPtr.Add(ImageBase, (int)(ModuleMetaInfo.PEHeader.Is32BitHeader ?
					ModuleMetaInfo.PEHeader.OptionalHeader32.BaseOfCode :
					ModuleMetaInfo.PEHeader.OptionalHeader64.BaseOfCode));
			}
		}
		public readonly IntPtr ImageBase;
		public readonly string ImageFile;

		public readonly ExecutableMetaInfo ModuleMetaInfo;
		public readonly bool ContainsSymbolData;

		public DebugProcessModule(IntPtr imageBase, string imageFile, ExecutableMetaInfo emi)
		{
			this.ImageBase = imageBase;
			this.ImageFile = imageFile;
			this.ModuleMetaInfo = emi;
			ContainsSymbolData = emi.CodeViewSection != null;
		}

		/// <summary>
		/// Converts an offset that is relative to the code base of this module
		/// to an absolute virtual address that can be used for read/write operations
		/// </summary>
		public IntPtr ToVirtualAddress(int pureCodeOffset)
		{
			return IntPtr.Add(CodeBase, pureCodeOffset);
		}

		/// <summary>
		/// Converts an absolute virtual address to an offset relative to the code base of this module.
		/// Used e.g. for getting the code offset from breakpoint addresses.
		/// </summary>
		public uint ToCodeOffset(IntPtr virtualAddress)
		{
			return (uint)IntPtr.Subtract(virtualAddress, CodeBase.ToInt32()).ToInt32();
		}
	}
}
