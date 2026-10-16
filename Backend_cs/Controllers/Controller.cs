using Microsoft.AspNetCore.Mvc;


namespace Backend_cs.Controllers
{
    [ApiController]             // Указывает, что этот класс — это API-контроллер
    [Route("[controller]")]     // Задает маршрут по имени контроллера (в данном случае: /home)
    public class Controller : ControllerBase
    {
        // Этот метод сработает при GET-запросе на адрес /
        [HttpGet]
        public string Get()
        {
            return "ГЛАВНЫЙ";
        }
        // Этот метод сработает при GET-запросе на адрес /home/test
        [HttpGet("test")]
        public string GetTest()
        {
            return "TEST"; 
        }
    }
}
