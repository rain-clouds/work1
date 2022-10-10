using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{[SerializeField]
    private float speed;

    protected Animator MyAnimator;

    protected Vector2 direction;

    private Rigidbody2D myRigidBody;

    protected bool isAttacking = false;

    protected Coroutine attackRoutine;

    [SerializeField]
    protected Transform hitBox;

    [SerializeField]
    protected Stat health;

    public Stat MyHealth
     {
        get 
        {
            return health;
        }
     }

    [SerializeField]
    private float initialHealth ;

    public bool IsMoving
    {
        get { return direction.x != 0 || direction.y != 0; }
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        health.Initialize(initialHealth, initialHealth);

        myRigidBody = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleLayers();
    }
    
    private void FixedUpdate()
    {
        Move();
    }


    public void Move()
    {
        myRigidBody.velocity = direction.normalized * speed;

    }
    
public void HandleLayers()
    {
        if(IsMoving)
        { 

            ActivateLayer("Walk Layer");

            MyAnimator.SetFloat("x", direction.x);
            MyAnimator.SetFloat("y", direction.y);

            StopAttack();
        }

        else if( isAttacking)
        {
            ActivateLayer("Attack Layer");
        }

        else
        { ActivateLayer("Idle Layer"); }
    }


    public void ActivateLayer(string layerName)
    {
        for (int i= 0;i< MyAnimator.layerCount;i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

    public virtual void StopAttack()
    {
        if(attackRoutine!=null)
        {
        StopCoroutine(attackRoutine);
        isAttacking = false;
        MyAnimator.SetBool("attack", isAttacking);
        }
        
    }

    public virtual void TakeDamage(float damage)
    {
        health.MyCurrentValue -= damage;

        if(health.MyCurrentValue <= 0)
        {
            MyAnimator.SetTrigger("die");
        }

    }

}
