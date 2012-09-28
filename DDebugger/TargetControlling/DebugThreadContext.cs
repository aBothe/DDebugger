using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using DDebugger.Win32;

namespace DDebugger.TargetControlling
{
	public class DebugThreadContext : IEnumerable<string>
	{
		#region Properties
		public readonly DebugThread Thread;
		static Type ut = typeof(uint);
		internal CONTEXT_x86 lastReadCtxt = new CONTEXT_x86 { ContextFlags = ContextFlags.CONTEXT_ALL };
		readonly List<Stackframe> callstack = new List<Stackframe>();

		public Stackframe[] CallStack
		{
			get
			{
				return callstack.ToArray();
			}
		}

		public uint this[string registerName]
		{
			get
			{
				var f = lastReadCtxt.GetType().GetField(registerName);

				if (f != null && f.FieldType.Equals(ut))
					return (uint)f.GetValue(lastReadCtxt);
				return 0;
			}
			set
			{
				var f = lastReadCtxt.GetType().GetField(registerName);

				if (f != null && f.FieldType.Equals(ut))
				{
					f.SetValue(lastReadCtxt, value);
					if (!API.SetThreadContext(Thread.Handle, ref lastReadCtxt))
						throw new Win32Exception(Marshal.GetLastWin32Error());
				}
				else
					throw new ArgumentException("Register " + registerName + " doesn't exist");
			}
		}
		#endregion

		public DebugThreadContext(DebugThread thread)
		{
			this.Thread = thread;
		}

		public void Update()
		{
			if (!API.GetThreadContext(Thread.Handle, ref lastReadCtxt))
				throw new Win32Exception(Marshal.GetLastWin32Error());

			BuildCallStack();
		}

		void BuildCallStack()
		{
			callstack.Clear();

			var ebp = lastReadCtxt.ebp;

			const int MaxFrames = 128;
			var proc = Thread.OwnerProcess;
			var p = proc.Handle;

			callstack.Add(new Stackframe(new IntPtr(ebp), new IntPtr(lastReadCtxt.eip)));

			do
			{
				// The return address is stored at ebp+1
				var returnTo = APIIntermediate.Read<uint>(p, new IntPtr(ebp + 4));
				ebp = APIIntermediate.Read<uint>(p, new IntPtr(ebp));

				if (ebp == 0 || returnTo == ebp)
					break;

				callstack.Add(new Stackframe(new IntPtr(ebp), new IntPtr(returnTo)));
			}
			while (callstack.Count < MaxFrames);

			string file="";
			ushort line=0;

			foreach(var sf in callstack)
				if (proc.MainModule.ContainsSymbolData &&
					proc.MainModule.ModuleMetaInfo.TryDetermineCodeLocation((uint)sf.CodeAddress.ToInt32(), out file, out line))
				{
					Console.WriteLine(file + ":" + line);
				}
				else
				{
					Console.WriteLine("@ 0x" + sf.CodeAddress.ToString("X8"));
				}
		}

		public bool ContainsRegister(string name)
		{
			var f = lastReadCtxt.GetType().GetField(name);

			return f != null && f.FieldType.Equals(ut);
		}
		
		public IEnumerator<string> GetEnumerator()
		{
			foreach (var f in lastReadCtxt.GetType().GetFields())
				if (f.FieldType.Equals(ut))
					yield return f.Name;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
