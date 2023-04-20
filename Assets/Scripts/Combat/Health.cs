using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int health;
    private bool isInvunreable;
    public event Action OnTakeDamage;
    public event Action OnDie;
    public bool IsDead => health == 0;
    // Start is called before the first frame update
    private void Start()
    {
        health = maxHealth;
    }
    public void SetIsInvulnreable(bool isInvunreable)
    {
        this.isInvunreable = isInvunreable;
    }
    public void DealDamage(int damage)
    {
        if (health == 0) { return; }
        if(isInvunreable) { return; }

        //clamping health value
        health = Mathf.Max(health-damage,0);
        OnTakeDamage?.Invoke();

        if(health== 0)
        {
            OnDie?.Invoke();
        }
        Debug.Log(health);
    }
}
