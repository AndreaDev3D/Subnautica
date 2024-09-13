using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AD3D_EnergySolution.BZ.Runtime
{
    public class LubricantStorageController : StorageContainer
    {
        public float LubricantAmount = 0f;
        private float _maxLubricantAmount = 1f;
        public Transform LubricantAmountObj;

        void Start()
        {
            this.container.isAllowedToAdd += new global::IsAllowedToAdd(this.IsAllowedToAdd);
            this.container.isAllowedToRemove += new global::IsAllowedToRemove(this.IsAllowedToRemove);
            this.container.onAddItem += AddLubricant;

            LubricantAmountObj = transform.Find("LubricantAmount");
        }

        private bool IsAllowedToAdd(Pickupable pickupable, bool verbose) => pickupable.GetTechType() == TechType.Lubricant;

        private bool IsAllowedToRemove(Pickupable pickupable, bool verbose)
        {
            if (LubricantAmount >= 0.25f)
            {
                SetLubricantAmount(-0.25f);
                return true;
            }
            return false;
        }


        private void AddLubricant(InventoryItem inventoryItem)
        {
            container.Clear();
            SetLubricantAmount(0.25f);
        }

        public override void OnClose()
        {
            base.OnClose();

        }

        public float SetLubricantAmount(float amount)
        {
            LubricantAmount += amount;

            if (LubricantAmount > _maxLubricantAmount)
                LubricantAmount = _maxLubricantAmount;

            if (LubricantAmount < 0)
            {
                LubricantAmount = 0;
                container.Clear();
            }


            LubricantAmountObj.transform.localScale = new Vector3(1, 1, LubricantAmount);

            return LubricantAmount;
        }
    }
}
