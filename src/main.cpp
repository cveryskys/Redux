#include <iostream>
#include "injector.h"

int main() {
    std::string dllPath = "sample.dll";
    std::cout << "Attempting to inject: " << dllPath << std::endl;

    InjectScript(dllPath);


    return 0;
}
