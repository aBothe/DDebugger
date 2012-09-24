using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using DDebugger.Win32;

namespace DDebugger.TargetControlling
{
	public class DebugException
	{
		public readonly ExceptionCode Code;
		/// <summary>
		/// The address where the exception occurred.
		/// </summary>
		public readonly IntPtr Address;
		public readonly bool IsFirstChance;
		public readonly DebugException InnerException;
		public readonly string Title;
		public readonly string Message;
		public readonly bool IsContinuable;

		public DebugException(EXCEPTION_RECORD32 ex, bool firstChance)
		{
			this.IsFirstChance = firstChance;
			Message = GetCodeMessage(Code = ex.Code, out Title);
			this.Address = ex.ExceptionAddress;
			this.IsContinuable = ex.ExceptionFlags == 0;

			if (ex.ExceptionRecord != IntPtr.Zero)
			{
				var innerExt = new EXCEPTION_RECORD32();
				Marshal.PtrToStructure(ex.ExceptionRecord, innerExt);
				InnerException = new DebugException(innerExt, firstChance);
			}
		}

		public static string GetCodeMessage(ExceptionCode code, out string title)
		{
			switch (code)
			{
				case ExceptionCode.AccessViolation:
					title = "Access violation";
					return @"The thread tried to read from or write to a virtual address for which it does not have the appropriate access.";
				case ExceptionCode.ArrayBoundsExceeded:
					title = "Array bounds exceeded";
					return @"The thread tried to access an array element that is out of bounds and the underlying hardware supports bounds checking.";
				case ExceptionCode.Breakpoint:
					title = "Breakpoint";
					return @"A breakpoint was encountered.";
				case ExceptionCode.DataTypeMisalignment:
					title = "Data type misalignment";
					return @"The thread tried to read or write data that is misaligned on hardware that does not provide alignment. For example, 16-bit values must be aligned on 2-byte boundaries; 32-bit values on 4-byte boundaries, and so on.";
				case ExceptionCode.Float_DenormalOperand:
					title = "Denormal Operand";
					return @"One of the operands in a floating-point operation is denormal. A denormal value is one that is too small to represent as a standard floating-point value.";
				case ExceptionCode.Float_DivideByZero:
					title = "Divide by zero";
					return @"The thread tried to divide a floating-point value by a floating-point divisor of zero.";
				case ExceptionCode.Float_InexactResult:
					title = "Inexact result";
					return @"The result of a floating-point operation cannot be represented exactly as a decimal fraction.";
				case ExceptionCode.Float_InvalidOperation:
					return title = "Invalid floatint-point operation";
				case ExceptionCode.Float_Overflow:
					title = "Float overflow";
					return "The exponent of a floating-point operation is greater than the magnitude allowed by the corresponding type.";
				case ExceptionCode.Float_StackCheck:
					title = "Stack over-/underflow";
					return "The stack overflowed or underflowed as the result of a floating-point operation.";
				case ExceptionCode.Float_Underflow:
					title = "Stack underflow";
					return "The exponent of a floating-point operation is less than the magnitude allowed by the corresponding type.";
				case ExceptionCode.IllegalInstruction:
					title = "Illegal instruction";
					return "The thread tried to execute an invalid instruction.";
				case ExceptionCode.InPageError:
					title = "In-Page error";
					return "The thread tried to access a page that was not present, and the system was unable to load the page. For example, this exception might occur if a network connection is lost while running a program over the network.";
				case ExceptionCode.Integer_DivideByZero:
					title = "Divide by zero";
					return "The thread tried to divide an integer value by an integer divisor of zero.";
				case ExceptionCode.Integer_Overflow:
					title = "Integer overflow";
					return "The result of an integer operation caused a carry out of the most significant bit of the result.";
				case ExceptionCode.InvalidDisposition:
					title = "Invalid disposition";
					return "An exception handler returned an invalid disposition to the exception dispatcher. Programmers using a high-level language such as C should never encounter this exception.";
				case ExceptionCode.NoncontinuableException:
					title = "Noncontinuable exception";
					return "The thread tried to continue execution after a noncontinuable exception occurred.";
				case ExceptionCode.PrivilegedInstruction:
					title = "Privileged instruction";
					return "The thread tried to execute an instruction whose operation is not allowed in the current machine mode.";
				case ExceptionCode.SingleStep:
					title = "Single step";
					return "A trace trap or other single-instruction mechanism signaled that one instruction has been executed.";
				case ExceptionCode.StackOverflow:
					title = "Stack overflow";
					return "The thread used up its stack.";
				default:
					title = null;
					return null;
			}
		}
	}
}
