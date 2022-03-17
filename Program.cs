using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace snakeSensores
{
    enum Direction
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    }

    enum Tile
    {
        Open = 0,
        Snake,
        Food,
    }

    class Program
    {
        static char[] DirectionChars = { '^', 'v', '<', '>', };
        static TimeSpan sleep = TimeSpan.FromMilliseconds(100);
        static int width = Console.WindowWidth;
        static int height = Console.WindowHeight;
        static Random random = new Random();
        static Tile[,] map = new Tile[width, height];
        static Direction? direction = null;
        static Queue<(int X, int Y)> snake = new Queue<(int X, int Y)>();
        static (int X, int Y) position;
        static bool closeRequested = false;

        static void Main(string[] args)
        {

            position.X = width / 2;
            position.Y = height / 2;
            try
            {
                Console.CursorVisible = false;
                Console.Clear();
                snake.Enqueue(position);
                map[position.X, position.Y] = Tile.Snake;
                PositionFood();
                Console.SetCursorPosition(position.X, position.Y);
                Console.Write('@');

                Thread serialThr = new Thread(serialComm);
                serialThr.Start();
                Thread keyboardThr = new Thread(keyboard);
                keyboardThr.Start();


                while (!direction.HasValue && !closeRequested) ;

                while (!closeRequested)
                {
                    if (Console.WindowWidth != width || Console.WindowHeight != height)
                    {
                        Console.Clear();
                        Console.Write("Console was resized. Snake game has ended.");
                        return;
                    }
                    switch (direction)
                    {
                        case Direction.Up: position.Y--; break;
                        case Direction.Down: position.Y++; break;
                        case Direction.Left: position.X--; break;
                        case Direction.Right: position.X++; break;
                    }
                    if (position.X < 0 || position.X >= width ||
                        position.Y < 0 || position.Y >= height ||
                        map[position.X, position.Y] is Tile.Snake)
                    {
                        Console.Clear();
                        Console.Write("Game Over. Score: " + (snake.Count - 1) + ".");
                        return;
                    }
                    Console.SetCursorPosition(position.X, position.Y);
                    Console.Write(DirectionChars[(int)direction]);
                    snake.Enqueue(position);
                    if (map[position.X, position.Y] == Tile.Food)
                    {
                        PositionFood();
                    }
                    else
                    {
                        (int x, int y) = snake.Dequeue();
                        map[x, y] = Tile.Open;
                        Console.SetCursorPosition(x, y);
                        Console.Write(' ');
                    }
                    map[position.X, position.Y] = Tile.Snake;

                    Thread.Sleep(sleep);
                }
                Console.Clear();
            }
            finally
            {
                Console.CursorVisible = true;
                Environment.Exit(0);

            }
        }

        static void GetDirection()
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow: direction = Direction.Up; break;
                case ConsoleKey.DownArrow: direction = Direction.Down; break;
                case ConsoleKey.LeftArrow: direction = Direction.Left; break;
                case ConsoleKey.RightArrow: direction = Direction.Right; break;
                case ConsoleKey.Escape: closeRequested = true; break;
            }
        }

        static void PositionFood()
        {
            int xvalue;
            int yvalue;

            while (true)
            {
                xvalue = random.Next(width);
                yvalue = random.Next(height);
                if (map[xvalue, yvalue] is Tile.Open) break;
            }
            map[xvalue, yvalue] = Tile.Food;
            Console.SetCursorPosition(xvalue, yvalue);
            Console.Write('+');
        }

        static void keyboard()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    GetDirection();
                }

            }

        }

        static void serialComm()
        {
            // 1. Inicializa y abre el puerto serial
            while (true)
            {
                // 2. Implementa la lógica con tu solución.
                Thread.Sleep(50);
            }
        }
    }
}
