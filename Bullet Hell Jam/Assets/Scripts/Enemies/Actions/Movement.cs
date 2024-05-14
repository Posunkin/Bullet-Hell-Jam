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

    public void Stay()
    {
        _rb.velocity = Vector2.zero;
    }

    public void MoveToPosition(Vector2 pos)
    {
        _rb.MovePosition(_rb.position + pos * (_moveSpeed * Time.fixedDeltaTime));
    }

    public Vector2 GetPatrolPosition()
    {
       return _newPos = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
    }
}
