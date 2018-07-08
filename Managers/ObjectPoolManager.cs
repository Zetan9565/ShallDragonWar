using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {

    public static ObjectPoolManager Instance;

    public Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();
    public Transform poolRoot;

    private void Awake()
    {
        Instance = this;
        if (!poolRoot) poolRoot = transform;
    }

    public void Put(GameObject go)
    {
        if (!go) return;
        MyTools.SetActive(go, false);
        go.transform.SetParent(poolRoot, false);
        string name = go.name;
        if(pool.ContainsKey(name))
        {
            pool[name].Add(go);
        }
        else
        {
            pool.Add(name, new List<GameObject>() { go });
        }
    }

    public GameObject Get(GameObject prefab, Transform parent, bool worldPositonStays = true)
    {
        if (pool.ContainsKey(prefab.name + "(Clone)") && pool[prefab.name + "(Clone)"].Count > 0)
        {
            GameObject go = pool[prefab.name + "(Clone)"][0];
            pool[prefab.name + "(Clone)"].Remove(pool[prefab.name + "(Clone)"][0]);
            go.transform.SetParent(parent, worldPositonStays);
            MyTools.SetActive(go, true);
            return go;
        }
        else
        {
            GameObject go = Instantiate(prefab, parent) as GameObject;
            return go;
        }
    }

    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (pool.ContainsKey(prefab.name + "(Clone)") && pool[prefab.name + "(Clone)"].Count > 0)
        {
            GameObject go = pool[prefab.name + "(Clone)"][0];
            go.transform.position = position;
            go.transform.rotation = rotation;
            if (parent) go.transform.SetParent(parent, true);
            else go.transform.parent = null;
            pool[prefab.name + "(Clone)"].Remove(go);
            MyTools.SetActive(go, true);
            return go;
        }
        else
        {
            GameObject go = Instantiate(prefab, position, rotation) as GameObject;
            if (parent) go.transform.SetParent(parent, true);
            return go;
        }
    }
}
