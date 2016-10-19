using System.Collections.Generic;
using UnityEngine;
using Yk;

[ExecuteInEditMode]
public class DebugMap2D : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float elementSize = 1.5f;
    private Map2D map2D;
    public GameObject go;
    private bool putted = false;

    public DebugMap2D()
    {
        RecretaeMap();
    }

    private void RecretaeMap()
    {
        HashSet<IAabb> all = new HashSet<IAabb>();
        if (map2D != null)
        {
            all = map2D.GetIntersections(new FloatRect(0, 0, map2D.Width * map2D.ElementSize, map2D.Height * map2D.ElementSize));
        }
        map2D = new Map2D(width, height, elementSize);
        foreach (var a in all)
        {
            map2D.Put(a);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (map2D == null || map2D.Width != width || map2D.Height != height || map2D.ElementSize != elementSize)
        {
            RecretaeMap();
        }

        if (go == null) return;
        var ab = go.GetComponent<AabbScript>();
        if (ab == null) return;

        if (!putted)
        {
            putted = true;
            map2D.Put(ab);
        }
        else
        {
            map2D.UpdatePos(ab);
        }
    }

    void OnDrawGizmos()
    {
        if (map2D == null) return;
        DebugPhysics.DrawGizmos(map2D);
    }
}
