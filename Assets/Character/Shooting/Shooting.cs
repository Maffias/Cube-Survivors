﻿using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject SpikeBall;
    public GameObject SpikeBallv2;
    public GameObject MagicMissile;
    public GameObject SNDMagicMissile;
    [Tooltip("Prędkość pocisków super umiejętności")] public float ultSpeed;
    [Tooltip("Czas odnowienia super umiejętności po użyciu")] public float ultCd;
    [Tooltip("Prędkość zwykłych pocisków")] public float shootingSpeed;
    [Tooltip("Odległość pojawianai pocisków od gracza")] public float spawnDistance;
    [Tooltip("Poziom gracza")] public int level;
    public bool isFire;
    public bool isFrost;
    public int points = 0;
    public int pointsToRefund = 0;
    public int pointCounter = 0;
    private float lastUltTime;
    private bool isUltActive = false;
    private float SNDlastUltTime;
    private bool SNDisUltActive = false;

    //pomocnicze
    public int ballLvl = 0;
    public int speed = 0;
    public int frost = 0;
    public int fire = 0;
    public int finisher = 0;
    public int dash = 0;


    /*Tablica wektorów kierunku pocisków super umiejętności*/
    private readonly Vector3[] shootingDirections = {
        new Vector3(0, 0, 1),
        new Vector3(-1, 0, 1),
        new Vector3(-1, 0, 0),
        new Vector3(-1, 0, -1),
        new Vector3(0, 0, -1),
        new Vector3(1, 0, -1),
        new Vector3(1, 0, 0),
        new Vector3(1, 0, 1)
    };

    void Start()
    {
        lastUltTime = -ultCd; //Możliwość natychmiastowego użycia super umiejętności po rozpoczęciu gry
        StartCoroutine(GenerateCapsuleWithDelayCoroutine());
        StartCoroutine(ShootInAllDirectionsCoroutine());
    }

    /*Funkcja odpowiedzialna za super umiejętność*/
    IEnumerator ShootInAllDirectionsCoroutine()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.R) && Time.time - lastUltTime >= ultCd && isUltActive)
            {
                bool reverseOrder = Random.Range(0, 2) == 1;
                Vector3[] directionsToUse = reverseOrder ? ReverseArray(shootingDirections) : shootingDirections;

                foreach (Vector3 direction in directionsToUse)
                {
                    GenerateProjectile(direction);
                    yield return new WaitForSeconds(0.1f);
                }
                lastUltTime = Time.time;
            }
            yield return null;
            if (Input.GetKeyDown(KeyCode.Keypad4) && Time.time - SNDlastUltTime >= ultCd && SNDisUltActive)
            {
                bool reverseOrder = Random.Range(0, 2) == 1;
                Vector3[] directionsToUse = reverseOrder ? ReverseArray(shootingDirections) : shootingDirections;

                foreach (Vector3 direction in directionsToUse)
                {
                    GenerateSNDProjectile(direction);
                    yield return new WaitForSeconds(0.1f);
                }
                SNDlastUltTime = Time.time;
            }
            yield return null;
        }
    }

    public void ActivateShootInAllDirections(bool activate)
    {
        isUltActive = activate;
        SNDisUltActive = activate;
}

    /*Funkcja generująca pojedynczy pocisk super umiejętności*/
    void GenerateProjectile(Vector3 direction)
    {
        Player_Movement playerMovement = FindObjectOfType<Player_Movement>();
        GameObject projectile;

        if (playerMovement != null)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            Vector3 projectileSpawnPosition = playerMovement.transform.position + direction * spawnDistance + new Vector3(0, 1.5f, 0);

            projectile = Instantiate(MagicMissile, projectileSpawnPosition, rotation);
            projectile.SetActive(true);

            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            projectileRigidbody.velocity = direction * ultSpeed;
            projectileRigidbody.useGravity = false;
        }
    }

    /*Funkcja generująca pojedynczy pocisk super umiejętności*/
    void GenerateSNDProjectile(Vector3 direction)
    {
        Second_Movement playerMovement = FindObjectOfType<Second_Movement>();
        GameObject projectile;

        if (playerMovement != null)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            Vector3 projectileSpawnPosition = playerMovement.transform.position + direction * spawnDistance + new Vector3(0, 1.5f, 0);

            projectile = Instantiate(SNDMagicMissile, projectileSpawnPosition, rotation);
            projectile.SetActive(true);

            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            projectileRigidbody.velocity = direction * ultSpeed;
            projectileRigidbody.useGravity = false;
        }
    }

    /*Funkcja odwracająca tablicę*/
    Vector3[] ReverseArray(Vector3[] array)
    {
        Vector3[] reversedArray = new Vector3[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            reversedArray[i] = array[array.Length - 1 - i];
        }
        return reversedArray;
    }

    IEnumerator GenerateCapsuleWithDelayCoroutine()
    {
        // Znajdź obiekt PlayerHealth
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

        while (playerHealth != null && playerHealth.isAlive)
        {
            GenerateCapsuleWithDelay();
            yield return new WaitForSeconds(0.5f); // Opoznienie co pół sekundy
        }
    }

    public void GenerateCapsuleWithDelay()
    {
        // Znajdź obiekt Player_Movement
        Player_Movement playerMovement = FindObjectOfType<Player_Movement>();
        Second_Movement secondMovement = FindObjectOfType<Second_Movement>();
        GameObject ball;
        GameObject SNDball;

        if (playerMovement != null)
        {
            Vector3 capsuleSpawnPosition = playerMovement.transform.position +
                playerMovement.transform.forward * spawnDistance + new Vector3(0, 1.5f, 0);

            Vector3 SNDcapsuleSpawnPosition = secondMovement.transform.position +
                secondMovement.transform.forward * spawnDistance + new Vector3(0, 1.5f, 0);

            if (level < 3)
            {
                ball = Instantiate(SpikeBall, capsuleSpawnPosition, playerMovement.transform.rotation);
                SNDball = Instantiate(SpikeBall, SNDcapsuleSpawnPosition, secondMovement.transform.rotation);
            }
            else
            {
                ball = Instantiate(SpikeBallv2, capsuleSpawnPosition, playerMovement.transform.rotation);
                SNDball = Instantiate(SpikeBallv2, SNDcapsuleSpawnPosition, secondMovement.transform.rotation);
            }
                

            ball.SetActive(true);
            SNDball.SetActive(true);

            if (isFire || isFrost)
            {
                Light lightComponent = ball.GetComponent<Light>();
                Light SNDlightComponent = SNDball.GetComponent<Light>();
                if (lightComponent == null)
                {
                    lightComponent = ball.AddComponent<Light>();
                    SNDlightComponent = SNDball.AddComponent<Light>();
                }

                lightComponent.type = LightType.Point;
                lightComponent.intensity = 16f;
                SNDlightComponent.type = LightType.Point;
                SNDlightComponent.intensity = 16f;

                if (isFire)
                {
                    lightComponent.color = Color.red;
                    SNDlightComponent.color = Color.red;
                }
                    
                if (isFrost)
                {
                    lightComponent.color = Color.blue;
                    SNDlightComponent.color = Color.blue;
                }
                    
            }

            Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
            Rigidbody SNDballRigidbody = SNDball.GetComponent<Rigidbody>();
            ballRigidbody.velocity = playerMovement.transform.forward * shootingSpeed;
            SNDballRigidbody.velocity = secondMovement.transform.forward * shootingSpeed;
            ballRigidbody.useGravity = false; 
            SNDballRigidbody.useGravity = false;
        }
    }
}
