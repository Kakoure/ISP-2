using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class weaponController : MonoBehaviour
{
    [SerializeField]
    private UnityEditor.Animations.AnimatorController iconAC;
    [SerializeField]
    private UnityEditor.Animations.AnimatorController activeAC;
    private Animator anim;
    private Rigidbody2D rg2d;
    private string mode;
    /*
     *  Modes:
     *  "dropped" - object exists as a floating icon that player can pick up
     *  "equipped" - object is attached to player and can be used to attack
     */
    // Use this for initialization
    void Start()
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
        }

    }
}