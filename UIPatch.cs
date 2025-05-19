using System;
using System.CodeDom;
using System.Threading.Tasks;
using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Utilities;
using UnityEngine;

namespace GravityIndicator
{
    [HarmonyPatch(typeof(PLUIOutsideWorldUI), "Update")]
    internal class UIAddition
    {
        internal static GameObject gravityIndicator;
        internal static MeshRenderer indicatorMesh;
        internal static bool Created = false;
        internal static bool IndicatorEnabled = true;
        static void Postfix(PLUIOutsideWorldUI __instance, ref bool ___pilotingHUDActive)
        {
            if (!PLLoader.Instance.IsLoaded || PLServer.Instance == null || PLEncounterManager.Instance == null)
            {
                return;
            }
            if (Created && gravityIndicator != null && indicatorMesh != null)
            {
                if (PulsarModLoader.Keybinds.KeybindManager.Instance.GetButtonDown("gravIndicator"))
                {
                    UIAddition.IndicatorEnabled = !UIAddition.IndicatorEnabled;
                }
                indicatorMesh.enabled = IndicatorEnabled && ___pilotingHUDActive;
                //if (IndicatorEnabled && ___pilotingHUDActive)
                //{
                //    //gravityIndicator.transform.position = PLEncounterManager.Instance.PlayerShip.Exterior.transform.position;
                //    //gravityIndicator.transform.position = PLCameraSystem.Instance.CurrentSubSystem.MainCameras[0].transform.position + PLCameraSystem.Instance.CurrentSubSystem.MainCameras[0].transform.forward * 20f;
                //}
            }
            else
            {
                if (Created)
                {
                    return;
                }
                else
                {
                    Assembler.AssembleIndicator();
                    Created = true;
                }
            }
        }
    }
}

