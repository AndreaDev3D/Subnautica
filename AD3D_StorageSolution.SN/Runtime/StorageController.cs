﻿using AD3D_Common.Utils;
using AD3D_Common.Extentions;
using UnityEngine.UI;
using UnityEngine;

namespace AD3D_StorageSolution.SN.Runtime
{
    public class StorageController : StorageContainer
    {
        private uGUI_ItemIcon Icon;
        private bool _isInited;

        public void Start()
        {
            FindIcon();
        }

        public void Update()
        {
            if (!_isInited)
            {
                FindIcon();
                if (Icon == null)
                    return;

                if (container != null)
                {
                    _isInited = true;
                    container.Sort();
                    SetIcon();
                }

            }
        }

        public override void OnClose()
        {
            base.OnClose();
            SetIcon();
        }

        public void FindIcon()
        {
            if (Icon != null)
                return;

            var IconGO = gameObject.FindByName("Icon");
            var img = IconGO.GetComponent<Image>();
            Destroy(img);

            Icon = IconGO.AddComponent<uGUI_ItemIcon>();
            if(Icon != null)
            {
                Icon.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
            }
        }

        public void SetIcon()
        {
            if (Icon != null && (container != null || container.itemsMap != null))
            {
                var firstItem = container.itemsMap[0, 0];
                if (firstItem == null)
                {
                    return;
                }
                Icon.SetForegroundSprite(SpriteManager.Get(firstItem.techType));

            }
            else
            {
                Plugin.Logger.LogError($"SetIcon something is broken");
            }
        }
        public new void OnHandHover(GUIHand hand)
        {
            if (!this.enabled)
                return;

            Constructable component = this.gameObject.GetComponent<Constructable>();
            if ((bool)(UnityEngine.Object)component && !component.constructed)
                return;


            HandReticle.main.SetText(HandReticle.TextType.Hand, $"{this.hoverText}", true, GameInput.Button.LeftHand);
            HandReticle.main.SetText(HandReticle.TextType.HandSubscript, this.IsEmpty() ? "Empty" : string.Empty, true);
            HandReticle.main.SetIcon(HandReticle.IconType.Hand);
        }
    }
}
