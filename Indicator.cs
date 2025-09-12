using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GravityIndicator
{

    internal class Assembler
    {
        internal static async void AssembleIndicator()
        {
            await Task.Yield();
            //PLShipInfo ship = PLEncounterManager.Instance.PlayerShip;
            //Transform Parent = ship.Exterior.transform;
            GameObject abyssShip = Resources.Load("NetworkPrefabs/AbyssSubmersible") as GameObject;
            await Task.Delay(500);
            GameObject gravityIndicatorOri = abyssShip.transform.Find("InteriorStatic").Find("Ship").Find("AbyssMap").Find("GravityDirection").gameObject;
            //Transform gravityIndicatorOri = PLAbyssShipInfo.Instance.Map.MapCompass as Transform;
            IndicatorManager.gravityIndicator = global::UnityEngine.Object.Instantiate<GameObject>(gravityIndicatorOri);
            IndicatorManager.gravityIndicator.layer = LayerMask.NameToLayer("OutsideWorldUI");
            IndicatorManager.gravityIndicator.tag = "Gravity Indicator";
            global::UnityEngine.Object.DontDestroyOnLoad(IndicatorManager.gravityIndicator);
            IndicatorManager.gravityIndicator.transform.localScale = GUI.Scale;
            IndicatorManager.gravityIndicator.transform.Find("GroundRegresentation").gameObject.GetComponent<MeshRenderer>().enabled = false;
            IndicatorManager.indicatorMesh = IndicatorManager.gravityIndicator.GetComponent<MeshRenderer>();
            IndicatorManager.indicatorMesh.material.shader = Shader.Find("Unlit/Texture");
            IndicatorManager.indicatorMesh.enabled = false;
            IndicatorManager.Created = true;
            //Messaging.Echo(PLNetworkManager.Instance.LocalPlayer, "build done");
        }
    }
}
