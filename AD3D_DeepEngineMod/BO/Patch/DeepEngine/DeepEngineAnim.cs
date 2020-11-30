using AD3D_DeepEngineMod.BO.Helper;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace AD3D_DeepEngineMod.BO.Patch.DeepEngine
{
    public class DeepEngineAnim : MonoBehaviour
    {
        public bool IsEnabled;
        public Button btnActivate;
        public GameObject engine;


        private Animation anim;
        // Start is called before the first frame update
        public void Start()
        {
            IsEnabled = false;

            engine = GameObjectFinder.FindByName(this.gameObject, "Engine");

            anim = engine.GetComponent<Animation>();
            anim.AddClip(Import.Bundle.LoadAsset<AnimationClip>("Drilling"), "Drilling");

            btnActivate = GameObjectFinder.FindByName(this.gameObject, "btnActivate").GetComponent<Button>();
            btnActivate.onClick.AddListener(() => StartNStop());

            StartNStop();

            Import.LogEvent("DeepEngineAnim Activate");
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
