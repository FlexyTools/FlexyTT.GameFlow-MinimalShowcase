namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Play
{
	public class CallBinder_TriggerEnter : CallBinder
	{
		private		Action?	_action;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<Mob_Player>(out var pl))
			{
				Do();
			}
		}

		private		void	Do			( )	
		{
			_action?.Invoke();
		}

		private		void	Awake		( )	
		{
			Init( ref _action );	
		}
	}
}