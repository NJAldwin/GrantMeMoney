﻿#region

using System;
using ColossalFramework;
using ICities;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace GrantMeMoney
{
    [UsedImplicitly]
    public class GrantMeMoneyLoader : LoadingExtensionBase
    {
        public override void OnCreated(ILoading loading)
        {
            Dbg.Log($"OnCreated {loading}");
            base.OnCreated(loading);
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            Dbg.Log("OnLevelLoaded");
            try
            {
                if (GrantMeMoney.Instance == null)
                {
                    Dbg.Log("Creating Instance");
                    GrantMeMoney.Instance = new GameObject(GrantMeMoney.ModName).AddComponent<GrantMeMoney>();
                    Object.DontDestroyOnLoad(GrantMeMoney.Instance);
                    GrantMeMoney.Instance.Start();
                    GrantMeMoney.Instance.enabled = true;
                }
                else
                {
                    Dbg.Log("Starting Instance");
                    GrantMeMoney.Instance.Start();
                    GrantMeMoney.Instance.enabled = true;
                }

                if (ModUi.Instance == null)
                    try
                    {
                        Dbg.Log("Creating UI...");
                        ModUi.Instance = ToolsModifierControl.toolController.gameObject.AddComponent<ModUi>();
                    }
                    catch (Exception e)
                    {
                        Dbg.Err("Could not create UI", e);
                    }
            }
            catch (Exception e)
            {
                Dbg.Err("Creating Instance FAILED", e);
            }
        }

        public override void OnLevelUnloading()
        {
            Dbg.Log("Level Unloading");
            if (GrantMeMoney.Instance != null)
            {
                Dbg.Log("Disabling Instance");
                GrantMeMoney.Instance.enabled = false;
            }

            if (ModUi.Instance != null)
            {
                ModUi.Instance.Hide();
                Object.Destroy(ModUi.Instance);
                ModUi.Instance = null;
                Dbg.Log("UI Destroyed");
            }
        }

        public override void OnReleased()
        {
            Dbg.Log("OnReleased");
            base.OnReleased();
        }
    }

    public class GrantMeMoney : MonoBehaviour
    {
        public const string ModName = "GrantMeMoney";

        public static GrantMeMoney Instance;

        private static readonly Vector2 DefaultMainButtonPos = new Vector2(605, 25);

        private readonly SavedInt _mGrantAmount = new SavedInt("grantAmount", ModName, 1_000_000, true);

        private readonly SavedFloat _mMainButtonX = new SavedFloat("mainButtonX", ModName, DefaultMainButtonPos.x, true);
        private readonly SavedFloat _mMainButtonY = new SavedFloat("mainButtonY", ModName, DefaultMainButtonPos.y, true);

        public int GrantAmount
        {
            get => _mGrantAmount.value;
            set => _mGrantAmount.value = Mathf.Clamp(value, -1_000_000_000, 1_000_000_000);
        }
        public Vector2 ButtonPos
        {
            get => new Vector2(_mMainButtonX.value, _mMainButtonY.value);
            set
            {
                _mMainButtonX.value = value.x;
                _mMainButtonY.value = value.y;
            }
        }

        public void Start()
        {
            Dbg.Log("Start");
        }

        public void OnDisable()
        {
            Dbg.Log("Disable");
        }

        internal void GrantMoney(int amt)
        {
            GrantAmount = amt;
            try
            {
                Dbg.Log($"Granting ₡{GrantAmount:N}");
                if (Singleton<EconomyManager>.exists)
                    // RewardAmount seems logical but does not work
                    // RefundAmount and LoanAmount both seem to work
                    Singleton<EconomyManager>.instance.AddResource(EconomyManager.Resource.LoanAmount,
                        GrantAmount * 100,
                        ItemClass.Service.None,
                        ItemClass.SubService.None,
                        ItemClass.Level.None);
                else
                    Dbg.Warn("EconomyManager does not exist -- cannot grant money");

                Dbg.Log($"Finished granting ₡{GrantAmount:N}");
            }
            catch (Exception e)
            {
                Dbg.Err("GrantMoney FAILED", e);
            }
        }
    }
}
