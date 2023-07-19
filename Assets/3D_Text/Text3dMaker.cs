using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text3dMaker : MonoBehaviour
{
    #region �̱���
    public static Text3dMaker Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Text3dMaker>();
            }
            return instance;
        }
    }
    private static Text3dMaker instance;
    #endregion

    [SerializeField]
    GameObject textPrefab;

    public GameObject MakeText(string str, Vector3 pos)
    {
        //Quaternion quat = Camera.main.transform.rotation;
        Quaternion quat = textPrefab.transform.rotation;
        GameObject go = Instantiate(textPrefab, pos, quat);
        go.GetComponent<TextMesh>().text = str;

        // �پ������ ���� ȿ�� �߰�
        {
            //Vector3 vec = Random.onUnitSphere * 5;
            //if (vec.y < 0) vec.y *= -1;
            //vec += Vector3.up * 1.5f;
            // �߷� Ȱ��ȭ �Ұ�
            //go.GetComponent<Rigidbody>().AddForce(vec, ForceMode.Impulse);
        }        

        return go;
    }
}
