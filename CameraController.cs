using System;
using Godot;

namespace Subterrania.Core
{
	public class CameraController : Camera
	{
		private float _rotationX = 0f;
		private float _rotationY = 0f;
		
		private const float LookAroundSpeed = 0.5f;
		
		public CameraController() {}
		
		public override void _Input(InputEvent @event)
		{
			if (@event is InputEventMouseMotion mouseMotion)
			{
				_rotationX += mouseMotion.Relative.x * LookAroundSpeed;
				_rotationY +=mouseMotion.Relative.y * LookAroundSpeed;
				
				//Transform transform = Transform;
				//transform.basis = Basis.IDENTITY;
				//this.Transform = transform;
				
				RotateObjectLocal(Vector3.UP, _rotationX);
				RotateObjectLocal(Vector3.RIGHT, _rotationY);
				// not currently compiling
			}
		}
		
		//public override void _Ready () {}
		
		//public override void _Process(float delta) {}
	}
}
