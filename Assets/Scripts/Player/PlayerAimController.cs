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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Enable or disable crosshair
    public void SetCrosshairEnabled(bool show = true)
    {
        if(show)
        {
            _cameraDefault.SetActive(false);
            _cameraAiming.SetActive(true);
            StartCoroutine(ShowCrossHair());
        }
        else
        {
            _cameraDefault.SetActive(true);
            _cameraAiming.SetActive(false);
            StartCoroutine(ShowCrossHair(false));
        }
    }

    // Show crosshair after a delay
    IEnumerator ShowCrossHair(bool show = true)
    {
        yield return new WaitForSeconds(0.5f);
        _crossHair.SetActive(show);
    }
}
