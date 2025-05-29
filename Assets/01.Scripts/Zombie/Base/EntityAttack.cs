using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttack : MonoBehaviour
{
    [SerializeField] BoxCollider attackCollider;
    [SerializeField] Entity _entity;

    private void Reset()
    {
        attackCollider = GetComponentInChildren<BoxCollider>();

        attackCollider.isTrigger = true;
        attackCollider.enabled = false;

        _entity = GetComponentInParent<Entity>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamagable damagable = other.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(_entity.baseStatus.Damage);
            }
        }
    }

    public void Attack()
    {
       CoroutineManager.Instance.SetCoroutine(_entity, AttackOn());
    }

    IEnumerator AttackOn()
    {
        attackCollider.enabled = true;
        yield return CoroutineHelper.GetTime(0.2f);
        attackCollider.enabled = false;
        yield return null;
    }


}
