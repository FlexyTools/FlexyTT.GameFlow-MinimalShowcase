using UnityEngine.EventSystems;

namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Controls
{
	public class Stick : BindableBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
	{
		[SerializeField] RectTransform	_thumb			= null!;
		[SerializeField] Single			_maxDistance	= 200;

		private RectTransform	_base = null!;
		private Vector2			_startPosition;

		[Bindable] public Boolean	IsDown		{ get; private set; }
		[Bindable] public Single	TurnValue	{ get; private set; }
		
		private	void	Start		( )								
		{
			_base = GetComponent<RectTransform>();
			_startPosition = _thumb.anchoredPosition;
		}

		public	void	OnPointerDown	( PointerEventData eventData )	
		{
			IsDown = true;
		}
		public	void	OnPointerUp		( PointerEventData eventData )	
		{
			IsDown = false;
		}
		public	void	OnBeginDrag		( PointerEventData eventData )	
		{
		}
		public	void	OnDrag			( PointerEventData eventData )	
		{
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_base, eventData.position, eventData.pressEventCamera, out var localPoint)) 
				return;
			
			var maxDistance = _maxDistance;
			var clampedX = Mathf.Clamp(localPoint.x, -maxDistance, maxDistance);
			_thumb.anchoredPosition = new Vector2(clampedX, _startPosition.y);
				
			TurnValue = maxDistance > 0 ? clampedX / maxDistance : 0f;
			RebindProperty(nameof(TurnValue));
		}
		public	void	OnEndDrag		( PointerEventData eventData )	
		{
			_thumb.anchoredPosition = _startPosition;
			TurnValue = 0f;
			RebindProperty(nameof(TurnValue));
		}
	}
}