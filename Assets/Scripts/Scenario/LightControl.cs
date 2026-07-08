using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightControl : MonoBehaviour
{
    #region Variables
    [SerializeField] private Light2D topLight;
    [SerializeField] private Light2D centerLight;
    [SerializeField] private Light2D bottomLight;

    private Camera cam;

    #endregion

    void Awake()
    {
        cam = Camera.main;
        LightAdjustOnCamera();
        IntroLayout();
    }

    private void LightAdjustOnCamera()
    {
        topLight.pointLightOuterRadius = cam.orthographicSize * .7f;
        centerLight.pointLightOuterRadius = cam.orthographicSize * .7f;
        bottomLight.pointLightOuterRadius = cam.orthographicSize * .7f;

        topLight.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + cam.orthographicSize, 0);
        centerLight.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0);
        bottomLight.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y - cam.orthographicSize, 0);
    }

    public void IntroLayout()
    {
        topLight.enabled = true;
        centerLight.enabled = false;
        bottomLight.enabled = true;
    }

    public void DefaultLayout()
    {
        topLight.enabled = true;
        centerLight.enabled = false;
        bottomLight.enabled = false;
    }

    public void CountdownLayout()
    {
        topLight.enabled = false;
        centerLight.enabled = true;
        bottomLight.enabled = false;
    }

}
