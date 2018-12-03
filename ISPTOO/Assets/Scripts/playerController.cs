using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {
    private float horMove;
    private float verMove;
    private float moveAngle;
    private string currentMoveState;
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
    private Animator wAnim;
    private SpriteRenderer wSprRen;
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
        if (state == "move")//Movement Inputs
        { 
            horMove = Input.GetAxis("Horizontal");
            verMove = Input.GetAxis("Vertical");
            if(rg2d.velocity.sqrMagnitude > .1f)
            {
                anim.SetBool("moving", true);
            } else 
            {
            anim.SetBool("moving", false);
            } 
        
            if (horMove != 0 || verMove != 0)
            {
                moveAngle = Mathf.Atan2(verMove, horMove) * Mathf.Rad2Deg;
                anim.SetBool(currentMoveState, false);
                if (moveAngle >= 135 || moveAngle <= -135)
                {
                    anim.SetBool("moveLeft", true);
                    currentMoveState = "moveLeft";
                }
                else if (moveAngle <= 45 && moveAngle >= -45)
                {
                    anim.SetBool("moveRight", true);
                    currentMoveState = "moveRight";
                }
                else if (moveAngle > 45 && moveAngle < 135)
                {
                    anim.SetBool("moveUp", true);
                    currentMoveState = "moveUp";
                }
                else if (moveAngle < -45 && moveAngle > -135)
                {
                    anim.SetBool("moveDown", true);
                    currentMoveState = "moveDown";
                }
            }
        }
        if (state == "move")//Attack Inputs
        {
            attacking = Input.GetButton("Fire1");
        } else
        {
            attacking = false;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            EquipWeapon();
            Debug.Log("F pressed");
        }
        //Update weaponAnimBools
        if(weapon != null)
        {
            wAnim.SetBool("moveState", anim.GetBool("moveState"));
            wAnim.SetBool("moveRight", anim.GetBool("moveRight"));
            wAnim.SetBool("moveLeft", anim.GetBool("moveLeft"));
            wAnim.SetBool("moveUp", anim.GetBool("moveUp"));
            wAnim.SetBool("moveDown", anim.GetBool("moveDown"));
            wAnim.SetBool("moving", anim.GetBool("moving"));
            if (wAnim.GetBool("moveRight"))
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
            if(selectedWeapon == null && Vector3.Distance(transform.position, wep.transform.position) < 2f)
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
            weapon.GetComponent<weaponController>().ParentUpdate();
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
        weapon.GetComponent<weaponController>().ParentUpdate();
        weapon = null;
        wAnim = null;
        wSprRen = null;
    }
}
