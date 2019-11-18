using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
    public Transform mParticleSystem;

    private MeshRenderer mMeshrenderer;

    private void Awake()
    {
        mMeshrenderer = GetComponent<MeshRenderer>();
    }

    void OnTriggerStay(Collider other)
    {
        UpdateStream(GetHeight(other));    
    }

    void OnTriggerExit(Collider other)
    {
        UpdateStream(0);
    }

    private float GetHeight(Collider collider)
    {
        return collider.transform.position.y + collider.bounds.size.y;
    }

    private void UpdateStream(float newHeight)
    {
        // Particle
        Vector3 newPosition = new Vector3(transform.position.x, newHeight, transform.position.z);
        mParticleSystem.position = newPosition;
        //Height cutoff
        newHeight /= transform.localScale.y;
        mMeshrenderer.material.SetFloat("_Cutoff", newHeight);
    } 
}
