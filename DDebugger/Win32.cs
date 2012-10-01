using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DDebugger.Win32
{
	/// <summary>
	/// Contains plenty of imported WinAPI-Functions
	/// </summary>
	public class API
	{
		#region Process
		/// <summary>
		/// Open process and retrieve handle for manipulation
		/// </summary>
		/// <param name="dwDesiredAccess"><see cref="ProcessAccessFlags"/> for external process.</param>
		/// <param name="bInheritHandle">Indicate whether to inherit a handle.</param>
		/// <param name="dwProcessId">Unique process ID of process to open</param>
		/// <returns>Returns a handle to opened process if successful or <see cref="IntPtr.Zero"/> if unsuccessful.
		/// Use <see cref="Marshal.GetLastWin32Error" /> to get Win32 Error on failure</returns>
		[DllImport("kernel32.dll", EntryPoint = "OpenProcess", SetLastError = true)]
		public static extern IntPtr OpenProcess(
			ProcessAccessFlags dwDesiredAccess,
			[MarshalAs(UnmanagedType.Bool)] 
            bool bInheritHandle,
			int dwProcessId);

		/// <summary>
		/// Creates a new process and its primary thread. The new process runs in the security context of the calling process.
		/// </summary>
		/// <param name="lpApplicationName">The name of the module to be executed. The string can specify the full path and file name of hte module to execute
		/// or it can specify a partial name.</param>
		/// <param name="lpCommandLine">The command line to be executed.</param>
		/// <param name="lpProcessAttributes">A pointer to a SECURITY_ATTRIBUTES structure that determines whether the returned handle to the new process object can be inherited by child processes. If lpProcessAttributes is <see cref="IntPtr.Zero"/>, the handle cannot be inherited.</param>
		/// <param name="lpThreadAttributes">A pointer to a SECURITY_ATTRIBUTES structure that determines whether the returned handle to the new thread object can be inherited by child processes. If lpThreadAttributes is <see cref="IntPtr.Zero"/>, the handle cannot be inherited.</param>
		/// <param name="bInheritHandles">If this parameter is true, each inheritable handle in the calling process is inherited by the new process. If the parameter is FALSE, the handles are not inherited. Note that inherited handles have the same value and access rights as the original handles.</param>
		/// <param name="dwCreationFlags">The flags that control the priority class and the creation of the process. See <see cref="ProcessCreationFlags"/></param>
		/// <param name="lpEnvironment">A pointer to the environment block for the new process. If this parameter is <see cref="IntPtr.Zero"/>, the new process uses the environment of the calling process.</param>
		/// <param name="lpCurrentDirectory">The full path to the current directory for the process. The string can also specify a UNC path.</param>
		/// <param name="lpStartupInfo">A pointer to a <see cref="STARTUPINFO"/> structure.</param>
		/// <param name="lpProcessInformation">A pointer to a <see cref="PROCESS_INFORMATION"/> structure that receives identification information about the new process.</param>
		/// <returns>If the function succeeds, the return value is true. If the function fails, the return value is false. Call <see cref="Marshal.GetLastWin32Error"/> to get the Win32 Error.</returns>
		[DllImport("kernel32.dll", EntryPoint = "CreateProcessW", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CreateProcess(
			[MarshalAs(UnmanagedType.LPWStr)] string lpApplicationName,
			[MarshalAs(UnmanagedType.LPWStr)] string lpCommandLine,
			IntPtr lpProcessAttributes,
			IntPtr lpThreadAttributes,
			bool bInheritHandles,
			ProcessCreationFlags dwCreationFlags,
			IntPtr lpEnvironment,
			string lpCurrentDirectory,
			ref STARTUPINFO lpStartupInfo,
			out PROCESS_INFORMATION lpProcessInformation);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hProcess"></param>
		/// <param name="isWoWProcess"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWow64Process(
			IntPtr hProcess,
			out bool isWoWProcess);

		[DllImport("kernel32.dll")]
		public static extern uint GetProcessId(IntPtr hProcess);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

		[DllImport("kernel32.dll")]
		public static extern bool GetExitCodeProcess(IntPtr hProcess, out uint lpExitCode);
		#endregion

		#region Module

		/// <summary>
		/// Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
		/// </summary>
		/// <param name="lpFileName">
		/// <para>The name of the module. This can be either a library module (.dll) or an executable module (.exe).</para>
		/// <para>If the string specifies a full path, the function searches only that path for the module. 
		/// Relative paths or files without a path will be searched for using standard strategies.</para>
		/// </param>
		/// <returns>If the function succeeds, a handle to the module is returned. 
		/// Otherwise, <see cref="IntPtr.Zero"/> is returned. Call <see cref="Marshal.GetLastWin32Error"/> on failure to get Win32 error.</returns>
		[DllImport("kernel32.dll", EntryPoint = "LoadLibraryW", SetLastError = true)]
		public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

		[DllImport("kernel32.dll", EntryPoint = "LoadLibraryA", SetLastError = true)]
		public static extern IntPtr LoadLibraryA(string lpFileName);

		/// <summary>
		/// Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
		/// </summary>
		/// <param name="lpFileName"><para>The name of the module. This can be either a library module (.dll) or an executable module (.exe).</para>
		/// <para>If the string specifies a full path, the function searches only that path for the module. 
		/// Relative paths or files without a path will be searched for using standard strategies.</para></param>
		/// <param name="hFile">This parameter is reserved for future use. It must be NULL (<see cref="IntPtr.Zero"/>)</param>
		/// <param name="dwFlags">The action to be taken when loading the module. If no flags are specified, the behaviour is identical to <see cref="LoadLibrary"/>.
		/// The parameter can be one of the values defined in <see cref="LoadLibraryExFlags"/></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", EntryPoint = "LoadLibraryExW", SetLastError = true)]
		public static extern IntPtr LoadLibraryEx(
			[MarshalAs(UnmanagedType.LPWStr)]
            string lpFileName,
			IntPtr hFile,
			LoadLibraryExFlags dwFlags);

		/// <summary>
		/// Frees the loaded Dll module.
		/// </summary>
		/// <param name="hModule">Handle to the loaded library to free</param>
		/// <returns>True if the function succeeds, otherwise false. Call <see cref="Marshal.GetLastWin32Error"/> on failure to get Win32 error.</returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FreeLibrary(IntPtr hModule);

		/// <summary>
		/// Retrieve a module handle for the specified module. The module must have been loaded by the calling process.
		/// </summary>
		/// <param name="lpModuleName">The name of the loaded module.</param>
		/// <returns>If the function succeeds, a handle to the module is returned. Otherwise, <see cref="IntPtr.Zero"/> is returned. Call <see cref="Marshal.GetLastWin32Error"/> on failure to get last Win32 error</returns>
		[DllImport("kernel32.dll", EntryPoint = "GetModuleHandleW", SetLastError = true)]
		public static extern IntPtr GetModuleHandle(
			[MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);

		/// <summary>
		/// Retrieves the address of an exported function from the specified Dll.
		/// </summary>
		/// <param name="hModule">Handle to the Dll module that contains the exported function</param>
		/// <param name="procName">The function name.</param>
		/// <returns></returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		#endregion

		#region Thread

		/// <summary>
		/// Create a thread that runs in the virtual address space of another process
		/// </summary>
		/// <param name="hProcess">A handle to the process in which the thread is to be created</param>
		/// <param name="lpThreadAttributes">A pointer to a SECURITY_ATTRIBUTES structure that specifies a security descriptor for the new thread and determines whether child processes can inherit the returned handle.</param>
		/// <param name="dwStackSize">The initial size of the stack, in bytes. The system rounds this value to the nearest page. If this parameter is 0 (zero), the new thread uses the default size for the executable.</param>
		/// <param name="lpStartAddress">A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting address of the thread in the remote process. The function must exist in the remote process.</param>
		/// <param name="lpParameter">A pointer to a variable to be passed to the thread function</param>
		/// <param name="dwCreationFlags">The flags that control the creation of the thread</param>
		/// <param name="lpThreadId">A pointer to a variable that receives the thread identifier. If this parameter is <see cref="IntPtr.Zero"/>, the thread identifier is not returned.</param>
		/// <returns>If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is <see cref="IntPtr.Zero"/>. Call <see cref="Marshal.GetLastWin32Error"/> to get Win32 Error.</returns>
		[DllImport("kernel32.dll", EntryPoint = "CreateRemoteThread", SetLastError = true)]
		public static extern IntPtr CreateRemoteThread(
			IntPtr hProcess,
			IntPtr lpThreadAttributes,
			uint dwStackSize,
			IntPtr lpStartAddress,
			IntPtr lpParameter,
			uint dwCreationFlags,
			[Out] IntPtr lpThreadId);

		/// <summary>
		/// Waits until the specified object is in the signaled state or the time-out interval elapses.
		/// </summary>
		/// <param name="hObject">A handle to the object. For a list of the object types whose handles can be specified, see the following Remarks section.</param>
		/// <param name="dwMilliseconds">The time-out interval, in milliseconds. The function returns if the interval elapses, even if the object's state is nonsignaled. If dwMilliseconds is zero, the function tests the object's state and returns immediately. If dwMilliseconds is INFINITE, the function's time-out interval never elapses.</param>
		/// <returns>If the function succeeds, the return value indicates the event that caused the function to return. If the function fails, the return value is WAIT_FAILED ((DWORD)0xFFFFFFFF).</returns>
		[DllImport("kernel32.dll", EntryPoint = "WaitForSingleObject")]
		public static extern uint WaitForSingleObject(IntPtr hObject, uint dwMilliseconds);

		/// <summary>
		/// Retrieves the termination status of the specified thread.
		/// </summary>
		/// <param name="hThread">Handle to the thread</param>
		/// <param name="lpExitCode">A pointer to a variable to receive the thread termination status. If this works properly, this should be the return value from the thread function of <see cref="CreateRemoteThread"/></param>
		/// <returns></returns>
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetExitCodeThread(IntPtr hThread, out IntPtr lpExitCode);

		/// <summary>
		/// Retrieves the termination status of the specified thread.
		/// </summary>
		/// <param name="hThread">Handle to the thread</param>
		/// <param name="lpExitCode">A pointer to a variable to receive the thread termination status. If this works properly, this should be the return value from the thread function of <see cref="CreateRemoteThread"/></param>
		/// <returns></returns>
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetExitCodeThread(IntPtr hThread, out uint lpExitCode);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int ResumeThread(IntPtr hThread);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern uint SuspendThread(IntPtr hThread);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenThread(ThreadAccessFlags dwDesiredAccess, bool bInheritHandle, int dwThreadId);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool TerminateThread(IntPtr hThread, int exitCode);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern uint GetThreadId(IntPtr hThread);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool GetThreadContext(IntPtr hThread, ref CONTEXT_x86 lpContext);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool SetThreadContext(IntPtr hThread, ref CONTEXT_x86 lpContext);
		#endregion

		#region Handle

		/// <summary>
		/// Close an open handle
		/// </summary>
		/// <param name="hObject">Object handle to close</param>
		/// <returns>True if success, false if failure. Use <see cref="Marshal.GetLastWin32Error"/> on failure to get Win32 error.</returns>
		[DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr hObject);
		#endregion

		#region Memory

		/// <summary>
		/// Reserves or commits a region of memory within the virtual address space of a specified process.
		/// The function initializes the memory it allocates to zero, unless <see cref="AllocationType.Reset"/> is used.
		/// </summary>
		/// <param name="hProcess">The handle to a process. The function allocated memory within the virtual address space of this process.
		/// The process must have the <see cref="ProcessAccessFlags.VMOperation"/> access right.</param>
		/// <param name="lpAddress">Optional desired address to begin allocation from. Use <see cref="IntPtr.Zero"/> to let the function determine the address</param>
		/// <param name="dwSize">The size of the region of memory to allocate, in bytes</param>
		/// <param name="flAllocationType">
		/// <see cref="AllocationType"/> type of allocation. Must contain one of <see cref="AllocationType.Commit"/>, <see cref="AllocationType.Reserve"/> or <see cref="AllocationType.Reset"/>.
		/// Can also specify <see cref="AllocationType.LargePages"/>, <see cref="AllocationType.Physical"/>, <see cref="AllocationType.TopDown"/>.
		/// </param>
		/// <param name="flProtect">One of <see cref="MemoryProtection"/> constants.</param>
		/// <returns>If the function succeeds, the return value is the base address of the allocated region of pages.
		/// If the function fails, the return value is <see cref="IntPtr.Zero"/>. Call <see cref="Marshal.GetLastWin32Error"/> to get Win32 error.</returns>
		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		public static extern IntPtr VirtualAllocEx(
			IntPtr hProcess,
			IntPtr lpAddress,
			uint dwSize,
			AllocationType flAllocationType,
			MemoryProtection flProtect);

		/// <summary>
		/// Reserves or commits a region of pages in the virtual address space of the calling process.
		/// Memory allocated by this function is automatically initialized to zero, unless <see cref="AllocationType.Reset" />
		/// is specified.
		/// </summary>
		/// <param name="lpAddress">The starting address of the region to allocate. If null, the system determines where to allocate the region</param>
		/// <param name="dwSize">The size of the region, in bytes.</param>
		/// <param name="flAllocationType">The type of memory allocation.</param>
		/// <param name="flProtect">The memory protection for the region of pages to be allocated.</param>
		/// <returns></returns>
		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		public static extern IntPtr VirtualAlloc(
			IntPtr lpAddress,
			uint dwSize,
			AllocationType flAllocationType,
			MemoryProtection flProtect);

		/// <summary>
		/// Releases, decommits, or releases and decommits a region of memory within the virtual address space of a specified process
		/// </summary>
		/// <param name="hProcess">A handle to a process. The function frees memory within the virtual address space of this process.
		/// The handle must have the <see cref="ProcessAccessFlags.VMOperation"/> access right</param>
		/// <param name="lpAddress">A pointer to the starting address of the region of memory to be freed.
		/// If the <paramref name="dwFreeType"/> parameter is <see cref="AllocationType.Release"/>, this address must be the base address
		/// returned by <see cref="VirtualAllocEx"/>.</param>
		/// <param name="dwSize">The size of the region of memory to free, in bytes.
		/// If the <paramref name="dwFreeType"/> parameter is <see cref="AllocationType.Release"/>, this parameter must be 0. The function
		/// frees the entire region that is reserved in the initial allocation call to <see cref="VirtualAllocEx"/></param>
		/// <param name="dwFreeType">The type of free operation. This parameter can be one of the following values: 
		/// <see cref="AllocationType.Decommit"/> or <see cref="AllocationType.Release"/></param>
		/// <returns>If the function is successful, it returns true. If the function fails, it returns false. Call <see cref="Marshal.GetLastWin32Error"/> to get Win32 error.</returns>
		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool VirtualFreeEx(
			IntPtr hProcess,
			IntPtr lpAddress,
			uint dwSize,
			AllocationType dwFreeType);

		/// <summary>
		/// Releases, decommits or releases and decommits a region of pages within the virtual address space of the calling process.
		/// </summary>
		/// <param name="lpAddress">A pointer to the base address of the region of pages to be freed.</param>
		/// <param name="dwSize">The size of the region of memory to be freed, in bytes.</param>
		/// <param name="dwFreeType">The type of free operation.</param>
		/// <returns>If the function succeeds, the return value is true. If the function fails, it returns false. Call <see cref="Marshal.GetLastWin32Error" /> to get Win32 error.</returns>
		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool VirtualFree(
			IntPtr lpAddress,
			uint dwSize,
			AllocationType dwFreeType);

		/// <summary>
		/// Changes the protection on a region of committed pages in the virtual address space of a specified process.
		/// </summary>
		/// <param name="hProcess">A handle to the process whose memory protection is to be changed. The handle must have the PROCESS_VM_OPERATION access right.</param>
		/// <param name="lpAddress">A pointer to the base address of the region of pages whose access protection attributes are to be changed.
		/// All pages in the specified region must be within the same reserved region allocated when calling the 
		/// VirtualAlloc or VirtualAllocEx function using MEM_RESERVE. 
		/// The pages cannot span adjacent reserved regions that were allocated 
		/// by separate calls to VirtualAlloc or VirtualAllocEx using MEM_RESERVE.</param>
		/// <param name="dwSize"></param>
		/// <param name="flNewProtect"></param>
		/// <param name="lpflOldProtect"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool VirtualProtectEx(
			IntPtr hProcess,
			IntPtr lpAddress,
			uint dwSize,
			MemoryProtection flNewProtect,
			[Out] MemoryProtection lpflOldProtect);

		/// <summary>
		/// Flushes the instruction cache for the specified process.
		/// </summary>
		/// <param name="hProcess"></param>
		/// <param name="lpAddress">A pointer to the base of the region to be flushed. This parameter can be NULL.</param>
		/// <param name="size">The size of the region to be flushed if the lpBaseAddress parameter is not NULL, in bytes.</param>
		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FlushInstructionCache(IntPtr hProcess, IntPtr lpAddress, uint size = 0u);

		/// <summary>
		/// Reads data from an area of memory in the specified process.
		/// </summary>
		/// <param name="hProcess">Handle to the process from which the memory is being read. 
		/// The handle must have <see cref="ProcessAccessFlags.VMRead"/> access to the process.</param>
		/// <param name="lpBaseAddress">A pointer to the base address in the specified process to begin reading from</param>
		/// <param name="lpBuffer">A pointer to a buffer that receives the contents from the address space of the process</param>
		/// <param name="dwSize">The number of bytes to be read</param>
		/// <param name="lpNumberOfBytesRead">The number of bytes read into the specified buffer</param>
		/// <returns>If the function succeeds, it returns true. Otherwise, false is returned and calling <see cref="Marshal.GetLastWin32Error"/> will retrieve the error.</returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ReadProcessMemory(
			IntPtr hProcess,
			IntPtr lpBaseAddress,
			[Out] byte[] lpBuffer,
			uint dwSize,
			out int lpNumberOfBytesRead);

		/// <summary>
		/// Reads data from an area of memory in the specified process.
		/// </summary>
		/// <param name="hProcess">Handle to the process from which the memory is being read. 
		/// The handle must have <see cref="ProcessAccessFlags.VMRead"/> access to the process.</param>
		/// <param name="lpBaseAddress">A pointer to the base address in the specified process to begin reading from</param>
		/// <param name="lpBuffer">A pointer to a buffer that receives the contents from the address space of the process</param>
		/// <param name="dwSize">The number of bytes to be read</param>
		/// <param name="lpNumberOfBytesRead">The number of bytes read into the specified buffer</param>
		/// <returns>If the function succeeds, it returns true. Otherwise, false is returned and calling <see cref="Marshal.GetLastWin32Error"/> will retrieve the error.</returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ReadProcessMemory(
			IntPtr hProcess,
			IntPtr lpBaseAddress,
			IntPtr lpBuffer,
			uint dwSize,
			out int lpNumberOfBytesRead);

		/// <summary>
		/// Writes data to an area of memory in a specified process.
		/// </summary>
		/// <param name="hProcess">Handle to the process to write memory to.
		/// The handle must have <see cref="ProcessAccessFlags.VMWrite"/> and <see cref="ProcessAccessFlags.VMOperation"/> access to the process</param>
		/// <param name="lpBaseAddress">A pointer to the base address to write to in the specified process</param>
		/// <param name="lpBuffer">A pointer to a buffer that contains the data to be written</param>
		/// <param name="nSize">The number of bytes to write</param>
		/// <param name="lpNumberOfBytesWritten">The number of bytes written.</param>
		/// <returns>If the function succeeds, it returns true. Otherwise false is returned and calling <see cref="Marshal.GetLastWin32Error"/> will retrieve the error.</returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool WriteProcessMemory(
			IntPtr hProcess,
			IntPtr lpBaseAddress,
			byte[] lpBuffer,
			uint nSize,
			out int lpNumberOfBytesWritten);

		/// <summary>
		/// Writes data to an area of memory in a specified process.
		/// </summary>
		/// <param name="hProcess">Handle to the process to write memory to.
		/// The handle must have <see cref="ProcessAccessFlags.VMWrite"/> and <see cref="ProcessAccessFlags.VMOperation"/> access to the process</param>
		/// <param name="lpBaseAddress">A pointer to the base address to write to in the specified process</param>
		/// <param name="lpBuffer">A pointer to a buffer that contains the data to be written</param>
		/// <param name="nSize">The number of bytes to write</param>
		/// <param name="lpNumberOfBytesWritten">The number of bytes written.</param>
		/// <returns>If the function succeeds, it returns true. Otherwise false is returned and calling <see cref="Marshal.GetLastWin32Error"/> will retrieve the error.</returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool WriteProcessMemory(
			IntPtr hProcess,
			IntPtr lpBaseAddress,
			IntPtr lpBuffer,
			uint nSize,
			out int lpNumberOfBytesWritten);

		#endregion

		#region File
		/// <summary>
		/// For Windows Vista and later only!
		/// </summary>
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern uint GetFinalPathNameByHandleW(
			IntPtr hFile,
			IntPtr lpszFilePath,
			uint cchFilePath = 0u,
			uint dwFlags = 0u);
		#endregion

		#region Window

		[DllImport("user32.dll", SetLastError = true, EntryPoint = "FindWindowW")]
		public static extern IntPtr FindWindow([MarshalAs(UnmanagedType.LPWStr)] string lpClassName, [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName);

		#endregion

		#region Debugger
		/// <summary>
		/// Enables a debugger to attach to an active process and debug it.
		/// http://msdn.microsoft.com/en-us/library/windows/desktop/ms679295(v=vs.85).aspx
		/// </summary>
		/// <param name="dwProcessId">The identifier for the process to be debugged. The debugger is granted debugging access to the process as if it created the process with the DEBUG_ONLY_THIS_PROCESS flag.</param>
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool DebugActiveProcess(uint dwProcessId);

		/// <summary>
		/// Causes a breakpoint exception to occur in the specified process. This allows the calling thread to signal the debugger to handle the exception.
		/// </summary>
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool DebugBreakProcess(IntPtr Process);

		/// <summary>
		/// http://msdn.microsoft.com/en-us/library/windows/desktop/ms681423(v=vs.85).aspx
		/// </summary>
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool WaitForDebugEvent(out DEBUG_EVENT lpDebugEvent, uint dwMilliseconds);

		/// <summary>
		/// Enables a debugger to continue a thread that previously reported a debugging event.
		/// </summary>
		/// <param name="dwProcessId">The process identifier of the process to continue.</param>
		/// <param name="dwThreadId">The thread identifier of the thread to continue. The combination of process identifier and thread identifier must identify a thread that has previously reported a debugging event.</param>
		/// <param name="dwContinueStatus">The options to continue the thread that reported the debugging event.</param>
		/// <returns></returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool ContinueDebugEvent(uint dwProcessId, uint dwThreadId, ContinueStatus dwContinueStatus);

		[DllImport("dbghelp.dll", SetLastError = true)]
		public static extern bool StackWalk64(
			MachineType machineType,
			IntPtr hProcess, IntPtr hThread,
			ref STACKFRAME64 StackFrame,
			ref CONTEXT_x86 ContextRecord,
			[Optional] IntPtr ReadMemoryRoutine,
			[Optional] IntPtr FunctionTableAccessRoutine,
			[Optional] IntPtr GetModuleBaseRoutine,
			[Optional] IntPtr TranslateAddress
);
		#endregion
	}

	#region Enums
	/// <summary>
	/// Memory allocation type - taken from #defines in WinNT.h
	/// </summary>
	[Flags]
	public enum AllocationType : uint
	{
		Commit = 0x1000,       //#define MEM_COMMIT           0x1000     
		Reserve = 0x2000,       //#define MEM_RESERVE          0x2000     
		Decommit = 0x4000,       //#define MEM_DECOMMIT         0x4000     
		Release = 0x8000,       //#define MEM_RELEASE          0x8000     
		Free = 0x10000,      //#define MEM_FREE            0x10000     
		Private = 0x20000,      //#define MEM_PRIVATE         0x20000     
		Mapped = 0x40000,      //#define MEM_MAPPED          0x40000     
		Reset = 0x80000,      //#define MEM_RESET           0x80000     
		TopDown = 0x100000,     //#define MEM_TOP_DOWN       0x100000     
		WriteWatch = 0x200000,     //#define MEM_WRITE_WATCH    0x200000     
		Physical = 0x400000,     //#define MEM_PHYSICAL       0x400000     
		Rotate = 0x800000,     //#define MEM_ROTATE         0x800000     
		LargePages = 0x20000000,   //#define MEM_LARGE_PAGES  0x20000000     
		FourMbPages = 0x80000000    //#define MEM_4MB_PAGES    0x80000000
	}

	/// <summary>
	/// Memory protection type - taken from #defines in WinNT.h
	/// </summary>
	public enum MemoryProtection : uint
	{
		NoAccess = 0x001,    //#define PAGE_NOACCESS          0x01     
		ReadOnly = 0x002,    //#define PAGE_READONLY          0x02     
		ReadWrite = 0x004,    //#define PAGE_READWRITE         0x04     
		WriteCopy = 0x008,    //#define PAGE_WRITECOPY         0x08     
		Execute = 0x010,    //#define PAGE_EXECUTE           0x10     
		ExecuteRead = 0x020,    //#define PAGE_EXECUTE_READ      0x20     
		ExecuteReadWrite = 0x040,    //#define PAGE_EXECUTE_READWRITE 0x40     
		ExecuteWriteCopy = 0x080,    //#define PAGE_EXECUTE_WRITECOPY 0x80     
		PageGuard = 0x100,    //#define PAGE_GUARD            0x100     
		NoCache = 0x200,    //#define PAGE_NOCACHE          0x200     
		WriteCombine = 0x400,    //#define PAGE_WRITECOMBINE     0x400
	}

	/// <summary>
	/// Process access flags - taken from #defines in WinNT.h
	/// </summary>
	[Flags]
	public enum ProcessAccessFlags : uint
	{
		Terminate = 0x0001,     //#define PROCESS_TERMINATE                  (0x0001)  
		CreateThread = 0x0002,     //#define PROCESS_CREATE_THREAD              (0x0002) 
		SetSessionID = 0x0004,     //#define PROCESS_SET_SESSIONID              (0x0004)
		VMOperation = 0x0008,     //#define PROCESS_VM_OPERATION               (0x0008)  
		VMRead = 0x0010,     //#define PROCESS_VM_READ                    (0x0010) 
		VMWrite = 0x0020,     //#define PROCESS_VM_WRITE                   (0x0020)
		DUPHandle = 0x0040,     //#define PROCESS_DUP_HANDLE                 (0x0040)
		CreateProcess = 0x0080,     //#define PROCESS_CREATE_PROCESS             (0x0080)  
		SetQuota = 0x0100,     //#define PROCESS_SET_QUOTA                  (0x0100)  
		SetInformation = 0x0200,     //#define PROCESS_SET_INFORMATION            (0x0200)
		QueryInformation = 0x0400,     //#define PROCESS_QUERY_INFORMATION          (0x0400)
		SuspendResume = 0x0800,     //#define PROCESS_SUSPEND_RESUME             (0x0800)
		QueryLimitedInformation = 0x1000,     //#define PROCESS_QUERY_LIMITED_INFORMATION  (0x1000)
		AllAccess = Synchronize | StandardRightsRequired | 0xFFFF,
		//#define PROCESS_ALL_ACCESS        (STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFFF)

		Synchronize = 0x100000,     //#define SYNCHRONIZE                      (0x00100000L)
		StandardRightsRequired = 0x0F0000      //#define STANDARD_RIGHTS_REQUIRED         (0x000F0000L)
	}

	[Flags]
	public enum ThreadAccessFlags : uint
	{
		Terminate = 0x0001,
		SuspendResume = 0x0002,
		GetContext = 0x0008,
		SetContext = 0x0010,
		SetInformation = 0x0020,
		QueryInformation = 0x0040,
		SetThreadToken = 0x0080,
		Impersonate = 0x0100,
		DirectImpersonation = 0x0200,
		SetLimitedInformation = 0x0400,
		QueryLimitedInformation = 0x0800,
		AllAccess = Synchronize | StandardRightsRequired | 0xFFFF,

		Synchronize = 0x100000,
		StandardRightsRequired = 0x0F0000,
	}

	/// <summary>
	/// Flags used in LoadLibraryEx to determine behaviour when loading library into process
	/// </summary>
	[Flags]
	public enum LoadLibraryExFlags : uint
	{
		DontResolveDllReferences = 0x00000001,     //#define DONT_RESOLVE_DLL_REFERENCES         0x00000001
		LoadLibraryAsDatafile = 0x00000002,     //#define LOAD_LIBRARY_AS_DATAFILE            0x00000002
		LoadLibraryWithAlteredSearchPath = 0x00000008,     //#define LOAD_WITH_ALTERED_SEARCH_PATH       0x00000008
		LoadIgnoreCodeAuthzLevel = 0x00000010,     //#define LOAD_IGNORE_CODE_AUTHZ_LEVEL        0x00000010
		LoadLibraryAsImageResource = 0x00000020,     //#define LOAD_LIBRARY_AS_IMAGE_RESOURCE      0x00000020
		LoadLibraryAsDatafileExclusive = 0x00000040      //#define LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE  0x00000040
	}

	[Flags]
	public enum ProcessCreationFlags : uint
	{
		None = 0x00000000u,

		/// <summary>
		/// The calling thread starts and debugs the new process 
		/// and all child processes created by the new process. 
		/// It can receive all related debug events using the WaitForDebugEvent function.
		/// A process that uses DEBUG_PROCESS becomes the root of a debugging chain. 
		/// This continues until another process in the chain is created with DEBUG_PROCESS.
		/// If this flag is combined with DEBUG_ONLY_THIS_PROCESS, 
		/// the caller debugs only the new process, not any child processes.
		/// </summary>
		DebugProcess = 0x00000001u,
		/// <summary>
		/// The calling thread starts and debugs the new process. It can receive all related debug events using the WaitForDebugEvent function.
		/// </summary>
		DebugOnlyThisProcess = 0x00000002u,
		/// <summary>
		/// The primary thread of the new process is created in a suspended state, and does not run until the ResumeThread function is called.
		/// </summary>
		CreateSuspended = 0x00000004u,
		DetachedProcess = 0x00000008u,
		CreateNewConsole = 0x00000010u,

		CreateNewProcessGroup = 0x00000200u,
		CreateUnicodeEnvironment = 0x00000400u,
		CreateSeparateWowVDM = 0x00000800u,
		CreateSharedWowVDM = 0x00001000u,

		InheritParentAffinity = 0x00010000u,
		CreateProtectedProcess = 0x00040000u,
		ExtendedStartupInfoPresent = 0x00080000u,

		CreateBreakawayFromJob = 0x01000000u,
		CreatePreserveCodeAuthzLevel = 0x02000000u,
		CreateDefaultErrorMode = 0x04000000u,
		CreateNoWindow = 0x08000000u,
	}

	public enum DebugEventCode : uint
	{
		EXCEPTION_DEBUG_EVENT = 1,
		CREATE_THREAD_DEBUG_EVENT = 2,
		CREATE_PROCESS_DEBUG_EVENT = 3,
		EXIT_THREAD_DEBUG_EVENT = 4,
		EXIT_PROCESS_DEBUG_EVENT = 5,
		LOAD_DLL_DEBUG_EVENT = 6,
		UNLOAD_DLL_DEBUG_EVENT = 7,
		OUTPUT_DEBUG_STRING_EVENT = 8,
		RIP_EVENT = 9
	}

	/// <summary>
	/// http://msdn.microsoft.com/en-us/library/windows/desktop/aa363082(v=vs.85).aspx
	/// </summary>
	public enum ExceptionCode : uint
	{
		None = 0,
		/// <summary>
		/// The thread tried to read from or write to a virtual address for which it does not have the appropriate access.
		/// </summary>
		AccessViolation = 0xC0000005,
		/// <summary>
		/// The thread tried to access an array element that is out of bounds and the underlying hardware supports bounds checking.
		/// </summary>
		DataTypeMisalignment = 0x80000002,
		/// <summary>
		/// A breakpoint was encountered.
		/// </summary>
		Breakpoint = 0x80000003,
		/// <summary>
		/// A trace trap or other single-instruction mechanism signaled that one instruction has been executed.
		/// </summary>
		SingleStep = 0x80000004,
		/// <summary>
		/// The thread tried to read or write data that is misaligned on hardware that does not provide alignment. For example, 16-bit values must be aligned on 2-byte boundaries; 32-bit values on 4-byte boundaries, and so on.
		/// </summary>
		ArrayBoundsExceeded = 0xC000008C,
		/// <summary>
		/// One of the operands in a floating-point operation is denormal. A denormal value is one that is too small to represent as a standard floating-point value.
		/// </summary>
		Float_DenormalOperand = 0xC000008D,
		/// <summary>
		/// The thread tried to divide a floating-point value by a floating-point divisor of zero.
		/// </summary>
		Float_DivideByZero = 0xC000008E,
		/// <summary>
		/// The result of a floating-point operation cannot be represented exactly as a decimal fraction.
		/// </summary>
		Float_InexactResult = 0xC000008F,
		/// <summary>
		/// This exception represents any floating-point exception not included in this enumeration.
		/// </summary>
		Float_InvalidOperation = 0xC0000090,
		/// <summary>
		/// The exponent of a floating-point operation is greater than the magnitude allowed by the corresponding type.
		/// </summary>
		Float_Overflow = 0xC0000091,
		/// <summary>
		/// The stack overflowed or underflowed as the result of a floating-point operation.
		/// </summary>
		Float_StackCheck = 0xC0000092,
		/// <summary>
		/// The exponent of a floating-point operation is less than the magnitude allowed by the corresponding type.
		/// </summary>
		Float_Underflow = 0xC0000093,
		/// <summary>
		/// The thread tried to divide an integer value by an integer divisor of zero.
		/// </summary>
		Integer_DivideByZero = 0xC0000094,
		/// <summary>
		/// The result of an integer operation caused a carry out of the most significant bit of the result.
		/// </summary>
		Integer_Overflow = 0xC0000095,
		/// <summary>
		/// The thread tried to execute an instruction whose operation is not allowed in the current machine mode.
		/// </summary>
		PrivilegedInstruction = 0xC0000096,
		/// <summary>
		/// The thread tried to access a page that was not present, and the system was unable to load the page. For example, this exception might occur if a network connection is lost while running a program over the network.
		/// </summary>
		InPageError = 0xC0000006,
		/// <summary>
		/// The thread tried to execute an invalid instruction.
		/// </summary>
		IllegalInstruction = 0xC000001D,
		/// <summary>
		/// The thread tried to continue execution after a noncontinuable exception occurred.
		/// </summary>
		NoncontinuableException = 0xC0000025,
		/// <summary>
		/// The thread used up its stack.
		/// </summary>
		StackOverflow = 0xC00000FD,
		/// <summary>
		/// An exception handler returned an invalid disposition to the exception dispatcher.
		/// Programmers using a high-level language such as C should never encounter this exception.
		/// </summary>
		InvalidDisposition = 0xC0000026,
		/// <summary>
		/// 
		/// </summary>
		GuardPageViolation = 0x80000001,
		/// <summary>
		/// 
		/// </summary>
		InvalidHandle = 0xC0000008,
		/// <summary>
		/// The DBG_CONTROL_C exception code occurs when CTRL+C is input to a console process that handles CTRL+C signals and is being debugged. This exception code is not meant to be handled by applications. It is raised only for the benefit of the debugger, and is raised only when a debugger is attached to the console process.
		/// </summary>
		CtrlC = 0x40010005,
		/// <summary>
		/// Will contain 1 parameter: the address to the thrown exception object.
		/// See druntime\src\rt\deh.d
		/// </summary>
		DigitalMarsDException = 0xE0440001,
	}

	public enum RipType : uint
	{
		/// <summary>
		/// Indicates that invalid data was passed to the function that failed. This caused the application to fail.
		/// </summary>
		SLE_ERROR = 1u,
		/// <summary>
		/// Indicates that invalid data was passed to the function, but the error probably will not cause the application to fail.
		/// </summary>
		SLE_MINORERROR = 2u,
		/// <summary>
		/// Indicates that potentially invalid data was passed to the function, but the function completed processing.
		/// </summary>
		SLE_WARNING = 3u,
		/// <summary>
		/// Indicates that only dwError was set.
		/// </summary>
		None = 0u,
	}

	public enum ContinueStatus : uint
	{
		/// <summary>
		/// If the thread specified by the dwThreadId parameter 
		/// previously reported an EXCEPTION_DEBUG_EVENT debugging event, 
		/// the function stops all exception processing and continues the thread. 
		/// For any other debugging event, this flag simply continues the thread.
		/// </summary>
		DBG_CONTINUE = 0x00010002,
		/// <summary>
		/// If the thread specified by dwThreadId previously reported 
		/// an EXCEPTION_DEBUG_EVENT debugging event, 
		/// the function continues exception processing. 
		/// If this is a first-chance exception event, 
		/// the search and dispatch logic of the structured exception 
		/// handler is used; otherwise, the process is terminated. 
		/// For any other debugging event, this flag simply continues the thread.
		/// </summary>
		DBG_EXCEPTION_NOT_HANDLED = 0x80010001
	}

	[Flags]
	public enum ContextFlags : uint
	{
		/// <summary>
		/// Both working for 386 and 486 CPUs
		/// </summary>
		CONTEXT_i386 = 0x00010000u,
		/// <summary>
		/// SS:SP, CS:IP, FLAGS, BP
		/// </summary>
		CONTEXT_CONTROL = (CONTEXT_i386 | 0x00000001u),
		/// <summary>
		/// AX, BX, CX, DX, SI, DI
		/// </summary>
		CONTEXT_INTEGER = (CONTEXT_i386 | 0x00000002u),
		/// <summary>
		/// DS, ES, FS, GS
		/// </summary>
		CONTEXT_SEGMENTS = (CONTEXT_i386 | 0x00000004u),
		/// <summary>
		/// 387 state
		/// </summary>
		CONTEXT_FLOATING_POINT = (CONTEXT_i386 | 0x00000008u),
		/// <summary>
		/// DB 0-3,6,7
		/// </summary>
		CONTEXT_DEBUG_REGISTERS = (CONTEXT_i386 | 0x00000010u),
		/// <summary>
		/// cpu specific extensions
		/// </summary>
		CONTEXT_EXTENDED_REGISTERS = (CONTEXT_i386 | 0x00000020u),

		CONTEXT_FULL = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS,
		CONTEXT_ALL = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS |
								 CONTEXT_FLOATING_POINT | CONTEXT_DEBUG_REGISTERS |
								 CONTEXT_EXTENDED_REGISTERS
	}

	public enum MachineType : uint
	{
		/// <summary>
		/// x86
		/// </summary>
		i386 = 0x014cu,
		/// <summary>
		/// Itanium
		/// </summary>
		iA64 = 0x0200u,
		/// <summary>
		/// AMD64
		/// </summary>
		AMD64 = 0x8664u
	}
	#endregion

	#region Structures
	/// <summary>
	/// Startup Info struct used with <see cref="API.CreateProcess"/>. Definition from pinvoke.net.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct STARTUPINFO
	{
		public Int32 cb;
		public string lpReserved;
		public string lpDesktop;
		public string lpTitle;
		public Int32 dwX;
		public Int32 dwY;
		public Int32 dwXSize;
		public Int32 dwYSize;
		public Int32 dwXCountChars;
		public Int32 dwYCountChars;
		public Int32 dwFillAttribute;
		public Int32 dwFlags;
		public Int16 wShowWindow;
		public Int16 cbReserved2;
		public IntPtr lpReserved2;
		public IntPtr hStdInput;
		public IntPtr hStdOutput;
		public IntPtr hStdError;
	}

	/// <summary>
	/// Process Information struct, used with <see cref="API.CreateProcess"/>. Definition from pinvoke.net.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct PROCESS_INFORMATION
	{
		public IntPtr hProcess;
		public IntPtr hThread;
		public uint dwProcessId;
		public uint dwThreadId;
	}

	/// <summary>
	/// Describes a debugging event.
	/// http://msdn.microsoft.com/en-us/library/windows/desktop/ms679308(v=vs.85).aspx
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct DEBUG_EVENT
	{
		/// <summary>
		/// The code that identifies the type of debugging event
		/// </summary>
		public DebugEventCode dwDebugEventCode;
		/// <summary>
		/// The identifier of the process in which the debugging event occurred. A debugger uses this value to locate the debugger's per-process structure. These values are not necessarily small integers that can be used as table indices.
		/// </summary>
		public uint dwProcessId;
		/// <summary>
		/// The identifier of the thread in which the debugging event occurred. A debugger uses this value to locate the debugger's per-thread structure. These values are not necessarily small integers that can be used as table indices.
		/// </summary>
		public uint dwThreadId;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public byte[] furtherStructData;
		public EXCEPTION_DEBUG_INFO Exception
		{
			get { return getStruct<EXCEPTION_DEBUG_INFO>(); }
		}
		public CREATE_THREAD_DEBUG_INFO CreateThread
		{
			get { return getStruct<CREATE_THREAD_DEBUG_INFO>(); }
		}
		public CREATE_PROCESS_DEBUG_INFO CreateProcessInfo
		{
			get { return getStruct<CREATE_PROCESS_DEBUG_INFO>(); }
		}
		public EXIT_THREAD_DEBUG_INFO ExitThread
		{
			get { return getStruct<EXIT_THREAD_DEBUG_INFO>(); }
		}
		public EXIT_PROCESS_DEBUG_INFO ExitProcess
		{
			get { return getStruct<EXIT_PROCESS_DEBUG_INFO>(); }
		}
		public LOAD_DLL_DEBUG_INFO LoadDll
		{
			get { return getStruct<LOAD_DLL_DEBUG_INFO>(); }
		}
		public UNLOAD_DLL_DEBUG_INFO UnloadDll
		{
			get { return getStruct<UNLOAD_DLL_DEBUG_INFO>(); }
		}
		public OUTPUT_DEBUG_STRING_INFO DebugString
		{
			get { return getStruct<OUTPUT_DEBUG_STRING_INFO>(); }
		}
		public RIP_INFO RipInfo
		{
			get { return getStruct<RIP_INFO>(); }
		}

		S getStruct<S>() where S : struct
		{
			var handle = GCHandle.Alloc(furtherStructData, GCHandleType.Pinned);
			var stuff = (S)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(S));
			handle.Free();
			return stuff;
		}
	}

	public class DebugEventData
	{
		public void ApplyFrom(DEBUG_EVENT d)
		{
			this.dwDebugEventCode = d.dwDebugEventCode;
			this.dwProcessId = d.dwProcessId;
			this.dwThreadId = d.dwThreadId;
			this.furtherStructData = d.furtherStructData;
		}

		/// <summary>
		/// The code that identifies the type of debugging event
		/// </summary>
		public DebugEventCode dwDebugEventCode;
		/// <summary>
		/// The identifier of the process in which the debugging event occurred. A debugger uses this value to locate the debugger's per-process structure. These values are not necessarily small integers that can be used as table indices.
		/// </summary>
		public uint dwProcessId;
		/// <summary>
		/// The identifier of the thread in which the debugging event occurred. A debugger uses this value to locate the debugger's per-thread structure. These values are not necessarily small integers that can be used as table indices.
		/// </summary>
		public uint dwThreadId;

		byte[] furtherStructData;
		public EXCEPTION_DEBUG_INFO Exception
		{
			get { return getStruct<EXCEPTION_DEBUG_INFO>(); }
		}
		public CREATE_THREAD_DEBUG_INFO CreateThread
		{
			get { return getStruct<CREATE_THREAD_DEBUG_INFO>(); }
		}
		public CREATE_PROCESS_DEBUG_INFO CreateProcessInfo
		{
			get { return getStruct<CREATE_PROCESS_DEBUG_INFO>(); }
		}
		public EXIT_THREAD_DEBUG_INFO ExitThread
		{
			get { return getStruct<EXIT_THREAD_DEBUG_INFO>(); }
		}
		public EXIT_PROCESS_DEBUG_INFO ExitProcess
		{
			get { return getStruct<EXIT_PROCESS_DEBUG_INFO>(); }
		}
		public LOAD_DLL_DEBUG_INFO LoadDll
		{
			get { return getStruct<LOAD_DLL_DEBUG_INFO>(); }
		}
		public UNLOAD_DLL_DEBUG_INFO UnloadDll
		{
			get { return getStruct<UNLOAD_DLL_DEBUG_INFO>(); }
		}
		public OUTPUT_DEBUG_STRING_INFO DebugString
		{
			get { return getStruct<OUTPUT_DEBUG_STRING_INFO>(); }
		}
		public RIP_INFO RipInfo
		{
			get { return getStruct<RIP_INFO>(); }
		}

		S getStruct<S>() where S : struct
		{
			var handle = GCHandle.Alloc(furtherStructData, GCHandleType.Pinned);
			var stuff = (S)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(S));
			handle.Free();
			return stuff;
		}
	}

	#region Debug info structs
	[StructLayout(LayoutKind.Sequential)]
	public struct EXCEPTION_DEBUG_INFO
	{
		/// <summary>
		/// An EXCEPTION_RECORD structure with information specific to the exception. This includes the exception code, flags, address, a pointer to a related exception, extra parameters, and so on.
		/// </summary>
		public EXCEPTION_RECORD32 ExceptionRecord;
		/// <summary>
		/// A value that indicates whether the debugger has previously encountered the exception specified by the ExceptionRecord member. If the dwFirstChance member is nonzero, this is the first time the debugger has encountered the exception. Debuggers typically handle breakpoint and single-step exceptions when they are first encountered. If this member is zero, the debugger has previously encountered the exception. This occurs only if, during the search for structured exception handlers, either no handler was found or the exception was continued.
		/// </summary>
		public int dwFirstChance;
	}

	public static class Constants
	{
		public const uint STILL_ACTIVE = 0x00000103;
		public const int EXCEPTION_MAXIMUM_PARAMETERS = 15; // maximum number of exception parameters
		public const uint INFINITE = 0xFFFFFFFF;
		public const int MAXIMUM_SUPPORTED_EXTENSION = 512;

		/// <summary>
		/// Define the size of the 80387 save area, which is in the context frame.
		/// </summary>
		public const int SIZE_OF_80387_REGISTERS = 80;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EXCEPTION_RECORD32
	{
		/// <summary>
		/// The reason the exception occurred. This is the code generated by a hardware exception, 
		/// or the code specified in the RaiseException function for a software-generated exception
		/// </summary>
		public ExceptionCode Code;
		/// <summary>
		/// The exception flags. This member can be either zero, indicating a continuable exception, or EXCEPTION_NONCONTINUABLE indicating a noncontinuable exception. Any attempt to continue execution after a noncontinuable exception causes the EXCEPTION_NONCONTINUABLE_EXCEPTION exception.
		/// </summary>
		public uint ExceptionFlags;
		/// <summary>
		/// A pointer to an associated EXCEPTION_RECORD structure. Exception records can be chained together to provide additional information when nested exceptions occur.
		/// </summary>
		public IntPtr ExceptionRecord;
		/// <summary>
		/// The address where the exception occurred.
		/// </summary>
		public IntPtr ExceptionAddress;
		/// <summary>
		/// The number of parameters associated with the exception. This is the number of defined elements in the ExceptionInformation array.
		/// </summary>
		public int NumberParameters;
		/// <summary>
		/// An array of additional arguments that describe the exception. The RaiseException function can specify this array of arguments. For most exception codes, the array elements are undefined.
		/// 
		/// For ExceptionCode.AccessViolation:
		/// The first element of the array contains a read-write flag that indicates the type of 
		/// operation that caused the access violation. If this value is zero, 
		/// the thread attempted to read the inaccessible data. If this value is 1, 
		/// the thread attempted to write to an inaccessible address. 
		/// If this value is 8, the thread causes a user-mode data execution prevention (DEP) violation.
		/// The second array element specifies the virtual address of the inaccessible data.
		/// 
		/// For ExceptionCode.InPageError:
		/// The first element of the array contains a read-write flag that indicates 
		/// the type of operation that caused the access violation. 
		/// If this value is zero, the thread attempted to read the inaccessible data. 
		/// If this value is 1, the thread attempted to write to an inaccessible address. 
		/// If this value is 8, the thread causes a user-mode data execution prevention (DEP) violation.
		/// The second array element specifies the virtual address of the inaccessible data.
		/// The third array element specifies the underlying NTSTATUS code that resulted in the exception.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.EXCEPTION_MAXIMUM_PARAMETERS, ArraySubType = UnmanagedType.U4)]
		public uint[] ExceptionInformation;
	}

	/// <summary>
	/// http://msdn.microsoft.com/en-us/library/windows/desktop/ms679287(v=vs.85).aspx
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct CREATE_THREAD_DEBUG_INFO
	{
		/// <summary>
		/// A handle to the thread whose creation caused the debugging event. 
		/// If this member is NULL, the handle is not valid. 
		/// Otherwise, the debugger has THREAD_GET_CONTEXT, THREAD_SET_CONTEXT, 
		/// and THREAD_SUSPEND_RESUME access to the thread, allowing the debugger 
		/// to read from and write to the registers of the thread and control execution of the thread.
		/// </summary>
		public IntPtr hThread;
		/// <summary>
		/// A pointer to a block of data. 
		/// At offset 0x2C into this block is another pointer, 
		/// called ThreadLocalStoragePointer, 
		/// that points to an array of per-module thread local storage blocks. 
		/// This gives a debugger access to per-thread data in the threads of 
		/// the process being debugged using the same algorithms that a compiler would use.
		/// </summary>
		public IntPtr lpThreadLocalBase;
		/// <summary>
		/// A pointer to the starting address of the thread. 
		/// This value may only be an approximation of the thread's starting address, 
		/// because any application with appropriate access to the thread can 
		/// change the thread's context by using the SetThreadContext function.
		/// </summary>
		public IntPtr lpStartAddress;
	}

	/// <summary>
	/// http://msdn.microsoft.com/en-us/library/windows/desktop/ms679286(v=vs.85).aspx
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct CREATE_PROCESS_DEBUG_INFO
	{
		public IntPtr hFile;
		public IntPtr hProcess;
		public IntPtr hThread;
		public IntPtr lpBaseOfImage;
		/// <summary>
		/// The offset to the debugging information in the file identified by the hFile member.
		/// </summary>
		public uint dwDebugInfoFileOffset;
		/// <summary>
		/// The size of the debugging information in the file, in bytes. If this value is zero, there is no debugging information.
		/// </summary>
		public uint nDebugInfoSize;
		/// <summary>
		/// A pointer to a block of data. At offset 0x2C into this block is another pointer, 
		/// called ThreadLocalStoragePointer, that points to an array of per-module thread local storage blocks. 
		/// This gives a debugger access to per-thread data in the threads of the process being debugged 
		/// using the same algorithms that a compiler would use.
		/// </summary>
		public IntPtr lpThreadLocalBase;
		/// <summary>
		/// A pointer to the starting address of the thread. 
		/// This value may only be an approximation of the thread's starting address, 
		/// because any application with appropriate access to 
		/// the thread can change the thread's context by using the SetThreadContext function.
		/// </summary>
		public IntPtr lpStartAddress;
		/// <summary>
		/// A pointer to the file name associated with the hFile member. 
		/// This parameter may be NULL, or it may contain the address of a string pointer 
		/// in the address space of the process being debugged. 
		/// That address may, in turn, either be NULL or point to the actual filename. 
		/// If fUnicode is a nonzero value, the name string is Unicode; otherwise, it is ANSI.
		/// 
		/// This member is strictly optional. 
		/// Debuggers must be prepared to handle the case where lpImageName is 
		/// NULL or *lpImageName (in the address space of the process being debugged) is NULL. 
		/// Specifically, the system does not provide an image name for a create process event, 
		/// and will not likely pass an image name for the first DLL event. 
		/// The system also does not provide this information in the case of debug events 
		/// that originate from a call to the DebugActiveProcess function.
		/// </summary>
		public IntPtr lpImageName;
		/// <summary>
		/// A value that indicates whether a file name specified by the lpImageName member is Unicode or ANSI. 
		/// A nonzero value indicates Unicode; zero indicates ANSI.
		/// </summary>
		public ushort fUnicode;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EXIT_THREAD_DEBUG_INFO
	{
		/// <summary>
		/// The exit code for the thread.
		/// </summary>
		public uint dwExitCode;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EXIT_PROCESS_DEBUG_INFO
	{
		/// <summary>
		/// The exit code for the process.
		/// </summary>
		public uint dwExitCode;
	}

	/// <summary>
	/// Contains information about a dynamic-link library (DLL) that has just been loaded.
	/// http://msdn.microsoft.com/en-us/library/windows/desktop/ms680351(v=vs.85).aspx
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LOAD_DLL_DEBUG_INFO
	{
		/// <summary>
		/// A handle to the loaded DLL. If this member is NULL, the handle is not valid. 
		/// Otherwise, the member is opened for reading and read-sharing in the context of the debugger.
		/// 
		/// When the debugger is finished with this file, it should close the handle using the CloseHandle function.
		/// </summary>
		public IntPtr hFile;
		/// <summary>
		/// A pointer to the base address of the DLL in the address space of the process loading the DLL.
		/// </summary>
		public IntPtr lpBaseOfDll;
		/// <summary>
		/// The offset to the debugging information in the file identified by the hFile member, in bytes. The system expects the debugging information to be in CodeView 4.0 format. This format is currently a derivative of Common Object File Format (COFF).
		/// </summary>
		public uint dwDebugInfoFileOffset;
		/// <summary>
		/// The size of the debugging information in the file, in bytes. If this member is zero, there is no debugging information.
		/// </summary>
		public int nDebugInfoSize;
		/// <summary>
		/// A pointer to the file name associated with hFile. 
		/// Note: It points to a pointer which points to the first char - lpImageName is not a pointer to the first char!
		/// This member may be NULL, or it may contain the address of a string pointer 
		/// in the address space of the process being debugged. 
		/// That address may, in turn, either be NULL or point to the actual filename. 
		/// If fUnicode is a nonzero value, the name string is Unicode; otherwise, it is ANSI.
		/// 
		/// This member is strictly optional. Debuggers must be prepared to handle 
		/// the case where lpImageName is NULL or *lpImageName (in the address space of the process being debugged) is NULL. 
		/// Specifically, the system will never provide an image name for a create process event, 
		/// and it will not likely pass an image name for the first DLL event. 
		/// The system will also never provide this information in the case of 
		/// debugging events that originate from a call to the DebugActiveProcess function.
		/// </summary>
		public IntPtr lpImageName;
		/// <summary>
		/// A value that indicates whether a filename specified by lpImageName is Unicode or ANSI. A nonzero value for this member indicates Unicode; zero indicates ANSI.
		/// </summary>
		public ushort fUnicode;
	}

	/// <summary>
	/// ontains information about a dynamic-link library (DLL) that has just been unloaded.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct UNLOAD_DLL_DEBUG_INFO
	{
		/// <summary>
		/// A pointer to the base address of the DLL in the address space of the process unloading the DLL.
		/// </summary>
		public IntPtr lpBaseOfDll;
	}

	/// <summary>
	/// Contains the address, format, and length, in bytes, of a debugging string.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct OUTPUT_DEBUG_STRING_INFO
	{
		/// <summary>
		/// The debugging string in the calling process's address space. The debugger can use the ReadProcessMemory function to retrieve the value of the string.
		/// </summary>
		public IntPtr lpDebugStringData;
		/// <summary>
		/// The format of the debugging string. If this member is zero, the debugging string is ANSI; if it is nonzero, the string is Unicode.
		/// </summary>
		public ushort fUnicode;
		/// <summary>
		/// The size of the debugging string, in characters. The length includes the string's terminating null character.
		/// </summary>
		public ushort nDebugStringLength;
	}

	/// <summary>
	/// Contains the error that caused the RIP debug event.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RIP_INFO
	{
		/// <summary>
		/// The error that caused the RIP debug event.
		/// </summary>
		public uint dwError;
		public RipType dwType;
	}
	#endregion

	[StructLayout(LayoutKind.Sequential)]
	public struct CONTEXT_x86
	{
		//
		// The flags values within this flag control the contents of
		// a CONTEXT record.
		//

		// If the context record is used as an input parameter, then
		// for each portion of the context record controlled by a flag
		// whose value is set, it is assumed that that portion of the
		// context record contains valid context. If the context record
		// is being used to modify a threads context, then only that
		// portion of the threads context will be modified.
		//

		// If the context record is used as an IN OUT parameter to capture
		// the context of a thread, then only those portions of the thread's
		// context corresponding to set flags will be returned.
		//

		// The context record is never used as an OUT only parameter.
		//
		public ContextFlags ContextFlags;

		//
		// This section is specified/returned if CONTEXT_DEBUG_REGISTERS is
		// set in ContextFlags. Note that CONTEXT_DEBUG_REGISTERS is NOT
		// included in CONTEXT_FULL.
		//
		public uint Dr0;
		public uint Dr1;
		public uint Dr2;
		public uint Dr3;
		public uint Dr6;
		public uint Dr7;

		//
		// This section is specified/returned if the
		// ContextFlags word contians the flag CONTEXT_FLOATING_POINT.
		//

		public FLOATING_SAVE_AREA FloatSave;

		//
		// This section is specified/returned if the
		// ContextFlags word contians the flag CONTEXT_SEGMENTS.
		//

		public uint SegGs;
		public uint SegFs;
		public uint SegEs;
		public uint SegDs;

		//
		// This section is specified/returned if the
		// ContextFlags word contians the flag CONTEXT_INTEGER.
		//

		public uint edi;
		public uint esi;
		public uint ebx;
		public uint edx;
		public uint ecx;
		public uint eax;

		//
		// This section is specified/returned if the
		// ContextFlags word contians the flag CONTEXT_CONTROL.
		//

		/// <summary>
		/// Base/Frame pointer
		/// </summary>
		public uint ebp;
		/// <summary>
		/// Instruction pointer
		/// </summary>
		public uint eip;
		public uint segCs; // MUST BE SANITIZED
		public Extendedx86ContextFlags eFlags; // MUST BE SANITIZED
		/// <summary>
		/// Stack pointer
		/// </summary>
		public uint esp;
		public uint segSs;

		//
		// This section is specified/returned if the ContextFlags word
		// contains the flag CONTEXT_EXTENDED_REGISTERS.
		// The format and contexts are processor specific
		//
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAXIMUM_SUPPORTED_EXTENSION)]
		public byte[] ExtendedRegisters;
	}

	[Flags]
	public enum Extendedx86ContextFlags : uint
	{
		/// <summary>
		/// Carry Flag. Set if the last arithmetic operation carried 
		/// (addition) or borrowed (subtraction) a bit beyond the 
		/// size of the register. This is then checked when the 
		/// operation is followed with an add-with-carry or 
		/// subtract-with-borrow to deal with values too large 
		/// for just one register to contain.
		/// </summary>
		Carry = 0,
		/// <summary>
		/// Parity Flag. Set if the number of set bits in the 
		/// least significant byte is a multiple of 2.
		/// </summary>
		Parity = 2,
		/// <summary>
		/// Adjust Flag. Carry of Binary Code Decimal (BCD) numbers arithmetic operations.
		/// </summary>
		Adjust = 1u<<4,
		/// <summary>
		/// Zero Flag. Set if the result of an operation is Zero (0).
		/// </summary>
		Zero = 1u<<6,
		/// <summary>
		/// Sign Flag. Set if the result of an operation is negative.
		/// </summary>
		Sign = 1u<<7,
		/// <summary>
		///  Trap Flag. Set if step by step debugging.
		/// </summary>
		Trap= 1u<<8,
		/// <summary>
		/// Interruption Flag. Set if interrupts are enabled.
		/// </summary>
		Interruption = 1u<<9,
		/// <summary>
		///  Direction Flag. Stream direction. If set, string operations 
		///  will decrement their pointer rather than incrementing it, 
		///  reading memory backwards.
		/// </summary>
		Direction = 1u<<10,
		/// <summary>
		/// Overflow Flag. Set if signed arithmetic operations result 
		/// in a value too large for the register to contain.
		/// </summary>
		Overflow = 1u<<11,
		/// <summary>
		/// I/O Privilege Level field (2 bits). I/O Privilege Level of the current process.
		/// </summary>
		IOPL_firstBit = 1u<<12,
		/// <summary>
		/// I/O Privilege Level field (2 bits). I/O Privilege Level of the current process.
		/// </summary>
		IOPL_lastBit = 1u<<13,
		/// <summary>
		/// Nested Task flag. Controls chaining of interrupts. 
		/// Set if the current process is linked to the next process.
		/// </summary>
		NestedTask = 1u<<14,
		/// <summary>
		/// Resume Flag. Response to debug exceptions.
		/// </summary>
		Resume = 1u << 16,
		/// <summary>
		/// Virtual-8086 Mode. Set if in 8086 compatibility mode.
		/// </summary>
		Virtual8086Mode = 1u << 17,
		/// <summary>
		/// Alignment Check. Set if alignment checking of memory references is done.
		/// </summary>
		AlignmentCheck = 1u<<18,
		/// <summary>
		/// Virtual Interrupt Flag. Virtual image of IF.
		/// </summary>
		VirtualInterrupt = 1u<<19,
		/// <summary>
		/// Virtual Interrupt Pending flag. Set if an interrupt is pending.
		/// </summary>
		VirtualInterruptPending = 1u <<20,
		/// <summary>
		/// Identification Flag. Support for CPUID instruction if can be set.
		/// </summary>
		Id = 1u<<21
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct FLOATING_SAVE_AREA
	{
		public uint ControlWord;
		public uint StatusWord;
		public uint TagWord;
		public uint ErrorOffset;
		public uint ErrorSelector;
		public uint DataOffset;
		public uint DataSelector;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.SIZE_OF_80387_REGISTERS)]
		public byte[] RegisterArea;
		public uint Cr0NpxState;
	}

	/// <summary>
	/// 
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct STACKFRAME64
	{
		/// <summary>
		/// An ADDRESS64 structure that specifies the program counter.
		/// x86:  The program counter is EIP.
		/// Intel Itanium:  The program counter is StIIP.
		/// x64:  The program counter is RIP.
		/// </summary>
		public ADDRESS64 AddrPC;
		/// <summary>
		/// An ADDRESS64 structure that specifies the return address.
		/// </summary>
		public ADDRESS64 AddrReturn;
		/// <summary>
		/// An ADDRESS64 structure that specifies the frame pointer.
		/// x86:  The frame pointer is EBP.
		/// Intel Itanium:  There is no frame pointer, but AddrBStore is used.
		/// x64:  The frame pointer is RBP or RDI. This value is not always used.
		/// </summary>
		public ADDRESS64 AddrFrame;
		/// <summary>
		/// An ADDRESS64 structure that specifies the stack pointer.
		/// x86:  The stack pointer is ESP.
		/// Intel Itanium:  The stack pointer is SP.
		/// x64:  The stack pointer is RSP.
		/// </summary>
		public ADDRESS64 AddrStack;
		/// <summary>
		/// Intel Itanium:  An ADDRESS64 structure that specifies the backing store (RsBSP).
		/// </summary>
		public ADDRESS64 AddrBStore;
		/// <summary>
		/// On x86 computers, this member is an FPO_DATA structure. If there is no function table entry, this member is NULL.
		/// </summary>
		public IntPtr FuncTableEntry;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public ulong[] Params;
		[MarshalAs(UnmanagedType.Bool)]
		public bool Far;
		[MarshalAs(UnmanagedType.Bool)]
		public bool Virtual;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public ulong[] Reserved;
		public KDHELP64 KdHelp;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct KDHELP64 {
	  ulong Thread;
	  uint   ThCallbackStack;
	  uint   ThCallbackBStore;
	  uint   NextCallback;
	  uint   FramePointer;
	  ulong KiCallUserMode;
	  ulong KeUserCallbackDispatcher;
	  ulong SystemRangeStart;
	  ulong KiUserExceptionDispatcher;
	  ulong StackBase;
	  ulong StackLimit;
	  [MarshalAs(UnmanagedType.ByValArray,SizeConst=5)]
		ulong[] Reserved;
	}

	/// <summary>
	/// Represents an address. It is used in the STACKFRAME64 structure.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ADDRESS64
	{
		/// <summary>
		/// The offset into the segment, or a 32-bit virtual address. The interpretation of this value depends on the value contained in the Mode member.
		/// </summary>
		public ulong Offset;
		/// <summary>
		/// The segment number. This value is used only for 16-bit addressing.
		/// </summary>
		public byte Segment;
		/// <summary>
		/// The addressing mode.
		/// </summary>
		public ADDRESS_MODE Mode;
	}

	public enum ADDRESS_MODE : uint
	{
		/// <summary>
		/// 16:16 addressing. To support this addressing mode, you must supply a TranslateAddressProc64 callback function.
		/// </summary>
		AddrMode1616 = 0,
		/// <summary>
		/// 16:32 addressing. To support this addressing mode, you must supply a TranslateAddressProc64 callback function.
		/// </summary>
		AddrMode1632=1,
		/// <summary>
		/// Real-mode addressing. To support this addressing mode, you must supply a TranslateAddressProc64 callback function.
		/// </summary>
		AddrModeReal=2,
		/// <summary>
		/// Flat addressing. This is the only addressing mode supported by the library.
		/// </summary>
		AddrModeFlat=3
	}
	#endregion
}
