using System.Collections;
using UnityEngine;

public enum Material
{
    WOOD,
    GLASS,
    STEAL,
    STONE
}

public class Destructable : MonoBehaviour
{
    //Public Fields
    public GameObject destroyedObjectPefab;
    public Material material = Material.WOOD;
    
    public bool removeFragments = true;
    public float minFragmentLiveTime = 3;
    public float maxFragmentLiveTime = 8;

    //Private Fields
    private float destructionTime = 1;
    private float destructionForce = 0;
    private ParticleSystem ps;
    

    //Unity Methods
    private void Start()
    {
        //Find Particle Systems
        GameObject particleSystems = GameObject.Find("Particle_Systems").gameObject;

        switch (material)
        {
            case Material.GLASS:
                {
                    destructionForce = 2.0f;
                    ps = particleSystems.transform.Find("Glass").gameObject.GetComponent<ParticleSystem>();
                    break;
                }
            case Material.WOOD:
                {
                    destructionForce = 3.0f;
                    ps = particleSystems.transform.Find("WOOD").GetComponent<ParticleSystem>();
                    break;
                }
            case Material.STONE:
                {
                    destructionForce = 4.0f;
                    ps = particleSystems.transform.Find("STONE").GetComponent<ParticleSystem>();
                    break;
                }
            case Material.STEAL:
                {
                    destructionForce = 5.0f;
                    ps = particleSystems.transform.Find("STEAL").GetComponent<ParticleSystem>();
                    break;
                }
        }

        var main = ps.main;
        main.startColor = gameObject.GetComponent<MeshRenderer>().material.color;
      

    }

    private void Update()
    {
       
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > destructionForce)
        {
            Destroy(collision);
        }

    }



    //Helper Methode
    private void Destroy(Collision collision)
    {
        GameObject destroyedObject = Instantiate(destroyedObjectPefab, transform.position, transform.rotation);
        Destroy(gameObject);

        //Particle System
        RenderParticles(collision);

        if (!removeFragments) return;

        System.Random rng = new System.Random();

        for (int i = 0; i < destroyedObject.transform.childCount; i++)
        {
            GameObject child = destroyedObject.transform.GetChild(i).gameObject;
            double liveTime = (rng.NextDouble() * maxFragmentLiveTime) + minFragmentLiveTime;
            CoroutineManager.Instance.StartCoroutine(RemoveObject(child, (float)liveTime));
        }
    }

    private IEnumerator RemoveObject(GameObject obj, float liveTime)
    {
        yield return new WaitForSeconds(liveTime);
        Destroy(obj.GetComponent<Collider>());
        yield return new WaitForSeconds(destructionTime);
        Destroy(obj);
    }

    private void RenderParticles(Collision collision)
    {
        ps.transform.position = collision.contacts[0].point;
        ps.Play();
    }

}
