using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuccessWindow : MonoBehaviour
{
    private void OnFade()
    {
        UiManager.instance.fade.OnFade(ChangeScene);
    }

    private void ChangeScene()
    {
        //애니메이션 이벤트 호출 메서드
        SceneManager.LoadScene("Loby");
        UiManager.instance.fade.OnFade();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Player.Instance.transform.position = Vector3.zero;
        Player.Instance.ResetPlayer();

        UiManager.instance.lobyUi.gameObject.SetActive(true);
        UiManager.instance.interactionUi.gameObject.SetActive(true);

        UiManager.instance.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
