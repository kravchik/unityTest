using System;
using System.Collections.Generic;

namespace Yk
{
    public class YkPhysics
    {
        public Map2D map2D = new Map2D(60, 60, 2);

        public List<Sphere> spheres = new List<Sphere>();

        public void tick(float dt)
        {
            foreach (var a in spheres)
            {
                var candidates = a.GetCache();
//                var candidates = map2D.GetIntersections(a.GetAabb());

                foreach (var c in candidates)
                    foreach (var b in c)
                    {
                        if (a != b) workSphereSphere(a, (Sphere) b);
                    }
            }
            foreach (var a in spheres)
            {
                a.massDot.tick(dt);
                map2D.UpdatePos(a);
            }
        }

        public void AddSphere(Sphere sphere)
        {
            map2D.Put(sphere);
            spheres.Add(sphere);
        }

        public void RemoveSphere(Sphere sphere)
        {
            map2D.Remove(sphere);
            spheres.Remove(sphere);
        }

        public static void workSphereSphere(Sphere a, Sphere b) {
            float baX = a.massDot.pos.x - b.massDot.pos.x;
            float baY = a.massDot.pos.y - b.massDot.pos.y;

            float distance = (float) Math.Sqrt(baX * baX + baY * baY);
            if (distance == 0)return;

            float penetration = a.radius + b.radius - distance;
            if (penetration >= 0) {//when only bouncing needed
                float baDirX = baX / distance;
                float baDirY = baY / distance;

                float aSpeedProj = (a.massDot.impulse.x * baDirX + a.massDot.impulse.y * baDirY) / a.massDot.mass;
                float bSpeedProj = (b.massDot.impulse.x * baDirX + b.massDot.impulse.y * baDirY) / b.massDot.mass;

                float aPart = a.massDot.mass / (b.massDot.mass + a.massDot.mass);
                if (aSpeedProj < bSpeedProj) {
                    float difSpeed = aSpeedProj - bSpeedProj;

                    a.massDot.impulse.x -= difSpeed * baDirX * a.massDot.mass * (1-aPart);
                    a.massDot.impulse.y -= difSpeed * baDirY * a.massDot.mass * (1-aPart);
                    b.massDot.impulse.x += difSpeed * baDirX * b.massDot.mass*aPart;
                    b.massDot.impulse.y += difSpeed * baDirY * b.massDot.mass*aPart;
                }
                a.massDot.correction.x += penetration * baDirX * (1 - aPart) * 0.1f;
                a.massDot.correction.y += penetration * baDirY * (1 - aPart) * 0.1f;
//            b.massDot.correction.x -= penetration * baDirX * (aPart) * 0.1;
//            b.massDot.correction.y -= penetration * baDirY * (aPart) * 0.1;
            }
        }

    }
}