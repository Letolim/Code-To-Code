using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    public Player playerScript;
    public GameWorld world;
    public ProjectileBuffer projectileBuffer;
    private List<ProjectileUpgrade> upgrades = new List<ProjectileUpgrade>();

    public bool addUpgrade = false;

    // Start is called before the first frame update
    void Start()
    {
        upgrades.Add(new ProjectileUpgrade(0, 0, 1, 0, 10));            //count              0 > 10
        upgrades.Add(new ProjectileUpgrade(0, 1, 0, 1, 9));             //splash radius      1 > 10
        upgrades.Add(new ProjectileUpgrade(0, 2, 0, .025f, 100));       //delay              3.5 > 1

        upgrades.Add(new ProjectileUpgrade(1, 1, 0, 0, 12));            //max targets        3 > 15
        upgrades.Add(new ProjectileUpgrade(1, 3, 0, .025f, 50));        //delay              2.5 > 1.25
        upgrades.Add(new ProjectileUpgrade(1, 2, 1, 0, 20));            //max distance       10 > 30
        upgrades.Add(new ProjectileUpgrade(1, 0, 1, 0, 1));             //count              0 > 1

        upgrades.Add(new ProjectileUpgrade(2, 0, 1, 0, 1));             //count              0 > 1
        upgrades.Add(new ProjectileUpgrade(2, 1, 0, .5f, 30));          //max distance       5 > 20
        upgrades.Add(new ProjectileUpgrade(2, 2, 0, .25f, 40));         //speed              5 > 15

        upgrades.Add(new ProjectileUpgrade(10, 0, 1, 0, 5));            //count              0 > 5
        upgrades.Add(new ProjectileUpgrade(10, 1, 0, .25f, 12));        //radius             1 > 4      
        upgrades.Add(new ProjectileUpgrade(10, 2, 1, 0, 7));            //bounce count       3 > 10

        upgrades.Add(new ProjectileUpgrade(-1, 0, 0, .25f, 10));        //speed              7.5 > 10
        upgrades.Add(new ProjectileUpgrade(-2, 0, 0, -.25f, 12));       //shield delay       6 > 2

        //Respawn(false);
    }

    private void Update()
    {
        if (addUpgrade)
        {
            ApplyUpdate();
        }

    }

    public void ApplyUpdate()
    {
        if (upgrades.Count == 0)
            return;

        int upgrade = Random.Range(0, upgrades.Count);

        if (upgrades[upgrade].completed)
        {
            upgrades.RemoveAt(upgrade);
            ApplyUpdate();
            return;
        }

        upgrades[upgrade].Increase();

        if (upgrades[upgrade].type >= 0)
            projectileBuffer.UpdateProjectile(upgrades[upgrade].type, upgrades[upgrade].value01, upgrades[upgrade].value02, upgrades[upgrade].updateType);
        else
            playerScript.UpdatePlayer(upgrades[upgrade].type, upgrades[upgrade].value01, upgrades[upgrade].value02);

    }


    public class ProjectileUpgrade
    {
        public int type;
        public int updateType;
        public int value01;
        public float value02;
        public int limit;
        private int currentCount = 0;
        public bool completed = false;

        public ProjectileUpgrade(int type, int updateType, int value01, float value02, int limit)
        {
            this.type = type;
            this.updateType = updateType;
            this.value01 = value01;
            this.value02 = value02;
            this.limit = limit;
        }

        public void Increase()
        {
            currentCount++;
            if (currentCount == limit)
                completed = true;
        }
    }
}
