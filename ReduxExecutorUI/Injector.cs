using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ReduxExecutorUI {
    public static class Injector {
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

        public static bool Inject(string processName, string dllPath) {
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0) {
                Console.WriteLine($"Process {processName} not found.");
                return false;
            }

            Process targetProcess = processes[0];
            IntPtr hProcess = OpenProcess(0x001F0FFF, false, targetProcess.Id);

            if (hProcess == IntPtr.Zero) {
                Console.WriteLine("Failed to open target process.");
                return false;
            }

            IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), 0x3000, 0x40);

            if (allocMemAddress == IntPtr.Zero) {
                Console.WriteLine("Failed to allocate memory in target process.");
                return false;
            }

            byte[] dllBytes = System.Text.Encoding.ASCII.GetBytes(dllPath);
            WriteProcessMemory(hProcess, allocMemAddress, dllBytes, (uint)dllBytes.Length, out _);

            IntPtr hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, out _);
            if (hThread == IntPtr.Zero) {
                Console.WriteLine("Failed to create remote thread in target process.");
                return false;
            }

            Console.WriteLine("Injection successful!");
            return true;
        }
    }
}
