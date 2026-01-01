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
		
		protected override	void	OnShow				( )		
		{
			base.OnShow();
		
			if (!Node.FullyInited)
			{
				// Special case of first Show called from BackShow
				// This happens in case of opening many states in a row so state 1 will transition to state 4
				// than back operations will open state 3 -> 2 -> 1. foreach BackShow will be called
				// but for 3 and 2 Show will be called before BackShow because they had never shown before  
				// BackShow will be called next so we skip loading scene
				return;
			}
			
			Game.Audio.SwitchToMeta(); 
			LoadStage().Forget(); 
		}
		protected override	void	OnBackShow			( )		
		{
			Game.Audio.SwitchToMeta();
			LoadStage().Forget();
		}
		
		private async	UniTask		LoadStage			( )		
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