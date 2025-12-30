namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Maps
{
	public class EnterPoint : MonoBehEx
	{
		[SerializeField]	Transform _enterPoint = null!;

		public Transform Point => _enterPoint;
	}
}