using Flexy.Core.Extensions;
using FlexyTT.GameFlow_MinimalShowcase.Coregame.Play;
using UnityEngine.SceneManagement;

namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Mode
{
	public class GameMode_Escape : MonoBehEx
	{
		[SerializeField]	AssetRef<Mob_Player>	_playerMobRef;
	
		private Mob_Player _playerMob = null!;
	
		public		Single		StartTime	{ get; set; }
		public		Single		RunTime		=> Time.time - StartTime;
		public		Boolean		IsEscaped	{ get; set; }
		
		public		Single		EscapeTime	{ get; private set; }
		public		Mob_Player	PlayerMob	=> _playerMob;

		public		void		StartPlay	( )		
		{
			StartTime = Time.time;
			
			var playerMobPrefab = _playerMobRef.LoadAssetSync()!;
			_playerMob = playerMobPrefab.InstantiateInactive();
			SceneManager.MoveGameObjectToScene(_playerMob.gameObject, gameObject.scene);
			
			var spawnPoint = this.GetService<MapContext>().GetRandomSpawnPoint();
			_playerMob.transform.position = spawnPoint.position;
			_playerMob.transform.rotation = spawnPoint.rotation;
			_playerMob.gameObject.SetActive(true);
		}

		[Callable]	
		public		void		Escape		( )		
		{
			IsEscaped = true;
			EscapeTime = RunTime;
		}
	}
}