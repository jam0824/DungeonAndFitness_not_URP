//\$ Copyright 2015-22, Code Respawn Technologies Pvt Ltd - All Rights Reserved \$//\n
using UnityEngine;
using DungeonArchitect;

public class FirstFloorWallSelectionRule : SelectorRule {
	public override bool CanSelect(PropSocket socket, Matrix4x4 propTransform, DungeonModel model, System.Random random) {

		float width = 5.0f;
		//return (socket.gridPosition.x + socket.gridPosition.z) % 2 == 0;
		if((socket.gridPosition.x == -100 / width) &&
			(socket.gridPosition.z == -25 / width))
				return false;
		if ((socket.gridPosition.x == -100 / width) && 
			(socket.gridPosition.z == -35 / width))
			return false;
		return true;
	}
}
