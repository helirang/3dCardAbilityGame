using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileManager : MonoBehaviour
{
    [Tooltip("총알 오브젝트")]
    [SerializeField] GameObject projectilePrefab;
    public IObjectPool<Projectile> projectilePool;
    GameObject projectileManagerObj;

    private void Awake()
    {
        projectileManagerObj = new GameObject(this.gameObject.name+"projectileManager");
        projectilePool = new ObjectPool<Projectile>(CreateProjectile,OnGetProjectile,
            OnReleaseProjectile, OnDestroyProjectile,maxSize:30);
    }

    Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
        projectile.SetPool(projectilePool);
        projectile.transform.parent = projectileManagerObj.transform;
        return projectile;
    }

    private void OnGetProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    private void OnReleaseProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyProjectile(Projectile projectile)
    {
        if(projectile!=null)
            Destroy(projectile.gameObject);
    }

    private void OnDestroy()
    {
        projectilePool.Clear();
    }
}
