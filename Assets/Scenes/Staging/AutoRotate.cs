using UnityEngine;

public class AutoRotate : MonoBehaviour
{   
    void Update()
    {
        transform.eulerAngles = transform.eulerAngles - 40 * Vector3.forward * Time.deltaTime;
    }
}
