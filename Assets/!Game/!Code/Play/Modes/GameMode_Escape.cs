using Flexy.Core.Extensions;
using FlexyTT.GameFlow_MinimalShowcase.Play.Mobs;
using UnityEngine.SceneManagement;

namespace FlexyTT.GameFlow_MinimalShowcase.Play.Modes
{
	public class GameMode_Escape : MonoBehEx
	{
		[SerializeField]	AssetRef<Mob_Player>	_playerMobRef;
	
		private		Mob_Player	_playerMob		= null!;
		private		Int32		_exitPointNumber= -1;
	
		public		Single		StartTime		{ get; set; }
		public		Single		RunTime			=> Time.time - StartTime;
		public		Boolean		IsEscaped		{ get; set; }
		
		public		Single		EscapeTime		{ get; private set; }
		public		Mob_Player	PlayerMob		=> _playerMob;
		public		Int32		ExitPointNumber	=> _exitPointNumber;

		public		void		Init		( Int32 exitsCount )		
		{
			_exitPointNumber = Random.Range(0, exitsCount)+1;
		}
		public		void		StartPlay	( Scene firstLoadedMap )	
		{
			StartTime		 = Time.time;
			
			var playerMobPrefab = _playerMobRef.LoadAssetSync()!;
			_playerMob = playerMobPrefab.InstantiateInactive();
			SceneManager.MoveGameObjectToScene(_playerMob.gameObject, gameObject.scene);
			
			var spawnPoint = firstLoadedMap.GetService<MapContext>().GetRandomSpawnPoint();
			_playerMob.transform.position = spawnPoint.position;
			_playerMob.transform.rotation = spawnPoint.rotation;
			_playerMob.gameObject.SetActive(true);
		}
		private		void		OnDestroy	( )							
		{
			Destroy(_playerMob.gameObject);
		}

		[Callable]	
		public		void		Escape		( )		
		{
			IsEscaped = true;
			EscapeTime = RunTime;
		}
	}
}