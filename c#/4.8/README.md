# Auth Request Service

This project is a .NET Framework 4.8 application designed to generate a digitally signed authentication request to the Ordoclic API. It utilizes the BouncyCastle library for RSA signature generation and includes detailed steps for configuring and running the project.

## Prerequisites

### .NET Framework 4.8

This project requires .NET Framework 4.8, which is only supported on Windows. Ensure you are using a Windows machine to build and run the project.

### Visual Studio Installation

1. Download and install [Visual Studio](https://visualstudio.microsoft.com/downloads/).
2. During the installation, select the following workloads:
   - **.NET desktop development**
   - **ASP.NET and web development** (optional, if you plan to extend the project to web-based scenarios).
   - 4.8 developement tools

### Required Packages

After setting up the project in Visual Studio, ensure the following NuGet packages are installed:

1. **Newtonsoft.Json**: For JSON serialization and deserialization.
2. **BouncyCastle.NetCore**: For cryptographic operations.

To install the packages, run the following commands in the NuGet Package Manager Console:

```powershell
Install-Package Newtonsoft.Json
Install-Package BouncyCastle.NetCore
```

## How to Run the Project

1. Clone the repository or copy the project files to your local machine.

2. Open the solution file (`.sln`) in Visual Studio.

3. Add your private key in PEM format to the project directory and name it `private_key.pem`.

4. Modify the following values in the `Program.cs` file:

   - Replace `"private_key.pem"` with the path to your private key.
   - Replace the `partnerId` and `rpps` arguments with your specific values.

5. Build the project:

   - Open the **Build** menu in Visual Studio and select **Build Solution** (or press `Ctrl+Shift+B`).

6. Run the project:

   - Press `F5` or select **Start Debugging** from the Debug menu.

## HTTP Response Body

The HTTP response body from the API request will be printed to the console. For detailed documentation on how to interpret the response, refer to the main README file of the parent project.

## Output

When the project is built and executed successfully, Visual Studio generates an `.exe` file in the `bin\Debug` or `bin\Release` directory, depending on the build configuration. This executable can be used to run the program independently.

