using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace TheWorld
{
    class WorldGenerator
    {
        List<RoomLayout> roomLayouts = new List<RoomLayout>();
        List<RoomLayout> tunnelLayouts = new List<RoomLayout>();
        List<RoomLayout> rooms = new List<RoomLayout>();

        public WorldGenerator()
        {
            roomLayouts.Add(new RoomLayout(11, 11, new List<Vector2>() { new Vector2(0, 5), new Vector2(10, 5), new Vector2(5, 0), new Vector2(5, 10) }));
            roomLayouts.Add(new RoomLayout(21, 21, new List<Vector2>() { new Vector2(0, 10), new Vector2(20, 10), new Vector2(10, 0), new Vector2(10, 20) }));
            tunnelLayouts.Add(new RoomLayout(7, 3, new List<Vector2>() { new Vector2(0, 1), new Vector2(6, 1) }));
            tunnelLayouts.Add(new RoomLayout(11, 3, new List<Vector2>() { new Vector2(0, 1), new Vector2(10, 1) }));

        }
        public void run(out bool[,] map)
        {
            int mapWidth = 501;
            int mapHeight = 501;
            map = new bool[mapWidth, mapHeight];

            RoomLayout roomBegin = new RoomLayout(5, 5, new List<Vector2>() { new Vector2(0, 2), new Vector2(4, 2), new Vector2(2, 0), new Vector2(2, 4) });
            int roomBeginX = mapWidth/2 - roomBegin.WIDTH / 2;
            int roomBeginY = mapHeight/2 - roomBegin.HEIGHT / 2;

            bool[,] mapTemp = new bool[mapWidth, mapHeight];
            if(checkAddRoom(map, mapWidth, mapHeight, roomBeginX, roomBeginY, roomBegin.WIDTH, roomBegin.HEIGHT))
            {
                markRoom(map, roomBeginX, roomBeginY, roomBegin.WIDTH, roomBegin.HEIGHT);
                Room roomStart = getRoom(roomBegin, roomBeginX, roomBeginY);
                {
                    Room r = roomStart;// = connectRoom(map, mapWidth, mapHeight, roomStart, tunnelLayouts[0]);
                    for (int i = 0; i < 100; i++)
                    {

                        RoomLayout tunnelLayout = tunnelLayouts[(int)UnityEngine.Random.Range(0, tunnelLayouts.Count)];
                        Room roomBefore = r;
                        r = connectRoom(map, mapWidth, mapHeight, r, tunnelLayout);
                        if (r == null)
                        {
                            r = connectRoom(map, mapWidth, mapHeight, roomBefore, tunnelLayout.getPerpendicular());
                            Debug.Log("PERPENDICULAR TRY");
                            if (r == null) break;

                        }
                        RoomLayout roomLayout = roomLayouts[(int)UnityEngine.Random.Range(0, roomLayouts.Count)];
                        Room newRoomConnected = connectRoom(map, mapWidth, mapHeight, r, roomLayout);
                        if (newRoomConnected == null)
                        {
                            r = roomBefore;
                            Debug.Log("Failed to connect TRY");
                        }
                        else
                        {
                            r = newRoomConnected;
                        }
                    }
                }
                
            }

        }
        bool checkAddRoom(bool[,] map, int mapWidth,int mapHeight, int x, int y,int width, int height)
        {
            for(int i = 0; i < width; i++)
                for(int j = 0; j < height; j++)
                {
                    if (checkMap(map, mapWidth, mapHeight, x + i, y + j)) return false;
                }
            return true;
        }
        void markRoom(bool[,] map, int x, int y, int width, int height)
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    map[x + i, y + j] = true;
                }
        }
        Room getRoom(RoomLayout layout, int x, int y)
        {
            Room r = new Room();
            r.width = layout.WIDTH;
            r.height = layout.HEIGHT;
            r.x = x;
            r.y = y;
            foreach (var c in layout.connections)
                r.connectionsOpen.Add(c);
            return r;
        }
        bool checkMap(bool[,] map, int width, int height, int x, int y)
        {
            if(x <0 || y<0 || x>= width|| y >= height)return true;//occupied;
            return map[x, y];
        }
        Room connectRoom(bool[,] map,int mapWidth,int mapHeight, Room currentRoom, RoomLayout layout)
        {
            Debug.Log("new " + currentRoom);
            Vector2[] directions = new Vector2[] { new Vector2(1, 0) , new Vector2(-1, 0) , new Vector2(0, 1) , new Vector2(0, -1) };
            for(int connectionUsedIndex = 0; connectionUsedIndex < currentRoom.connectionsOpen.Count; connectionUsedIndex++)
            {
                Debug.Log("i  " + connectionUsedIndex+"/" + currentRoom.connectionsOpen.Count);
                Vector2 doorPosition = currentRoom.connectionsOpen[connectionUsedIndex];
                List<Vector2> availableSpots = new List<Vector2>();
                for(int d = 0; d < 4; d++)
                {
                    int doorX = (int)(currentRoom.x + doorPosition.x + directions[d].x);
                    int doorY = (int)(currentRoom.y + doorPosition.y + directions[d].y);
                    if (!checkMap(map, mapWidth,mapHeight,doorX, doorY))
                        availableSpots.Add(new Vector2(doorX, doorY));
                }
                foreach (var spots in availableSpots)
                {
                    Debug.Log("SPOT " + spots);
                    for(int layoutConnectionIndex = 0; layoutConnectionIndex < layout.connections.Count; layoutConnectionIndex++)
                    {
                        var connection = layout.connections[layoutConnectionIndex];

                        int potentialX = (int)(spots.x - connection.x);
                        int potentialY = (int)(spots.y - connection.y);
                        if (checkAddRoom(map, mapWidth, mapHeight, potentialX, potentialY, layout.WIDTH, layout.HEIGHT))
                        {
                            markRoom(map, potentialX, potentialY, layout.WIDTH, layout.HEIGHT);
                            Room newRoom = getRoom(layout, potentialX, potentialY);
                            newRoom.closeConnectionAt(layoutConnectionIndex);
                            currentRoom.closeConnectionAt(connectionUsedIndex);
                            Debug.Log("CONNECTED  " + newRoom);
                            return newRoom;
                        }
                    }
                }


            }
            Debug.Log("FAIL");
            return null;
        }
    }
}
