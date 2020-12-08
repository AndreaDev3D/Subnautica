using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AD3D_LightSolutionMod.BO.Base
{
    public class SwitchItem
    {
        public string Id { get; set; }
        public bool IsEnable { get; internal set; }
        public int SyncCode { get; internal set; }
        public float R { get; internal set; }
        public float G { get; internal set; }
        public float B { get; internal set; }
        public float CurrentIntensity { get; internal set; }
        public virtual DateTime LastUpdate { get; internal set; }

        public SwitchItem(string id)
        {
            LastUpdate = DateTime.Now;
        }

        public void SetEnable(bool isEnable)
        {
            Update();
            IsEnable = isEnable;
        }

        public void SetSyncCode(int syncCode)
        {
            Update();
            SyncCode = syncCode;
        }

        public void SetColor(float r, float g, float b)
        {
            Update();
            R = r;
            G = g;
            B = b;
        }

        public Color GetColor()
        {
            return new Color(R, G, B, 1.0f);
        }

        public void SetIntensity(float currentIntensity)
        {
            Update();
            CurrentIntensity = currentIntensity;
        }

        private void Update()
        {
            LastUpdate = DateTime.Now;
        }
    }
}
