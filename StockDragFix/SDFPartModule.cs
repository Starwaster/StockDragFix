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

			drag_multiplier = StockDragFix.dragScale;

			/* Extra drag should be a multiple of the dragScale, so it stays a fraction of the drag. */
			if(StockDragFix.stackDrag > 0) {
				float extraDrag = StockDragFix.stackDrag * StockDragFix.dragScale;

				/* check if we have stack attach nodes */
				foreach(AttachNode node in part.attachNodes) {
					if(node.nodeType == AttachNode.NodeType.Stack) {
						if(node.attachedPart == null) {
							/* add a increase to drag per unfilled stack attach point,
							 * since cones only have 1 stack node, and it's always filled.
							 * This also meaning that flying vessels will also be more stable
							 * as the part with highest drag (the engines) will want to be behind it.
							 */
							drag_multiplier += extraDrag;
						}
					}
				}
			}
		}

		public void FixedUpdate() {
			part.maximum_drag = maximum_drag * part.mass / (part.mass + part.GetResourceMass());
			part.minimum_drag = minimum_drag * part.mass / (part.mass + part.GetResourceMass());

			part.maximum_drag *= drag_multiplier;
			part.minimum_drag *= drag_multiplier;

			//IF part.minimum_drag = minimum_drag IS NOT A bug THEN
			//part.maximum_drag = maximum_drag * part.mass / (part.mass + part.GetResourceMass());
			//part.maximum_drag *= drag_multiplier;
			//part.minimum_drag = part.maximum_drag;
			//ENDIF
		}
	}
}

