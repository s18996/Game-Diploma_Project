using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUsingTree : MonoBehaviour {

    [SerializeField] private Enemy enemy;
    [SerializeField] private string projectileTag;
    [SerializeField] private GameObject[] enemiesToSpawn;
    [SerializeField] private float healValue;
    [SerializeField] private float threshold;
    [SerializeField] private float range;
    [SerializeField] private float shotCooldown;
    [SerializeField] private float healCooldown;
    [SerializeField] private float spawnCooldown;
    [SerializeField] private BossSpriteChanger spriteChanger;
    private Transform player;
    private EnemiesCounter counter;
    private Node topNode;

    private void Awake() {
        counter = gameObject.GetComponentInParent<EnemiesCounter>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start() {
        ConstructBehaviourTree();
    }
    private void Update() {
        topNode.Evaluate();
        if(topNode.nodeState == NodeState.FAILURE) {
            //Debug.Log("topNode fail");
        }
    }

    private void ConstructBehaviourTree() {
        CanSpawnNode canSpawnNode = new CanSpawnNode(counter);
        HealNode healNode = new HealNode(enemy, healValue, healCooldown, spriteChanger);
        HealthNode healthNode = new HealthNode(enemy, threshold);
        RangeNode rangeNode = new RangeNode(range, player, transform);
        ShootAtPlayerNode shootAtPlayerNode = new ShootAtPlayerNode(player, transform, projectileTag, shotCooldown, spriteChanger);
        SpawnEnemyNode spawnEnemyNode = new SpawnEnemyNode(enemiesToSpawn, counter, spawnCooldown, spriteChanger);

        Sequence shootSequence = new Sequence(new List<Node> { new Inverter(rangeNode), shootAtPlayerNode });
        Sequence healSequence = new Sequence(new List<Node> { healthNode, healNode });

        Sequence tryToSpawnEnemySequence = new Sequence(new List<Node> { canSpawnNode, spawnEnemyNode });
        Sequence spawnSequence = new Sequence(new List<Node> { rangeNode, tryToSpawnEnemySequence });

        topNode = new Selector(new List<Node> { spawnSequence, healSequence, shootSequence });

    }

}
