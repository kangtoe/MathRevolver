using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleCheck : MonoBehaviour
{
    public bool Visible => visible;
    bool visible;

    private void OnBecameVisible()
    {
        visible = true;
        //Debug.Log("OnBecameVisible");
    }

    private void OnBecameInvisible()
    {
        visible = false;
        //Debug.Log("OnBecameInvisible");        
    }
}
