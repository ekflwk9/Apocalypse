using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadWindow : MonoBehaviour
{
    private void ChangeScene()
    {
        //애니메이션 이벤트 호출 메서드
        UiManager.instance.fade.OnFade();
        SceneManager.LoadScene("Loby");
        SoundManager.BackgroundPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        UiManager.instance.status.ResetInventory();
        ItemManager.Instance.Inventory.RemoveAll();

        Player.Instance.transform.position = Vector3.zero;
        Player.Instance.ResetPlayer();

        UiManager.instance.lobyUi.gameObject.SetActive(true);
        UiManager.instance.interactionUi.gameObject.SetActive(true);

        UiManager.instance.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
