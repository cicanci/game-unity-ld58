using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BaseStats), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private BaseStats _stats;
    private InputControls _controls;
    private Rigidbody _rigidbody;
    private Vector2 _input;
    private Coroutine _damageCoroutine;

    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _controls = new InputControls();
        _rigidbody = GetComponent<Rigidbody>();
        _stats = GetComponent<BaseStats>();
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
        _controls.Player.Move.performed += context => _input = context.ReadValue<Vector2>();
        _controls.Player.Move.canceled += _ => _input = Vector2.zero;
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(_input.x, 0, _input.y);
        _rigidbody.MovePosition(_rigidbody.position + move * _stats.MovementSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Debug.Log($"OnCollisionEnter: {collision.collider.name}");
            _damageCoroutine = StartCoroutine(DamageOverTime(collision.gameObject));
        }
        if (collision.collider.CompareTag("Card"))
        {
            Debug.Log($"OnCollisionEnter: {collision.collider.name}");
            collision.gameObject.GetComponent<CardController>().PickUpCard();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Debug.Log($"OnCollisionExit: {collision.collider.name}");
            StopDamageOverTime();
        }
    }

    private IEnumerator DamageOverTime(GameObject other)
    {
        BaseStats otherStats = other.GetComponent<BaseStats>();

        if (otherStats == null)
        {
            yield break;
        }

        Debug.Log($"DamageOverTime: {other.name}. Heath = {otherStats.Health}");

        while (otherStats.Health > 0)
        {
            float multiplier = GetMultiplier(otherStats);

            otherStats.Health -= _stats.Damage * multiplier;
            Debug.Log($"DamageOverTime: {other.name} tooke {_stats.Damage} damage with {multiplier}x multiplier. Heath = {otherStats.Health}");

            if (otherStats.Health <= 0)
            {
                Debug.Log($"DamageOverTime: {other.name} is dead.");
                other.GetComponent<EnemyController>().DropLoot();
                yield break;
            }

            yield return new WaitForSeconds(otherStats.AttackCooldown);
        }
    }

    private void StopDamageOverTime()
    {
        if (_damageCoroutine != null)
        {
            StopCoroutine(_damageCoroutine);
            _damageCoroutine = null;
        }
    }

    private float GetMultiplier(BaseStats other)
    {
        if (_stats.Attributes.HasFlag(AttributeTypes.FireAttack))
        {
            if (other.Attributes.HasFlag(AttributeTypes.FireResistence))
            {
                return 0.5f;
            }

            if (other.Attributes.HasFlag(AttributeTypes.IceResistence))
            {
                return 2f;
            }
        }

        if (_stats.Attributes.HasFlag(AttributeTypes.IceAttack))
        {
            if (other.Attributes.HasFlag(AttributeTypes.IceResistence))
            {
                return 0.5f;
            }

            if (other.Attributes.HasFlag(AttributeTypes.FireResistence))
            {
                return 2f;
            }
        }

        return 1f;
    }
}
