using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : Character
{
  

    [SerializeField]
    private Stat mana;


    
   

    private float initialMana=200;



    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex=2;

    private SpellBook spellBook;

    public Transform MyTarget { get; set; }

    [SerializeField]
    private Block[] blocks;

    // Start is called before the first frame update
    protected override void Start()
    {
        spellBook = GetComponent<SpellBook>();
        
        mana.Initialize(initialMana, initialMana);

        //target = GameObject.Find("Target").transform;

        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();

       

        base.Update();
        
    }
    

    private void GetInput()
    {
        direction = Vector2.zero;

        //testing


        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 10;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 10;
        }


        //-----testing


        if (Input.GetKey(KeyCode.W))
        {
            exitIndex = 0;
            direction += Vector2.up;
        }

        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {

            exitIndex = 2;
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {

            exitIndex = 1;
            direction += Vector2.right;
        }

       
    }


    private IEnumerator Attack( int spellIndex)
    {
        Transform currentTarget = MyTarget;
        Spell newSpell = spellBook.CastSpell(spellIndex);

        isAttacking = true;
        MyAnimator.SetBool("attack", isAttacking);
        yield return new WaitForSeconds(newSpell.MyCastTime);


        if (currentTarget  != null && InLineOfSight())
        { 
         SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
            s.Initialize(currentTarget,newSpell.MyDamage);
        }

       
       
        StopAttack();
       
    }

    public void CastSpell( int spellIndex)
    {
        Block();

        if (MyTarget != null && !isAttacking && !IsMoving && InLineOfSight())
        { attackRoutine = StartCoroutine(Attack(spellIndex)); }

    }

    
    

    private bool InLineOfSight()
    {
        if (MyTarget != null)
        {
            Vector3 targetDirec = (MyTarget.transform.position - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirec, Vector2.Distance(transform.position, MyTarget.transform.position), 256);

            if (hit.collider == null)
            {
                return true;
            }
        }

        return false;

    }

    private void Block()
    {
        foreach(Block b in blocks)
        {
            b.Deactivate();
        }

        blocks[exitIndex].Activate();
    }

    public override void StopAttack()
    {
        spellBook.StopCasting();

        base.StopAttack();

    }

}
