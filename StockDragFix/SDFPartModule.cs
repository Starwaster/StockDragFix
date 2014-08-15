#define DEBUG

using UnityEngine;
using System;
using System.Collections.Generic;

namespace StockDragFix {
	public class SDFPartModule : PartModule {
		[KSPField()]
		float maximum_drag = -1f;

		[KSPField()]
		float minimum_drag = -1f;

		public override void OnStart(PartModule.StartState start) {
			base.OnStart(start);
			print("StockDragFix PartModule: OnStart()");
		}

		public override void OnAwake() {
			base.OnAwake();
			if(part && part.Modules != null) {
				if(maximum_drag <= 0f) {
					maximum_drag = part.partInfo.partPrefab.maximum_drag;
				}
				if(minimum_drag <= 0f) {
					minimum_drag = part.partInfo.partPrefab.minimum_drag;
				}
				print(part.name + " drag set to " + minimum_drag.ToString() + " / " + maximum_drag.ToString());
			}
		}

		public void FixedUpdate() {
			part.minimum_drag = maximum_drag * part.mass / (part.mass + part.GetResourceMass());
			part.maximum_drag = maximum_drag * part.mass / (part.mass + part.GetResourceMass());
			part.minimum_drag *= Mathf.Abs(StockDragFix.dragScale);
			part.maximum_drag *= Mathf.Abs(StockDragFix.dragScale);
		}
	}
}

