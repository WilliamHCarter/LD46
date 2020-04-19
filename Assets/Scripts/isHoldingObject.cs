using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isHoldingObject : MonoBehaviour
{
    // Start is called before the first frame update
    private bool ho = false;

    public bool holdingObject()
    {
        return ho;
    }
    public void holdingObject(bool b)
    {
        ho = b;
    }

}
