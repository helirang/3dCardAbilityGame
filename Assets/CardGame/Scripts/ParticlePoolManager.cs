using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticlePoolManager : MonoBehaviour
{
    Dictionary<int, ParticleObjPool> poolToID;
    Dictionary<int, GameObject> gameObjToID;

    private void Awake()
    {
        poolToID = new Dictionary<int, ParticleObjPool>();
        gameObjToID = new Dictionary<int, GameObject>();
    }

    public void MakeParticlePool(GameObject obj)
    {
        int id = obj.GetInstanceID();
        gameObjToID.Add(id, obj);

        GameObject emptyObj = new GameObject($"{id} : PariclePool");
        ParticleObjPool particleObjPool = emptyObj.AddComponent<ParticleObjPool>();
        particleObjPool.particleObj = obj;
        poolToID.Add(id, particleObjPool);
    }

    public void InvokeParticle(int particleID,Vector3 position)
    {
        GameObject obj = poolToID[particleID].particlePool.Get();
        obj.transform.position = position;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.GetComponent<ParticleSystem>().Play();
    }

    private void OnDestroy()
    {
        gameObjToID.Clear();
        poolToID.Clear();
    }
}

public class ParticleObjPool : MonoBehaviour
{
    public IObjectPool<GameObject> particlePool;
    public GameObject particleObj;

    private void Awake()
    {
        particlePool = new ObjectPool<GameObject>(CreateParticle, OnGetParticle,
            OnReleaseParticle, OnDestroyParticle, maxSize: 5);
    }

    GameObject CreateParticle()
    {
        GameObject obj = Instantiate<GameObject>(particleObj,this.transform);
        obj.AddComponent<ManagedParticle>().pool = particlePool;
        return obj;
    }

    private void OnGetParticle(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReleaseParticle(GameObject obj)
    {
        obj.GetComponent<ParticleSystem>().Stop();
        obj.SetActive(false);
    }

    private void OnDestroyParticle(GameObject obj)
    {
        if (obj != null)
            Destroy(obj);
    }

    private void OnDestroy()
    {
        particlePool.Clear();
    }
}
