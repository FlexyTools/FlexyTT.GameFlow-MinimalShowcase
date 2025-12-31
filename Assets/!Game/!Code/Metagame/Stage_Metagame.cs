namespace FlexyTT.GameFlow_MinimalShowcase.Metagame
{
	[ServiceTypes(typeof(GameStage))]
	public class Stage_Metagame : GameStageEx
	{
		[SerializeField] AssetRef<GameStage> _coreGameStage;

		private			Facade_Game		_game; 
		private			Facade_Game		Game		=> _game.GetCached( this );

		public async	UniTaskVoid		Play_Map	( params SceneRef[] maps )	
		{
			var score = await Graph.Open( _coreGameStage, maps ).WaitResult<Single>();
				
			if (score == default) // If data is empty then map is not completed
				return;
		
			var isArena = maps.Length == 1;
		
			Game.Leaderboards.AddRecord( isArena, score );
			Game.UI.Leaderboards.Open( isArena );
		}
	}
}