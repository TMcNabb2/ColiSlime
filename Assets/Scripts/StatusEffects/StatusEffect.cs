using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    public string statusName;
    public float statusDuration;

    protected float _currentTime;

    public virtual void OnTick()
    {
            
    }

    public void Awake()
    {
        _currentTime = statusDuration;
        StatusEffectManager.AddEffectToList(this);
    }

    public void Update()
    {
        if (_currentTime >= 0)
        {
            OnTick();   
        }
        else
        {
            Destroy(this.gameObject);
        }
        _currentTime -= Time.deltaTime;
    }

    void OnDestroy()
    {
        StatusEffectManager.RemoveEffectFromList(this);
    }
}
