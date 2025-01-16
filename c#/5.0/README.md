# Authentication Service Program

This program is a C# application designed to generate a signed authentication request for the Ordoclic API. The application reads a private key, signs the payload, and sends the request to the authentication endpoint.

## Prerequisites

1. .NET SDK installed (tested with .NET 5.0 or newer).
2. A private key file in PEM format.
3. OpenSSL installed to convert the private key if necessary.

## Key Conversion

The private key must be in PKCS#8 format. Use the following OpenSSL command to convert your private key:

```bash
openssl pkcs8 -topk8 -inform PEM -outform PEM -in private_key.pem -out pkcs8_private_key.pem -nocrypt
```

Replace `private_key.pem` with the path to your original private key file. The output file will be `pkcs8_private_key.pem`.

## Building the Project

To build the project, run the following command in the project directory:

```bash
dotnet build
```

This will compile the project and generate the executable files.

## Running the Program

Run the program with the following command:

```bash
dotnet run ./pkcs8_private_key.pem <partnerId> <rpps>
```

- Replace `./pkcs8_private_key.pem` with the path to your converted private key file.
- Replace `<partnerId>` with your actual partner ID (e.g., `eee1fc83-1781-4275-a5a0-c5505cfebd55`).
- Replace `<rpps>` with the RPPS number (e.g., `90000000001`).

### Example:

```bash
dotnet run ./pkcs8_private_key.pem eee1fc83-1781-4275-a5a0-c5505cfebd55 90000000001
```

## Program Output

1. **Unsigned JSON:**
   The unsigned JSON payload generated from the input parameters.
2. **Signed JSON:**
   The JSON payload with the signature included.
3. **Response:**
   The response from the authentication endpoint, if the request is successful.

### Error Handling

If an error occurs during execution (e.g., invalid key format, network error), it will be logged to the console with the corresponding message and stack trace.

## Additional Notes

- Ensure the `AuthUrl` constant in the program is pointing to the correct API endpoint.
- The private key file should be securely stored and accessible only to authorized personnel.

## Contact

For support or further information, please contact the development team.

