using System.Diagnostics;

namespace ReduxExecutorUI {
    public static class Injector {
        public static bool Inject(string processName, string scriptPath) {
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0) return false;

            Process targetProcess = processes[0];
            ProcessStartInfo startInfo = new ProcessStartInfo {
                FileName = "RobloxPlayerBeta.exe",
                Arguments = $"-script {scriptPath}",
                UseShellExecute = true
            };

            Process.Start(startInfo);
            return true;
        }
    }
}
