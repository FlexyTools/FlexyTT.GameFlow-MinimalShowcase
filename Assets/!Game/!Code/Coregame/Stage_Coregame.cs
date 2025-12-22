using FlexyTT.GameFlow_MinimalShowcase.Common;
using FlexyTT.GameFlow_MinimalShowcase.Coregame.Kit;
using UnityEngine.SceneManagement;

namespace FlexyTT.GameFlow_MinimalShowcase.Coregame
{
	[ServiceTypes(typeof(GameStage))]
	public class Stage_Coregame : GameStageEx, IStateWithResult<Single>
	{
		[SerializeField]	GameObject _loaderOverlay = null!;
	
		[Bindable] Int32	LoadingProgress		=> (Int32)(LoadingProgress01 * 100);
        [Bindable] Single	LoadingProgress01	{ get; set; } 
        
		private Single			_resultScore;
		private Boolean			_isLeaving;
		private GameMode_FindExit?		_gameMode;

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
			
			LoadMap( OpenParams as SceneRef? ).Forget();
		}
		protected override	void	OnLastChildHide		( )		
		{
			if (OpenParams is (Boolean replay, SceneRef map))
			{
				LoadMap( map ).Forget();
				return;
			}
		
			_resultScore = default;
		
			if (!_isLeaving && _gameMode != null)
				_resultScore = _gameMode.Result;
			
			UnloadMap().Forget();
		}
		protected override	void	OnHide				( )		
		{
			Game.Audio.SwitchToMeta();
		}
		
		private			void		Update				( )		
		{
			if (_gameMode == null || !_gameMode.IsWin)
				return;
			
			enabled = false;
			FinishGameAsync().Forget();
				
			async UniTaskVoid FinishGameAsync ( ) 
			{
				await UniTask.Delay( 1000, DelayType.UnscaledDeltaTime );
				CloseSubStates(true);
				Game.States.PlayComplete.Open( _gameMode.Result );
			}
		}
		
		private async	UniTask		LoadMap				( SceneRef? mapToLoad )		
		{
			_loaderOverlay.gameObject.SetActive(true);
		
			Scene loadedScene;
			
			if (mapToLoad == null)
			{
				//We started from coregame scene so just simulate short loading and open root state
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
			GameStage.MoveToLoadedScene( loadedScene );
			
			_gameMode = loadedScene.GetService<GameMode_FindExit>();
			
			GameStage.OpenMainSubState();
			Game.RecacheCtx();
			
			_loaderOverlay.gameObject.SetActive(false);
		}
		private async	UniTask		UnloadMap			( )							
		{
			_loaderOverlay.gameObject.SetActive(true);
			GameStage.MoveToServiceScene();
			
			await UniTask.NextFrame();
			await SceneRef.LoadDummySceneAsync( gameObject, LoadSceneMode.Single );
			await UniTask.Delay( 350, ignoreTimeScale:true );
			
			CloseAndDestroy();
		}
	}
}