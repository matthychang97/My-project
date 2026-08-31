using Unity.VisualScripting;
using UnityEngine;

public class FireProjectile : MonoBehaviour

{
    public GameObject projectilePrefab;
    public Transform spawnTransform;
    public float force = 500;


    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            GameObject newProjectile = Instantiate(projectilePrefab, spawnTransform.position, spawnTransform.rotation);
            newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * force);
        }
    }
}

