namespace Sandbox;

public partial class Pawn : AnimatedEntity
{
	public override void Spawn()
	{
		SetModel( "models/tank.vmdl" );
		Components.Create<AboveCameraMode>();
	}

	private Vector3 TargetOffset = new(50, 100, 0);

	public override void Simulate( Client cl )
	{
		var target = Position + TargetOffset;

		DebugOverlay.Axis( Position + Vector3.Up * 50, Rotation.Identity );
		DebugOverlay.Sphere( target, 5f, IsServer ? Color.Red : Color.Blue, 0f, false );

		if ( Input.Pressed( InputButton.PrimaryAttack ) ) { TargetOffset.x *= -1f; }

		SetAnimParameter( "target", Transform.NormalToLocal( TargetOffset ) );
	}
}
