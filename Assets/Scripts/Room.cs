using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TheWorld
{
    class Room
    {
        public int x=0, y=0;
        public int width =1, height =1 ;
        public List<Room> roomsConnected = new List<Room>();
        public List<Vector2> connectionsOpen = new List<Vector2>();
        public List<Vector2> connectionsClosed = new List<Vector2>();

        public Room()
        {

        }
        public Room(int x, int y, int width, int height, List<Vector2> connections)
        {
            this.x = x;
            this.y = y; 
            this.width = width;
            this.height = height;
        }
        public void closeConnectionAt(int index)
        {
            connectionsClosed.Add(connectionsOpen[index]);
            connectionsOpen.RemoveAt(index);

        }


    }
}
