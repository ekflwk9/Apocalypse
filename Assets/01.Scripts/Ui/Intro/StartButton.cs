using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject info;

    protected void Reset()
    {
        anim = this.TryGetComponent<Animator>();
        info = this.TryFindChild("Info").gameObject;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        info.SetActive(false);
        anim.Play("Start");
    }

    private void EndAction()
    {
        SceneManager.LoadScene("Loby");
    }
}
