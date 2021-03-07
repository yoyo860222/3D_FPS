using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSController : MonoBehaviour
{
    #region 控制宣告
    [Header("移動速度"), Range(0, 2000)]
    public float speed;
    [Header("旋轉"), Range(0, 2000)]
    public float turn;
    [Header("跳愈高度"), Range(0, 2000)]
    public float jump;
    [Header("地板偵側位移")]
    public Vector3 floorOffset;
    [Header("地板偵測半徑"), Range(0, 2000)]
    public float floorRaduis = 1;
    [Header("攻擊力"), Range(0f, 100f)]
    public float attack = 5f;
    #endregion

    #region 開槍宣告
    [Header("子彈生成位置")]
    public Transform pointFire;
    [Header("子彈")]
    public GameObject bullet;
    [Header("子彈目前數量")]
    public int bulletCurrent = 30;
    [Header("子彈總數")]
    public int bulletTotal = 150;
    [Header("子彈速度")]
    public int bulletSpeed = 1000;
    [Header("彈匣存量顯示")]
    public Text textBulletCurrent;
    [Header("子彈總數顯示")]
    public Text textBulletTotal;
    [Header("換彈時間"), Range(0, 5)]
    public float addBulletsTime = 1;
    [Header("開槍音效")]
    public AudioClip soundFire;
    [Header("換彈匣音效")]
    public AudioClip soundAddBullet;
    [Header("開槍間隔時間"), Range(0f, 1f)]
    public float fireInterval = 0.1f; 

    #endregion

    private bool isAddBullet;
    private float timer;
    private Animator ani;
    private Rigidbody rig;
    private AudioSource aud;
    

    private void Awake()
    {
        Cursor.visible = false;                    //游標 隱藏
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position + floorOffset, floorRaduis);
    }

    private void Update()
    {
        Move();
        Jump();
        Fire();
        AddBullet();
    }

    private void Fire()
    {
        if (Input.GetKey(KeyCode.Mouse0) && bulletCurrent > 0 && !isAddBullet)
        {
            if (timer >= fireInterval)
            {
                timer = 0;
                ani.SetTrigger("開槍觸發");
                aud.PlayOneShot(soundFire, Random.Range(0.8f, 1.2f));
                //扣除子彈
                bulletCurrent--;
                textBulletCurrent.text = bulletCurrent.ToString();

                //暫存子彈 = 生成(物件，座標，角度)
                GameObject temp = Instantiate(bullet, pointFire.position, pointFire.rotation);
                temp.GetComponent<Rigidbody>().AddForce(pointFire.forward * bulletSpeed);
                temp.GetComponent<Bullet>().attack = attack;
            }
            else timer += Time.deltaTime;
        }
    }

    private void AddBullet()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isAddBullet &&bulletTotal > 0 && bulletCurrent < 30) StartCoroutine(AddBulletDelay());
    }

    /// <summary>
    /// 補充子彈協程
    /// </summary>
    /// <returns></returns>
    private IEnumerator AddBulletDelay()
    {
        ani.SetTrigger("換彈觸發");
        aud.PlayOneShot(soundAddBullet, 1);
                
        isAddBullet = true;
        yield return new WaitForSeconds(addBulletsTime);
        isAddBullet = false;

        if (bulletCurrent < 30)
        {
            int add = 30 - bulletCurrent;  //計算捕幾顆子彈

            if (bulletTotal >= add)        //如果子彈總數 大於 要補充的子彈
            {
                bulletCurrent += add;
                bulletTotal -= add;
            }
            else                           //否則 將剩餘的總數 補充過來
            {
                bulletCurrent += bulletTotal;
                bulletTotal = 0;
            }

            textBulletCurrent.text = bulletCurrent.ToString();
            textBulletTotal.text = bulletTotal.ToString();
        }
        
    }

    private void Jump()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position + floorOffset, floorRaduis, 1 << 8);
        print(hit[0].name);

        if (hit.Length > 0 && hit[0] && Input.GetKeyDown(KeyCode.Space))
        {
            rig.AddForce(0, jump, 0);
        }
    }

    private void Move()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        rig.MovePosition(transform.position + transform.forward * v * speed * Time.deltaTime +  transform.right * h * speed * Time.deltaTime);

        float X = Input.GetAxis("Mouse X");                  //偵查 滑鼠 左右移動
        transform.Rotate(0, X * Time.deltaTime * turn, 0);
        
    }


}
