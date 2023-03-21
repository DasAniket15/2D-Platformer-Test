using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class trailSprite : MonoBehaviour
{
    
    [SerializeField] private Material trailMaterial1;
    [SerializeField] private Material trailMaterial2;
    [SerializeField] private TrailRenderer trailRenderer;
    public PlayerController playerController;


    



    void Update()
    {
        if (playerController.isFacingRight == true)
        {
            trailRenderer.material = trailMaterial1;
        }
        if (playerController.isFacingRight == false)
        {
            trailRenderer.material = trailMaterial2;
        }
    }
}
