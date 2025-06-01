using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieHearingComponent : MonoBehaviour
{
    [SerializeField] SphereCollider hearingCollider;
    private void Reset()
    {
        hearingCollider = GetComponent<SphereCollider>();
        if (hearingCollider == null)
        {
            hearingCollider = gameObject.AddComponent<SphereCollider>();
            hearingCollider.isTrigger = true;
            hearingCollider.enabled = false;
            hearingCollider.radius = 1f;
            return;
        }
        hearingCollider.enabled = false;
        hearingCollider.isTrigger = true;
        hearingCollider.radius = 1f;
    }

    Vector3 SoundLocation;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagHelper.Monster))
        {
            Entity entity = other.GetComponent<Entity>();
            if(entity == null)
            {
                return;
            }
            if (entity.GetState() == EntityEnum.Idle || entity.GetState() == EntityEnum.Walk)
            {
                entity.Hearing(SoundLocation);
            }
        }
    }

    public void OnSound(float _Radius, Vector3 _SoundedLocation)
    {
        if (SoundCoroutine != null)
        {
            CoroutineManager.Instance.UnSetCoroutine(this, SoundCoroutine);
            SoundLocation = _SoundedLocation;
        }
        SoundCoroutine = CoroutineManager.Instance.SetCoroutine(this, ISoundCoroutine(_Radius));
        SoundLocation = _SoundedLocation;
    }


    Coroutine SoundCoroutine;

    IEnumerator ISoundCoroutine(float Radius)
    {
        hearingCollider.enabled = true;
        hearingCollider.radius = Radius;
        yield return CoroutineHelper.GetTime(0.1f);
        hearingCollider.enabled = false;
        CoroutineManager.Instance.UnSetCoroutine(this, SoundCoroutine);
    }
}