using System.Collections.Generic;
using UnityEngine;
using Yk;

//[ExecuteInEditMode]
public class AabbScript : MonoBehaviour, IAabb
{
    public FloatRect FloatRect = new FloatRect();
    private HashSet<List<IAabb>> cache = new HashSet<List<IAabb>>();

    // Use this for initialization
	void Start ()
	{
	    UpdateAabb();
	}

    public FloatRect GetAabb()
    {
        return FloatRect;
    }

    public void UpdateAabb()
    {
        var tr = transform;
        var sc = GetComponent<SphereCollider>();
        FloatRect.l = tr.position.x - sc.radius;
        FloatRect.b = tr.position.y - sc.radius;
        FloatRect.r = tr.position.x + sc.radius;
        FloatRect.t = tr.position.y + sc.radius;
    }

    public HashSet<List<IAabb>> GetCache()
    {
        return cache;
    }

    // Update is called once per frame
	void Update () {

	}
}
