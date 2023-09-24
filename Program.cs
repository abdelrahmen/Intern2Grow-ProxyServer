namespace ProxyServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapGet("/proxy", async context =>
            {
                // Get the target URL from the query parameters
                var targetUrl = context.Request.Query["url"].ToString();

                if (string.IsNullOrWhiteSpace(targetUrl))
                {
                    context.Response.StatusCode = 400; // Bad Request
                    await context.Response.WriteAsync("You must provide a 'url' query parameter.");
                    return;
                }

                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        // Forward the client's request to the specified target URL
                        var targetResponse = await httpClient.GetAsync(targetUrl);

                        if (targetResponse.Content.Headers.ContentType != null)
                        {
                            // Get the ContentType header value
                            var contentType = targetResponse.Content.Headers.ContentType.MediaType;

                            // Set the response's ContentType header to match the target server's ContentType
                            context.Response.ContentType = contentType;
                        }
                        else
                        {
                            context.Response.ContentType = "text/html"; // Adjust content type as needed
                        }

                        // Read and forward the target server's response content
                        var content = await targetResponse.Content.ReadAsStringAsync();

                        // Set the target server's response as the response to the client
                        await context.Response.WriteAsync(content);
                    }
                    catch (Exception ex)
                    {
                        // Handle errors appropriately
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync($"Proxy Error: {ex.Message}");
                    }
                }

            });


            app.Run();
        }
    }
}