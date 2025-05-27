using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttack : MonoBehaviour
{
    [SerializeField] BoxCollider attackCollider;
    [SerializeField] Entity _entity;

    Coroutine AttackCoroutine;

    private void Reset()
    {
        attackCollider = GetComponent<BoxCollider>();

        if(attackCollider == null)
        {
            attackCollider = gameObject.AddComponent<BoxCollider>();
            attackCollider.isTrigger = true;
            attackCollider.size = new Vector3(1f, 1f, 1f);
            attackCollider.center = Vector3.zero;
        }
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
        AttackCoroutine = StartCoroutine(AttackOn());
    }

    public void StopAttack()
    {
        if (AttackCoroutine != null)
        {
            StopCoroutine(AttackCoroutine);
            AttackCoroutine = null;
        }
    }

    IEnumerator AttackOn()
    {
        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.2f);
        attackCollider.enabled = false;
        yield return null;
    }


}
