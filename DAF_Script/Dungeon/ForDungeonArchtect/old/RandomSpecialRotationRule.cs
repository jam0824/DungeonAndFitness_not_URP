//\$ Copyright 2015-22, Code Respawn Technologies Pvt Ltd - All Rights Reserved \$//\n
using UnityEngine;
using DungeonArchitect;

public class RandomPositionRule : TransformationRule {
	
	public override void GetTransform(PropSocket socket, DungeonModel model, Matrix4x4 propTransform, System.Random random, out Vector3 outPosition, out Quaternion outRotation, out Vector3 outScale) {
		base.GetTransform(socket, model, propTransform, random, out outPosition, out outRotation, out outScale);

		float cellWidth = 2.0f;
		
		float rX = random.NextFloat() * (cellWidth * 2) - cellWidth;
		float rZ = random.NextFloat() * (cellWidth * 2) - cellWidth;
		var variation = new Vector3(rX, 0f, rZ);
		outPosition = Vector3.Scale (random.OnUnitSphere(), variation);
		

	}
}
