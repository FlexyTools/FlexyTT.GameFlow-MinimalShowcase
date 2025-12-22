namespace FlexyTT.GameFlow_MinimalShowcase.Coregame.Play
{
	public class Mob_Player : Mob
	{
		[SerializeField]	Single	_moveSpeed = 5;
		[SerializeField]	Single	_turnSpeed = 90;
	
		public	void	MoveForward		( )		
		{
			transform.position += transform.forward * Time.deltaTime * _moveSpeed;
		}
		public	void	MoveBackward	( )		
		{
			transform.position += transform.forward * Time.deltaTime * -_moveSpeed;
		}
		public	void	RotateLeft		( )		
		{
			transform.Rotate(Vector3.up, -_turnSpeed * Time.deltaTime);
		}
		public	void	RotateRight		( )		
		{
			transform.Rotate(Vector3.up, _turnSpeed * Time.deltaTime);
		}
	}
}