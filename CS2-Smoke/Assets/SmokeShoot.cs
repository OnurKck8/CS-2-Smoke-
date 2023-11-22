using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeShoot : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private float threshold;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 rayPoint = Input.mousePosition;
            Ray ray = mainCam.ScreenPointToRay(rayPoint);

            if(Physics.Raycast(ray,out RaycastHit hit,hitLayer))
            {
                ShootParticle(hit.transform.GetComponent<ParticleSystem>(), rayPoint);
            }
        }
    }

    private void ShootParticle(ParticleSystem particleSys, Vector3 position)
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSys.main.maxParticles];
        int numParticles = particleSys.GetParticles(particles);

        Vector2 positinoViewportPoint = mainCam.ScreenToViewportPoint(position);

        for (int i = 0; i < numParticles; i++)
        {
            Vector3 wordPos = particleSys.transform.TransformPoint(particles[i].position);
            Vector2 viewPoint = mainCam.WorldToViewportPoint(wordPos);

            if(Vector2.Distance(positinoViewportPoint,viewPoint) <= threshold)
            {
                particles[i].remainingLifetime = 0f;
            }
        }

        particleSys.SetParticles(particles, numParticles);
    }
}
