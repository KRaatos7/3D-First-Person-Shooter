using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pistol : MonoBehaviour
{
    RaycastHit hit;

    [SerializeField]
    float damageEnemy = 10f;

    [SerializeField]
    Transform shootPoint;

    public Text currentAmmoText;
    public Text carriedAmmoText;

    
    public int currentAmmo = 30;
    public int maxAmmo = 30;
    public int carriedAmmo = 90;
    bool isReloading;

    public ParticleSystem muzzleFlash;

    AudioSource gunAS;
    public AudioClip shootAC;

    [SerializeField]
    float rateofFire;
    float nextFire = 0;

    [SerializeField]
    float weaponRange;

    Animator anim;

    void Start()
    {
        muzzleFlash.Stop();
        gunAS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        UpdateAmmoUI();
    }

    void Update()
    {
        if(Input.GetButton("Fire1") && currentAmmo > 0)
        {
            Shoot();
        }
        else if (Input.GetButton("Fire1") && currentAmmo <= 0 && !isReloading)
        {
            DryFire();
        }
        if(Input.GetKeyDown(KeyCode.R) && currentAmmo <= maxAmmo && !isReloading)
        {
            isReloading = true;
            Reload();
        }
    }

    void Shoot()
    {
        if(Time.time > nextFire)
        {
            nextFire = 0f;
            nextFire = Time.time + rateofFire;
            anim.SetTrigger("Shoot");

            currentAmmo--;

            gunAS.PlayOneShot(shootAC);

            UpdateAmmoUI();

            if(Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponRange))
            {
                if(hit.transform.tag == "Enemy")
                {
                    EnemyHealth enemyHealthScript = hit.transform.GetComponent<EnemyHealth>();
                    enemyHealthScript.DeductHealth(damageEnemy);
                }
                else
                {
                    Debug.Log("Hit something else");
                }
            }
        }

    }

    void Reload()
    {

        if (carriedAmmo <= 0) return;
        anim.SetTrigger("Reload");
        StartCoroutine(ReloadCountdown(2f));
        
    }

    void UpdateAmmoUI()
    {
        currentAmmoText.text = currentAmmo.ToString();
        carriedAmmoText.text = carriedAmmo.ToString();
    }

    

    void DryFire()
    {
        if (Time.time > nextFire)
        {
            nextFire = 0f;
            nextFire = Time.time + rateofFire;
            Debug.Log("play dry fire sound");

        }
    }

    IEnumerator ReloadCountdown(float timer) 
    {
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        if(timer <= 0)
        {
            int bulletNeededToFillMag = maxAmmo - currentAmmo;
            int bulletsToDeduct = (carriedAmmo >= bulletNeededToFillMag) ? bulletNeededToFillMag : carriedAmmo;
            carriedAmmo -= bulletsToDeduct;
            currentAmmo += bulletsToDeduct;
            isReloading = false;
            UpdateAmmoUI();
        }
    }

    IEnumerator PistolMuzzleFlash()
    {
        muzzleFlash.Play();
        yield return new WaitForEndOfFrame();
        muzzleFlash.Stop();
    }
}
