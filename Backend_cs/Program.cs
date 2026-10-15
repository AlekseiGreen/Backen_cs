using Microsoft.AspNetCore.Builder;


namespace Backend_cs
{
    public class Program
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


