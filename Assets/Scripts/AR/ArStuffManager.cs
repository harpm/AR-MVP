using System.Collections.ObjectModel;
using UnityEngine;

public class ArStuffManager : MonoBehaviour
{
    private Collection<ArStuff> trackables;

    public int Count
    {
        get
        {
            if (trackables == null)
                trackables = new Collection<ArStuff>();

            return trackables.Count;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        trackables = new Collection<ArStuff>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ArStuff Spawn(ArStuff stuff, Vector3 position)
    {
        var obj = Instantiate(stuff, position, Quaternion.identity);
        trackables.Add(obj);

        return obj;
    }

    public void Remove(ArStuff stuff)
    {
        Destroy(stuff.gameObject);
        trackables.Remove(stuff);
    }

    
    public void Reset()
    {
        foreach (ArStuff stuff in trackables)
        {
            trackables.Remove(stuff);
            Destroy(stuff.gameObject);
        }
    }
    
}
