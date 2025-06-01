using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    /// <summary>
    /// 해당 위치에서 플레이어가 보이면 false
    /// 안보이면 true
    /// </summary>
    /// <returns></returns>
    /// 
    public bool CheckHide()
    {
        Vector3 playerPos = Player.Instance.transform.position;
        Ray ray = new Ray(transform.position, (playerPos - transform.position).normalized);
        float distance = Vector3.Distance(playerPos, transform.position);


        if (Physics.Raycast(ray, out RaycastHit hit, distance + 10f))
        {
            if(true == hit.collider.CompareTag(TagHelper.Player))
            {
                return false;   //여기는 뻥 뚤린곳이다.
            }

            if (true == hit.collider.CompareTag(TagHelper.Monster))
            {
                return false;   //여기는 뻥 뚤린곳이다.
            }
            else
            {
                return true;   //레이를 몬스터한테 쏴서 몬스터한테 막혀서 아 여기는 보이지 않는곳이구나
            }
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 direction = (playerPos - transform.position).normalized;
        float distance = Vector3.Distance(playerPos, transform.position);

        // 정확한 방향 벡터로 그리기
        DrawRayToTarget(transform.position, direction, distance + 10f, Color.red);
    }

    private void DrawRayToTarget(Vector3 origin, Vector3 direction, float length, Color color)
    {
        Debug.DrawRay(origin, direction * length, color, 1f);
    }
}
