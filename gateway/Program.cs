using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p =>
        p.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader());
});

var app = builder.Build();
app.UseCors("AllowAll");

app.Map("/{**path}", async (HttpContext context, IHttpClientFactory factory) =>
{
    var http = factory.CreateClient();
    var path = context.Request.Path.ToString();
    var method = context.Request.Method;

    string target = path switch
    {
        var p when p.StartsWith("/api/product") || p.StartsWith("/api/inventory") => $"http://inventory:8080{path}",
        var p when p.StartsWith("/api/invoice") => $"http://billing:8080{path}",
        _ => null
    };

    if (target == null)
        return Results.NotFound("Rota não encontrada no gateway");

    string body = "";
    if (context.Request.ContentLength > 0)
    {
        using var reader = new StreamReader(context.Request.Body);
        body = await reader.ReadToEndAsync();
    }

    var request = new HttpRequestMessage(new HttpMethod(method), target);
    if (!string.IsNullOrEmpty(body))
        request.Content = new StringContent(body, Encoding.UTF8, "application/json");

    foreach (var header in context.Request.Headers)
    {
        if (header.Key.Equals("Host", StringComparison.OrdinalIgnoreCase)) continue;

        if (!request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
            request.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
    }

    // 🔹 Garantir que a resposta seja completa antes de retornar
    HttpResponseMessage response;
    try
    {
        response = await http.SendAsync(request, HttpCompletionOption.ResponseContentRead);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Erro ao conectar ao serviço de destino: {ex.Message}");
    }

    var content = await response.Content.ReadAsStringAsync();
    return Results.Content(
        content,
        response.Content.Headers.ContentType?.ToString() ?? "application/json",
        Encoding.UTF8,
        (int)response.StatusCode
    );
});

app.Run();