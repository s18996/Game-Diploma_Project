using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MapGenerator {

    public List<Room> listOfRooms;
    private int gridSize;
    private bool[,] rooms;
    private bool[,] singleDoors;
    private int numberOfRooms;
    private Queue queue;
    private float maxProbability = 100f;
    private float neighboursOffsetValue = -14f;
    private List<Vector2Int> singleDoorList;
    private Vector2Int bossRoom;
    private Vector2Int upgradeRoom;

    public MapGenerator(int gridSize, int numberOfRooms) {
        this.numberOfRooms = numberOfRooms;
        this.gridSize = gridSize;
        listOfRooms = new List<Room>();
        rooms = new bool[gridSize, gridSize];
        singleDoors = new bool[gridSize, gridSize];
        singleDoorList = new List<Vector2Int>();
        queue = new Queue();

        if (numberOfRooms > gridSize * gridSize / 2)
            numberOfRooms = gridSize * gridSize / 2;

        //GenerateLayout();
        GenerateTestLayout();
        CreateRooms();
        ShowRoomsConsole();
        //ShowAllRoomsPositions();
    }

    private void GenerateLayout() {
        rooms[gridSize / 2, gridSize / 2] = true;
        Vector2Int starting = new Vector2Int(gridSize / 2, gridSize / 2);
        queue.Enqueue(starting);
        int placedRooms = 1;
        while (placedRooms < numberOfRooms) {
            bool oneWrong = false;
            int wrongs = 0;
            Vector2Int current = (Vector2Int)queue.Dequeue();
            foreach (Vector2Int neighbour in GetDisabledNeighbours(current)) {
                float probability = CalculateProbability(neighbour);
                if (probability > maxProbability)
                    probability = maxProbability;
                if (Random.Range(0f, 100f) < probability) {
                    queue.Enqueue(neighbour);
                    rooms[neighbour.x, neighbour.y] = true;
                    placedRooms++;
                }
                else {
                    oneWrong = true;
                    wrongs++;
                }
            }
            if (wrongs >= 2 && queue.Count > 0)
                continue;
            if (oneWrong)
                queue.Enqueue(GetRandomRoomPos());
        }
    }

    private void GenerateTestLayout() {
        rooms[gridSize / 2, gridSize / 2] = true;
        Vector2Int starting = new Vector2Int(gridSize / 2, gridSize / 2);
        queue.Enqueue(starting);
        int placedRooms = 1;
        while (placedRooms < numberOfRooms) {
            bool oneTrue = false;
            Vector2Int current = (Vector2Int)queue.Dequeue();
            List<Vector2Int> neighbours = GetDisabledNeighbours(current);
            foreach (Vector2Int neighbour in neighbours) {
                float probability = CalculateProbability(neighbour);
                if (probability > maxProbability)
                    probability = maxProbability;
                if (Random.Range(0f, 100f) < probability) {
                    queue.Enqueue(neighbour);
                    singleDoors[current.x, current.y] = false;
                    rooms[neighbour.x, neighbour.y] = true;
                    if (GetEnabledNeighbours(neighbour).Count > 1)
                        singleDoors[neighbour.x, neighbour.y] = false;
                    else
                        singleDoors[neighbour.x, neighbour.y] = true;
                    foreach (var adj in GetEnabledNeighbours(neighbour)) {
                        singleDoors[adj.x, adj.y] = false;
                    }

                    placedRooms++;
                    oneTrue = true;
                }
            }
            if (!oneTrue && queue.Count < 1) {
                queue.Enqueue(GetRandomRoomPos());
            }
        }
        singleDoorList = SetSingleDoors();
        Vector2Int randomRoom = GetFurthestSingleDoorRoom();
        if (randomRoom.x != -1) {
            bossRoom = randomRoom;
            randomRoom = GetEnabledNeighbours(bossRoom)[0];
            upgradeRoom = randomRoom;
        }
    }

    private List<Vector2Int> SetSingleDoors() {
        List<Vector2Int> list = new List<Vector2Int>();
        for (int x = 0; x < gridSize; x++) {
            for (int y = 0; y < gridSize; y++) {
                if (singleDoors[x, y])
                    list.Add(new Vector2Int(x, y));
            }
        }
        return list;
    }

    private Vector2Int GetRandomSingleDoorRoom() {
        if (singleDoorList.Count > 0)
            return singleDoorList[Random.Range(0, singleDoorList.Count)];
        else
            return new Vector2Int(-1, -1);
    }
    private Vector2Int GetFurthestSingleDoorRoom() {
        if (singleDoorList.Count > 0) {
            Vector2Int resultRoom = Vector2Int.zero;
            int maxDist = 0;
            foreach (Vector2Int room in singleDoorList) {
                int dist = GetDistanceFromStart(room);
                if (maxDist < dist){
                    maxDist = dist;
                    resultRoom = room;
                }
            }
            return resultRoom;
        }
        else
            return new Vector2Int(-1, -1);
    }

    private Vector2Int GetRandomRoomPos() {
        List<Vector2Int> list = new List<Vector2Int>();
        for (int y = gridSize - 1; y >= 0; y--) {
            for (int x = 0; x < gridSize; x++) {
                if (rooms[x, y]) {
                    list.Add(new Vector2Int(x, y));
                }
            }
        }
        int random = Random.Range(0, list.Count);
        return list[random];
    }

    private float CalculateProbability(Vector2Int neighbour) {
        float value = Mathf.Pow(GetDisabledNeighbours(neighbour).Count, 4) + neighboursOffsetValue + GetDistanceFromStart(neighbour);
        return value < 2 ? 2 : value;
    }

    private int GetDistanceFromStart(Vector2Int current) {
        if (current.x == gridSize / 2 && current.y == gridSize / 2) {
            return 0;
        }
        else {
            int diffX = Mathf.Abs(gridSize / 2 - current.x);
            int diffY = Mathf.Abs(gridSize / 2 - current.y);
            return diffX + diffY;
        }
    }

    private List<Vector2Int> GetDisabledNeighbours(Vector2Int current) {
        List<Vector2Int> vectors = new List<Vector2Int>();
        if (current.x - 1 >= 0) {
            if (!rooms[current.x - 1, current.y]) {
                vectors.Add(new Vector2Int(current.x - 1, current.y));
            }
        }
        if (current.y + 1 < gridSize) {
            if (!rooms[current.x, current.y + 1]) {
                vectors.Add(new Vector2Int(current.x, current.y + 1));
            }
        }
        if (current.x + 1 < gridSize) {
            if (!rooms[current.x + 1, current.y]) {
                vectors.Add(new Vector2Int(current.x + 1, current.y));
            }
        }
        if (current.y - 1 >= 0) {
            if (!rooms[current.x, current.y - 1]) {
                vectors.Add(new Vector2Int(current.x, current.y - 1));
            }
        }
        return vectors;
    }

    private List<Vector2Int> GetEnabledNeighbours(Vector2Int current) {
        List<Vector2Int> vectors = new List<Vector2Int>();
        if (current.x - 1 >= 0) {
            if (rooms[current.x - 1, current.y]) {
                vectors.Add(new Vector2Int(current.x - 1, current.y));
            }
        }
        if (current.y + 1 < gridSize) {
            if (rooms[current.x, current.y + 1]) {
                vectors.Add(new Vector2Int(current.x, current.y + 1));
            }
        }
        if (current.x + 1 < gridSize) {
            if (rooms[current.x + 1, current.y]) {
                vectors.Add(new Vector2Int(current.x + 1, current.y));
            }
        }
        if (current.y - 1 >= 0) {
            if (rooms[current.x, current.y - 1]) {
                vectors.Add(new Vector2Int(current.x, current.y - 1));
            }
        }
        return vectors;
    }

    public void CreateRooms() {
        for (int y = gridSize - 1; y >= 0; y--) {
            for (int x = 0; x < gridSize; x++) {
                if (rooms[x, y]) {
                    if (bossRoom.x == x && bossRoom.y == y) {
                        AddNewRoom(x, y, RoomType.Boss);
                    }
                    else if (upgradeRoom.x == x && upgradeRoom.y == y) {
                        AddNewRoom(x, y, RoomType.Upgrade);
                    }
                    else {
                        AddNewRoom(x, y, RoomType.Default);
                    }
                }
            }
        }
    }

    private void AddNewRoom(int x, int y, RoomType roomType) {
        Room room = new Room(new Vector2Int(x, y), roomType);
        room.SetActiveDoors(GetNeighbourDoors(room.GetPosition()));
        listOfRooms.Add(room);
    }

    private List<DoorDir> GetNeighbourDoors(Vector2Int current) {
        List<DoorDir> doors = new List<DoorDir>();
        if (current.x - 1 >= 0) {
            if (rooms[current.x - 1, current.y]) {
                doors.Add(DoorDir.LeftDoor);
            }
        }
        if (current.y + 1 < gridSize) {
            if (rooms[current.x, current.y + 1]) {
                doors.Add(DoorDir.UpDoor);
            }
        }
        if (current.x + 1 < gridSize) {
            if (rooms[current.x + 1, current.y]) {
                doors.Add(DoorDir.RightDoor);
            }
        }
        if (current.y - 1 >= 0) {
            if (rooms[current.x, current.y - 1]) {
                doors.Add(DoorDir.DownDoor);
            }
        }
        return doors;
    }

    public Room GetRoom(Vector2Int position) {
        foreach(Room room in listOfRooms) {
            if (room.GetPosition().Equals(position))
                return room;
        }
        return null;
    }

    public void ShowRoomsConsole() {
        StringBuilder str = new StringBuilder();
        str.Append('\n');
        for (int y = gridSize - 1; y >= 0; y--) {
            for (int x = 0; x < gridSize; x++) {
                if (bossRoom.x == x && bossRoom.y == y)
                    str.Append("B.");
                else if (singleDoors[x, y])
                    str.Append("s.");
                else if (rooms[x, y])
                    str.Append("*.");
                else
                    str.Append("-.");
            }
            str.Append('\n');
        }
        Debug.Log(str);
    }

    public void ShowAllRoomsPositions() {
        foreach(Room room in listOfRooms) {
            Debug.Log(room.GetPosition().x + " " + room.GetPosition().y);
        }
    }

}
