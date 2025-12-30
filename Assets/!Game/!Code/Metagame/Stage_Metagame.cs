namespace FlexyTT.GameFlow_MinimalShowcase.Metagame
{
	[ServiceTypes(typeof(GameStage))]
	public class Stage_Metagame : GameStageEx
	{
		[SerializeField] AssetRef<GameStage> _coreGameStage;

		private			Facade_Game		_game; 
		private			Facade_Game		Game		=> _game.GetCached( this );

		public async	UniTaskVoid		Play_Map	( SceneRef map )	
		{
			var score = await Graph.Open( _coreGameStage, map ).WaitResult<Single>();
				
			if (map == default && score == default) // If data is empty then map is not completed
				return;
		
			Game.Leaderboards.AddRecord( 1, score );
			Game.UI.Leaderboards.Open( 1 );
		}
	}
}