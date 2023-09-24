# Simple HTTP Proxy Server

This is a basic HTTP proxy server built using ASP.NET Core. It allows clients to proxy requests to a specified target URL and forwards responses from the target server back to clients while preserving the content type.

## Getting Started

1. Clone this repository to your local machine.

2. Make sure you have [.NET Core](https://dotnet.microsoft.com/download/dotnet) installed.

3. Navigate to the project directory.

4. Run the application using the following command:
   ```dotnet run```
5. The proxy server will start on the port specified in the launchsettings

## Usage

To use the proxy server, make a GET request to the `/proxy` endpoint with a `url` query parameter specifying the target URL you want to proxy to. For example:

**GET http://localhost:5000/proxy?url=https://example.com**

The server will forward the request to the specified URL and return the response to you while preserving the content type.

## Error Handling

The proxy server handles errors gracefully. If the target URL is invalid or the target server encounters an error, the server will respond with an appropriate error message.

