using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperMethods {
    public static List<GameObject> GetChildren(this GameObject go)
    {
        List<GameObject> children = new List<GameObject>();
        foreach(Transform t in go.transform)
        {
            children.Add(t.gameObject);
        }
        return children;
    }

	
}
