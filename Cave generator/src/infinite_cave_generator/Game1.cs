using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace infinite_cave_generator
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        // rules: B678/S345678
        // final smoothing: B5678/S5678

        const int MAP_SIDE = 256;
        const int TILES = 4;
        const int TILE_SIDE = MAP_SIDE / TILES;
        const int PLAYER_SIZE = 2;
        const float PLAYER_SPEED = 0.5f;

        private SpriteFont font;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D lineTexture;
        Texture2D tileFill;
        Texture2D cellFill;
        List<Tile> loadedTiles;
        List<Tile> tileMap;
        Player player;
        Vector2 oldPos;
        Camera camera;
        string debug;

        Thread thread1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            loadedTiles = new List<Tile>();
            tileMap = new List<Tile>();
            player = new Player(GraphicsDevice, PLAYER_SIZE, PLAYER_SPEED, new Vector2(128, 128));
            camera = new Camera();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            lineTexture = new Texture2D(GraphicsDevice, 1, 1);
            lineTexture.SetData(new Color[] { Color.White });

            cellFill = new Texture2D(GraphicsDevice, 1, 1);
            cellFill.SetData(new Color[] { Color.Black });

            tileFill = new Texture2D(GraphicsDevice, TILE_SIDE, TILE_SIDE);
            Color[] tiles = new Color[TILE_SIDE * TILE_SIDE];
            font = Content.Load<SpriteFont>("debug");

            

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            base.UnloadContent();
            lineTexture.Dispose();
            cellFill.Dispose();
            tileFill.Dispose();
            player.Texture.Dispose();
            spriteBatch.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.move(loadedTiles, TILE_SIDE);
            camera.Pos = player.Pos;
            player.TilePos = new Vector2(
                (float)Math.Ceiling(player.CenterPos.X / (TILE_SIDE*2)),
                (float)Math.Ceiling(player.CenterPos.Y / (TILE_SIDE*2))
                );
            debug = "X: " + (int)player.CenterPos.X + " Y: " + (int)player.CenterPos.Y;
            if (player.TilePos != oldPos)
            {
                tileHandle();
            }
            
            oldPos = player.TilePos;
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        camera.get_transformation(GraphicsDevice));

            foreach (Tile tile in tileMap)
            {
                drawTile(tile);
            }
            
            spriteBatch.Draw(player.Texture, player.Pos, player.Color);
            spriteBatch.DrawString(font, debug, new Vector2(player.Pos.X - 200 + 5, player.Pos.Y - 113 + 5), Color.Red);
            spriteBatch.DrawString(font, "tile X: " + player.TilePos.X + " tile Y: " + player.TilePos.Y, new Vector2(player.Pos.X - 200 + 5, player.Pos.Y - 113 + 13), Color.Red);

            //DrawGrid();

            spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(lineTexture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                Color.Black, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }

        public void DrawGrid()
        {
            for (int i = -TILES/2; i <= TILES/2; i++)
            {
                DrawLine(spriteBatch, new Vector2(i * TILE_SIDE, -MAP_SIDE/2), new Vector2(i * TILE_SIDE, MAP_SIDE/2));

                for (int j = -TILES/2; j <= TILES/2; j++)
                {
                    DrawLine(spriteBatch, new Vector2(-MAP_SIDE/2, j * TILE_SIDE), new Vector2(MAP_SIDE/2, j * TILE_SIDE));
                }
            }
        }

        public void drawTile(Tile tile)
        {
            //Console.WriteLine(tile.TilePos.X + " - " + tile.TilePos.Y);
            for (int i = 0; i < tile.CellArray.GetLength(0); i++)
            {
                for (int j = 0; j < tile.CellArray.GetLength(1); j++)
                {
                    if (tile.CellArray[i,j])
                        spriteBatch.Draw(cellFill, new Vector2(j + (tile.TilePos.X * TILE_SIDE*2), i + (-tile.TilePos.Y * TILE_SIDE * 2)), Color.Red);
                    //Console.Write(tile.CellArray[i,j] ? "1" : "0");
                }
                //Console.WriteLine();
            }
        }

        public void tileHandle()
        {
            storeTiles();
            showTiles();
            clearTiles();
        }
        public void populateTiles()
        {
            neighbourTiles(1,1);
        }

        public int neighbourTiles(int x, int y)
        {
            int count = 0;
            Vector2 neighbour;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int neighbour_x = x + i;
                    int neighbour_y = y + j;
                    neighbour = new Vector2(neighbour_x, neighbour_y);
                    //If we're looking at the middle point
                    if (i == 0 && j == 0)
                    {
                        //Do nothing, we don't want to add ourselves in!
                    }
                    //Otherwise, a normal check of the neighbour
                    else if (!tileMap.Exists(tile => tile.TilePos == neighbour))
                    {
                        
                    }
                }
            }

            return count;
        }

        public void storeTiles()
        {
            bool tileExists = false;
            Vector2 tilePos;
            Vector2 neighbour;
            Tile neighbourTile;
            Tile currentTile;
            Tile[] neighbours;
            int tileIndex = tileMap.FindIndex(tile => tile.TilePos == player.TilePos);

            if (tileIndex >= 0)
                currentTile = tileMap[tileIndex];
            else
                currentTile = new Tile(player.TilePos, TILE_SIDE);

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    tilePos = new Vector2((player.TilePos.X - 1 + i), (player.TilePos.Y + j));
                    foreach (Tile tile in loadedTiles)
                    {
                        if (tile.TilePos != tilePos)
                        {
                            tileExists = false;
                        }
                        else
                        {
                            tileExists = true;
                            break;
                        }
                    }
                    if (!tileExists)
                    {
                        neighbours = new Tile[4];
                        for (int x = -1; x <= 1; x++)
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                neighbour = new Vector2(tilePos.X + x, tilePos.Y + y);
                                tileIndex = tileMap.FindIndex(tile => tile.TilePos == neighbour);
                                if (tileIndex >= 0)
                                {
                                    neighbourTile = tileMap[tileIndex];
                                    if (x == 0 && y == 1)
                                    {
                                        Console.WriteLine("north");
                                        neighbours[0] = neighbourTile;
                                    }
                                    if (x == 1 && y == 0)
                                    {
                                        Console.WriteLine("east");
                                        neighbours[1] = neighbourTile;
                                    }
                                    if (x == 0 && y == -1)
                                    {
                                        Console.WriteLine("south");
                                        neighbours[2] = neighbourTile;
                                    }
                                    if (x == -1 && y == 0)
                                    {
                                        Console.WriteLine("west");
                                        neighbours[3] = neighbourTile;
                                    }
                                }
                                else if (x == 1 && y == 1)
                                {
                                    if(!tileMap.Exists(tile => tile.TilePos == neighbour))
                                        loadedTiles.Add(new Tile(tilePos, TILE_SIDE, neighbours));
                                }
                                else
                                {
                                    if (!tileMap.Exists(tile => tile.TilePos == neighbour))
                                        loadedTiles.Add(new Tile(tilePos, TILE_SIDE));
                                }
                            }
                        }
                    }
                }
            }
        }

        public void showTiles()
        {
            bool tileShown = false;
            Vector2 tilePos;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int k = 0;
                    tilePos = new Vector2((player.TilePos.X - 1 + i), (player.TilePos.Y + j));
                    foreach (Tile tile in loadedTiles)
                    {
                        if(tile.TilePos == tilePos)
                        {
                            if (!tileMap.Contains(tile))
                            {
                                tileShown = false;
                                break;
                            }
                            else
                            {
                                tileShown = true;
                                break;
                            }    
                        }
                        k++;
                    }
                    if (!tileShown)
                    {
                        if(k < loadedTiles.Count)
                            tileMap.Add(loadedTiles[k]);
                    }
                }
            }
        }

        public void clearTiles()
        {
            bool tileExists = false;
            Vector2 tilePos;
            List<Vector2> tileRange = new List<Vector2>();
            List<Tile> newTileMap = new List<Tile>();

            for (int i = -3; i <= 3; i++)
            {
                for (int j = -3; j <= 3; j++)
                {
                    tilePos = new Vector2((player.TilePos.X - 1 + i), (player.TilePos.Y + j));
                    tileRange.Add(tilePos);
                }
            }
            foreach (Tile tile in tileMap)
            {
                foreach(Vector2 storeTile in tileRange)
                {
                    if (tile.TilePos == storeTile)
                    {
                        tileExists = true;
                        newTileMap.Add(tile);
                    }
                    else
                        tileExists = false;
                }
            }
            tileMap = newTileMap;
        }


    }
}
