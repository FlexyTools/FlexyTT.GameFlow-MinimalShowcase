using Flexy.Core.Extensions;
using FlexyTT.GameFlow_MinimalShowcase.Coregame.Maps;
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

		public async	UniTask		GoToMapAtPoint		( GlobalRef<EnterPoint> enterPointRef )	
		{
			Debug.LogError( $"SL GoToMap: {enterPointRef.ToStringNice()}" );
			
			Node.FirstChild!.GetLastSibling().State.enabled = false;
			_backOveraly.alpha	= 0.0f;
			_backOveraly.gameObject.SetActive(true);
			
			while (_backOveraly.alpha < 1.0f)
			{
				_backOveraly.alpha	+= Time.deltaTime * 2;
				await UniTask.NextFrame();
			}
			
			// Load new map
			_gameMode.PlayerMob.gameObject.SetActive(false);
			var currentScene = SceneManager.GetActiveScene();
			
			Debug.LogError( $"SL Load Dummy" );
			var dummy = await SceneRef.LoadDummySceneAsync(gameObject, LoadSceneMode.Additive);
			
			Debug.LogError( $"SL Unload current scene {currentScene.name}" );
			await SceneManager.UnloadSceneAsync(currentScene);
			
			Debug.LogError( $"SL Load Scene: {enterPointRef.ToStringNice()}" );
			var nextScene	= await enterPointRef.Scene.LoadSceneAsync(gameObject, LoadSceneMode.Additive);
			var enterPoint	= enterPointRef.Get(nextScene);
			
			await SceneManager.UnloadSceneAsync(dummy);
			
			Debug.LogError( $"SL Load Scene DONE" );
			
			_gameMode.PlayerMob.transform.SetLocalPositionAndRotation(enterPoint.Point.position, enterPoint.Point.rotation);
			_gameMode.PlayerMob.gameObject.SetActive(true);
			
			while (_backOveraly.alpha > 0.5f)
			{
				_backOveraly.alpha	-= Time.deltaTime;
				await UniTask.NextFrame();
			}

			Node.FirstChild!.GetLastSibling().State.enabled = true;

			while (_backOveraly.alpha > 0.0f)
			{
				_backOveraly.alpha	-= Time.deltaTime;
				await UniTask.NextFrame();
			}

			_backOveraly.alpha	= 0.0f;
			_backOveraly.gameObject.SetActive(false);
		}
	}
}