using UnityEngine;
//Made by Matthew Chang
public class Projectiles : MonoBehaviour
{
    public float projectileLife = 3.0f;
    private void Start()
    {
        Destroy(gameObject, projectileLife);
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
