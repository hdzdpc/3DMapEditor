using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLeader : MonoBehaviour
{
    public float viewDistance;
    public GameObject visuableKey;
    public GameObject vcamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void merge()
    {
        /*MeshFilter[] meshFilters = visuableKey.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)                                  //����
        {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;                   //������mesh����ֵ
            combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix; //��������ת���󣬸�ֵ
        }
        Mesh newMesh = new Mesh();                                  //����һ�����������
        newMesh.CombineMeshes(combineInstances);                    //��combineInstances���鴫�뺯��
        visuableKey.gameObject.AddComponent<MeshFilter>().sharedMesh = newMesh; //����ǰ�����壬���������������ϲ�������񣬸�����������
        */
    }

    // Update is called once per frame
    void Update()
    {
        vcamera.transform.localPosition = new Vector3(0, 0, viewDistance) / 5;
        Vector2 camera2DPos = new Vector2(
            vcamera.transform.position.x,
            vcamera.transform.position.z);
        Vector2 leaderPos = new Vector2(transform.position.x, transform.position.z);
        if (Vector2.Distance(camera2DPos, leaderPos) > viewDistance)
        {
            visuableKey.SetActive(false);
        }
        else
        {
            visuableKey.SetActive(true);
        }
    }
}
