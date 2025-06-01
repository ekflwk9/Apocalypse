using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] SphereCollider sphereCollider;

    [SerializeField] string PrefabString;

    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        if (sphereCollider == null)
        {
            sphereCollider = gameObject.AddComponent<SphereCollider>();
        }
        sphereCollider.isTrigger = true;
        rb.useGravity = false;

    }
    // Start is called before the first frame update
    void Start()
    {
        Vector3 PlayerPos = Player.Instance.transform.position + new Vector3(0, 1, 0);
        Vector3 Direction = (PlayerPos - gameObject.transform.position).normalized;

        Quaternion LookDirection = Quaternion.LookRotation(Direction);
        transform.rotation = LookDirection;

        rb.velocity = Direction * 10f;
        CoroutineManager.Instance.SetCoroutine(this, DestroyCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagHelper.Player))
        {
            Player.Instance.TakeDamage(10);
            CoroutineManager.Instance.UnSetAllCoroutine(this);

            GameObject obj = ContentManager.GetAsset<GameObject>(PrefabString);

            ObjectPool.Instance.Set(obj, gameObject);
            //ObjectPool.Instance.Set(ObjectPool.PoolType.Projectile, gameObject);
        }
    }


    IEnumerator DestroyCoroutine()
    {
        yield return CoroutineHelper.GetTime(5f);
        Destroy(gameObject);
        //ObjectPool.Instance.Set(ObjectPool.PoolType.Projectile, gameObject);
    }


}
