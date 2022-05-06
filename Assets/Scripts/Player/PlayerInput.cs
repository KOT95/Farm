using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    private void Update()
    {
        Horizontal = joystick.Horizontal;
        Vertical = joystick.Vertical;
    }
}
