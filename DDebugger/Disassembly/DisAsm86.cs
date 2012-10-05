// See license.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDebugger.Disassembly
{
	/*
	 * What an instruction is made of:
	 * (furthermore, see the Intel x86/x64 spec manual / http://download.intel.com/products/processor/manual/325462.pdf / Chapter 2.1)
	 * 
	 * # bytes
	 * 0-4		Prefix
	 * 1-3		Opcode
	 * 1		Mod/Rm (if required)
	 *		Bit 5,6 Mod
	 *		Bit 4,3	Reg/Opcode
	 *		Bit 2,1 R/m	
	 * 1		SIB/Scale-Index-Base (if required)
	 * 0/1/2/4	Displacement
	 * 0/1/2/4	Operand data
	 */


	public class DisAsm86
	{
		public const int MaximumInstructionLength = 15;

		public static InstructionPrefixes GetInstructionPrefix(byte[] code, bool isX64, out byte prefixLength)
		{
			var x86Pref = InstructionPrefix_x86.None;
			var rexPref = (byte)0;

			for (prefixLength = 0; prefixLength <= code.Length; prefixLength++)
			{
				var c = code[prefixLength];
				if (Enum.IsDefined(typeof(InstructionPrefix_x86), c))
					x86Pref |= (InstructionPrefix_x86)c;
				else if (isX64 && c >= 0x40 && c <= 0x4f)
					rexPref = c;
				else
					break;
			}

			return new InstructionPrefixes { 
				Prefixes = x86Pref,
				PrefixLength = prefixLength,
				RexPrefix = rexPref
			};
		}

		// Code taken & ported from MagoDebugger / DecodeX86.cpp, see license.txt for details
		static int GetModRmSize16(byte modRm)
		{
			int instSize = 1;       // already includes modRm byte
			var mod = (modRm >> 6) & 3;
			var rm = (modRm & 7);

			// mod == 3 is only for single direct register values
			if (mod != 3)
			{
				if (mod == 2)
					instSize += 2;      // disp16
				else if (mod == 1)
					instSize += 1;      // disp8

				if ((mod == 0) && (rm == 6))
					instSize += 2;      // disp16
			}

			return instSize;
		}

		static int GetModRmSize32(byte modRm)
		{
			int instSize = 1;       // already includes modRm byte
			var mod = (modRm >> 6) & 3;
			var rm = (modRm & 7);

			// mod == 3 is only for single direct register values
			if (mod != 3)
			{
				if (rm == 4)
					instSize += 1;      // SIB

				if (mod == 2)
					instSize += 4;      // disp32
				else if (mod == 1)
					instSize += 1;      // disp8

				if ((mod == 0) && (rm == 5))
					instSize += 4;      // disp32
			}

			return instSize;
		}



		public static InstructionType GetInstructionType(byte[] code, bool isX64, out int instructionLength)
		{
			byte modRM = 0;
			byte prefLength= 0;
			var pref = GetInstructionPrefix(code, isX64, out prefLength);

			switch (code[prefLength])
			{
				case 0xcc: // int3
				instructionLength = 1;
				return InstructionType.Breakpoint;


				case 0xe8: // call
				if (pref.Prefixes.HasFlag(InstructionPrefix_x86.OperandSize) && !isX64)
					instructionLength = 5;
				else 
					instructionLength = 7;
				return InstructionType.Call;


				case 0x9a: // call
				if (!isX64)
					if (pref.Prefixes.HasFlag(InstructionPrefix_x86.OperandSize))
						instructionLength = 5;
					else
						instructionLength = 7;
				else
				{
					instructionLength = 0;
					return InstructionType.Other;
				}
				return InstructionType.Call;


				case 0xff: // call, jmp
				{
					if (code.Length - prefLength < 2)
						break;

					modRM = code[prefLength + 1];
					var regOp = (modRM >> 3) & 7;
					if (regOp == 2 || regOp == 3)
					{
						if (isX64 || !pref.Prefixes.HasFlag(InstructionPrefix_x86.AddressSizeOverride))
							instructionLength = 1 + GetModRmSize32(modRM);
						else
							instructionLength = 1 + GetModRmSize16(modRM);

						if (instructionLength > 0)
							return InstructionType.Call;
					}
					else if (regOp == 4 || regOp == 5)
					{
						if (isX64 || !pref.Prefixes.HasFlag(InstructionPrefix_x86.AddressSizeOverride))
							instructionLength = 1 + GetModRmSize32(modRM);
						else
							instructionLength = 1 + GetModRmSize16(modRM);

						return InstructionType.Jump;
					}
				}
				break;


				case 0xEB: // jmp
				instructionLength = 2;
				return InstructionType.Jump;


				case 0xE9:
				if (pref.Prefixes.HasFlag(InstructionPrefix_x86.OperandSize) && !isX64)
					instructionLength = 3;
				else
					instructionLength = 5;
				return InstructionType.Jump;


				case 0xEA: // jmp
				if (!isX64)
				{
					if (pref.Prefixes.HasFlag(InstructionPrefix_x86.OperandSize))
						instructionLength = 5;
					else
						instructionLength = 7;
					return InstructionType.Jump;
				}
				break;


				case 0x0F: // system call instructions
				if (code.Length - prefLength < 2)
					break;

				modRM = code[prefLength + 1];
				if (modRM == 0x05 || modRM == 0x34)
				{
					instructionLength = 2;
					return InstructionType.Call;
				}
				break;


				default:
				// rep prefixed instructions
				if (code.Length - prefLength < 2)
					break;
				modRM = code[prefLength + 1];

				if (pref.Prefixes.HasFlag(InstructionPrefix_x86.RepeatNonZero))
				{
					if (modRM == 0xa6 || modRM == 0xa7 || modRM == 0xae || modRM == 0xaf)
					{
						instructionLength = 2;
						return InstructionType.RepeatString;
					}
				}
				else if (pref.Prefixes.HasFlag(InstructionPrefix_x86.Repeat))
				{
					if ((modRM >= 0x6c && modRM <= 0x6f) || (modRM >= 0xa4 && modRM <= 0xa7) || (modRM >= 0xaa && modRM <= 0xaf))
					{
						instructionLength = 2;
						return InstructionType.RepeatString;
					}
				}
				break;
			}

			instructionLength = 1;
			return InstructionType.Invalid;
		}
	}

	public enum InstructionPrefix_x86 : byte
	{
		None = 0,

		// Group 1 - Lock and repeat prefixes
		Lock = 0xF0,
		/// <summary>
		/// Repeat-Not-Zero prefix applies only to string and input/output instructions. 
		/// (F2H is also used as a mandatory prefix for some instructions)
		/// </summary>
		RepeatNonZero = 0xF2,
		/// <summary>
		/// Repeat prefix applies only to string and input/output instructions.
		/// (F3H is also used as a mandatory prefix for some instructions)
		/// </summary>
		Repeat = 0xF3,

		// Group 2 - Segment override prefixes
		Override_Cs = 0x2E,
		Override_Ss = 0x36,
		Override_Ds = 0x3E,
		Override_Es = 0x26,
		Override_Fs = 0x64,
		Override_Gs = 0x65,
		// - Branch hints
		/// <summary>
		/// Branch not taken (used only with Jcc instructions)
		/// </summary>
		BranchNotTaken = 0x2E,
		/// <summary>
		/// Branch taken (used only with Jcc instructions)
		/// </summary>
		BranchTaken = 0x3E,

		// Group 3
		/// <summary>
		/// The operand-size override prefix allows a program to switch between 16- and 32-bit operand sizes. 
		/// Either size can be the default; use of the prefix selects the non-default size.
		/// </summary>
		OperandSize = 0x66,

		// Group 4
		/// <summary>
		/// Address-size override prefix
		/// </summary>
		AddressSizeOverride = 0x67,
	}

	public struct InstructionPrefixes
	{
		public InstructionPrefix_x86 Prefixes;
		public byte PrefixLength;
		public byte RexPrefix;
	}

	public enum InstructionType
	{
		Invalid = 0,
		Breakpoint,
		Call,
		Jump,
		RepeatString,
		Other
	}
}
