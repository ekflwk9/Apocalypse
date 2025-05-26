using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("State")] public float Health = 100f;

    public float Stamina = 100f;

    private float _health;
    private float _stamina;
    public static Player Instance { get; private set; }

    private void Awake()
    {
        //항상 구현하던 방식으로 구현하였기에 필요없다면 삭제.
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _health = Health;
        _stamina = Stamina;
    }
}