using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float glassDamage = 1;
    public float woodDamage = 1;
    public float stoneDamage = 1;
    public float stealDamage = 1;
    
    public GameObject speedMeasurementPoint;

    private Vector3 lastPosition;
    private Vector3 currPosition;
    private float speed;

    private void Start()
    {
        lastPosition = speedMeasurementPoint.transform.position;
        currPosition = speedMeasurementPoint.transform.position;
        speed = 0;
    }

    private void FixedUpdate()
    {
        lastPosition = currPosition;
        currPosition = speedMeasurementPoint.transform.position;

        float distance = Vector3.Distance(lastPosition, currPosition);
        speed = distance / Time.deltaTime;
    }


    public float GetDamage(Material material)
    {
        float damageMultiplier = 1;

        switch (material)
        {
            case Material.GLASS: 
                {
                    damageMultiplier = glassDamage;
                    break;
                }
            case Material.WOOD:
                {
                    damageMultiplier = woodDamage;
                    break;
                }
            case Material.STONE:
                {
                    damageMultiplier = stoneDamage;
                    break;
                }
            case Material.STEAL:
                {
                    damageMultiplier = stealDamage;
                    break;
                }
        }

        return speed * damageMultiplier;
    }
   
}
