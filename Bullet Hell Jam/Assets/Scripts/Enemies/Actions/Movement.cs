using UnityEngine;

public class Movement
{
    private Enemy _enemy;
    private float _moveSpeed;
    private Rigidbody2D _rb;
    private Vector2 _newPos;

    public Movement(Enemy enemy, Rigidbody2D rb, float moveSpeed)
    {
        _enemy = enemy;
        _rb = rb;
        _moveSpeed = moveSpeed;
    }

    public void MoveToPosition()
    {
        _rb.MovePosition(_rb.position + _newPos * (_moveSpeed * Time.fixedDeltaTime));
    }

    public void GetPatrolPosition()
    {
        _newPos = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
    }
}
