using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth;
    int currentHealth;
    public HealthBAr healthBar;
    public UnityEvent OnDeath;
    Player player;
    private void OnEnable()
    {
        OnDeath.AddListener(Death);
    }
    private void OnDisable()
    {
        OnDeath.RemoveListener(Death);
    }
    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.UpdateBar(currentHealth, maxHealth);
        player = GetComponent<Player>();
    }
    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            OnDeath.Invoke();
        }
        healthBar.UpdateBar(currentHealth, maxHealth);
    }
    public void Death()
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        takeDamage(player.DamageEnemy);
        player.DamageEnemy = 0;
    }
}
