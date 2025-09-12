using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Animations;

namespace GravityIndicator
{
    internal class IndicatorManager : UnityEngine.MonoBehaviour
    {
        internal static GameObject gravityIndicator;
        internal static MeshRenderer indicatorMesh;
        internal static bool Created = false;
        internal static bool IndicatorEnabled = true;
        void Awake()
        {
            Assembler.AssembleIndicator();
        }
        void Update()
        {
            if (Created && gravityIndicator != null && indicatorMesh != null)
            {
                if (PLServer.Instance == null)
                {
                    if (indicatorMesh.enabled)
                    {
                        indicatorMesh.enabled = false;
                    }
                    return;
                }
                if (PLInput.Instance.GetButtonDown("gravIndicator"))
                {
                    IndicatorEnabled = !IndicatorEnabled;
                }
                if (IndicatorEnabled)
                {
                    indicatorMesh.enabled = IndicatorEnabled && (PLUIOutsideWorldUI.Instance.pilotingHUDActive || (PLCameraSystem.Instance.GetModeString() == "SensorDish"));
                    if (GUI.ElementMode == 1)
                    {
                        PLPawn pawn = PLNetworkManager.Instance.ViewedPawn;
                        if (pawn != null && pawn.CurrentShip != null)
                        {
                            Transform ship = pawn.CurrentShip.Exterior.transform;
                            gravityIndicator.transform.position = ship.position +
                            ship.forward * GUI.Offset.z +
                            ship.right * GUI.Offset.x +
                            ship.up * GUI.Offset.y;
                        }
                    }
                } 
                else if(indicatorMesh.enabled)
                {
                    indicatorMesh.enabled = false;
                }
            }
        }
    }
}
