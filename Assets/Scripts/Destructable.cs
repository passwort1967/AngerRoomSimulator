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
    public GameObject destroyedObjectPefab;
    public Material material = Material.WOOD;
    public int destructionForce = 4;
    public bool removeFragments = true;
    public float minFragmentLiveTime = 3;
    public float maxFragmentLiveTime = 8;
    private float destructionTime = 1;



    //Unity Methods
    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > destructionForce)
        {
            destroy();
        }

    }

    //User Methods
    void destroy()
    {
        GameObject destroyedObject = Instantiate(destroyedObjectPefab, transform.position, transform.rotation);
        Destroy(gameObject);

        if (!removeFragments) return;

        System.Random rng = new System.Random();

        for (int i = 0; i < destroyedObject.transform.childCount; i++)
        {
            GameObject child = destroyedObject.transform.GetChild(i).gameObject;
            double liveTime = (rng.NextDouble() * maxFragmentLiveTime) + minFragmentLiveTime;
            CoroutineManager.Instance.StartCoroutine(RemoveObject(child, (float)liveTime));
        }
    }

    IEnumerator RemoveObject(GameObject obj, float liveTime)
    {
        yield return new WaitForSeconds(liveTime);
        Destroy(obj.GetComponent<Collider>());
        yield return new WaitForSeconds(destructionTime);
        Destroy(obj);
    }
    
}
