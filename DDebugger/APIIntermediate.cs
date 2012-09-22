using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DDebugger.Win32
{
	/// <summary>
	/// Contains abstracted API methods for the debug engine.
	/// </summary>
	public static class APIIntermediate
	{
		private const int MaxStringSizeBytes = 1024;

		#region Read/Write/Allocate

		// Code was taken from Jeff Burn's Extemory project. See 'External Resources.txt' for details
		public static unsafe T Read<T>(this Process proc, IntPtr addr) where T : struct
		{
			var type = typeof(T);
			var size = type == typeof(char) ? Marshal.SystemDefaultCharSize : Marshal.SizeOf(type);
			var bytes = new byte[size];
			int bytesRead;
			if (!API.ReadProcessMemory(proc.Handle, addr, bytes, (uint)size, out bytesRead))
				throw new Win32Exception(Marshal.GetLastWin32Error());
			if (bytesRead != size)
				throw new Exception(string.Format("Unable to read {0} bytes from process for intput type {1}", size, type.Name));

			object ret;
			if (type == typeof(IntPtr))
			{
				fixed (byte* pByte = bytes)
					ret = new IntPtr(*(void**)pByte);
				return (T)ret;
			}

			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Boolean:
					ret = BitConverter.ToBoolean(bytes, 0);
					break;
				case TypeCode.Char:
					ret = BitConverter.ToChar(bytes, 0);
					break;
				case TypeCode.Byte:
					ret = bytes[0];
					break;
				case TypeCode.SByte:
					ret = (sbyte)bytes[0];
					break;
				case TypeCode.Int16:
					ret = BitConverter.ToInt16(bytes, 0);
					break;
				case TypeCode.Int32:
					ret = BitConverter.ToInt32(bytes, 0);
					break;
				case TypeCode.Int64:
					ret = BitConverter.ToInt64(bytes, 0);
					break;
				case TypeCode.UInt16:
					ret = BitConverter.ToUInt16(bytes, 0);
					break;
				case TypeCode.UInt32:
					ret = BitConverter.ToUInt32(bytes, 0);
					break;
				case TypeCode.UInt64:
					ret = BitConverter.ToUInt64(bytes, 0);
					break;
				case TypeCode.Single:
					ret = BitConverter.ToSingle(bytes, 0);
					break;
				case TypeCode.Double:
					ret = BitConverter.ToDouble(bytes, 0);
					break;
				default:
					throw new NotSupportedException(type.FullName + " is not supported yet");
				// TODO: general struct
			}

			return (T)ret;
		}

		public static void Write<T>(this Process proc, IntPtr addr, T value) where T : struct
		{
			object val = value;

			if (val.GetType() == typeof(IntPtr))
			{
				val = IntPtr.Size == 8 ? ((IntPtr)val).ToInt64() : ((IntPtr)val).ToInt32();
			}

			byte[] bytes;
			switch (Type.GetTypeCode(val.GetType()))
			{
				case TypeCode.Boolean:
					bytes = BitConverter.GetBytes((bool)val);
					break;
				case TypeCode.Byte:
					bytes = new[] { (byte)val };
					break;
				case TypeCode.SByte:
					bytes = new[] { (byte)((sbyte)val) };
					break;
				case TypeCode.Char:
					bytes = BitConverter.GetBytes((char)val);
					break;
				case TypeCode.Int16:
					bytes = BitConverter.GetBytes((short)val);
					break;
				case TypeCode.UInt16:
					bytes = BitConverter.GetBytes((ushort)val);
					break;
				case TypeCode.Int32:
					bytes = BitConverter.GetBytes((int)val);
					break;
				case TypeCode.UInt32:
					bytes = BitConverter.GetBytes((uint)val);
					break;
				case TypeCode.Int64:
					bytes = BitConverter.GetBytes((long)val);
					break;
				case TypeCode.UInt64:
					bytes = BitConverter.GetBytes((ulong)val);
					break;
				case TypeCode.Single:
					bytes = BitConverter.GetBytes((float)val);
					break;
				case TypeCode.Double:
					bytes = BitConverter.GetBytes((double)val);
					break;
				default:
					throw new ArgumentException(string.Format("Unsupported type argument supplied: {0}", val.GetType()));
			}
			int bytesWritten;
			if (!API.WriteProcessMemory(proc.Handle, addr, bytes, (uint)bytes.Length, out bytesWritten) || bytesWritten != bytes.Length)
				throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		public static T[] ReadArray<T>(this Process proc, IntPtr addr, int count) where T : struct
		{
			var dataSize = Marshal.SizeOf(typeof(T)) * count;
			int bytesRead;
			var pBytes = Marshal.AllocHGlobal(dataSize);
			try
			{
				if (!API.ReadProcessMemory(proc.Handle, addr, pBytes, (uint)dataSize, out bytesRead))
					throw new Win32Exception(Marshal.GetLastWin32Error());
				if (bytesRead != dataSize)
					throw new Exception(string.Format("Unable to read {0} bytes for array of type {1} with {2} elements", dataSize, typeof(T).Name, count));

				switch (Type.GetTypeCode(typeof(T)))
				{
					case TypeCode.Byte:
						var bytes = new byte[count];
						Marshal.Copy(pBytes, bytes, 0, count);
						return bytes.Cast<T>().ToArray();
					case TypeCode.Char:
						var chars = new char[count];
						Marshal.Copy(pBytes, chars, 0, count);
						return chars.Cast<T>().ToArray();
					case TypeCode.Int16:
						var shorts = new short[count];
						Marshal.Copy(pBytes, shorts, 0, count);
						return shorts.Cast<T>().ToArray();
					case TypeCode.Int32:
						var ints = new int[count];
						Marshal.Copy(pBytes, ints, 0, count);
						return ints.Cast<T>().ToArray();
					case TypeCode.Int64:
						var longs = new long[count];
						Marshal.Copy(pBytes, longs, 0, count);
						return longs.Cast<T>().ToArray();
					case TypeCode.Single:
						var floats = new float[count];
						Marshal.Copy(pBytes, floats, 0, count);
						return floats.Cast<T>().ToArray();
					case TypeCode.Double:
						var doubles = new double[count];
						Marshal.Copy(pBytes, doubles, 0, count);
						return doubles.Cast<T>().ToArray();
					default:
						throw new ArgumentException(string.Format("Unsupported type argument supplied: {0}", typeof(T).Name));
				}
			}
			finally
			{
				if (pBytes != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pBytes);
				}
			}
		}

		public static void WriteArray<T>(this Process proc, IntPtr addr, T[] data) where T : struct
		{
			var size = data.Length;
			var dataSize = size * Marshal.SizeOf(typeof(T));

			var pTemp = Marshal.AllocHGlobal(dataSize);

			try
			{
				switch (Type.GetTypeCode(typeof(T)))
				{
					case TypeCode.Byte:
						var bytes = data.Cast<byte>().ToArray();
						Marshal.Copy(bytes, 0, pTemp, size);
						break;
					case TypeCode.Char:
						var chars = data.Cast<char>().ToArray();
						Marshal.Copy(chars, 0, pTemp, size);
						break;
					case TypeCode.Int16:
						var shorts = data.Cast<short>().ToArray();
						Marshal.Copy(shorts, 0, pTemp, size);
						break;
					case TypeCode.Int32:
						var ints = data.Cast<int>().ToArray();
						Marshal.Copy(ints, 0, pTemp, size);
						break;
					case TypeCode.Int64:
						var longs = data.Cast<long>().ToArray();
						Marshal.Copy(longs, 0, pTemp, size);
						break;
					case TypeCode.Single:
						var floats = data.Cast<float>().ToArray();
						Marshal.Copy(floats, 0, pTemp, size);
						break;
					case TypeCode.Double:
						var doubles = data.Cast<double>().ToArray();
						Marshal.Copy(doubles, 0, pTemp, size);
						break;
					default:
						throw new ArgumentException(string.Format("Unsupported type argument supplied: {0}", typeof(T).Name));
				}

				int bytesWritten;
				if (!API.WriteProcessMemory(proc.Handle, addr, pTemp, (uint)dataSize, out bytesWritten) || bytesWritten != dataSize)
					throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			finally
			{
				if (pTemp != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pTemp);
				}
			}
		}

		public static string ReadString(this Process proc, IntPtr addr, Encoding encoding, int maxSize = MaxStringSizeBytes)
		{
			if (!(encoding.Equals(Encoding.UTF8) || encoding.Equals(Encoding.Unicode) || encoding.Equals(Encoding.ASCII)))
			{
				throw new ArgumentException(string.Format("Encoding type {0} is not supported", encoding.EncodingName), "encoding");
			}

			var bytes = proc.ReadArray<byte>(addr, maxSize);
			var terminalCharacterByte = encoding.GetBytes(new[] { '\0' });
			var buffer = new List<byte>();
			var variableByteSize = encoding.Equals(Encoding.UTF8);
			if (variableByteSize)
			{
				for (int i = 0; i < bytes.Length; )
				{
					var match = true;
					var shortBuffer = new List<byte>();
					for (int j = 0; j < terminalCharacterByte.Length; j++)
					{
						shortBuffer.Add(bytes[i + j]);
						if (bytes[i + j] != terminalCharacterByte[j])
						{
							match = false;
							break;
						}
					}
					if (match)
					{
						break;
					}
					buffer.AddRange(shortBuffer);
					i += shortBuffer.Count;
				}
			}
			else
			{
				for (int i = 0; i < bytes.Length; i += terminalCharacterByte.Length)
				{
					var range = new byte[terminalCharacterByte.Length];
					var match = true;
					for (int j = 0; j < terminalCharacterByte.Length; j++)
					{
						range[j] = bytes[i + j];
						if (range[j] != terminalCharacterByte[j]) match = false;
					}
					if (!match)
					{
						buffer.AddRange(range);
					}
					else
					{
						break;
					}
				}
			}

			var result = encoding.GetString(buffer.ToArray());
			return result;
		}

		public static void WriteString(this Process proc, IntPtr addr, string value, Encoding encoding, bool appendNullCharacter = true)
		{
			var bytes = encoding.GetBytes(value);
			if (appendNullCharacter)
			{
				bytes = bytes.Concat(encoding.GetBytes(new[] { '\0' })).ToArray();
			}

			proc.WriteArray(addr, bytes);
		}

		public static IntPtr Allocate(this Process proc, IntPtr addr, uint size, AllocationType allocationType = AllocationType.Commit | AllocationType.Reserve, MemoryProtection memoryProtection = MemoryProtection.ReadWrite)
		{
			if (proc.Id == Process.GetCurrentProcess().Id)
			{
				return API.VirtualAlloc(addr, size, allocationType, memoryProtection);
			}
			return API.VirtualAllocEx(proc.Handle, addr, size, allocationType, memoryProtection);
		}

		public static bool Free(this Process proc, IntPtr addr, uint size = 0u, AllocationType freeType = AllocationType.Release)
		{
			if (proc.Id == Process.GetCurrentProcess().Id)
			{
				return API.VirtualFree(addr, size, freeType);
			}
			return API.VirtualFreeEx(proc.Handle, addr, size, freeType);
		}

		public static bool SetMemoryProtection(this Process proc, IntPtr addr, uint size, MemoryProtection newProtection, out MemoryProtection oldProtection)
		{
			MemoryProtection o = 0;
			var ret = API.VirtualProtectEx(proc.Handle, addr, size, newProtection, o);
			oldProtection = o;
			return ret;
		}
		#endregion

		public static void SetInterrupt(this Process p, IntPtr addr, out byte originalInstruction)
		{
			originalInstruction = p.Read<byte>(addr);

			// See e.g. Wikipedia for an explanation of the 'int3' instruction.
			p.Write<byte>(addr, 0xCC);
			API.FlushInstructionCache(p.Handle, addr, 1u);
		}

		public static void RemoveInterrupt(this Process p, IntPtr addr, byte originalInstruction)
		{
			p.Write(addr, originalInstruction);
			API.FlushInstructionCache(p.Handle, addr, 1u);
		}
	}
}
