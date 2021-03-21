using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{

    private GameManager gm;
    private Transform player;
    private NavMeshAgent nav;
    private Animator ani;

    private float timer;
    private bool isAddBullet;
    
    [Header("移動速度"), Range(0, 30)]
    public float speed = 2.5f;
    [Header("攻擊範圍"), Range(2, 100)]
    public float rangeAttack = 5f;
    [Header("攻擊力"), Range(0f, 100f)]
    public float attack = 5f;
    [Header("子彈生成位置")]
    public Transform point;
    [Header("子彈")]
    public GameObject bullet;
    [Header("子彈速度"), Range(0, 3000)]
    public float speedBullet = 500;
    [Header("開槍間隔"), Range(0f, 5f)]
    public float interval = 0.5f;
    [Header("轉身速度"), Range(0f, 100f)]
    public float speedFace = 10f;
    [Header("子彈目前數量")]
    public int bulletCount = 30 ;
    [Header("彈匣數量")]
    public int bulletClip = 30;
    [Header("換彈匣時間"), Range(0f, 5f)]
    public float addBulletTime = 2.5f;
    [Header("血量"), Range(10 ,200)]
    public float hp = 100;
    



    private void Awake()
    {
        player = GameObject.Find("swat").transform;
        ani = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        nav.speed = speed;
        nav.stoppingDistance = rangeAttack;

        gm = FindObjectOfType<GameManager>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, rangeAttack);
    }

    private void Update()
    {
        if (isAddBullet) return;
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
            GameObject temp = Instantiate(bullet, point.position, point.rotation);
            temp.GetComponent<Rigidbody>().AddForce(point.right * -speedBullet);
            temp.GetComponent<Bullet>().attack = attack;
            temp.name += name;    //將子彈標注發射者
            ManageBulletCount();
        }
        else
        {
            FaceToPlayer();
            timer += Time.deltaTime;
        }


    }

    private void FaceToPlayer()
    {
        Quaternion faceAngle = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, faceAngle, Time.deltaTime * speedFace);
    }

    private void ManageBulletCount()
    {
        bulletCount--;
        if (bulletCount <= 0)
        {
            StartCoroutine(AddBullet());
        }
    }

    private IEnumerator AddBullet()
    {
        ani.SetTrigger("換彈");
        isAddBullet = true;
        yield return new WaitForSeconds(addBulletTime);
        isAddBullet = false;
        bulletCount += bulletClip;
    }

    private void Damage(float getDamage)
    {
        hp -= getDamage;
        if (hp <= 0) Dead();
    }

    private void Dead()
    {
        ani.SetTrigger("死亡");
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        enabled = false;

        gm.UpdateDataKill(ref gm.killPlayer, gm.textPlayer, "", gm.deadPlayer);

        if (name == "壞人 1") gm.UpdateDataDead(gm.killNpc1, gm.textNpc1, " ", ref gm.deadNpc1);
        else if (name == "壞人 2") gm.UpdateDataDead(gm.killNpc2, gm.textNpc2, " ", ref gm.deadNpc2);
        else if (name == "壞人 3") gm.UpdateDataDead(gm.killNpc3, gm.textNpc3, " ", ref gm.deadNpc3);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "子彈")
        {
            float damage = collision.gameObject.GetComponent<Bullet>().attack;
            if (collision.contacts[0].thisCollider.GetType().Equals(typeof(SphereCollider)))
            {
                print("暴頭");
                Damage(100);
            }
            else Damage(damage);
        }
    }
}
