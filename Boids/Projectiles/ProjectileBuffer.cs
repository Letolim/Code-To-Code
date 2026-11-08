using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBuffer : MonoBehaviour
{
    public GameObject fireBall;
    public int fireBallCount = 1;

    public GameObject chainLightning;
    public int chainLightningCount = 1;

    public GameObject discProjectile;
    public GameObject discExplosion;
    public int discProjectileCount = 1;

    public GameObject orb;
    public int orbCount = 0;

    public ChainLightning orbScript;
    public GameObject orbExplosion;
    public ChainLightning orbExplosionScript;

    public GameObject creepExplosion; 
    public GameObject creepDeath;
    public GameObject floorTrapBeam;
    public GameObject creepBombProjectile;
    public GameObject creepBomb;

    public GameObject teleportFX;

    private List<List<Projectile>> projectileBuffer;
    private List<float> t;
    // Start is called before the first frame update
    void Start()
    {
        projectileBuffer = new List<List<Projectile>>();
        t = new List<float>();

        for (int i = 0; i < 12; i++)
        {
            projectileBuffer.Add(new List<Projectile>());
            t.Add(0);
        }

      
    }
    

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < t.Count; i++)
        {
            t[i] += Time.deltaTime;

            if (i == 0 && t[i] > fireBall.GetComponent<Fireball>().delay)
            {
                t[i] = 0;
                for (int n = 0; n < fireBallCount; n++)
                    SpawnNewProjectile(i, n);
            }

            if (i == 1 && t[i] > chainLightning.GetComponent<ChainLightning>().delay)
            {
                t[i] = 0;
                for (int n = 0; n < chainLightningCount; n++)
                    SpawnNewProjectile(i, n);
            }

            if (i == 10 && t[i] > discProjectile.GetComponent<DiscProjectile>().delay)
            {
                t[i] = 0;
                for (int n = 0; n < discProjectileCount; n++)
                    SpawnNewProjectile(i, n);
            }
        }
    }


    public void UpdateProjectile(int type, int value1, float value2,int upgradeType)
    {
        if (type == 0)
        {
            if(upgradeType == 0)
                fireBallCount += value1;

            if (upgradeType == 1)
            {
                fireBall.GetComponent<Fireball>().splashRadius += value2;
                ParticleSystem.ShapeModule mainShape = fireBall.GetComponent<Fireball>().pSystemExplosion.shape;
                mainShape.radius = fireBall.GetComponent<Fireball>().splashRadius * .3f;

                for (int i = 0; i < projectileBuffer[type].Count; i++)
                {
                    projectileBuffer[type][i].GetComponent<Fireball>().splashRadius += value2;
                    ParticleSystem.ShapeModule shape = projectileBuffer[type][i].GetComponent<Fireball>().pSystemExplosion.shape;
                    shape.radius = projectileBuffer[type][i].GetComponent<Fireball>().splashRadius * .3f;
                }
            }

            if (upgradeType == 2)
            {
                fireBall.GetComponent<Fireball>().delay -= value2;

                for (int i = 0; i < projectileBuffer[type].Count; i++)
                    projectileBuffer[type][i].GetComponent<Fireball>().delay -= value2;
            }
        }

        if (type == 1)
        {
            if (upgradeType == 0)
                chainLightningCount += value1;

            if (upgradeType == 1)
            {
                chainLightning.GetComponent<ChainLightning>().maxTargets += value1;

                for (int i = 0; i < projectileBuffer[type].Count; i++)
                    projectileBuffer[type][i].GetComponent<ChainLightning>().maxTargets += value1;
            }

            if (upgradeType == 2)
            {
                chainLightning.GetComponent<ChainLightning>().maxDistance += value2;

                for (int i = 0; i < projectileBuffer[type].Count; i++)
                    projectileBuffer[type][i].GetComponent<ChainLightning>().maxDistance += value2;
            }

            if (upgradeType == 3)
            {
                chainLightning.GetComponent<ChainLightning>().delay -= value2;

                for (int i = 0; i < projectileBuffer[type].Count; i++)
                    projectileBuffer[type][i].GetComponent<ChainLightning>().delay -= value2;
            }
        }

        if (type == 2)
        {
            if (upgradeType == 0 && !orb.active)
            {
                orbCount = 1;
                orb.SetActive(true);
                orb.GetComponent<Orb>().InitiateProjectile(0);
            }
            
            if (upgradeType == 1)
                orb.GetComponent<Orb>().maxDistance += value2;

            if (upgradeType == 2)
                orb.GetComponent<Orb>().speed += value2;
        }

        if (type == 10)
        {
            if (upgradeType == 0)
                discProjectileCount += value1;

            if (upgradeType == 1)
            {
                discProjectile.GetComponent<DiscProjectile>().radius += value2;

                for (int i = 0; i < projectileBuffer[type].Count; i++)
                    projectileBuffer[type][i].GetComponent<DiscProjectile>().radius += value2;
            }

            if (upgradeType == 2)
            {
                discProjectile.GetComponent<DiscProjectile>().bounceCount += value1;

                for (int i = 0; i < projectileBuffer[type].Count; i++)
                    projectileBuffer[type][i].GetComponent<DiscProjectile>().bounceCount += value1;
            }
        }
    }

    public void SpawnNewProjectile(int type,int number)
    {
        for (int i = 0; i < projectileBuffer[type].Count; i++)
            if (!projectileBuffer[type][i].isActiveAndEnabled)
            {
                projectileBuffer[type][i].InitiateProjectile(number);
                return;
            }

        if (type == 0)
        {
            GameObject projectile = GameObject.Instantiate(fireBall);
            projectile.GetComponent<Fireball>().InitiateProjectile(number);
            projectileBuffer[type].Add(projectile.GetComponent<Fireball>());
            return;
        }
        if (type == 1)
        {
            GameObject projectile = GameObject.Instantiate(chainLightning);
            projectile.GetComponent<ChainLightning>().InitiateProjectile(number);
            projectileBuffer[type].Add(projectile.GetComponent<ChainLightning>());
            return;
        }
        if (type == 10)
        {
            GameObject projectile = GameObject.Instantiate(discProjectile);
            projectile.GetComponent<DiscProjectile>().InitiateProjectile(number);
            projectileBuffer[type].Add(projectile.GetComponent<DiscProjectile>());
            return;
        }
    }

    public void SpawnNewProjectile(int type, GameObject origin)
    {
        for (int i = 0; i < projectileBuffer[type].Count; i++)
            if (!projectileBuffer[type][i].isActiveAndEnabled)
            {
                projectileBuffer[type][i].InitiateProjectile(origin);
                return;
            }

        if (type == 2)
        {
            GameObject projectile = GameObject.Instantiate(creepExplosion);
            projectile.GetComponent<CreepExplosion>().InitiateProjectile(origin);
            projectileBuffer[type].Add(projectile.GetComponent<CreepExplosion>());
            return;
        }

        if (type == 3)
        {
            GameObject projectile = GameObject.Instantiate(creepDeath);
            projectile.GetComponent<CreepDeath>().InitiateProjectile(origin);
            projectileBuffer[type].Add(projectile.GetComponent<CreepDeath>());
            return;
        }

        if (type == 5)
        {
            GameObject projectile = GameObject.Instantiate(orbExplosion);
            projectile.GetComponent<OrbExplosion>().InitiateProjectile(origin);
            projectileBuffer[type].Add(projectile.GetComponent<OrbExplosion>());
            return;
        }

        if (type == 11)
        {
            GameObject projectile = GameObject.Instantiate(discExplosion);
            projectile.GetComponent<DiscProjectileExplosion>().InitiateProjectile(origin);
            projectileBuffer[type].Add(projectile.GetComponent<DiscProjectileExplosion>());
            return;
        }
    }

    public void SpawnNewProjectile(int type, Vector3 origin)
    {
        for (int i = 0; i < projectileBuffer[type].Count; i++)
            if (!projectileBuffer[type][i].isActiveAndEnabled)
            {
                projectileBuffer[type][i].InitiateProjectile(origin);
                return;
            }

        if (type == 7)
        {
            GameObject projectile = GameObject.Instantiate(creepBombProjectile);
            projectile.GetComponent<CreepBombProjectile>().InitiateProjectile(origin);
            projectileBuffer[type].Add(projectile.GetComponent<CreepBombProjectile>());
            return;
        }
        if (type == 8)
        {
            GameObject projectile = GameObject.Instantiate(creepBomb);
            projectile.GetComponent<CreepBomb>().InitiateProjectile(origin);
            projectileBuffer[type].Add(projectile.GetComponent<CreepBomb>());
            return;
        }
        if (type == 9)
        {
            GameObject projectile = GameObject.Instantiate(teleportFX);
            projectile.GetComponent<TeleportFX>().InitiateProjectile(origin);
            projectileBuffer[type].Add(projectile.GetComponent<TeleportFX>());
            return;
        }
    }

    public void SpawnNewProjectile(int type, Vector4 origin)
    {
        for (int i = 0; i < projectileBuffer[type].Count; i++)
            if (!projectileBuffer[type][i].isActiveAndEnabled)
            {
                projectileBuffer[type][i].InitiateProjectile(origin);
                return;
            }

        if (type == 6)
        {
            GameObject projectile = GameObject.Instantiate(floorTrapBeam);
            projectile.GetComponent<FloorTrapBeam>().InitiateProjectile(origin);
            projectileBuffer[type].Add(projectile.GetComponent<FloorTrapBeam>());
            return;
        }
    }



    private void AddProjectile(int type,int number)
    {
        for (int i = 0; i < projectileBuffer[type].Count; i++)
        {
            if (!projectileBuffer[type][i].isActiveAndEnabled)
            {
                projectileBuffer[type][i].InitiateProjectile(number);
                return;
            }
        }

        if (type == 0)
        {
            GameObject projectile = GameObject.Instantiate(fireBall);
            projectile.GetComponent<Fireball>().InitiateProjectile(number);
            projectileBuffer[type].Add(projectile.GetComponent<Fireball>());
            return;
        }


        if (type == 1)
        {
            GameObject projectile = GameObject.Instantiate(chainLightning);
            projectile.GetComponent<ChainLightning>().InitiateProjectile(number);
            projectileBuffer[type].Add(projectile.GetComponent<ChainLightning>());
            return;
        }
    }




}
