using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleCheck : MonoBehaviour
{
    public bool Visible => visible;
    [SerializeField]
    bool visible = false;

    private void OnBecameVisible()
    {
        visible = true;
        //Debug.Log("OnBecameVisible");
        //PlayerFarCheck();
    }

    private void OnBecameInvisible()
    {
        visible = false;
        //Debug.Log("OnBecameInvisible");        
    }

    void PlayerFarCheck()
    {
        float playerZ = Player.Instance.transform.position.z;
        float myZ = transform.position.z;
        float far = myZ - playerZ;

        Debug.Log("playerZ : " + playerZ + "|| myZ : " + myZ + "|| far : " + far);
    }
}
