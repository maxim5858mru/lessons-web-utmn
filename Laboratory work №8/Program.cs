using Microsoft.AspNetCore.Mvc.RazorPages;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var pages = new string[] { "index", "content-csharp", "content-go", "content-java", "content-python", "error" };
var fonts = new string[] { "Consola B", "Consola I", "Consola Z", "Consola", "Segoe Icons" };

app.UseHttpsRedirection();

app.MapWhen(
    context => context.Request.Path == "/",
    appBuilder => appBuilder.Run(async context =>
    {
        context.Response.Redirect("/index.html", true);
    })
);

app.MapWhen(
    context => context.Request.Path == $"/style.css",
    appBuilder => appBuilder.Run(async context =>
    {
        context.Response.ContentType = "text/css; charset=utf-8";
        await context.Response.SendFileAsync($"wwwroot/style.css");
    })
);

foreach (var font in fonts)
{
    app.MapWhen(
        context => context.Request.Path == $"/fonts/{font}.ttf",
        appBuilder => appBuilder.Run(async context =>
        {
            context.Response.ContentType = "application/octet-stream; charset=utf-8";
            await context.Response.SendFileAsync($"wwwroot/fonts/{font}.ttf");
        })
    );
}

foreach (var page in pages)
{
    app.MapWhen(
        context => context.Request.Path == $"/{page}.html",
        appBuilder => appBuilder.Run(async context =>
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.SendFileAsync($"wwwroot/{page}.html");
        })
    );
}

app.Run();
