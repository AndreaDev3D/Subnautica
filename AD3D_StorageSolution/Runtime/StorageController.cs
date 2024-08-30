using UnityEngine.UI;
using Nautilus.Extensions;
using UnityEngine;
using System.Collections;
using AD3D_Common.Utils;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Linq;

namespace AD3D_StorageSolution.Runtime
{
    public class StorageController : StorageContainer
    {
        private Image Icon;
        private bool _isInited;

        public void Start()
        {
            Icon = gameObject.FindComponentByName<Image>("Icon");
        }

        public void Update()
        {
            if (!_isInited)
            {
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

        public void SetIcon()
        {
            if (Icon != null && (container != null || container.itemsMap != null))
            {
                var firstItem = container.itemsMap[0,0]; 
                if(firstItem == null)
                {
                    return;
                }
                Icon.sprite = SpriteManager.Get(firstItem.techType);
            }
            else
            {
                Plugin.Logger.LogError($"SetIcon something is broken");
            }
        }
        public new void OnHandHover(GUIHand hand)
        {
            if (!this.enabled || this.disableUseability)
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
