using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace Backend_cs
{
    public class Program
    {
        public static void Main(string[] sender)
        {
            var builder = WebApplication.CreateBuilder(sender);
            // 1. Регистрируем службу контроллеров в контейнере
            builder.Services.AddControllers();

            var app = builder.Build();
            // 2. Включаем маршрутизацию для контроллеров
            app.MapControllers();
            app.Run();

        }
    }
}


