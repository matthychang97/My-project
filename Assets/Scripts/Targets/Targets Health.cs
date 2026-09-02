using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int points = 1;
    private int currentHealth;

    private GameManager gameManager;

    public GameManager GameManager {  get { return gameManager; } set { gameManager = value; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        //Set current Health
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    private void DisableTarget()
    {
        Debug.Log("Pew");
        if(gameManager != null)
        {
            gameManager.AddScore(points);
        }
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
