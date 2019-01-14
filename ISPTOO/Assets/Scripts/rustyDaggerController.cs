using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RustyDaggerController : WeaponController {
    new void Start()
    {
        base.Start();
        atkType = "thrust";
    }
    void Attack()
    {
        if (playerControl != null)
        {
            playerControl.Lunge(50f, playerControl.GetAtkMousePos());

        }

    }

}
