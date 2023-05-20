using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AttentionRedirectController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject indicator;
    public Canvas canvas;

    private GameObject ind;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Find closest key
        var pos = transform.position;
        var dist = Mathf.Infinity;
        GameObject key_obj = null;

        if (!gameManager.keyFound)
        {
            foreach (var key in gameManager.Keys)
            {
                if (dist > (key.transform.position - pos).magnitude)
                {
                    dist = (key.transform.position - pos).magnitude;
                    key_obj = key;
                }
            }
        }
        else
        {
            key_obj = gameManager.exit;
        }

        // Create the indicator
        if(key_obj != null)
        {
            float width = Screen.width;
            float height = Screen.height;

            /*
             * 
             * Rotational computation of the indicator, tested for spherical VR view
             * 
             * 
            Vector3 cam_pos = Camera.main.transform.InverseTransformPoint(key_obj.transform.position);
            Vector3 cam_rot = Quaternion.LookRotation(cam_pos - Camera.main.transform.localPosition).eulerAngles;

            var vFov = Camera.main.fieldOfView;
            var hFov = Camera.VerticalToHorizontalFieldOfView(vFov, Camera.main.aspect);

            var x_angle = cam_rot.y;
            if(x_angle > 180)
            {
                x_angle = -360 + x_angle;
            }
            x_angle = -x_angle;

            var y_angle = cam_rot.x;
            if (y_angle > 180)
            {
                y_angle = -360 + y_angle;
            }

            Debug.Log(x_angle + " , " + y_angle);

            if(Mathf.Abs(x_angle) > hFov / 2 || Mathf.Abs(y_angle) > vFov / 2)
            {
                if (ind == null)
                {
                    ind = Instantiate(indicator, canvas.transform);
                }

                if (ind != null)
                {
                    var x = -(x_angle) / (hFov / 2);
                    var y = -y_angle / (vFov / 2);

                    //Debug.Log(" X: " + x + " Y: " + y);

                    if (Mathf.Abs(x) > 1)
                        x = Mathf.Sign(x);

                    if (Mathf.Abs(y) > 1)
                        y = Mathf.Sign(y);

                    ind.transform.localPosition = new Vector3(x * (width / 5 - ind.GetComponent<Image>().rectTransform.rect.width / 2), y * (height / 2 - ind.GetComponent<Image>().rectTransform.rect.height / 2), 0);

                    Vector3 rotation = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, new Vector2(x, y)));
                    ind.transform.localEulerAngles = rotation;

                }
            }*/

            // Compute from screen information, seems to work in both PC and VR
            Vector3 screenPos = Camera.main.WorldToScreenPoint(key_obj.transform.position);
            if(screenPos.z < 0 || screenPos.x < 0 || screenPos.x > width || screenPos.y < 0 || screenPos.y > height)
            {
                if (ind == null)
                {
                    ind = Instantiate(indicator, canvas.transform);
                }

                if (ind != null)
                {
                    var s = 1;
                    if(screenPos.z < 0)
                    {
                        screenPos *= -1;
                        s = -1;
                    }
                    
                    screenPos.x -= s * width / 2;
                    screenPos.y -= height / 2;
                    
                    var x = (screenPos.x) / (width / 2);
                    var y = screenPos.y / (height / 2);
                    
                    //Debug.Log(screenPos + " X: " + x + " Y: " + y);

                    if (Mathf.Abs(x) > 1)
                        x = Mathf.Sign(x);

                    if (Mathf.Abs(y) > 1)
                        y = Mathf.Sign(y);

                    ind.transform.localPosition = new Vector3(x * (width / 5 - ind.GetComponent<Image>().rectTransform.rect.width / 2), y * (height / 2 - ind.GetComponent<Image>().rectTransform.rect.height / 2), 0);

                    Vector3 rotation = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, new Vector2(x, y)));
                    ind.transform.localEulerAngles = rotation;

                }
            }
            else
            {
                Destroy(ind);
            }
        }
    }
}
