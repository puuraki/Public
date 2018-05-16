using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.IO;

namespace infinite_cave_generator
{
    public class Tile
    {
        /*
         * -position
         * -width
         * -height
         * -cells
         * +load
         * +unload
         * +create
         * +save
         * +doSimulationStep
         */

        private Vector2 _tilePos; // Position in tile coordinates
        private int _size;
        private bool[,] _cellArray;
        private bool[] _sideN, _sideE, _sideS, _sideW;
        private string bRule = "678"; // birth rule
        private string sRule = "345678"; // survival rule
        private string smoothBRule = "5678";
        private string smoothSRule = "5678";
        private int initialSmooth = 20;
        private int finalSmooth = 5;
        private const float SCALE = 2f;

        private const float INITIAL_CHANCE = 0.50f;

        public Tile(Vector2 pos, int size)
        {
            this._tilePos = pos;
            this._size = size / 2;
            if (!load())
            {
                create();
                finalize();
                save();
            }
            sides();
        }

        public Tile(Vector2 pos, int size, Tile[] neighbours)
        {
            this._tilePos = pos;
            this._size = size / 2;
            if (!load())
            {
                create();
                addSides(neighbours);
                finalize();
                save();
            }
            sides();
        }

        public Vector2 TilePos
        {
            get { return this._tilePos; }
            set { this._tilePos = value; }
        }

        public int Size
        {
            get { return this._size; }
            set { this._size = value; }
        }

        public bool[,] CellArray
        {
            get { return this._cellArray; }
        }

        public bool[] SideN
        {
            get { return this._sideN; }
        }

        public bool[] SideE
        {
            get { return this._sideE; }
        }

        public bool[] SideS
        {
            get { return this._sideS; }
        }

        public bool[] SideW
        {
            get { return this._sideW; }
        }

        public void create()
        {
            Random random = new Random();
            _cellArray = new bool[_size, _size];
            int entranceSize = _size / 8;
            int entrancePos = (_size / 2) - (entranceSize / 2);
            int entrances = random.Next(50, 100) / 25;
            int counter = 0;
            bool[] entranceLoc = new bool[4]; // [0]:north [1]:east [2]:south [3]:west

            do
            {
                for (int i = 0; i < entranceLoc.Length; i++)
                {
                    if ((random.Next(0, 100) / 100f) < 0.5f)
                        entranceLoc[i] = true;
                    else
                        entranceLoc[i] = false;
                }
                foreach (bool entrance in entranceLoc)
                {
                    if (entrance)
                        counter++;
                }
            } while (counter < entrances);
            

            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if ((random.Next(0, 100) / 100f) < INITIAL_CHANCE)
                        _cellArray[x, y] = true;
                    else
                        _cellArray[x, y] = false;

                    if ((_tilePos.X >= -1 && _tilePos.X <= 1) && (_tilePos.Y >= -1 && _tilePos.Y <= 1))
                    {
                        if (( ((x >= 0 && x <= entranceSize) || (x >= (_size - entranceSize) && x <= _size)) && (y >= entrancePos && y <= (entrancePos + entranceSize) ))
                           || ((y >= 0 && y <= entranceSize) || (y >= (_size - entranceSize) && y <= _size)) && (x >= entrancePos && x <= (entrancePos + entranceSize) ))
                        {
                            _cellArray[x, y] = false;
                        }
                    }
                    if (entranceLoc[0])
                        if ((x >= entrancePos && x <= (entrancePos + entranceSize)) && (y >= 0 && y <= entranceSize))
                            _cellArray[x, y] = false;

                    if (entranceLoc[1])
                        if ((x >= (_size - entranceSize) && x <= _size) && (y >= entrancePos && y <= (entrancePos + entranceSize)))
                            _cellArray[x, y] = false;

                    if (entranceLoc[2])
                        if ((x >= entrancePos && x <= (entrancePos + entranceSize)) && (y >= (_size - entranceSize) && y <= _size))
                            _cellArray[x, y] = false;

                    if (entranceLoc[3])
                        if ((x >= 0 && x <= entranceSize) && (y >= entrancePos && y <= (entrancePos + entranceSize)))
                            _cellArray[x, y] = false;
                }
            }
        }

        public void setNeighbours(Tile neighbour)
        {

        }

        public void addSides(Tile[] neighbours)
        {
            // side directions are from neighbours perspective
            // (neighbour)north => (this)south
            // (neighbour)east => (this)west
            // (neighbour)south => (this)north
            // (neighbour)west => (this)east

            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if (y == 0 && neighbours[0] != null)
                        _cellArray[x, y] = neighbours[0].SideS[x];

                    if (x == _size - 1 && neighbours[1] != null)
                        _cellArray[x, y] = neighbours[1].SideW[y];

                    if (y == _size - 1 && neighbours[2] != null)
                        _cellArray[x, y] = neighbours[2].SideN[x];

                    if (x == 0 && neighbours[3] != null)
                        _cellArray[x, y] = neighbours[3].SideE[y];
                }
            }
        }

        public void finalize()
        {
            for (int i = 0; i <= initialSmooth; i++)
            {
                _cellArray = doSimulationStep(_cellArray, bRule, sRule);
            }
            scale();
            for (int i = 0; i <= finalSmooth; i++)
            {
               _cellArray = doSimulationStep(_cellArray, smoothBRule, smoothSRule);
            }
            scale();
        }

        public void scale()
        {
            bool[,] newMap = new bool[_cellArray.GetLength(0) * (int)SCALE, _cellArray.GetLength(1) * (int)SCALE];
            for (int i = 0; i < _cellArray.GetLength(0)*SCALE; i++)
            {
                for (int j = 0; j < _cellArray.GetLength(1)*SCALE; j++)
                {
                    if (_cellArray[(int)Math.Floor(i / SCALE), (int)Math.Floor(j / SCALE)])
                        newMap[i, j] = true;
                    else
                        newMap[i, j] = false;
                }
            }
            _cellArray = newMap;
        }

        public bool[,] doSimulationStep(bool[,] oldMap, string birthRule, string survivalRule)
        {
            // rule strings:
            // initial rule: B678/S345678
            // final smoothing: B5678/S5678
            bool[,] newMap = new bool[oldMap.GetLength(0), oldMap.GetLength(1)];
            for (int x = 0; x < oldMap.GetLength(0); x++)
            {
                for (int y = 0; y < oldMap.GetLength(1); y++)
                {
                    int nbs = countAliveNeighbours(oldMap, x, y);
                    if (oldMap[x, y])
                    {
                        // If the cell is alive (true), check if it has the right 
                        // number of neighbours to survive, otherwise it dies.
                        if (survivalRule.Contains(nbs.ToString()))
                            newMap[x,y] = true;
                        else
                            newMap[x,y] = false;
                    }       
                    else
                    {
                        // If the cell is already dead (false), check if it
                        // has the right number of neighbours to be 'born'
                        if (birthRule.Contains(nbs.ToString()))
                            newMap[x, y] = true;
                        else
                            newMap[x, y] = false;
                    }
                }
            }
            return newMap;
        }

        static int countAliveNeighbours(bool[,] map, int x, int y)
        {
            int count = 0;
            Point neighbour;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int neighbour_x = x + i;
                    int neighbour_y = y + j;
                    neighbour = new Point(neighbour_x, neighbour_y);
                    //If we're looking at the middle point
                    if (i == 0 && j == 0)
                    {
                        //Do nothing, we don't want to add ourselves in!
                    }
                    //In case the index we're looking at is off the edge of the map
                    else if (neighbour_x < 0 || neighbour_y < 0 || neighbour_x >= map.GetLength(0) || neighbour_y >= map.GetLength(1))
                    {
                        count = count + 1;
                    }
                    //Otherwise, a normal check of the neighbour
                    else if (map[neighbour_x, neighbour_y])
                    {
                        count = count + 1;
                    }
                }
            }

            return count;
        }

        public void sides()
        {
            _sideN = _sideE = _sideS = _sideW = new bool[_size];

            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if (y == 0)
                        _sideN[x] = _cellArray[x, y];
                    if (x == _size - 1)
                        _sideE[y] = _cellArray[x, y];
                    if (y == _size - 1)
                        _sideS[x] = _cellArray[x, y];
                    if (x == 0)
                        _sideW[y] = _cellArray[x, y];
                }
            }
        }

        public bool load()
        {
            string[] lines;
            if (_cellArray != null)
            {
                return true;
            }
            if (!Directory.Exists("tiles"))
            {
                Directory.CreateDirectory(@"tiles");
            }
            if (File.Exists(@"tiles/" + _tilePos.X + "_" + _tilePos.Y + ".txt"))
            {
                lines = File.ReadAllLines(@"tiles/" + _tilePos.X + "_" + _tilePos.Y + ".txt");
                _cellArray = new bool[lines.Length, lines.Length];
                for ( int i = 0; i < lines.Length; i++ )
                {
                    for ( int j = 0; j < lines[i].Length; j++ )
                    {
                        if (lines[i][j] == '1')
                            _cellArray[i, j] = true;
                        else
                            _cellArray[i, j] = false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
            
        }

        public void save()
        {
            string output = "";
            if(!Directory.Exists("tiles"))
            {
                Directory.CreateDirectory(@"tiles");
            }
            for( int i = 0; i < _cellArray.GetLength(0); i++ )
            {
                using (StreamWriter file =
                    new StreamWriter(@"tiles/" + _tilePos.X + "_" + _tilePos.Y + ".txt", true))
                {
                    for (int j = 0; j < _cellArray.GetLength(1); j++)
                    {
                        if (_cellArray[i, j])
                            output += 1;
                        else
                            output += 0;
                    }
                    file.WriteLine(output);
                    output = "";
                }
            }
        }
    }
}
