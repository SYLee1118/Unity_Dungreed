using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    GameObject pooledObject;

    // 기본 오브젝트 수
    [SerializeField]
    int objectNum = 10;

    // 최대로 추가생성하는 오브젝트 수
    [SerializeField]
    int maxObjectNum = 100;

    [SerializeField]
    Transform parentObject;

    private List<GameObject> pooledObjectList = new List<GameObject>();

    private void Start()
    {
        for(int i = 0; i < objectNum; i++)
        {
            GameObject temp;
            if(parentObject != null)
                temp = Instantiate(pooledObject, parentObject);
            else
                temp = Instantiate(pooledObject);
            temp.SetActive(false);
            pooledObjectList.Add(temp);
        }
    }

    public GameObject GetObject()
    {
        // 리스트에서 비활성화 객체를 활성화시켜서 리턴.
        foreach (GameObject g in pooledObjectList)   
        {
            if(g.activeSelf == false)
            {
                g.SetActive(true);
                return g;
            }
        }

        // 이미 오브젝트 생성 최대치에 도달했다면 null 리턴
        if (objectNum >= maxObjectNum)
            return null;

        // 이미 전부 사용중이면 리스트에 추가 후 리턴.
        GameObject temp = Instantiate(pooledObject);
        pooledObjectList.Add(temp);
        temp.SetActive(true);
        objectNum++;
        return temp;
    }

    // pool에 존재하는 모든 오브젝트 비활성화
    public void DeactivateAll()
    {
        foreach (GameObject g in pooledObjectList)
        {
            if (g.activeSelf == true)
                g.SetActive(false);
        }
    }
}
