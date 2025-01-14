#include "injector.h"
#include <Windows.h>
#include <iostream>

void InjectScript(const std::string& dllPath) {
    HWND robloxWindow = FindWindowA(NULL, "Roblox");
    if (!robloxWindow) {
        std::cerr << "Error: Roblox is not running" << std::endl;
        return;
    }

    DWORD processID;
    GetWindowThreadProcessId(robloxWindow, &processID);
    HANDLE processHandle = OpenProcess(PROCESS_ALL_ACCESS, FALSE, processID);

    if (!processHandle) {
        std::cerr << "Error: Unable to open Roblox process" << std::endl;
        return;
    }

    void* allocatedMemory = VirtualAllocEx(processHandle, NULL, dllPath.size() + 1, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
    if (!allocatedMemory) {
        std::cerr << "Error: Memory allocation failed" << std::endl;
        return;
    }

    if (!WriteProcessMemory(processHandle, allocatedMemory, dllPath.c_str(), dllPath.size() + 1, NULL)) {
        std::cerr << "Error: Writing to process memory failed" << std::endl;
        return;
    }

    HANDLE remoteThread = CreateRemoteThread(processHandle, NULL, 0, (LPTHREAD_START_ROUTINE)GetProcAddress(GetModuleHandleA("kernel32.dll"), "LoadLibraryA"), allocatedMemory, 0, NULL);
    if (!remoteThread) {
        std::cerr << "Error: Remote thread creation failed" << std::endl;
        return;
    }

    std::cout << "Injection worked" << std::endl;

    // quick clean up buster
    CloseHandle(remoteThread);
    CloseHandle(processHandle);
}
