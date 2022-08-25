using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float fireRate = 0;  //zero value means this weapon has single-burst firing
    public int Damage = 10;
    public LayerMask whatToHit;  //what we want the raycast to hit
    
    public Transform BulletTrailPrefab;
    public Transform HitPrefab;
    public Transform MuzzleFlashPrefab;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    //Handle camera shaking
    public float cameraShakeAmount = 0.05f;
    public float cameraShakeLength = 0.1f;
    CameraShake cameraShake;

    public string weaponShootingSound = "DefaultShot";


    float timeToFire = 0;
    Transform firePoint;

    //Caching sounds
    AudioManager audioManager;


    //Is used for initialization
    void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("Weapon: No FirePoint found!");
        }
    }


    void Start()
    {
        cameraShake = GameMaster.gm.GetComponent<CameraShake>();
        if (cameraShake == null)
        {
            Debug.LogError("Weapon: No CameraShake script found on GM object!");
        }

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("Weapon: No AudioManager found in the scene!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        //Shoot();

        if (fireRate == 0)  //single burst weapon
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else    //automatic weapon
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1/fireRate;  //when it's time to fire again we need the time and thr delay of the next shot
                Shoot();
            }
        }

    }


    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, (mousePosition - firePointPosition), 100, whatToHit);
        
        
        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition)*100, Color.cyan);
        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Debug.Log("We hit " + hit.collider.name + " and did " + Damage + " damage.");

            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(Damage);
            }
        }
        
        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPosition;
            Vector3 hitNormal;

            if (hit.collider == null)  //if we don't hit anything, just continue into space
            {
                hitPosition = (mousePosition - firePointPosition)*100;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPosition = hit.point;
                hitNormal = hit.normal;
            }

            Effect(hitPosition, hitNormal);
            timeToSpawnEffect = Time.time + 1/effectSpawnRate;
        }

    }


    void Effect(Vector3 hitPosition, Vector3 hitNormal)
    {
        Transform trail = Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if (lr != null)
        {
            //Set positions
            lr.SetPosition(0, firePoint.position);  //this way it will look like its appearing from owr gun!
            lr.SetPosition(1, hitPosition);
        }

        Destroy(trail.gameObject, 0.05f);

        if (hitNormal != new Vector3(9999, 9999, 9999))
        {
            Transform hitParticle = Instantiate(HitPrefab, hitPosition, Quaternion.FromToRotation(Vector3.right, hitNormal)) as Transform;
            
            Destroy(hitParticle.gameObject, 1f);
        }
        

        Transform clone = Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3 (size, size, size);
        Destroy(clone.gameObject, 0.02f);

        //Shake the camera
        cameraShake.Shake(cameraShakeAmount, cameraShakeLength);

        //Play shooting sound
        audioManager.PlaySound(weaponShootingSound);
    }

}
