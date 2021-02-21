using UnityEngine;

public class FPSController : MonoBehaviour
{
    
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

    private Animator ani;
    private Rigidbody rig;

    private void Awake()
    {
        Cursor.visible = false;                    //游標 隱藏
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
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
