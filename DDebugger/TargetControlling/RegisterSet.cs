using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDebugger.TargetControlling
{
	/// <summary>
	/// Encapsules reading from/writing into CPU registers 
	/// such as eax, ebx, ecx etc.
	/// </summary>
	public class RegisterSet : IEnumerable<string>
	{
		int this[string name]
		{
			get
			{
				return 0;
			}
			set { }
		}

		public IEnumerator<string> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
