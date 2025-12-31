using Flexy.Core.Extensions;
using FlexyTT.GameFlow_MinimalShowcase.Coregame.Mode;
using UnityEngine.SceneManagement;

namespace FlexyTT.GameFlow_MinimalShowcase.Coregame
{
	[ServiceTypes(typeof(GameStage))]
	public class Stage_Coregame : GameStageEx, IStateWithResult<Single>
	{
		[SerializeField]	GameMode_Escape	_gameModePrefab	= null!;
		[SerializeField]	GameObject		_loaderOverlay	= null!;
		[SerializeField]	CanvasGroup		_backOveraly	= null!;
	
		[Bindable] Int32	LoadingProgress		=> (Int32)(LoadingProgress01 * 100);
        [Bindable] Single	LoadingProgress01	{ get; set; } 
        
        //public	GameMode_Escape	GameMode			=> _gameMode;
        
		private Single				_resultScore;
		private Boolean				_isLeaving;
		private GameMode_Escape		_gameMode = null!;

		public	Single				GetResult			( FlowNode node ) => _resultScore;
		public	void				Exit				( )		
		{
			_isLeaving = true;
			CloseSubStates(true);
		}
		
		protected override	void	OnShow				( )		
		{
			enabled = false;
			Game.Audio.SwitchToCore();
		
			if (AnySubStateOpened) // Case of special boot from state scene
			{
				_loaderOverlay.SetActive(false);
				return;
			}
			
			LoadStage( OpenParams as SceneRef? ).Forget();
		}
		protected override	void	OnLastChildHide		( )		
		{
			if (OpenParams is (Boolean replay, SceneRef map))
			{
				LoadStage( map ).Forget();
				return;
			}
		
			_resultScore = default;
		
			if (!_isLeaving && _gameMode != null)
				_resultScore = _gameMode.EscapeTime;
			
			UnloadStage().Forget();
		}
		protected override	void	OnHide				( )		
		{
			Game.Audio.SwitchToMeta();
		}
		
		private			void		Update				( )		
		{
			if (_gameMode == null || !_gameMode.IsEscaped)
				return;
			
			enabled = false;
			FinishGameAsync().Forget();
				
			async UniTaskVoid FinishGameAsync ( ) 
			{
				await UniTask.Delay( 1000, DelayType.UnscaledDeltaTime );
				CloseSubStates(true);
				Game.States.PlayComplete.Open( _gameMode.EscapeTime );
			}
		}
		
		private async	UniTask		LoadStage			( SceneRef? mapToLoad )		
		{
			_loaderOverlay.gameObject.SetActive(true);
			
			// Spawn GameMode
			{
				_gameMode = _gameModePrefab.InstantiateInactive();
				SceneManager.MoveGameObjectToScene(_gameMode.gameObject, Flow.gameObject.scene);
				_gameMode.gameObject.SetActive(true);
				Context.SetService(_gameMode);
			}

			Scene loadedScene;
			
			if (mapToLoad == null)
			{
				//We started from Map scene so just simulate short loading and open root state
				await UniTask.Delay( 100, ignoreTimeScale:true );
				loadedScene			= SceneManager.GetActiveScene();
				LoadingProgress01	= 1.0f;
			}
			else
			{
				LoadingProgress01	= 0.0f;
				var sceneRef		= mapToLoad.Value;
				var loadTask		= sceneRef.LoadSceneAsync( gameObject, LoadSceneMode.Single );
				
				while (!loadTask.IsDone)
				{
					LoadingProgress01	= loadTask.Progress * 0.9f;
					await UniTask.NextFrame();
				}
				
				loadedScene	= loadTask.Scene;
				await GameContext.GetCtx(loadedScene).WaitInitialization();
				
				LoadingProgress01	= 1.0f;
			}
			
			await UniTask.Delay( 350, ignoreTimeScale:true );
			//GameStage.MoveToLoadedScene( loadedScene );
			
			_gameMode = loadedScene.GetService<GameMode_Escape>();
			_gameMode.StartPlay();
			
			GameStage.OpenMainSubState();
			Game.RecacheCtx();
			
			_loaderOverlay.gameObject.SetActive(false);
		}
		private async	UniTask		UnloadStage			( )							
		{
			_loaderOverlay.gameObject.SetActive(true);
			//GameStage.MoveToServiceScene();
			
			await UniTask.NextFrame();
			await SceneRef.LoadDummySceneAsync( gameObject, LoadSceneMode.Single );
			await UniTask.Delay( 350, ignoreTimeScale:true );
			
			CloseAndDestroy();
		}
	}
}