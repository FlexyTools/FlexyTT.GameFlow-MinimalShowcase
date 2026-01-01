using UnityEngine.EventSystems;

namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Controls
{
	public class Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public Boolean	IsPressed			{ get; private set; }
		
		public	void	OnPointerDown	( PointerEventData eventData ) => IsPressed = true;
		public	void	OnPointerUp		( PointerEventData eventData ) => IsPressed = false;
	}
}