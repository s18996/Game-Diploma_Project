using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : MonoBehaviour {

    [SerializeField] private string currentStateName;

    private IBossState currentState;

    public ShootState shootState = new ShootState();
    public RunState runState = new RunState();
    public ChaseState chaseState = new ChaseState();
    public HideState hideState = new HideState();

    public GameObject projectilePrefab;
    public BossSpriteChanger spriteChanger;
    [HideInInspector] public GridManager grid;
    [HideInInspector] public AStarPathfinding aStar;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Enemy enemy;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Transform[] hidingSpots;
    [HideInInspector] public List<HidingSpot> spots;

    public float runSpeed;
    public float chaseSpeed;
    public float hidingSpeed;
    public float healthThreshold;
    public float runningRange;
    public float chasingRange;
    public float aStarPathCooldown;
    public float stateChangeTime;
    public float shotsPerSecond;
    public float shotTime;
    public float stateTimer;
    public float pathTimer;
    public float chaseTimer;

    public bool canChangeState;
    public List<Field> path;

    private void Awake() {
        grid = GameObject.Find("Grid").GetComponent<GridManager>();
        spots = GameObject.Find("Grid").GetComponent<SpotsManager>().spots;
        aStar = new AStarPathfinding();
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start() {
        shotTime = 0;
        stateTimer = stateChangeTime;
        pathTimer = aStarPathCooldown;
    }

    private void OnEnable() {
        currentState = chaseState;
    }

    private void Update() {
        stateTimer -= Time.deltaTime;
        pathTimer -= Time.deltaTime;
        if (stateTimer <= 0)
            canChangeState = true;
        currentState = currentState.UseState(this);
        currentStateName = currentState.ToString();
    }

    public void StateChange() {
        stateTimer = stateChangeTime;
        canChangeState = false;
    }

    public void ShootAtPlayer() {
        GameObject projectile = ObjectPooling.Instance.SpawnFromPool("Projectile",
            transform.position,
            Quaternion.Euler(new Vector3(0, 0, AngleBetweenTwoPoints(transform.position, player.transform.position) - 90f)));
        projectile.GetComponent<Projectile>().StartMove(transform, player.transform);
        spriteChanger.ShootSprite();
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public void MoveToClosestHidingSpot(Vector2 pos) {
        if (grid.GetField(transform.position) == grid.GetField(pos)) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(pos.x, -pos.y), hidingSpeed * Time.deltaTime);
        }
        if (path != null && path.Count > 1) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[1].x, path[1].y, 0), hidingSpeed * Time.deltaTime);
        }
        spriteChanger.DefaultSprite();
    }
    public bool CanSeePlayer() {
        RaycastHit2D[] hit2D = Physics2D.RaycastAll(transform.position, player.transform.position - transform.position);
        for (int i = 0; i < hit2D.Length; i++) {
            Transform hitTransform = hit2D[i].transform;
            if (hitTransform.CompareTag("Player")) {
                break;
            }
            else if (hitTransform.CompareTag("Projectile") || hitTransform.CompareTag("Enemy")) {
                continue;
            }
            else {
                return false;
            }
        }
        return true;
    }

    public void NewPathToHidingSpot(Vector2 pos) {
        path = aStar.FindPath(grid.GetField(transform.position), grid.GetField(pos));
    }

}
