using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum FireMode { Auto, Burst,Single};
    public FireMode fireMode;

    public Transform[] projectileSpawn;
    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 100;
    public float muzzleVelocity = 35;
    public int burstCount;

    public Transform shell;
    public Transform shellEjection;

    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;

    float nextShootTime;

    void Start() {
        shotsRemainingInBurst = burstCount;
    }

    void Shoot() {
        if (Time.time > nextShootTime) {
            if (fireMode == FireMode.Burst) {
                if (shotsRemainingInBurst == 0) {
                    return;
                }
                shotsRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single) {
                if (!triggerReleasedSinceLastShot) {
                    return;
                }
            }

            for (int i = 0; i < projectileSpawn.Length; i++) {
                nextShootTime = Time.time + msBetweenShots / 1000;
                Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                newProjectile.SetSpeed(muzzleVelocity);
            }
            Instantiate(shell, shellEjection.position, shellEjection.rotation);
        }
    }


    public void OnTriggerHold() {
        Shoot();
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease() {
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }
}
