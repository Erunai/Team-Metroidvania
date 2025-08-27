using UnityEngine;

public class InputManager : MonoBehaviour
{
    // To be archived



    public static System.Action OnDashPressed;
    public static System.Action OnJumpPressed;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpPressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) // Assuming LeftShift is used for dashing -- could add right shift too ?
        {
            OnDashPressed?.Invoke();
        }
    }
}
