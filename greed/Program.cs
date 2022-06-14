﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using greed.Game.Casting;
using greed.Game.Directing;
using greed.Game.Services;


namespace greed
{
    /// <summary>
    /// The program's entry point.
    /// </summary>
    class Program
    {
        private static int FRAME_RATE = 20;
        private static int MAX_X = 900;
        private static int MAX_Y = 600;
        private static int CELL_SIZE = 15;
        private static int FONT_SIZE = 15;
        private static int COLS = 60;
        private static int ROWS = 40;
        private static string CAPTION = "Greed";
        // private static string DATA_PATH = "Data/messages.txt";
        private static Color WHITE = new Color(255, 255, 255);
        private static int DEFAULT_ARTIFACTS = 40;
        


        /// <summary>
        /// Starts the program using the given arguments.
        /// </summary>
        /// <param name="args">The given arguments.</param>
        static void Main(string[] args)
        {
            // create the cast
            Cast cast = new Cast();

            // create the banner
            Actor banner = new Actor();
            banner.SetText("");
            banner.SetFontSize(FONT_SIZE * 2);
            banner.SetColor(WHITE);
            banner.SetPosition(new Point(CELL_SIZE, 0));
            cast.AddActor("banner", banner);

            // create the collector
            Actor collector = new Actor();
            collector.SetText("U");
            collector.SetFontSize(FONT_SIZE * 2);
            collector.SetColor(WHITE);
            collector.SetPosition(new Point(MAX_X / 2, 570));
            cast.AddActor("collector", collector);

            // load the messages
            // List<string> messages = File.ReadAllLines(DATA_PATH).ToList<string>();

            // create the artifacts
            Random random = new Random();
            for (int i = 0; i < DEFAULT_ARTIFACTS; i++)
            {
                // string text = (random.Next(48, 42)).ToString();
                // string message = messages[i];

                int x = random.Next(1, COLS);
                int y = random.Next(1, ROWS);
                Point position = new Point(x, y);
                position = position.Scale(CELL_SIZE);
                Point Gposition = new Point(y, x);
                Gposition = Gposition.Scale(CELL_SIZE);

                int r = random.Next(0, 256);
                int g = random.Next(0, 256);
                int b = random.Next(0, 256);
                Color color = new Color(r, g, b);

                Rock rock = new Rock();
                rock.SetText("0");
                rock.SetFontSize(FONT_SIZE);
                rock.SetColor(color);
                rock.SetPosition(position);
                cast.AddActor("rocks", rock);


                Gem gem = new Gem();
                gem.SetText("*");
                gem.SetFontSize(FONT_SIZE);
                gem.SetColor(color);
                gem.SetPosition(Gposition);
                cast.AddActor("gems", gem);
            }

            // start the game
            KeyboardService keyboardService = new KeyboardService(CELL_SIZE);
            VideoService videoService 
                = new VideoService(CAPTION, MAX_X, MAX_Y, CELL_SIZE, FRAME_RATE, false);
            Director director = new Director(keyboardService, videoService);
            director.StartGame(cast);

            // test comment
        }
    }
}