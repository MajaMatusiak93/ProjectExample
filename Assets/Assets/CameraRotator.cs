using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField]
    private float speed;

    public void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * speed, Space.World);
    }
}