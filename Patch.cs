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
    [HarmonyPatch(typeof(PLCameraSystem), "RepositionCameras")]
    internal class IndicatorUIPatch
    {
        static void Postfix(PLCameraSystem __instance)
        {
            if (!PLLoader.Instance.IsLoaded || PLServer.Instance == null || PLEncounterManager.Instance == null || (!UIAddition.Created && !UIAddition.IndicatorEnabled))
            {
                return;
            }
            if (UIAddition.gravityIndicator != null && UIAddition.indicatorMesh != null)
            {
                //UIAddition.gravityIndicator.transform.LookAt(PLCameraSystem.Instance.CurrentSubSystem.MainCameras[0].transform);
                //UIAddition.gravityIndicator.transform.eulerAngles = new Vector3((1f * Mathf.Atan(GUI.Offset.y / GUI.Offset.z) * (float)(180 / Math.PI)), PLCameraSystem.Instance.CurrentSubSystem,, (-1f * Mathf.Atan(GUI.Offset.x / GUI.Offset.z) * (float)(180 / Math.PI)));
                //Vector3 lookDir = UIAddition.gravityIndicator.transform.position - PLCameraSystem.Instance.CurrentSubSystem.MainCameraOffset.position * 2;
                //lookDir.y = 0;
                //UIAddition.gravityIndicator.transform.rotation = Quaternion.LookRotation(lookDir);
                
                if (GUI.ElementMode.Value == 0)
                {
                    UIAddition.gravityIndicator.transform.position = __instance.CurrentSubSystem.MainCameraOffset.position +
                    __instance.CurrentSubSystem.MainCameras[0].transform.forward * GUI.Offset.z +
                    __instance.CurrentSubSystem.MainCameras[0].transform.right * GUI.Offset.x +
                    __instance.CurrentSubSystem.MainCameras[0].transform.up * GUI.Offset.y;
                }
                else if (PLEncounterManager.Instance.PlayerShip != null)
                {
                    UIAddition.gravityIndicator.transform.position = PLEncounterManager.Instance.PlayerShip.Exterior.transform.position +
                    PLEncounterManager.Instance.PlayerShip.Exterior.transform.forward * GUI.Offset.z +
                    PLEncounterManager.Instance.PlayerShip.Exterior.transform.right * GUI.Offset.x +
                    PLEncounterManager.Instance.PlayerShip.Exterior.transform.up * GUI.Offset.y;
                }
                
                //Vector3 calcUseVector = __instance.CurrentSubSystem.MainCameraOffset.forward - Vector3.Normalize(__instance.CurrentSubSystem.MainCameras[0].transform.forward * GUI.Offset.z +
                //    __instance.CurrentSubSystem.MainCameras[0].transform.right * GUI.Offset.x +
                //    __instance.CurrentSubSystem.MainCameras[0].transform.up * GUI.Offset.y);
                //Vector3 directionVector = UIAddition.gravityIndicator.transform.position - __instance.CurrentSubSystem.MainCameraOffset.position;
                //Vector3 resultantVector = Vector3.Cross(directionVector, calcUseVector);
                //UIAddition.gravityIndicator.transform.rotation = Quaternion.LookRotation(new Vector3(-resultantVector.x,__instance.CurrentSubSystem.MainCameraOffset.rotation.y, resultantVector.z));
            }
        }
    }
    internal class Assembler
    {
        internal static async void AssembleIndicator()
        {
            while (!PLLoader.Instance.IsLoaded || PLServer.GetCurrentSector() == null)
            {
                await Task.Yield();
            }
            //PLShipInfo ship = PLEncounterManager.Instance.PlayerShip;
            //Transform Parent = ship.Exterior.transform;
            GameObject abyssShip = Resources.Load("NetworkPrefabs/AbyssSubmersible") as GameObject;
            await Task.Delay(500);
            GameObject gravityIndicatorOri = abyssShip.transform.Find("InteriorStatic").Find("Ship").Find("AbyssMap").Find("GravityDirection").gameObject;
            //Transform gravityIndicatorOri = PLAbyssShipInfo.Instance.Map.MapCompass as Transform;
            UIAddition.gravityIndicator = global::UnityEngine.Object.Instantiate<GameObject>(gravityIndicatorOri);
            UIAddition.gravityIndicator.layer = LayerMask.NameToLayer("OutsideWorldUI");
            UIAddition.gravityIndicator.tag = "Gravity Indicator";
            global::UnityEngine.Object.DontDestroyOnLoad(UIAddition.gravityIndicator);
            UIAddition.gravityIndicator.transform.localScale = GUI.Scale;
            UIAddition.gravityIndicator.transform.Find("GroundRegresentation").gameObject.GetComponent<MeshRenderer>().enabled = false;
            UIAddition.indicatorMesh = UIAddition.gravityIndicator.GetComponent<MeshRenderer>();
            UIAddition.indicatorMesh.material.shader = Shader.Find("Unlit/Texture");
            //Messaging.Echo(PLNetworkManager.Instance.LocalPlayer, "build done");
        }
    }
}

