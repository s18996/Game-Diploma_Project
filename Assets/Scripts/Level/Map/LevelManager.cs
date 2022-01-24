using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    //[SerializeField] List<GameObject> closedDoors;
    //[SerializeField] List<GameObject> openDoors;
    [SerializeField] List<GameObject> roomPrefabs;
    [SerializeField] List<GameObject> bossRoomPrefabs;
    [SerializeField] GameObject upgradeRoomPrefab;
    [SerializeField] GameObject startingRoom;

    public Room currentRoom;
    public int numberOfRooms;
    public int levelGridSize;

    private DefaultLayout defaultLayout;
    private List<GameObject> closedDoors;
    private List<GameObject> openDoors;
    private GameObject player;
    private MapGenerator map;
    private MinimapController minimap;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        defaultLayout = GameObject.Find("DefaultLevelLayout").GetComponent<DefaultLayout>();
        
        closedDoors = defaultLayout.closedDoors;
        openDoors = defaultLayout.openDoors;
    }

    private void Start() {
        minimap = GameManager.minimap.GetComponent<MinimapController>();
        LevelStart();
    }

    public void LevelStart() {
        map = new MapGenerator(levelGridSize, numberOfRooms);
        Vector2Int firstRoomPosition = new Vector2Int(levelGridSize / 2, levelGridSize / 2);
        currentRoom = map.GetRoom(firstRoomPosition);
        currentRoom.roomInstance = startingRoom;
        CreateRoomsInstances();
        minimap.CreateRooms(levelGridSize, map.listOfRooms);
        SetCurrentRoom(firstRoomPosition);
    }

    public void ToStartingRoom() {
        Vector2Int firstRoomPosition = new Vector2Int(levelGridSize / 2, levelGridSize / 2);
        currentRoom = map.GetRoom(firstRoomPosition);
        currentRoom.roomInstance = startingRoom;
        currentRoom.roomInstance.SetActive(true);
        SetActiveDoors();
        SetPlayerGrid();
        OpenAllDoors();
    }

    public void SetCurrentRoom(Vector2Int position) {
        currentRoom.roomInstance.SetActive(false); // disable previous room 
        currentRoom = map.GetRoom(position);
        currentRoom.roomInstance.SetActive(true);
        SetActiveDoors();
        SetPlayerGrid();
        if (AreAllEnemiesDead()) {
            OpenAllDoors();
        }        
        minimap.UpdatePlayerPosition(position);
    }

    private bool AreAllEnemiesDead() {
        return currentRoom.roomInstance.GetComponentInChildren<EnemiesCounter>().allEnemiesDead;
    }

    public void OpenAllDoors() {
        for (int i = 0; i < closedDoors.Count; i++) {
            if (closedDoors[i].GetComponent<SpriteRenderer>().enabled) {
                closedDoors[i].GetComponent<SpriteRenderer>().enabled = false;
                openDoors[i].SetActive(true);
            }
        }
    }

    private void SetPlayerGrid() {
        player.GetComponent<PlayerController>().grid = currentRoom.roomInstance.GetComponentInChildren<GridManager>();
    }

    private void SetActiveDoors() {
        for (int i = 0; i < currentRoom.activeDoors.Length; i++) {
            openDoors[i].SetActive(false);
            if (currentRoom.activeDoors[i])
                closedDoors[i].GetComponent<SpriteRenderer>().enabled = true;
            else
                closedDoors[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void CreateRoomsInstances() {
        foreach (Room room in map.listOfRooms) {
            if (room.roomInstance == null) {
                if (room.roomType == RoomType.Boss) {
                    InstantiateRoom(room, bossRoomPrefabs[0]);
                }
                else if (room.roomType == RoomType.Upgrade) {
                    InstantiateRoom(room, upgradeRoomPrefab);
                }
                else if (room.roomType == RoomType.Default) {
                    InstantiateRoom(room, roomPrefabs[Random.Range(0, roomPrefabs.Count)]);
                }
            }
        }
    }

    private void InstantiateRoom(Room room, GameObject roomPrefab) {
        GameObject roomInstance = Instantiate(roomPrefab, transform);
        roomInstance.SetActive(false);
        room.roomInstance = roomInstance;
    }
}
