using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    [SerializeField]
    private GameObject _crossHair;
    [SerializeField]
    private GameObject _cameraDefault;
    [SerializeField]
    private GameObject _cameraAiming;
    [SerializeField]
    private float _aimDistance = 100f;
    [SerializeField]
    private bool _showAimRay = true;
    private bool _adjustCamera = false;
    private bool _isAiming = false;
    private Camera _camera;
    private CinemachineFreeLook _cmAiming;
    private Transform _originalLookAt;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _cmAiming = _cameraAiming.GetComponent<CinemachineFreeLook>();
        _originalLookAt = _cmAiming.m_LookAt;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isAiming && !_cameraAiming.activeInHierarchy)
        {
            _cameraDefault.SetActive(false);
            _cameraAiming.SetActive(true);

            StartCoroutine(ShowCrossHair(true));
        }
        else if(!_isAiming && !_cameraDefault.activeInHierarchy)
        {
            _cameraDefault.SetActive(true);
            _cameraAiming.SetActive(false);
        
            StartCoroutine(ShowCrossHair(false));
        }

        HandleAutoAim();
    }

    // Switch to aiming camera
    public void StartAiming()
    {
        _cameraAiming.transform.position = Camera.main.transform.position;
        _cameraAiming.transform.rotation = Camera.main.transform.rotation;
        _isAiming = true;
    }

    // Switch to default camera
    public void StopAiming()
    {
        _isAiming = false;
        _cmAiming.m_LookAt = _originalLookAt;
    }

    //TODO: Implement auto aim
    private void HandleAutoAim()
    {
        if(_isAiming && _cameraAiming.activeInHierarchy)
        {
            if(Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _aimDistance))
            {
                if(hit.collider.GetComponent<AutoAimTarget>() != null)
                {
                    AutoAimTarget target = hit.collider.GetComponent<AutoAimTarget>();
                    _adjustCamera = true;
                    Quaternion rotation = Quaternion.Lerp(_camera.transform.rotation, Quaternion.LookRotation(target.transform.position - _camera.transform.position), 1f);
                    _camera.transform.rotation = rotation;
                    Debug.Log("Aiming at " + target.name);
                }
            }
            if(_showAimRay)
            {
                Color rayColor = hit.collider != null ? Color.green : Color.red;
                Debug.DrawRay(_camera.transform.position, _camera.transform.forward * hit.distance, rayColor);
            }
        }
    }

    // Show crosshair after a delay
    IEnumerator ShowCrossHair(bool show = true)
    {
        yield return new WaitForSeconds(0.5f);
        _crossHair.SetActive(show);
    }

    private void LateUpdate()
    {
        if (_adjustCamera)
        {
            _adjustCamera = false;
        }
    }
}
