using UnityEngine.SceneManagement;

namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Maps
{
	public class Service_SimpleScene : MonoBehEx
	{
		[SerializeField]	CanvasGroup		_backOverlay	= null!;
		[SerializeField]	String?			_playerMobTag;
	
		public async	UniTask		GoToMapAtPoint		( GlobalRef<EnterPoint> enterPointRef )	
		{
			Debug.Log( $"[Service_SimpleScene] GoToMapAtPoint {enterPointRef.ToStringNice()}" );
			
			var playerMob = String.IsNullOrWhiteSpace(_playerMobTag) ? null : GameObject.FindWithTag(_playerMobTag);
			
			_backOverlay.alpha	= 0.0f;
			_backOverlay.gameObject.SetActive(true);
			
			while (_backOverlay.alpha < 1.0f)
			{
				_backOverlay.alpha	+= Time.deltaTime * 2;
				await UniTask.NextFrame();
			}
			
			if (playerMob != null)
				playerMob.SetActive(false);	

			EnterPoint enterPoint;

			// Load new map
			{
				var currentScene = SceneManager.GetActiveScene();
				
				var dummy = await SceneRef.LoadDummySceneAsync(gameObject, LoadSceneMode.Additive);
				await SceneManager.UnloadSceneAsync(currentScene);
				
				var nextScene	= await enterPointRef.Scene.LoadSceneAsync(gameObject, LoadSceneMode.Additive);
				enterPoint		= enterPointRef.Get(nextScene);
				
				await SceneManager.UnloadSceneAsync(dummy);
			}
			
			if (playerMob != null)
			{
				playerMob.transform.SetLocalPositionAndRotation(enterPoint.Point.position, enterPoint.Point.rotation);
				playerMob.gameObject.SetActive(true);
			}
			
			while (_backOverlay.alpha > 0.0f)
			{
				_backOverlay.alpha	-= Time.deltaTime;
				await UniTask.NextFrame();
			}

			_backOverlay.alpha	= 0.0f;
			_backOverlay.gameObject.SetActive(false);
		}	
	}
}