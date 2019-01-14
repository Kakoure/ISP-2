using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private float horMove;
    private float verMove;
    private float moveAngle;
    private string currentDirState;
    private string state;
    /*
     * StateList:
     * "move"
     * 
     */
    [SerializeField]
    private float speed;
    [SerializeField]
    private Animator anim;
    private Rigidbody2D rg2d;
    private bool attacking;
    [SerializeField]
    private GameObject weapon;
    private WeaponController wController;
    private Animator wAnim;
    private SpriteRenderer wSprRen;
    private Vector2 atkMousePos;
    // Use this for initialization
    void Start() {
        horMove = 0f;
        verMove = 0f;
        rg2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        state = "move";
        attacking = false;
        weapon = null;
        anim.SetBool("moveState", true);
    }

    // Update is called once per frame
    void Update() {

        switch (state)
        {
            case "move":
                HandleMoveInputs();
                HandleAttackInputs();
                break;
            case "attack":
                break;
            default:
                break;


        }

        if (Input.GetKeyDown(KeyCode.F) && state != "attack")
        {
            EquipWeapon();
            Debug.Log("F pressed");
        }
        //Update weaponAnimBools
        if(weapon != null)
        {
            wAnim.SetBool("moveState", anim.GetBool("moveState"));
            wAnim.SetBool("attackState", anim.GetBool("attackState"));
            wAnim.SetBool("right", anim.GetBool("right"));
            wAnim.SetBool("left", anim.GetBool("left"));
            wAnim.SetBool("up", anim.GetBool("up"));
            wAnim.SetBool("down", anim.GetBool("down"));
            wAnim.SetBool("moving", anim.GetBool("moving"));
            if (wAnim.GetBool("right") || wAnim.GetBool("up"))
            {
                wSprRen.sortingOrder = -1;
            } else
            {
                wSprRen.sortingOrder = 1;
            }
        }
    }
    private void FixedUpdate()
    {
        switch (state)
        {
            case "move":
                rg2d.AddForce(Vector2.ClampMagnitude(new Vector2(horMove * speed, verMove * speed), speed), ForceMode2D.Impulse);
                break;
            case "attack":
                break;
            default:
                Debug.Log("State is default");
                break;
        }
    
        
    }
    void EquipWeapon()
    {
        
        
        GameObject selectedWeapon = null;
        foreach (GameObject wep in GameObject.Find("weaponFolder").GetChildren())
        {
            if(selectedWeapon == null && Vector3.Distance(transform.position, wep.transform.position) < 1f)
            {
                selectedWeapon = wep;
            } else if (selectedWeapon != null)
            {
                selectedWeapon = Vector3.Distance(transform.position,wep.transform.position) > Vector3.Distance(transform.position,selectedWeapon.transform.position) ? selectedWeapon : wep;
            }
        }
        if (weapon != null)
        {
            DropWeapon();
        }
        if (selectedWeapon != null)
        {
            
            weapon = selectedWeapon;
            weapon.transform.SetParent(transform);
            weapon.GetComponent<WeaponController>().ParentUpdate();
            wController = weapon.GetComponent<WeaponController>();
            wAnim = weapon.GetComponent<Animator>();
            wSprRen = weapon.GetComponent<SpriteRenderer>();
        } else
        {
            
            Debug.Log("no weapon found!");
        }
    }
    void DropWeapon()
    {
        weapon.transform.SetParent(GameObject.Find("weaponFolder").transform);
        wController.ParentUpdate();
        weapon = null;
        wController = null;
        wAnim = null;
        wSprRen = null;
    }
    public Vector2 GetAtkMousePos()
    {
        return atkMousePos;

    }
    public void Lunge(float strength, Vector2 to)
    {
        Vector2 dirVec = to - (Vector2)transform.position;
        dirVec = dirVec.normalized;
        rg2d.AddForce(dirVec * strength, ForceMode2D.Impulse);
    }
    void HandleMoveInputs()
    {
        horMove = Input.GetAxis("Horizontal");
        verMove = Input.GetAxis("Vertical");
        if (rg2d.velocity.sqrMagnitude > .1f)
        {
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }

        if (horMove != 0 || verMove != 0)
        {
            moveAngle = Mathf.Atan2(verMove, horMove) * Mathf.Rad2Deg;
            anim.SetBool(currentDirState, false);
            if (moveAngle >= 135 || moveAngle <= -135)
            {
                anim.SetBool("left", true);
                currentDirState = "left";
            }
            else if (moveAngle <= 45 && moveAngle >= -45)
            {
                anim.SetBool("right", true);
                currentDirState = "right";
            }
            else if (moveAngle > 45 && moveAngle < 135)
            {
                anim.SetBool("up", true);
                currentDirState = "up";
            }
            else if (moveAngle < -45 && moveAngle > -135)
            {
                anim.SetBool("down", true);
                currentDirState = "down";
            }
        }

    }
    void HandleAttackInputs()
    {
        attacking = Input.GetButton("Fire1") && weapon != null;
        if (attacking)
        {
            atkMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            state = "attack";
            anim.SetBool("attackState", true);
            anim.SetBool("moveState", false);
            anim.SetBool("moving", false);
            anim.SetBool("atk_" + wController.GetAttackType(), true);
            //Get attack dir
            Vector2 dirVec = atkMousePos - (Vector2)transform.position;
            dirVec = dirVec.normalized;
            float atkAngle = Mathf.Atan2(dirVec.y,dirVec.x) * Mathf.Rad2Deg;
            Debug.Log(atkAngle);
            anim.SetBool(currentDirState, false);
            if (atkAngle >= 135 || atkAngle <= -135)
            {
                anim.SetBool("left", true);
                currentDirState = "left";
            }
            else if (atkAngle <= 45 && atkAngle >= -45)
            {
                anim.SetBool("right", true);
                currentDirState = "right";
            }
            else if (atkAngle > 45 && atkAngle < 135)
            {
                anim.SetBool("up", true);
                currentDirState = "up";
            }
            else if (atkAngle < -45 && atkAngle > -135)
            {
                anim.SetBool("down", true);
                currentDirState = "down";
            }
           // Debug.Log(currentDirState);
        }
    }
    public Animator GetAnim()
    {
        return anim;

    }
    public string GetState()
    {
        return state;
    }
    public void SetState(string to)
    {
        state = to;
    }

}
