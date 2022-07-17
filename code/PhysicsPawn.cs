namespace Sandbox;

public partial class PhysicsPawn : Pawn
{
	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Keyframed );
	}
}
