namespace Sandbox;

public partial class MovingPawn : AnimatedEntity
{
	[Net] public Rotation TargetRotation { get; set; }

	public override void Spawn()
	{
		SetModel( "models/tank.vmdl" );
		Components.Create<AboveCameraMode>();
		SetupPhysicsFromModel( PhysicsMotionType.Keyframed ); // <- causes delay
	}

	public override void Simulate( Client cl )
	{
		DebugOverlay.Axis( Position + Vector3.Up * 50, Rotation.Identity );
		DebugOverlay.Sphere( Input.Cursor.Origin, 5f, Color.Red, 0f, false );

		if ( cl.Components.Get<CameraMode>() is not null ) { return; } // dont simulate stuff if in dev cam

		// Input.Cursor.Origin is position of mouse in world space
		var targetDir = Input.Cursor.Origin.WithZ( 0 ) - Position.WithZ( 0 );
		SetAnimParameter( "target", Transform.NormalToLocal( targetDir ) );

		Velocity = new Vector3( Input.Forward, Input.Left, 0 ).Normal * 500f;
		TargetRotation = Velocity.IsNearlyZero() ? TargetRotation : Rotation.LookAt( Velocity, Vector3.Up );

		Rotation = Rotation.Lerp( Rotation, TargetRotation, 0.2f );

		var helper = new MoveHelper( Position, Velocity );
		helper.Trace = helper.Trace.Size( 16 );
		if ( helper.TryMove( Time.Delta ) > 0 )
		{
			Position = helper.Position;
		}
	}

	public override void BuildInput( InputBuilder inputBuilder )
	{
		base.BuildInput( inputBuilder );

		var cannonHeight = GetAttachment( "cannon" )?.Position.z ?? Position.z;
		Vector3 direction = Screen.GetDirection( Mouse.Position );
		float distance = (cannonHeight - Input.Position.z) / direction.z;
		var mousePos = Input.Position + (distance * direction);

		inputBuilder.Cursor.Origin = mousePos;
	}
}
