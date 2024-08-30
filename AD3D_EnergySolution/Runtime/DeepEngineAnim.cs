using AD3D_Common.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AD3D_EnergySolution.BO.Runtime
{
    public class DeepEngineAnim : MonoBehaviour
    {
        public bool IsEnabled = true;
        public Button btnActivate;

        private Animation anim;
        public AnimationClip DrillingAnimation;

        public void Start()
        {
            IsEnabled = false;

            anim = this.gameObject.FindByName("Engine").GetComponent<Animation>();

            anim.AddClip(DrillingAnimation, "Drilling");

            btnActivate = this.gameObject.FindComponentByName<Button>("btnActivate");
            btnActivate.onClick.AddListener(() => StartNStop());

            StartNStop();
        }

        public void StartNStop()
        {
            IsEnabled = !IsEnabled;
            if (IsEnabled)
            {
                anim.Play("Drilling");
            }
            else
            {
                anim.Stop();
            }
        }
    }
}
