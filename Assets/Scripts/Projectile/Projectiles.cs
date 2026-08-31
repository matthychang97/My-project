using UnityEngine;
//Made by Matthew Chang
public class Projectiles : MonoBehaviour
{
    public float projectileLife = 3.0f;
    public int damageAmount = 1;
    private void Start()
    {
        Destroy(gameObject, projectileLife);
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        TargetHealth targetHit = collision.gameObject.GetComponent<TargetHealth>();
        if (targetHit != null)
        {
            targetHit.Damage(damageAmount);
        }
        Destroy(gameObject);
    }
}
