#include "injector.h"
#include <Windows.h>
#include <iostream>

void InjectScript(const std::string& dllPath) {
    HWND robloxWindow = FindWindow(NULL, "Roblox");
    if (!robloxWindow) {
        std::cerr << "Error: Roblox is not running." << std::endl;
        return;
    }


    DWORD processID;
     GetWindowThreadProcessId(robloxWindow, &processID);
        

    if (!processHandle) {

        std::cerr << "Error: Unable to open roblox source" << std::endl;
        return;
    }


    void* allocatedMemory = VirtualAllocEx(processHandle, NULL, dllPath.size() + 1, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE),

    if (!allocatedMemory) {

        std::cerr << "Error: Memory allocation failed" << std::endl;
        return;
    }


    WriteProcessMemory(processHandle, allocatedMemory, dllPath.c_str(), dllPath.size(), + 1, NULL);

    HANDLE remoteThread = CreateRemoteThread(processHandle, NULL, 0, (LPTHREAD_START_ROUTINE)GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA"), allocatedMemory, 0, NULL);
      if (!remoteThread) {
        std::cerr << "Error: Remote creation failed" << std::endl;
        return;
    }


    std::cout << "Injection worked" << std::endl;

    //clean up rq

    CloseHandle(remoteThread);
    CloseHandle(processHandle);
}