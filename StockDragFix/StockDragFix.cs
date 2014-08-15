#define DEBUG

using System;
using UnityEngine;
namespace StockDragFix {
	[KSPAddon(KSPAddon.Startup.MainMenu, false)]
	public class StockDragFix : UnityEngine.MonoBehaviour {
		public static float dragScale = 1f;
		public static bool enableConicDragTypes = false;

		public void Start() {
			// Load mod Configuration
			foreach(ConfigNode node in GameDatabase.Instance.GetConfigNodes("STOCK_DRAG_FIX_SETTINGS")) {
				if(node.HasValue("dragScale")) {
					float.TryParse(node.GetValue("dragScale"), out dragScale);
				}
				if(node.HasValue("enableConicDragTypes")) {
					bool.TryParse(node.GetValue("enableConicDragTypes"), out enableConicDragTypes);
				}
			}

			if(PartLoader.LoadedPartsList != null) {
				foreach(AvailablePart part in PartLoader.LoadedPartsList) {
					try {
						if(part.partPrefab != null) {
							if(enableConicDragTypes && part.partPrefab.Modules.Contains("ProceduralFairingSide") || part.partPrefab.partInfo.name.ToLowerInvariant().Contains("cone") || part.partPrefab.partInfo.title.ToLowerInvariant().Contains("cone")) {
								part.partPrefab.dragModel = Part.DragModel.CONIC;
								part.partPrefab.dragModelType = "Conical";
								Debug.Log(part.name + " switched to CONIC Drag Model");
							}
						}
					} catch(Exception e) {
						print("StockDragFix: Error: (" + part.name + ")" + e.Message);
					}
				}
			}
		}
	}

	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class DragFixer : UnityEngine.MonoBehaviour {
		public void Start() {
			GameEvents.onVesselGoOffRails.Add(SDFAddDragModules);

		}

		private void SDFAddDragModules(Vessel v) {
			Debug.Log("StockDragFixer " + v.name + " going off rails; adding SDFPartModules");
			foreach(Part part in v.parts) {
				if(part.mass != (part.mass + part.GetResourceMass()) && !part.Modules.Contains("SDFPartModule")) {
					part.AddModule("SDFPartModule");
					Debug.Log("Added SDFPartModule to " + part.name);
				}
				if(part.dragModel == Part.DragModel.CONIC) {
					Debug.Log(part.name + " drag model CONIC");
				}
			}
		}
	}
}