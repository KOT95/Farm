using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float speedSickle;
    [SerializeField] private float rotationSpeed;
    [Space]
    [SerializeField] private GameObject toolObj;
    [SerializeField] private Transform rayCast;
    
    public float SpeedPlayer { get; private set; }

    private Animator _anim;
    private Rigidbody _rb;
    private bool _isField;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        CheckField();
        MoveAndRotate();
    }

    private void MoveAndRotate()
    {
        Vector3 directionVector = new Vector3(_playerInput.Horizontal, 0, _playerInput.Vertical);
        if(directionVector.magnitude > Mathf.Abs(0.05f))
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionVector), Time.deltaTime * rotationSpeed);

        _anim.SetFloat("speed", Vector3.ClampMagnitude(directionVector, 1).magnitude);
        SpeedPlayer = _isField ? speedSickle : speed;
        _rb.velocity = Vector3.ClampMagnitude(directionVector, 1) * SpeedPlayer;
        _rb.angularVelocity = Vector3.zero;
    }

    private void CheckField()
    {
        Ray ray = new Ray(rayCast.position, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 2))
            _isField = hit.collider.gameObject.tag == "Field";

        if (_isField)
        {
            toolObj.SetActive(true);
            _anim.SetBool("Sickle", true);
        }
        else
        {
            toolObj.SetActive(false);
            _anim.SetBool("Sickle", false);
        }
    }

    public void TrueTriggerTool() => toolObj.GetComponent<BoxCollider>().enabled = true;
    public void FalseTriggerTool() => toolObj.GetComponent<BoxCollider>().enabled = false;
}
