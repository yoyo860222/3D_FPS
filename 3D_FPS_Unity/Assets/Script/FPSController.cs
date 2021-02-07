using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Header("移動速度"), Range(0, 2000)]
    public float speed;
    [Header("旋轉"), Range(0, 2000)]
    public float turn;

    private Animator ani;
    private Rigidbody rig;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        rig.MovePosition(transform.position + transform.forward * v * speed * Time.deltaTime +  transform.right * h * speed * Time.deltaTime);

    }


}
