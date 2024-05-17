using System.Collections;
using UnityEngine;
using Zenject;

public class JumpAttack : MonoBehaviour
{
    public bool Attacking { get; private set;}
    [SerializeField] private float _jumpHeight = 10f;
    [SerializeField] private float _fallSpeed = 5f;
    private bool _isFalling = false;

    private PlayerStats _player;
    private Animator _anim;
    private CapsuleCollider2D _collider;

    [Inject]
    private void Construct(PlayerStats player)
    {
        _player = player;
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider2D>();
    }

    public void Attack()
    {
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        Attacking = true;
        Vector2 jumpTarget = new Vector2(transform.position.x, transform.position.y + _jumpHeight);
        _collider.isTrigger = true;
        _collider.enabled = false;
        _anim.SetTrigger("Jump");
        while (jumpTarget.y - transform.position.y > 0.2f)
        {
            transform.Translate(Vector2.up * _fallSpeed * Time.deltaTime);
            yield return null;
        }
        _isFalling = true;
        _collider.enabled = true;
        Vector2 newPos = transform.position;
        newPos.x = _player.transform.position.x;
        transform.position = newPos;
        Vector2 target = _player.transform.position;
        while (_isFalling)
        {
            transform.Translate(Vector2.down * _fallSpeed * Time.deltaTime);

            if (transform.position.y <= target.y)
            { 
                _isFalling = false;
                Attacking = false;
                _anim.SetTrigger("Fall");
                _collider.isTrigger = false;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Attacking) return;
        if (collision.gameObject.TryGetComponent<PlayerStats>(out var player))
        {
            player.TakeDamage(2.5f);
        }
    }
}
