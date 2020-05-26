using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMon : Animon
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void onDamaged(int dmg) {
        curhp -= dmg;
        anim.SetTrigger("isHit");
        if(curhp <= 0)
        {
            manager.instance.yourDelay = 100000f;
            manager.instance.enemyDelay = 100000f;
            StartCoroutine("die");
        }
    }

    IEnumerator die() {

        anim.SetTrigger("isDead");
        int tempgain = UnityEngine.Random.Range(4, 8);
        if ((int)manager.instance.currentAnimon.currentState > (int)currentState)
        {
            tempgain = UnityEngine.Random.Range(1, 4);
        }
        for (int i = 0; i < tempgain; i++)
            {
                int temp = UnityEngine.Random.Range(0, 4);
                switch (temp)
                {
                    case 0:
                        if (statpref == stats.HP)
                        {
                            manager.instance.currentAnimon.hp += 10;
                            manager.instance.currentAnimon.curhp += 10;
                        }
                        if (statpref == stats.Agi)
                            agi += 1;
                        if (statpref == stats.Str)
                            str += 1;
                        break;
                    case 1:
                        hp += 10;
                        curhp += 10;
                        break;
                    case 2:
                        str += 1;
                        break;
                    case 3:
                        agi += 1;
                        break;
                }
            }
        
        yield return new WaitForSeconds(1f);

        manager.instance.onStartBattle();
        Destroy(this.gameObject);
    }
    public int onAttack()
    {
        int dmg = Mathf.Max(str + selectedAttack.dmgmod, 0);
        StartCoroutine("attackProjectile");
        return dmg;
    }

    IEnumerator attackProjectile()
    {
        selectedAttack.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.25f);
        selectedAttack.gameObject.SetActive(false);
        selectedAttack = attackList[UnityEngine.Random.RandomRange(0, attackList.Count)];

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
