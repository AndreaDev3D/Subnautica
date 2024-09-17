using AD3D_Common.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AD3D_StorageSolution.Runtime
{
    public class StorageController : StorageContainer
    {
#if SN
        private uGUI_ItemIcon Icon;
#elif BZ
        private Image Icon;
#endif
        private bool _isInited;

        public void Start()
        {
#if SN
            FindIcon();
#elif BZ
            Icon = gameObject.FindComponentByName<Image>("Icon");
#endif
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

        public void FindIcon()
        {
            if (Icon != null)
                return;
#if SN
            var IconGO = gameObject.FindByName("Icon");
            var img = IconGO.GetComponent<Image>();
            Destroy(img);

            Icon = IconGO.AddComponent<uGUI_ItemIcon>();
            if (Icon != null)
            {
                Icon.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
            }
#endif
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
#if SN
                Icon.SetForegroundSprite(SpriteManager.Get(firstItem.techType));
#elif BZ
                Icon.sprite = SpriteManager.Get(firstItem.techType);
#endif
            }
            else
            {
                Plugin.Logger.LogError($"SetIcon something is broken");
            }
        }

        public new void OnHandHover(GUIHand hand)
        {
#if SN
            if (!this.enabled)
                return;
#elif BZ
                if (!this.enabled || this.disableUseability)
                return;
#endif

            Constructable component = this.gameObject.GetComponent<Constructable>();
            if ((bool)(UnityEngine.Object)component && !component.constructed)
                return;


            HandReticle.main.SetText(HandReticle.TextType.Hand, $"{this.hoverText}", true, GameInput.Button.LeftHand);
            HandReticle.main.SetText(HandReticle.TextType.HandSubscript, this.IsEmpty() ? "Empty" : string.Empty, true);
            HandReticle.main.SetIcon(HandReticle.IconType.Hand);
        }
    }
}
