using System.Collections.Generic;
using greed.Game.Casting;
using greed.Game.Services;


namespace greed.Game.Directing
{
    /// <summary>
    /// <para>A person who directs the game.</para>
    /// <para>
    /// The responsibility of a Director is to control the sequence of play.
    /// </para>
    /// </summary>
    public class Director
    {
        private KeyboardService keyboardService = null;
        private VideoService videoService = null;
        public int pointValue = 0;

        /// <summary>
        /// Constructs a new instance of Director using the given KeyboardService and VideoService.
        /// </summary>
        /// <param name="keyboardService">The given KeyboardService.</param>
        /// <param name="videoService">The given VideoService.</param>
        public Director(KeyboardService keyboardService, VideoService videoService)
        {
            this.keyboardService = keyboardService;
            this.videoService = videoService;
        }

        /// <summary>
        /// Starts the game by running the main game loop for the given cast.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        public void StartGame(Cast cast)
        {
            videoService.OpenWindow();
            while (videoService.IsWindowOpen())
            {
                GetInputs(cast);
                DoUpdates(cast);
                DoOutputs(cast);
            }
            videoService.CloseWindow();
        }

        /// <summary>
        /// Gets directional input from the keyboard and applies it to the collector.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void GetInputs(Cast cast)
        {
            Actor collector = cast.GetFirstActor("collector");
            Point velocity = keyboardService.GetDirection();
            collector.SetVelocity(velocity);  

        }

        /// <summary>
        /// Updates the collector's position and resolves any collisions with artifacts.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void DoUpdates(Cast cast)
        {
            Actor banner = cast.GetFirstActor("banner");
            Actor collector = cast.GetFirstActor("collector");
            List<Actor> rocks = cast.GetActors("rocks");
            List<Actor> gems = cast.GetActors("gems");

            banner.SetText("");
            int maxX = videoService.GetWidth();
            int maxY = videoService.GetHeight();
            collector.MoveNext(maxX, maxY);

            int Rvalue = 0;
            int Gvalue = 0;

            foreach (Rock rock in rocks)
            {
                // setting velocity
                Point rockVelocity = keyboardService.MoveItem();
                rock.SetVelocity(rockVelocity);
                rock.MoveNext(maxX, maxY);

                if (collector.GetPosition().Equals(rock.GetPosition()))
                {    
                    pointValue = pointValue - 1;
                } 
            } 

            foreach(Gem gem in gems){

                Point gemVelocity = keyboardService.MoveItem();
                gem.SetVelocity(gemVelocity);
                gem.MoveNext(maxX, maxY);

                if (collector.GetPosition().Equals(gem.GetPosition())){
                    pointValue += 1;
                    // pointValue += Gvalue;
                }

            }
            // pointValue = Rvalue + Gvalue;
            string valuestring = pointValue.ToString();
            banner.SetText($"points: {valuestring}");
        }

        /// <summary>
        /// Draws the actors on the screen.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        public void DoOutputs(Cast cast)
        {
            List<Actor> actors = cast.GetAllActors();
            videoService.ClearBuffer();
            videoService.DrawActors(actors);
            videoService.FlushBuffer();
            
        }

    }
}