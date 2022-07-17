namespace Sandbox;

public class AboveCameraMode : CameraMode
{
	public override void Update()
	{
		Position = Entity.Position.WithZ( Entity.Position.z + 200 );
		Rotation = Rotation.LookAt( Vector3.Down );
	}
}
