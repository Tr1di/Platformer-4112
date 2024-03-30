using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Jumping : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f;
    
    private Rigidbody2D _body;
    private GroundChecker _ground;
    private Vector2 _velocity;

    private int _jumpPhase;
    private float _defaultGravityScale;
    private float _jumpSpeed;

    private bool _desiredJump;
    private bool _onGround;
    
    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<GroundChecker>();

        _defaultGravityScale = 1f;
    }

    private void OnEnable()
    {
        inputManager.onJump += Jump;
    }

    private void OnDisable()
    {
        inputManager.onJump -= Jump;
    }

    public void Jump()
    {
        _desiredJump = true;
    }
    
    private void FixedUpdate()
    {
        _onGround = _ground.OnGround;
        _velocity = _body.velocity;

        if (_onGround)
        {
            _jumpPhase = 0;
        }

        if (_desiredJump)
        {
            _desiredJump = false;
            JumpAction();
        }

        if (_body.velocity.y > 0)
        {
            _body.gravityScale = upwardMovementMultiplier;
        }
        else if (_body.velocity.y < 0)
        {
            _body.gravityScale = downwardMovementMultiplier;
        }
        else if(_body.velocity.y == 0)
        {
            _body.gravityScale = _defaultGravityScale;
        }

        _body.velocity = _velocity;
    }
    
    private void JumpAction()
    {
        if (_onGround || _jumpPhase < maxAirJumps)
        {
            _jumpPhase += 1;
                
            _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
                
            if (_velocity.y > 0f)
            {
                _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
            }
            else if (_velocity.y < 0f)
            {
                _jumpSpeed += Mathf.Abs(_body.velocity.y);
            }
            _velocity.y += _jumpSpeed;
        }
    }
}
