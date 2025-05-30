using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ObstacleContainer : MonoBehaviour
{
    [SerializeField] List<Obstacle> obstacles = new List<Obstacle>();

    private void Reset()
    {
        Obstacle[] objs = GetComponentsInChildren<Obstacle>();

        gameObject.tag = TagHelper.Obstacle;
        gameObject.layer = LayerHelper.InitLayer(LayerHelper.Obstacle);

        if (objs.Length == 0)
        {
            GameObject top = new GameObject("Top");
            GameObject bottom = new GameObject("Bottom");
            GameObject left = new GameObject("Left");
            GameObject right = new GameObject("Right");

            top.transform.SetParent(transform);
            bottom.transform.SetParent(transform);
            left.transform.SetParent(transform);
            right.transform.SetParent(transform);

            top.transform.position = transform.position + new Vector3(0, 0, 1);
            bottom.transform.position = transform.position + new Vector3(0, 0, -1);
            left.transform.position = transform.position + new Vector3(-1, 0, 0);
            right.transform.position = transform.position + new Vector3(1, 0, 0);

            obstacles.Add(top.AddComponent<Obstacle>());
            obstacles.Add(bottom.AddComponent<Obstacle>());
            obstacles.Add(left.AddComponent<Obstacle>());
            obstacles.Add(right.AddComponent<Obstacle>());

            return;
        }

        foreach (var obj in objs)
        {
            if (obj != null)
            {
                obstacles.Add(obj);
            }
        }
    }

    public List<Obstacle> GetObstacles()
    {
        return obstacles;
    }


}
