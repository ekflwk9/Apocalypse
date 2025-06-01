using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


  public class ShrinkWorldBorder : MonoBehaviour
  { 
    #region Events
    [Header("WorldBorder Events")]
    
    public UnityEvent onShrinkStart;  
    public UnityEvent onShrinked;
    
    #endregion

    #region States
    [Header("WorldBorder States")]

    public List<GameObject> checkObjects = new();

    public float borderDamage = 1;
    public float damagePeriodTimeSecond = 2;
    
    public float shrinkValue = 5;
    public float shrinkSecondDuration = 5;
    public float coolTime = 3;
    public float radius = 100;
    public float minRadius = 10;
    public Vector2 centerAxis = new(0, 0);
    public Vector2 targetCenterAxis = new(0, 0);
    
    [SerializeField] private bool started = false;
    public bool Started => started;
    [SerializeField] private bool paused = true;

    #endregion
    
    #region Bindings
    [Header("WorldBorder Bindings")]

    [SerializeField] private Transform cylinderTransform;
    #endregion
    
    private Coroutine shrinkCoroutine;

    public bool Pause
    {
      get => paused;
      set
      {
        paused = value;
      }
    }

    public bool IsInner(Vector2 position)
    {
      return Vector2.Distance(position, centerAxis) < radius / 2;
    }
    
    public bool IsInner(Vector3 position) => IsInner(new Vector2(position.x, position.z));
    
    public void StartShrink()
    {
      if(started) return;
      
      shrinkCoroutine = StartCoroutine(Shrink());
      checkObjects.Add(Player.Instance.gameObject);
      StartCoroutine(Damage());
      
      paused = false;
      started = true;
    }

    private IEnumerator Shrink()
    {
        yield return new WaitForSeconds(shrinkSecondDuration);

      do
      {
        onShrinkStart?.Invoke();
        var targetRadius = radius - shrinkValue;
        RandomizeCenter();
        var axisMoveSpeed = (targetCenterAxis - centerAxis) / shrinkSecondDuration * Time.fixedDeltaTime;
        
        for (float startRadius = radius, shrinkSpeed = (shrinkValue / shrinkSecondDuration) * Time.fixedDeltaTime;
             radius > startRadius - shrinkValue && radius > minRadius;)
        {
          if (!paused)
          {
            radius -= shrinkSpeed;
            centerAxis += axisMoveSpeed;
            cylinderTransform.localScale = new Vector3(radius, cylinderTransform.localScale.y, radius);
            transform.position = new Vector3(centerAxis.x, transform.position.y, centerAxis.y);
          }
        
          yield return new WaitForFixedUpdate();
        }
        
        radius = targetRadius;
        onShrinked?.Invoke();
        
        yield return new WaitForSeconds(shrinkSecondDuration);
      } while (radius > minRadius);
    }

    private IEnumerator Damage()
    {
        for (;;)
        {
            foreach (var obj in checkObjects)
            {
                if(!IsInner(obj.transform.position))
                {
                    obj.SendMessage("BorderDamage", borderDamage, SendMessageOptions.DontRequireReceiver);
                    Player.Instance.TakeDamage(borderDamage);
                }
            }
            yield return new WaitForSeconds(damagePeriodTimeSecond);
        }
    }

    public virtual void RandomizeCenter()
    {
      var target = Random.insideUnitCircle * shrinkValue;
      targetCenterAxis += target;
    }
    
    #region Unity Events

    private void Start()
    {
      StartShrink();
    }

    private void OnDestroy()
    {
      if (shrinkCoroutine != null)
        StopCoroutine(shrinkCoroutine);
    }
    
    #endregion
  }
