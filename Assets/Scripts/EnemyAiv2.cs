using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiv2 : MonoBehaviour
{
    public float patrolingValue = 100f;
    public NavMeshAgent agent;
    public GameObject player;
    public LayerMask whatIsGround, whatIsPlayer, whatIsWall;
    public Animator animator;

    public float health;
    public float maxhealth = 100f;
    public int damage = 30;

    public Vector3 walkPoint;
    public Vector3 hidePoint;

    bool walkPointSet;
    bool hidePointSet;

    public float walkPointRange, hidePointRange;

    public float timeBetweenAttack;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public Transform SpawnerPos;
    public GameObject Spawner;

    public GameObject[] Walls;      
    
    bool search=true;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.Find("Player");
        Spawner = GameObject.Find("SpawnerWhite");
        Walls = GameObject.FindGameObjectsWithTag("Wall");
    }

    private void Start()
    {
        SpawnerPos = Spawner.transform;

    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //if (!playerInSightRange && !playerInAttackRange && health > 30) Patroling();
        //if (playerInSightRange && !playerInAttackRange && health > 30) ChasePlayer();
        //if (playerInSightRange && playerInAttackRange && health > 30) AttackPlayer();
        if(playerInSightRange) Hide();

    }


    void Hide()
    {
        RaycastHit raycastHit;

        if (!hidePointSet)
        {
            HideFromPlayer();
        }

        else agent.SetDestination(hidePoint);

        if (Physics.Raycast(transform.position, player.transform.position, out raycastHit, sightRange))
        {
            if (raycastHit.transform.tag == "Player") hidePointSet=false;
        }
    }

    void HideFromPlayer()
    {
        GameObject closeWall = Walls[0];
        float randomZ = Random.Range(-hidePointRange, hidePointRange);
        float randomX = Random.Range(-hidePointRange, hidePointRange);

        for (int i = 0; i < Walls.Length; i++)
        {
            if (Vector3.Distance(closeWall.transform.position, player.transform.position) > Vector3.Distance(Walls[i].transform.position, player.transform.position))
            {
                closeWall = Walls[i];
            }
        }

        hidePoint = new Vector3(closeWall.transform.position.x + randomX, transform.position.y, closeWall.transform.position.z + randomZ);

        RaycastHit hit;
        if (Physics.Raycast(hidePoint, -transform.up, whatIsGround) ) 
        {
            if (Physics.Raycast(hidePoint, player.transform.position, out hit, 500))
            {
                
                if (hit.transform.tag == "Wall")
                {
                    hidePointSet = true; 
                }
            }     

        }
    }

    IEnumerator Repair()
    {
        int i = 0, repairValue=5;
        while (i <= repairValue)
        {
            i++;
            yield return new WaitForSeconds(1);
        }

        if (i >= repairValue)
        {
            Spawner.SetActive(true);
        }
    }
    void RepairSpawner()
    {
        agent.SetDestination(SpawnerPos.position);
        StartCoroutine(Repair());
    }

    void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distantToWalkPoint = transform.position - walkPoint;

        if (distantToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, whatIsGround))
        {
            if(!Physics.CheckSphere(walkPoint,0.3f, whatIsWall))
            {
                walkPointSet = true; 
            }
            
        }
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    void AttackPlayer()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        agent.SetDestination(transform.position);
        transform.LookAt(player.transform.position);

        if (Physics.Raycast(transform.position, fwd, out hit))
        {
            if (hit.transform.tag == "Wall")
            {
                playerInAttackRange = false;
                ChasePlayer();
            }

            else
            {
                if (!alreadyAttacked)
                {
                    alreadyAttacked = true;
                    Invoke(nameof(ResetAttack), timeBetweenAttack);
                }
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DestroyEnemy();
        }
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }

   

    void GoToPlayer()
    {
        agent.SetDestination(player.transform.position);
    }
}
