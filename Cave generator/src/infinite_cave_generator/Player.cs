using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace infinite_cave_generator
{
    class Player
    {
        /*
         * -roomPosition
         * -position in room
         * -speed
         * +spawn
         * +kill
         */
        private int _size;
        private float _speed;

        private Vector2 _pos; // top left corner of player
        private Vector2 _center; // center of player
        private Vector2 _tilePos; // Position in tile coordinates
        private Texture2D _texture;
        private Color _color;
        private Color[] _colorArr;
       

        public Player(GraphicsDevice graphicsDevice, int size, float speed, Vector2 position)
        {
            this._size = size;
            this._speed = speed;
            _color = Color.Yellow;
            _colorArr = new Color[size * size];
            _pos = position;

            for (int i = 0; i < _colorArr.Length; i++)
            {
                _colorArr[i] = _color;
            }

            _texture = new Texture2D(graphicsDevice, size, size);
            _texture.SetData(_colorArr);

            _center = new Vector2((int)Math.Floor((double)(_pos.X + size) / 2), (int)Math.Floor((double)(_pos.Y + size) / 2));
            _pos.X = _center.X - size/2;
            _pos.Y = _center.Y - size/2;
        }

        public int Size
        {
            get { return this._size; }
            set { this._size = value; }
        }

        public float Speed
        {
            get { return this._speed; }
            set { this._speed = value; }
        }

        public Vector2 Pos
        {
            get { return this._pos; }
            set {
                this._pos = value;
                this._center.X = _pos.X + _size / 2;
                this._center.Y = _pos.Y + _size / 2;
            }
        }

        public Vector2 CenterPos
        {
            get { return this._center; }
        }

        public Vector2 TilePos
        {
            get { return this._tilePos; }
            set { this._tilePos = value; }
        }

        public Texture2D Texture
        {
            get { return this._texture; }
            set { this._texture = value; }
        }

        public Color Color
        {
            get { return this._color; }
            set { this._color = value; }
        }

        public void move(List<Tile> tiles, int TILE_SIDE)
        {
            /*
             * Let's translate movement to:
             * UP       = Y+
             * DOWN     = Y-
             * LEFT     = X-
             * RIGHT    = X+
             */
            Tile current;
            Vector2 posInTile = new Vector2(this.Pos.X - ((this.TilePos.X-1) * 128), this.Pos.Y - (this.TilePos.Y * 128));
            int tileIndex = tiles.FindIndex(tile => tile.TilePos == this.TilePos);
            int nIndex = tiles.FindIndex(tile => tile.TilePos == new Vector2(this.TilePos.X-1, this.TilePos.Y + 1));
            int eIndex = tiles.FindIndex(tile => tile.TilePos == new Vector2(this.TilePos.X, this.TilePos.Y));
            int sIndex = tiles.FindIndex(tile => tile.TilePos == new Vector2(this.TilePos.X-1, this.TilePos.Y - 1));
            int wIndex = tiles.FindIndex(tile => tile.TilePos == new Vector2(this.TilePos.X, this.TilePos.Y));

            /*if (tileIndex >= 0 && nIndex >= 0 && eIndex >= 0 && sIndex >= 0 && wIndex >= 0)
            {
                current = tiles[tileIndex];

                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    if (current.CellArray[(int)posInTile.X, (int)posInTile.Y])
                        _pos.Y += _speed * 0;
                    else
                        _pos.Y += _speed * (-1);
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    if (current.CellArray[(int)this.Pos.X, (int)this.Pos.Y + 1 >= current.CellArray.GetLength(1) ? current.CellArray.GetLength(1)-1 : current.CellArray.GetLength(1)-1])
                        _pos.Y -= _speed * 0;
                    else
                        _pos.Y -= _speed * (-1);
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    if (current.CellArray[(int)this.Pos.X - 1 < 0 ? 0 : 0, (int)this.Pos.Y])
                        _pos.X -= _speed * 0;
                    else
                        _pos.X -= _speed;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    if (current.CellArray[(int)this.Pos.X + 1 >= current.CellArray.GetLength(0) ? current.CellArray.GetLength(0) - 1 : current.CellArray.GetLength(0) - 1, (int)this.Pos.Y])
                        _pos.X += _speed * 0;
                    else
                        _pos.X += _speed;
            }
            else
            {*/
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    _pos.Y += _speed * (-1);
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    _pos.Y -= _speed * (-1);
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    _pos.X -= _speed;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    _pos.X += _speed;
            //}
            
            

            _center.X = _pos.X + _size / 2;
            _center.Y = -_pos.Y - _size / 2;
        }
    }
}
