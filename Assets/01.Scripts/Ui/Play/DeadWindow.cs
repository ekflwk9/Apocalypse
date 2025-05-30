using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadWindow : MonoBehaviour
{
    private void ChangeScene()
    {
        //애니메이션 이벤트 호출 메서드
        SceneManager.LoadScene("Loby");
        UiManager.instance.fade.OnFade();

        UiManager.instance.SetActive(true);
    }
}
