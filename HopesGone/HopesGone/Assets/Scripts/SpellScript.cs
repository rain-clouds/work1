using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidBody;

    [SerializeField]
    private float speed;


    public Transform MyTarget { get; private set; }

    private int damage;

    // Start is called before the first frame update
    void Start()
    {
         myRigidBody = GetComponent<Rigidbody2D>();
        

    }

    public void Initialize(Transform target, int damage)
    {
        this.MyTarget = target;
        this.damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     
    private void FixedUpdate()
    {
        if (MyTarget!=null)
        {
            //spell direction
            Vector2 direction = MyTarget.position - transform.position;

            //spell movement
            myRigidBody.velocity = direction.normalized * speed;

            //rotation angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //spell rotates toward the target
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }
     }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "HitBox" && collision.transform == MyTarget)
        {
            speed = 0;
            collision.GetComponentInParent<Enemy>().TakeDamage(damage);
            GetComponent<Animator>().SetTrigger("Impact");
            myRigidBody.velocity = Vector2.zero;
            MyTarget = null;
        }
    }
}
