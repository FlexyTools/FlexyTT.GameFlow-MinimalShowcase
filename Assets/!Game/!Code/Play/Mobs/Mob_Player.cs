namespace FlexyTT.GameFlow_MinimalShowcase.Play.Mobs
{
	[RequireComponent(typeof(CharacterController))]
	public class Mob_Player : Mob
	{
		[SerializeField]	Single	_moveSpeed	= 5;
		[SerializeField]	Single	_turnSpeed	= 90;
		[SerializeField]	Single	_smooth		= 17;
	
		private	Single	_moveSpeedTarget;
		private	Single	_moveSpeedActual;
		
		private	Single	_turnSpeedTarget;
		private	Single	_turnSpeedActual;
	
		private	CharacterController	_cc = null!;
	
		public	void	MoveForward		( )		=> _moveSpeedTarget = _moveSpeed;
		public	void	MoveBackward	( )		=> _moveSpeedTarget = -_moveSpeed;

		public	void	RotateLeft		( )		=> _turnSpeedTarget = -_turnSpeed;
		public	void	RotateRight		( )		=> _turnSpeedTarget = +_turnSpeed;

		public	void	ResetInput		( )		
		{
			_moveSpeedTarget = 0;
			_turnSpeedTarget = 0;
		}

		private void	Awake			( )		
		{
			_cc = GetComponent<CharacterController>();
		}
		private void	Update			( )		
		{
			_moveSpeedActual = ExpLearp(_moveSpeedActual, _moveSpeedTarget, _smooth, Time.deltaTime);
			_turnSpeedActual = ExpLearp(_turnSpeedActual, _turnSpeedTarget, _smooth, Time.deltaTime);
			
			_cc.SimpleMove(transform.forward * _moveSpeedActual);
			transform.Rotate(Vector3.up, _turnSpeedActual * Time.deltaTime);
		}
		
		private Single	ExpLearp		( Single current, Single target, Single smooth, Single dt )	
		{
			return target + (current - target) * Mathf.Exp(-smooth * dt);
		}
	}
}