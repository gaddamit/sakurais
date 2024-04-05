using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    [SerializeField]
    private GameObject _crossHair;
    [SerializeField]
    private GameObject _cameraDefault;
    [SerializeField]
    private GameObject _cameraAiming;
    private bool _adjustCamera = false;
    private bool _isAiming = false;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    // Switch to aiming camera
    public void StartAiming()
    {
        _isAiming = true;
    }

    // Switch to default camera
    public void StopAiming()
    {
        _isAiming = false;
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
