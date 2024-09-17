using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AD3D_EnergySolution.BZ.Runtime
{
    public class GenericPowerController: MonoBehaviour, IHandTarget
    {
        public bool IsEnabled = true;

        public PowerSource powerSource;
        [AssertNotNull]
        public PowerRelay powerRelay;

        private Constructable _constructable;
        public Constructable Constructable => _constructable ??= gameObject.GetComponent<Constructable>();

        public int MaxPowerAllowed = 750;
        private float maxDepth = 200f;
        public float CurrentEmitRate = 0.25f;
        public float CurrentEmitIntervalSec = 2.0f;
        private float biomeSunlightScale = 1f;

        public AnimationCurve depthCurve;

        public LubricantStorageController lubricantStorageController;

        private float GetDepthScalar() => this.depthCurve.Evaluate(Mathf.Clamp01((this.maxDepth - Ocean.GetDepthOf(this.gameObject)) / this.maxDepth));        

        private float GetSunScalar() => DayNightCycle.main.GetLocalLightScalar() * this.biomeSunlightScale;
        
        private float GetRechargeScalar() => this.GetDepthScalar() * this.GetSunScalar();

        public virtual void Start()
        {
            // Manually build the curve
            depthCurve = new AnimationCurve();
            depthCurve.AddKey(0f, 0f);
            depthCurve.AddKey(new Keyframe(0.5f, 0.5f, 1.646463f, 1.646463f, 0.3333333f, 0.3333333f));
            depthCurve.AddKey(new Keyframe(1f, 1f, -2.087208E-06f, -2.087208E-06f, 0.3333333f, 0.3333333f));

            powerRelay = gameObject.GetComponent<PowerRelay>();
            powerSource = gameObject.GetComponent<PowerSource>();

            powerSource.maxPower = MaxPowerAllowed;

            lubricantStorageController = gameObject.GetAllComponentsInChildren<LubricantStorageController>().FirstOrDefault();

            InvokeRepeating("EmitEnergy", 0, CurrentEmitIntervalSec);
        }

        public virtual void EmitEnergy()
        {
            if (!IsEnabled)
                return;

            try
            {
                if (Constructable.constructed)
                {
                    powerRelay.ModifyPower(CurrentEmitRate, out float num);

                    if (lubricantStorageController != null)
                    {
                        var result = lubricantStorageController.SetLubricantAmount(-0.0001f);
                        if (result == 0f)
                        {
                            StartNStop();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError($"Error Emitting {gameObject.name} {ex.Message}");
            }
        }

        public virtual void OnHandHover(GUIHand hand)
        {
            if (!this.gameObject.GetComponent<Constructable>().constructed)
                return;

            var text = "";
            if (IsEnabled)
            {
                var recharge = Mathf.RoundToInt(this.GetRechargeScalar());
                var power = Mathf.RoundToInt(this.powerSource.GetPower());
                var maxPower = Mathf.RoundToInt(this.powerSource.GetMaxPower());
                text = $"Efficiency: {recharge:P0} \n Charge: {power}/{maxPower} kW";

                if (lubricantStorageController != null)
                    text += $"\nLubricant: {lubricantStorageController.LubricantAmount:P}";

            }else
            {
                text = "Power Off";
            }

            HandReticle.main.SetText(HandReticle.TextType.Hand, text, false);
            HandReticle.main.SetText(HandReticle.TextType.HandSubscript, string.Empty, false);
            HandReticle.main.SetIcon(HandReticle.IconType.Hand);
        }

        public virtual void OnHandClick(GUIHand hand)
        {
            StartNStop();
        }

        public virtual void StartNStop()
        {
            IsEnabled = !IsEnabled;
        }
    }
}
