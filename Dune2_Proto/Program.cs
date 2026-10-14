using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Backend_cs
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Запуск графического движка Dune 2...");

            using (var game = new DuneGame())
            {
                game.Run();
            }
        }
    }

    public class DuneGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _cursorTexture;

        // Текстура в 1 пиксель для рисования линий сетки
        private Texture2D _lineTexture;

        private int _frameCounter = 0;
        private double _elapsedTime = 0;
        private int _currentFps = 0;

        private int _mouseX = 0;
        private int _mouseY = 0;

        // ПАРАМЕТРЫ СЕТКИ КАРТЫ
        private const int TileSize = 40; // Размер одной клетки в пикселях (40х40)
        private int _gridWidth;          // Сколько клеток поместится по ширине
        private int _gridHeight;         // Сколько клеток поместится по высоте

        // Переменные для хранения клетки, на которую указывает мышь
        private int _hoveredTileX = -1;
        private int _hoveredTileY = -1;

        // Идеально отцентрованная матрица курсора
        private readonly string[] _cursorPattern = new string[]
        {
            "BB          ",
            "BWB         ",
            "BWWB        ",
            "BWWWB       ",
            "BWWWWB      ",
            "BWWWWWB     ",
            "BWWWWWWB    ",
            "BWWWWWWWB   ",
            "BWWWWWWWWB  ",
            "BWWWWWWWWB  ",
            "BWWWWBBBBB  ",
            "BWWBWBB     ",
            "BB  BWWB    ",
            "    BWWB    ",
            "     BWWB   ",
            "     BWWB   ",
            "      BB    "
        };

        public DuneGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.SynchronizeWithVerticalRetrace = true;
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            // Устанавливаем разрешение 800х600
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            // Вычисляем размеры сетки (800/40 = 20 клеток в ширину, 600/40 = 15 в высоту)
            _gridWidth = GraphicsDevice.Viewport.Width / TileSize;
            _gridHeight = GraphicsDevice.Viewport.Height / TileSize;

            IsMouseVisible = false;

            Window.Title = "Dune 2: Наследие Мегадрайва (Прототип)";
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // 1. Создаем текстуру для курсора
            int cWidth = 12;
            int cHeight = 17;
            _cursorTexture = new Texture2D(GraphicsDevice, cWidth, cHeight);
            Color[] cursorColorData = new Color[cWidth * cHeight];

            for (int y = 0; y < cHeight; y++)
            {
                for (int x = 0; x < cWidth; x++)
                {
                    char symbol = _cursorPattern[y][x];
                    int index = x + y * cWidth;

                    if (symbol == 'B') cursorColorData[index] = Color.Black;
                    else if (symbol == 'W') cursorColorData[index] = Color.White;
                    else cursorColorData[index] = Color.Transparent;
                }
            }
            _cursorTexture.SetData(cursorColorData);

            // 2. Создаем текстуру в 1 пиксель для рисования линий карты
            _lineTexture = new Texture2D(GraphicsDevice, 1, 1);
            _lineTexture.SetData(new[] { Color.White });

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime >= 1.0)
            {
                _currentFps = _frameCounter;
                _frameCounter = 0;
                _elapsedTime -= 1.0;

                Window.Title = $"Dune 2: Наследие Мегадрайва (Прототип) | FPS: {_currentFps}";
            }

            // Получаем координаты мыши
            MouseState mouseState = Mouse.GetState();
            _mouseX = mouseState.X;
            _mouseY = mouseState.Y;

            // РАССЧИТЫВАЕМ, НА КАКОЙ КЛЕТКЕ НАХОДИТСЯ МЫШЬ
            // Делим пиксельные координаты мыши на размер клетки (40)
            _hoveredTileX = _mouseX / TileSize;
            _hoveredTileY = _mouseY / TileSize;

            // Проверяем, чтобы значения не выходили за пределы экрана
            if (_hoveredTileX < 0 || _hoveredTileX >= _gridWidth) _hoveredTileX = -1;
            if (_hoveredTileY < 0 || _hoveredTileY >= _gridHeight) _hoveredTileY = -1;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _frameCounter++;

            GraphicsDevice.Clear(new Color(210, 140, 60));

            // Включаем ультра-четкий режим отрисовки без размытия
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);

            // 1. РИСУЕМ СЕТКУ КАРТЫ
            Color gridColor = new Color(190, 125, 50); // Цвет линий чуть темнее песка

            // Рисуем вертикальные линии
            for (int x = 0; x <= _gridWidth; x++)
            {
                _spriteBatch.Draw(_lineTexture, new Rectangle(x * TileSize, 0, 1, GraphicsDevice.Viewport.Height), gridColor);
            }

            // Рисуем горизонтальные линии
            for (int y = 0; y <= _gridHeight; y++)
            {
                _spriteBatch.Draw(_lineTexture, new Rectangle(0, y * TileSize, GraphicsDevice.Viewport.Width, 1), gridColor);
            }

            // 2. ПОДСВЕЧИВАЕМ КЛЕТКУ ПОД КУРСОРОМ
            if (_hoveredTileX != -1 && _hoveredTileY != -1)
            {
                Color highlightColor = new Color(255, 255, 255, 80); // Полупрозрачный белый (80 из 255 альфа)

                // Заливаем клетку мягким светом
                _spriteBatch.Draw(
                    _lineTexture,
                    new Rectangle(_hoveredTileX * TileSize, _hoveredTileY * TileSize, TileSize, TileSize),
                    highlightColor
                );
            }

            // 3. РИСУЕМ КУРСОР МЫШИ (поверх сетки и подсветки)
            _spriteBatch.Draw(_cursorTexture, new Rectangle(_mouseX, _mouseY, 24, 34), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);

        }
    }
}
