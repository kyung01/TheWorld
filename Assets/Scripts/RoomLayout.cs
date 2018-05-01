using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TheWorld
{
    class RoomLayout
    {
        int width=1, height=1;
        public List<Vector2> connections = new List<Vector2>();
        public int WIDTH { get { return width; } }
        public int HEIGHT { get { return height; } }

        public RoomLayout()
        {

        }
        public RoomLayout(int width, int height, List<Vector2> connections)
        {
            this.width = width;
            this.height = height;
            this.connections = connections;
        }
        public RoomLayout getPerpendicular()
        {
            List<Vector2> cons = new List<Vector2>();
            foreach (var v in connections) cons.Add(new Vector2(v.y, v.x));
            RoomLayout rl = new RoomLayout(height,width, cons);
            return rl;
        }


    }
}
