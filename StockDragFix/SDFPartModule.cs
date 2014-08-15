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

		float drag_multiplier = 1f;

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

			//check if we have stack attach nodes
			foreach(AttachNode node in part.attachNodes) {
				if(node.nodeType == AttachNode.NodeType.Stack) {
					if(node.attachedPart == null) {
						//add a 5% drag increase per unfilled stack attach point
						//since cones only have 1 stack node, and it's always filled... :D
						//this means that minimally we will have 5% extra drag for each stack due to the engines
						drag_multiplier += 0.05f;
					}
				}
			}
		}

		public void FixedUpdate() {
			part.minimum_drag = maximum_drag * part.mass / (part.mass + part.GetResourceMass());
			part.maximum_drag = maximum_drag * part.mass / (part.mass + part.GetResourceMass());

			part.minimum_drag *= Mathf.Abs(StockDragFix.dragScale);
			part.maximum_drag *= Mathf.Abs(StockDragFix.dragScale);

			part.minimum_drag *= drag_multiplier;
			part.maximum_drag *= drag_multiplier;
		}
	}
}

