using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent nav;
    private Animator ani;

    private float timer;

    [Header("移動速度"), Range(0, 30)]
    public float speed = 2.5f;
    [Header("攻擊範圍"), Range(2, 100)]
    public float rangeAttack = 5f;
    [Header("子彈生成位置")]
    public Transform point;
    [Header("子彈")]
    public GameObject bullet;
    [Header("子彈速度"), Range(0, 3000)]
    public float speedBullet = 500;
    [Header("開槍間隔"), Range(0f, 5f)]
    public float interval = 0.5f;

    private void Awake()
    {
        player = GameObject.Find("swat").transform;
        ani = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        nav.speed = speed;
        nav.stoppingDistance = rangeAttack;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, rangeAttack);
    }

    private void Update()
    {
        Track();
    }

    private void Track()
    {
        nav.SetDestination(player.position);

        if (nav.remainingDistance > rangeAttack)
        {
            ani.SetBool("跑步", true);
        }
        else
        {
            Fire();
            ani.SetBool("跑步", false);
        }
        
        
    }

    private void Fire()
    {
        if (timer >= interval)
        {
            timer = 0;
            Instantiate(bullet, point.position, point.rotation);
        }
        else
        {
            timer += Time.deltaTime;
        }


    }



}
