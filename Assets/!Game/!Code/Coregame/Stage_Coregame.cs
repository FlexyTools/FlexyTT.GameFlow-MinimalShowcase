using Flexy.Core.Extensions;
using FlexyTT.GameFlow_MinimalShowcase.Coregame.Mode;
using UnityEngine.SceneManagement;

namespace FlexyTT.GameFlow_MinimalShowcase.Coregame
{
	[ServiceTypes(typeof(GameStage))]
	public class Stage_Coregame : GameStageEx, IStateWithResult<Single>
	{
		[SerializeField]	AssetRef<GameMode_Escape>	_gameModeRef;
		[SerializeField]	GameObject					_loaderOverlay	= null!;
	
		[Bindable] Int32	LoadingProgress		=> (Int32)(LoadingProgress01 * 100);
        [Bindable] Single	LoadingProgress01	{ get; set; } 
        
		private Single				_resultScore;
		private Boolean				_isLeaving;
		private GameMode_Escape		_gameMode = null!;

		public	Single				GetResult			( FlowNode node ) => _resultScore;
		public	void				Exit				( )		
		{
			_isLeaving = true;
			CloseSubStates(true);
		}
		
		protected override	UniTask	OnShow				( )		
		{
			enabled = false;
			Game.Audio.SwitchToCore();
		
			if (AnySubStateOpened) // Case of special boot from state scene
			{
				_loaderOverlay.SetActive(false);
				return default;
			}
			
			LoadStage( OpenParams as SceneRef[] ).Forget();
			return default;
		}
		protected override	void	OnLastChildHide		( )		
		{
			_resultScore = default;
		
			if (!_isLeaving && _gameMode != null)
				_resultScore = _gameMode.EscapeTime;
			
			UnloadStage().Forget();
		}
		protected override	UniTask	OnHide				( )		
		{
			Game.Audio.SwitchToMeta();
			return default;
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
				Game.States.Escaped.Open( _gameMode.EscapeTime );
			}
		}
		
		private async	UniTask		LoadStage			( SceneRef[]? maps )		
		{
			_loaderOverlay.gameObject.SetActive(true);
			
			// Spawn GameMode
			{
				var gameModePrefab = _gameModeRef.LoadAssetSync()!;
				_gameMode = gameModePrefab.InstantiateInactive();
				SceneManager.MoveGameObjectToScene(_gameMode.gameObject, Flow.gameObject.scene);
				_gameMode.gameObject.SetActive(true);
				Context.SetService(_gameMode);
			}

			Scene loadedScene;
			
			if (maps == null)
			{
				//We entered play mode from Map scene so init game mode from map DebugExitsCount and simulate short loading
				loadedScene		= SceneManager.GetActiveScene();
				var exitsCount	= FindAnyObjectByType<MapContext>().DebugExitsCount;
				_gameMode.Init(exitsCount);
				
				await UniTask.Delay( 100, ignoreTimeScale:true );
				LoadingProgress01	= 1.0f;
			}
			else
			{
				_gameMode.Init(maps.Length);
			
				LoadingProgress01	= 0.0f;
				var sceneRef		= maps[ Random.Range(0, maps.Length) ];
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
			
			_gameMode.StartPlay( loadedScene );
			
			GameStage.OpenMainSubState();
			Game.RecacheCtx();
			
			_loaderOverlay.gameObject.SetActive(false);
		}
		private async	UniTask		UnloadStage			( )							
		{
			_loaderOverlay.gameObject.SetActive(true);
			
			Destroy(_gameMode.gameObject);
			
			await UniTask.NextFrame();
			await SceneRef.SceneLoader.LoadDummySceneAsync( gameObject, LoadSceneMode.Single );
			await UniTask.Delay( 350, ignoreTimeScale:true );
			
			CloseAndDestroy();
		}
	}
}