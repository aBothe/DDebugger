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
		CONTEXT_x86 lastReadCtxt = new CONTEXT_x86 { ContextFlags = ContextFlags.CONTEXT_ALL };

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
