using Microsoft.AspNetCore.Builder;


namespace Backend_cs
{
    public class Programm
    {
        public static void Main(string[] sender)
        {
            var builder = WebApplication.CreateBuilder(sender);
            var app = builder.Build();

            app.MapGet("/", () => "Hello C#");

            app.Run();
        }
    }
}


