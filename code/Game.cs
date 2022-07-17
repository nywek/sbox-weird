using System.Linq;

namespace Sandbox;

public partial class MyGame : Sandbox.Game
{
	public MyGame()
	{
		
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		DebugOverlay.ScreenText( $"Current Pawn: {(cl.Pawn is PhysicsPawn ? "physics pawn" : "normal pawn")}" );
		DebugOverlay.ScreenText( $"Press MOUSE1 to switch target vector", 1 );
		DebugOverlay.ScreenText( "Press RELOAD to respawn as normal pawn (cannon does not lag behind)", 2 );
		DebugOverlay.ScreenText( "Press JUMP to respawn as physics pawn (cannon DOES lag behind)", 3 );

		if ( IsServer && Input.Pressed( InputButton.Reload ) )
			RespawnPawn( cl, new Pawn() );

		if ( IsServer && Input.Pressed( InputButton.Jump ) )
			RespawnPawn( cl, new PhysicsPawn() );

		if ( IsServer && Input.Pressed( InputButton.Duck ) )
			RespawnPawn( cl, new MovingPawn() );
	}

	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );
		RespawnPawn( client, new Pawn() );
	}

	private void RespawnPawn( Client client, Entity newPawn )
	{
		if ( client.Pawn is not null ) { client.Pawn.Delete(); }
		client.Pawn = newPawn;
		var spawnPoint = Entity.All.OfType<SpawnPoint>().First();
		newPawn.Position = spawnPoint.Position + Vector3.Up * 50f;
	}
}
