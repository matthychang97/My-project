using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        //Set current Health
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    private void DisableTarget()
    {
        gameObject.SetActive(false);
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <=0)
        {
            DisableTarget();
        }
    }
}
