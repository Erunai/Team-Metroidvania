using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float yOffset = 5f;
    [SerializeField] float yMin;
    [SerializeField] float yMax;
    [SerializeField] float xMin;
    [SerializeField] float xMax;

    private float _yPos;
    private float _xPos;

    const float zOffset = -10f;
    // Update is called once per frame
    void Update()
    {
        /*
         * TODO:
         *      Make the camera lag behind the player to a maximum offset
         *      i.e. Ease the camera in and out of the movement -- speed the camera's movespeed up the further away it is from the player
         */
        _yPos = Mathf.Clamp(player.position.y + yOffset, yMin, yMax);
        _xPos = Mathf.Clamp(player.position.x, xMin, xMax);

        transform.position = new Vector3(_xPos, _yPos, zOffset);
    }
}
