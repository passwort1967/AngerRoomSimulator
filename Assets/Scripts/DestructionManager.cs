using UnityEngine;

public enum Material
{
    GLASS,
    WOOD,
    STONE,
    STEAL
}

public class DestructionManager : MonoBehaviour
{

    public GameObject intactObject;
    public GameObject brokenOject;

    //Public Fields
    public bool enableWeaponDestruction = true;
    public bool enableCollisionDestruction = true;
    public bool removeFragments = true;

    public Material material = Material.WOOD;
    private float weaponDestructionForceMultiplier = 2;
    public float minFragmentLiveTime = 3;
    public float maxFragmentLiveTime = 8;

    //Private Fields
    private float destructionTime = 1;
    private float collisionDestructionForce = 0;
    private float weaponDestructionForce = 0;

    private ParticleSystem ps;


    private GameObject currentObject;
    private bool isDestroyed = false;

    //Unity Methods
    private void Start()
    {
        currentObject = intactObject;

        //Find Particle Systems
        GameObject particleSystems = GameObject.Find("Particle_Systems").gameObject;

        switch (material)
        {
            case Material.GLASS:
                {
                    collisionDestructionForce = 4.0f;
                    ps = particleSystems.transform.Find("Glass").gameObject.GetComponent<ParticleSystem>();
                    break;
                }
            case Material.WOOD:
                {
                    collisionDestructionForce = 14.0f;
                    ps = particleSystems.transform.Find("Wood").GetComponent<ParticleSystem>();
                    break;
                }
            case Material.STONE:
                {
                    collisionDestructionForce = 11.0f;
                    ps = particleSystems.transform.Find("Stone").GetComponent<ParticleSystem>();
                    break;
                }
            case Material.STEAL:
                {
                    collisionDestructionForce = 19.0f;
                    ps = particleSystems.transform.Find("Steal").GetComponent<ParticleSystem>();
                    break;
                }
        }

        weaponDestructionForce = collisionDestructionForce * weaponDestructionForceMultiplier;

        var main = ps.main;
        main.startColor = gameObject.GetComponentInChildren<MeshRenderer>().material.color;

    }

    //Collision Destruction
    void OnCollisionEnter(Collision collision)
    {
        if (!enableCollisionDestruction) return;

        if (collision.relativeVelocity.magnitude >= collisionDestructionForce)
        {
            Destroy();
        }
    }


    //Weapon Destruction
    void OnTriggerEnter(Collider other)
    {
        if (!enableWeaponDestruction) return;

        Weapon weapon = other.gameObject.GetComponent<Weapon>();
        if (weapon == null) return;

        if (weapon.GetDamage(material) >= weaponDestructionForce)
        {
            Destroy();
        }
    }



    //Helper Methode
    private void Destroy()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        Vector3 position = intactObject.transform.position;
        Quaternion rotation = intactObject.transform.rotation;

        //Swapping Objects
        currentObject = Instantiate(brokenOject, position, rotation);
        Destroy(intactObject);
        
        //Setting Game Tree
        //currentObject.transform.parent = transform;

        //Remove Fragements
        if (!removeFragments) return;

        System.Random rng = new System.Random();

        double maxRemovalTime = 0;
        for (int i = 0; i < currentObject.transform.childCount; i++)
        {
            GameObject child = currentObject.transform.GetChild(i).gameObject;
            double liveTime = (rng.NextDouble() * maxFragmentLiveTime) + minFragmentLiveTime;
            double removalTime = liveTime + destructionTime;

            if (removalTime > maxRemovalTime) maxRemovalTime = removalTime;

            Destroy(child.GetComponent<Collider>(), (float) liveTime);
            Destroy(child.gameObject, (float) removalTime);
        }

        //Cleanup
        Destroy(currentObject, (float)maxRemovalTime);
        Destroy(gameObject, (float)maxRemovalTime);
    }


    private void RenderParticles(Collision collision)
    {
        ps.transform.position = collision.contacts[0].point;
        ps.Play();
    }
}
