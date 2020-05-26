using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manager : MonoBehaviour
{
    //0 = home 1 = fight 2 = train 3 = stats
    public List<Sprite> backgrounds;
    public Image background;
    public Text monstername;
    public Text stats;
    public Animon currentAnimon;
    public static manager instance = null;
    public List<ButtonActions> buttonList;
    public Image punchbag;
    public EnemyMon enemy;
    public float yourDelay;
    public float enemyDelay;
    public float globalDelay;
    public float tick;
    public bool isAttack;
    public bool inFight;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        tick += Time.deltaTime;
        if(inFight&tick > 2f)
        {
            tick = 0;
            yourDelay -= Mathf.Max(currentAnimon.agi+currentAnimon.selectedAttack.speedmod,10);
            enemyDelay -= Mathf.Max(enemy.agi+enemy.selectedAttack.speedmod,10);
            foreach(AdditionalEffect i in currentAnimon.bufflist)
            {

            }
            if(yourDelay <= 0)
            {
                int dmg = currentAnimon.onAttack();
                if(dmg >0)
                    enemy.onDamaged(dmg);
                foreach(AdditionalEffect i in currentAnimon.selectedAttack.ae)
                {
                    if (i.self)
                    {
                        i.onGain(currentAnimon);
                    }
                    else
                        i.onGain(enemy);
                    
                }
                yourDelay = globalDelay;
                return;
            }
            if(enemyDelay <= 0)
            {
                int dmg = enemy.onAttack();
                currentAnimon.onDamaged(dmg);
                foreach(AdditionalEffect i in enemy.selectedAttack.ae)
                {
                    i.onGain(currentAnimon);
                }
                enemyDelay = globalDelay;
                return;
            }
        }

    }

    public void onStartBattle()
    {
        inFight = true;
        GameObject temp = (GameObject)Instantiate(Resources.Load("EnemyMon/EnemyMon" + Random.RandomRange(1, 2)));
        temp.transform.SetParent(currentAnimon.transform.parent);
        temp.transform.localPosition = new Vector2(-221, 15);
        enemy = temp.GetComponent<EnemyMon>();
        globalDelay = Mathf.Min (currentAnimon.agi, enemy.agi) * 3;
        yourDelay = globalDelay;
        enemyDelay = globalDelay;
        foreach(Attack i in currentAnimon.attackList)
        {
            i.gameObject.SetActive(false);
        }
        enemy.selectedAttack = enemy.attackList[Random.RandomRange(0, enemy.attackList.Count)];
        currentAnimon.selectedAttack = currentAnimon.attackList[Random.RandomRange(0, currentAnimon.attackList.Count)];
        currentAnimon.anim.SetTrigger("inFight");
        
    }
}
