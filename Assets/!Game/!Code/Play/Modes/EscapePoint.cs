namespace FlexyTT.GameFlow_MinimalShowcase.Play.Modes
{
	public class EscapePoint : MonoBehEx
	{
		[SerializeField]	Int32	_pointNumber;
	
		[Callable]	void	Escape	( ) => this.GetService<GameMode_Escape>().Escape();
	
		private		void	Awake	( )	
		{
			var number = this.GetService<GameMode_Escape>().ExitPointNumber;
			
			if (number != _pointNumber)
				Destroy(gameObject);
		}
	}
}