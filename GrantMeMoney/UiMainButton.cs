#region

using System;
using ColossalFramework.UI;
using UnityEngine;

#endregion

namespace GrantMeMoney
{
    public class UiMainButton : UIButton
    {
        private const float Dim = 37f;
        private UIDragHandle _drag;

        public override void Start()
        {
            // omitting focused sprites because of some weird behavior I was seeing
            normalBgSprite = "ToolbarIconZoomOutGlobe";
            disabledBgSprite = "ToolbarIconZoomOutGlobeDisabled";
            hoveredBgSprite = "ToolbarIconZoomOutGlobeHovered";
            // focusedBgSprite = "ToolbarIconZoomOutGlobeFocused";
            pressedBgSprite = "ToolbarIconZoomOutGlobePressed";
            normalFgSprite = "ToolbarIconMoney";
            disabledFgSprite = "ToolbarIconMoneyDisabled";
            hoveredFgSprite = "ToolbarIconMoneyHovered";
            // focusedFgSprite = "ToolbarIconMoneyFocused";
            pressedFgSprite = "ToolbarIconMoneyPressed";

            // still doesn't seem to play any audio.  perhaps need to bind some sounds?
            playAudioEvents = true;
            size = new Vector2(Dim, Dim);

            UpdatePosition(GrantMeMoney.Instance.ButtonPos);

            var dragHandler = new GameObject("GMM_UiMainButton_DragHandler");
            dragHandler.transform.parent = transform;
            dragHandler.transform.localPosition = Vector3.zero;

            _drag = dragHandler.AddComponent<UIDragHandle>();
            _drag.size = size;
            _drag.enabled = true;

            var uiView = GetUIView();
            if (uiView != null)
                m_TooltipBox = uiView.defaultTooltipBox;
            tooltip = "Apply for a Grant of Money\n(drag button to move)";

            base.Start();
        }

        protected override void OnPositionChanged()
        {
            Dbg.Log($"Button position now {absolutePosition}");

            base.OnPositionChanged();
        }

        protected override void OnMouseUp(UIMouseEventParameter p)
        {
            Dbg.Log($"Mouse up; button position now {absolutePosition}; saving");

            GrantMeMoney.Instance.ButtonPos = absolutePosition;

            base.OnMouseUp(p);
        }

        private void UpdatePosition(Vector2 pos)
        {
            absolutePosition = pos;
            ClampToScreen();
            Invalidate();
        }

        protected override void OnClick(UIMouseEventParameter p)
        {
            try
            {
                ModUi.Instance.Toggle();
            }
            catch (Exception e)
            {
                Dbg.Err("Could not open UI", e);
            }
        }
    }
}
