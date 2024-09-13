using UnityEngine;

namespace AD3D_EnergySolution.BZ.Runtime
{
    public class DeepEngineAnimations : MonoBehaviour
    {
        public float speed = 0.5f;
        public Transform PistonHead;
        public Transform Piston;

        public float startY = 0f;
        public float targetY = 0.85f;

        public bool IsEnabled = false;

        void Start()
        {
            PistonHead = transform.Find("Engine/PistonHead");
            Piston = transform.Find("Engine/Piston");

            startY = PistonHead.localPosition.y;
            targetY = startY - targetY;
        }

        void Update()
        {
            if(!IsEnabled)
                return;

            float value = Mathf.PingPong(Time.time * speed, 1f);
            float easedValue = EaseOutBack(value);

            PistonHead.localPosition = new Vector3(PistonHead.localPosition.x, Mathf.Lerp(startY, targetY, easedValue), PistonHead.localPosition.z);
            Piston.localScale = new Vector3(Piston.localScale.x, Piston.localScale.y, Mathf.Lerp(100f, 50f, easedValue));
        }

        public static float EaseOutBack(float x)
        {
            return x < 0.5f ? 8f * x * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 4) / 2f;
        }
    }
}
