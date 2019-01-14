using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private UnityEditor.Animations.AnimatorController iconAC;
    [SerializeField]
    private UnityEditor.Animations.AnimatorController activeAC;
    private Animator anim;
    private Rigidbody2D rg2d;
    private string mode;
    protected PlayerController playerControl;
    protected string atkType; //Tells playerController what animation to use
    /*
     *  Modes:
     *  "dropped" - object exists as a floating icon that player can pick up
     *  "equipped" - object is attached to player and can be used to attack
     */
    // Use this for initialization
    protected void Start()
    {
        anim = GetComponent<Animator>();
        rg2d = GetComponent<Rigidbody2D>();
        ParentUpdate();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ParentUpdate()
    {
        if (gameObject.transform.parent.name == "weaponFolder")
        {
            mode = "dropped";
            anim.runtimeAnimatorController = iconAC;
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "items";
        }
        else
        {
            mode = "equipped";
            anim.runtimeAnimatorController = activeAC;
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
            playerControl = gameObject.transform.parent.gameObject.GetComponent<PlayerController>();
           
        }

    }
    void Attack()
    {


    }
    protected void EndAttack()
    {
        if(playerControl != null)
        {
            Animator parAnim = playerControl.GetAnim();
            parAnim.SetBool("attackState", false);
            parAnim.SetBool("moveState", true);
            playerControl.SetState("move");
            playerControl.GetAnim().SetBool("atk_" + atkType, false);
            
        }
    }
    public string GetAttackType()
    {
        return atkType;
    }
}