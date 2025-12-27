using UnityEngine.SceneManagement;

namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Maps
{
	public class MapSet : ScriptableObject
	{
		[SerializeField]	List<SceneRef>	_maps = null!;
	
		public UniTask<Scene>	LoadSet	( GameObject context )		
		{
			if (_maps.Count == 0)
				return default;
	
			var task = _maps[0].LoadSceneAsync(context);
			task.RangeMax = 1.0f/_maps.Count;
			task.SetNextStep(NextSteps);
		
			return task.AsUniTask();
		}

		private async UniTask NextSteps(LoadSceneTask sceneTask)
		{
			for (var i = 1; i < _maps.Count; i++)
			{
				sceneTask.RangeMin = i / (Single)_maps.Count;
				sceneTask.RangeMax = (i + 1) / (Single)_maps.Count;
		
				var sceneRef	= _maps[i];
				var sceneName	= AssetRef.AssetsLoader.GetSceneName(sceneRef);

				if (SceneManager.GetSceneByName(sceneName).IsValid())
					continue;

				await sceneRef.LoadSceneAsync(sceneTask.Context);
			}
		}
	}
}