//\$ Copyright 2015-22, Code Respawn Technologies Pvt Ltd - All Rights Reserved \$//\n
using UnityEngine;
using DungeonArchitect;

public class RandomSpecialRotationRule : TransformationRule {
	
	public override void GetTransform(PropSocket socket, DungeonModel model, Matrix4x4 propTransform, System.Random random, out Vector3 outPosition, out Quaternion outRotation, out Vector3 outScale) {
		base.GetTransform(socket, model, propTransform, random, out outPosition, out outRotation, out outScale);

		
		var angle = random.NextFloat() * 360;
		var rotation = Quaternion.Euler(0, angle, 0);
		outRotation = rotation;
		
		
		float r = random.NextFloat() * -0.2f - 0.05f;
		var variation = new Vector3(0f, r, 0f);
		outPosition = Vector3.Scale (random.OnUnitSphere(), variation);
		

	}
}
