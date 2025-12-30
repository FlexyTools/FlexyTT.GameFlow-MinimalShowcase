using UnityEngine.SceneManagement;

namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Maps
{
	public class MapSet : ScriptableObject
	{
		[SerializeField]	Boolean 		_skipLoadedScenes	= true;
		[SerializeField]	List<SceneRef>	_maps				= null!;
	
		public LoadSceneTask?	LoadSetAsync			( GameObject context )		
		{
			if (_maps.Count == 0)
				return default;
	
			var task = _maps[0].LoadSceneAsync(context);
			task.RangeMax = 1.0f/_maps.Count;
			task.AddLoadStep(LoadRestScenesFromSet);
		
			return task;
		}
		private async UniTask	LoadRestScenesFromSet	( LoadSceneTask sceneTask )	
		{
			for (var i = 1; i < _maps.Count; i++)
			{
				sceneTask.RangeMin = i / (Single)_maps.Count;
				sceneTask.RangeMax = (i + 1) / (Single)_maps.Count;
		
				var sceneRef	= _maps[i];
				
				if (_skipLoadedScenes && SceneManager.GetSceneByName( AssetRef.AssetsLoader.GetSceneName(sceneRef) ).IsValid())
					continue;

				await sceneRef.LoadSceneAsync(sceneTask.Context);
			}
		}
	}
}