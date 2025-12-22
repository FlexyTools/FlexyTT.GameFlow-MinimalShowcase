namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Play
{
	public class TriggerEnterBinder : CallBinder
	{
		private		Action?	_action;

		private void OnTriggerEnter(Collider other)
		{
			Do();
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