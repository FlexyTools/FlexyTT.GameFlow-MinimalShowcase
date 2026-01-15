using UnityEngine.SceneManagement;

namespace FlexyTT.GameFlow_MinimalShowcase.Metagame
{
	[ServiceTypes(typeof(GameStage))]
	public class Stage_Metagame : GameStageEx
	{
		[SerializeField] SceneRef				_metaStageScene;
		[SerializeField] CanvasGroup			_blackOveraly	= null!;
		[SerializeField] AssetRef<GameStage>	_coreGameStage;

		private			Facade_Game		_game; 
		private			Facade_Game		Game			=> _game.GetCached( this );

		public async	UniTaskVoid		Play_Map		( params SceneRef[] maps )	
		{
			var score = await Graph.Open( _coreGameStage, maps ).WaitResult<Single>();
				
			if (score == default) // If data is empty then map is not completed
				return;
		
			var isArena = maps.Length == 1;
		
			Game.Leaderboards.AddRecord( isArena, score );
			Game.UI.Leaderboards.Open( isArena );
		}
		
		protected override	UniTask		OnShow			( )		
		{
			Game.Audio.SwitchToMeta(); 
			LoadStage().Forget();
			return base.OnShow(); 
		}
		protected override	UniTask		OnBackShow		( )		
		{
			Game.Audio.SwitchToMeta();
			LoadStage().Forget();
			
			return base.OnBackShow();
		}
		
		private async		UniTask		LoadStage		( )		
		{
			if (SceneManager.GetActiveScene().name == SceneRef.SceneLoader.GetSceneName(_metaStageScene))
			{
				// We already on MetaStage Scene so do nothing
				return;
			}

			_blackOveraly.gameObject.SetActive(true);
			await _metaStageScene.LoadSceneAsync( gameObject, LoadSceneMode.Single );
			DynamicGI.UpdateEnvironment();
			_blackOveraly.gameObject.SetActive(false);
		}
	}
}