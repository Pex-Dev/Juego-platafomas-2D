using UnityEngine;

public class CameraControl : MonoBehaviour
{
    
    public Transform target;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (!target)
        {
            Debug.Log("No hay un objetivo asignado para la cámara");
            return;
        }
        transform.position = new Vector3(target.position.x,target.position.y,-10);
    }
}
