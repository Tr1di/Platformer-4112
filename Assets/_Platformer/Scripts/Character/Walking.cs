using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Walking : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [Space]
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;
    [Space]    
    [SerializeField] private GroundChecker groundChecker;

    private Vector2 _direction;
    private Vector2 _desiredVelocity;
    private Vector2 _velocity;
    
    private Rigidbody2D _body;
    
    private float _maxSpeedChange;
    private float _acceleration;
    
    private bool _onGround;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        _direction.x = inputManager.Move.x;
        _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(maxSpeed - groundChecker.Friction, 0f);
    }

    private void FixedUpdate()
    {
        _onGround = groundChecker.OnGround;
        _velocity = _body.velocity;

        _acceleration = _onGround ? maxAcceleration : maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

        _body.velocity = _velocity;
    }
}
