using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Yk;

public class DebugPhysics : MonoBehaviour
{
    private static Color[] colors = {Color.white, Color.grey, Color.yellow, Color.red, Color.black};

    public YkPhysics physics;
    public Text text;
    public Toggle toggle;

    void Start()
    {
        physics = new YkPhysics();
        var sphere = new Sphere(new Vector2(70, 10), 2);
        sphere.go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        float rr = 1.8f;
        sphere.go.transform.localScale = new Vector3(rr / 0.5f, rr / 0.5f, rr / 0.5f);
        sphere.massDot.impulse = new Vector2(0, 0);
        sphere.massDot.mass = 5;
        sphere.massDot.targetSpeed = new Vector2(-10, 10);
        physics.AddSphere(sphere);

        for (float x = 20; x < 20 + 10 * 2; x += 2)
        {
            for (float y = 20; y < 20 + 10 * 2; y += 2)
            {
                var s = new Sphere(new Vector2(x, y), 1);
                s.go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                float r = 0.8f;
                s.go.transform.localScale = new Vector3(r / 0.5f, r / 0.5f, r / 0.5f);
                s.massDot.targetSpeed = new Vector2(10, 0);
                physics.AddSphere(s);
            }
        }
//        physics.AddSphere(new Sphere(new Vector2(20, 20), 1));
    }

    void FixedUpdate()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        int count = 1;
        if (toggle != null && toggle.isOn) count = 20;
        float multiplier = 1f / count;
        float dt = Time.deltaTime / count;

        for (int i = 0; i < count; i++)physics.tick(dt);
        foreach (var s in physics.spheres) s.go.transform.position = new Vector3(s.massDot.pos.x, s.massDot.pos.y, 0);

        sw.Stop();
        text.text = "" + sw.Elapsed.Milliseconds * multiplier + "ms per physics update\n" +
                    (sw.Elapsed.Milliseconds / 1000f / Time.deltaTime * multiplier + "s elapsed per sec");
    }

    void OnDrawGizmos()
    {
        if (physics == null) return;
        var physicsMap2D = physics.map2D;
        DrawGizmos(physicsMap2D);

        foreach (var s in physics.spheres)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3(s.massDot.pos.x, s.massDot.pos.y, 0), s.radius);
        }
    }

    public static void DrawGizmos(Map2D physicsMap2D)
    {
        float es = physicsMap2D.ElementSize;
        for (int x = 0; x < physicsMap2D.Width; x++)
        {
            for (int y = 0; y < physicsMap2D.Height; y++)
            {
                Gizmos.color = colors[Math.Min(colors.Length - 1, physicsMap2D.Data[x][y].Count)];
                Vector3 pos = new Vector3(x * es + es / 2, y * es + es / 2);
                Gizmos.DrawCube(pos, new Vector3(es * 0.9f, es * 0.9f, es * 0.1f));
            }
        }
    }
}