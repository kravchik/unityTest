using UnityEngine;

namespace Yk
{
    public class MassDot {
        public float mass = 1;
        public Vector2 pos;
        public Vector2 impulse;
        public Vector2 correction;

        public Vector2 targetSpeed;

        public MassDot()
        {
        }

        public MassDot(Vector2 pos, float mass = 1) {
            this.pos = pos;
            this.mass = mass;
        }

        public void tick(float dt) {
            pos.x += impulse.x * dt / mass + correction.x;
            pos.y += impulse.y * dt / mass + correction.y;
            correction.x = 0;
            correction.y = 0;

            Vector2 targeImpulse = targetSpeed * mass;
            impulse = mix(impulse, targeImpulse, 5 * dt);
        }

        private static Vector2 mix(Vector2 a, Vector2 b, float prop)
        {
            return new Vector2(a.x * (1-prop) + b.x*prop, a.y * (1-prop) + b.y*prop);
        }
    }
}