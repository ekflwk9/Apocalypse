using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedUI : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] Canvas MyCanvas;


    private void Reset()
    {
        MyCanvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        Vector3 PlayerPos = Player.Instance.transform.position + new Vector3(0, 1, 0);
        Vector3 Direction = (PlayerPos - gameObject.transform.position).normalized;

        Quaternion LookDirection = Quaternion.LookRotation(Direction);
        transform.rotation = LookDirection;
    }

    public void On() => gameObject.SetActive(true);

    public void Off() => gameObject.SetActive(false);

}
