using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class NaviHelper
{
    public static Vector3 GetRandomNavMeshPosition(Vector3 center, float radius)
    {
        for (int i = 0; i < 30; i++) // 30�� �õ� (�ʿ�� ���� ���)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection.y = 0f; // ���� �̵��� ���
            Vector3 randomPos = center + randomDirection;

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        // ���������� ���� ��ġ ��ȯ
        return center;
    }

    /// <summary>
    /// �� ���������� true��ȯ
    /// �ƴϸ� false��ȯ
    /// </summary>
    /// <param name="_NavMeshAgent"></param>
    /// <returns></returns>
    public static bool IsArrived(NavMeshAgent _NavMeshAgent, float _Distance)
    {
        if (_NavMeshAgent.remainingDistance <= _Distance)
        {
            if (!_NavMeshAgent.hasPath || _NavMeshAgent.velocity.sqrMagnitude <= 0.1f)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsReached(NavMeshAgent _NavMeshAgent, float _Distance)
    {
        if (_NavMeshAgent.remainingDistance <= _Distance)
        {
            return true;
        }
        return false;
    }
}
