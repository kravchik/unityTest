using System.Collections.Generic;
using UnityEngine;

namespace Yk
{
    public class Sphere : IAabb
    {
        public FloatRect FloatRect = new FloatRect();
        private HashSet<List<IAabb>> cache = new HashSet<List<IAabb>>();

        public float radius;
        public MassDot massDot = new MassDot();
        public GameObject go;


        public FloatRect GetAabb()
        {
            return FloatRect;
        }

        public void UpdateAabb()
        {
            FloatRect.l = massDot.pos.x - radius;
            FloatRect.b = massDot.pos.y - radius;
            FloatRect.r = massDot.pos.x + radius;
            FloatRect.t = massDot.pos.y + radius;
        }

        public HashSet<List<IAabb>> GetCache()
        {
            return cache;
        }

        public Sphere(Vector2 pos, float radius)
        {
            this.massDot.pos = pos;
            this.radius = radius;
            UpdateAabb();
        }
    }
}