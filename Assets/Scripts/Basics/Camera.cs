using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Vector3 CameraPosition;
    public GameObject foco;
    public float VerticalFocus;
    public float HorizontalFocus;
    public float HorizontalLimitLeft;
    public float HorizontalLimitRight;
    public float VerticalLimitTop;
    public float VerticalLimitBottom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraPosition = new Vector3(foco.transform.position.x + VerticalFocus, foco.transform.position.y + HorizontalFocus, transform.position.z);
        
        if (CameraPosition.x >= HorizontalLimitRight) CameraPosition.x = HorizontalLimitRight;

        if (CameraPosition.x <= HorizontalLimitLeft) CameraPosition.x = HorizontalLimitLeft;

        if (CameraPosition.y >= VerticalLimitTop) CameraPosition.y = VerticalLimitTop;

        if (CameraPosition.y <= VerticalLimitBottom) CameraPosition.y = VerticalLimitBottom;

        transform.position = CameraPosition;
    }
}
