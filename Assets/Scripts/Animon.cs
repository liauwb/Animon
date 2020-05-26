using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class Evolution {
    public int minHP;
    public int minStr;
    public int minAgi;
    public string monsterName;
}

public enum state { egg = 0,baby = 1,child = 2,teen = 3,adult = 4,death = -1};

public enum activity {Home,Train,Fight,Stats}

public enum stats {Str, HP, Agi}

public class Animon : MonoBehaviour,IPointerClickHandler
{
    public int id;
    public string name;
    public int str;
    public int hp;
    public int curhp;
    public int agi;
    public float lifespan;
    public int statgain;
    public stats statpref;
    [SerializeField]
    bool isHappy = true;
    public List<Attack> attackList;
    public Attack selectedAttack;
    [SerializeField]
    bool reverse;
    public state currentState;
    public activity currentActivity;
    public List<Evolution> evolutionList;
    //public Skill[] skillList;
    public float timetrackr;
    public float timeInTraining;
    public Animator anim;
    Image img;
    public List<AdditionalEffect> bufflist;
    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
    
    public int onAttack()
    {
        int dmg = Mathf.Max(str + selectedAttack.dmgmod,0);
        StartCoroutine("attackProjectile");
        return dmg;
    }
    public void onDamaged(int dmg)
    {
        anim.SetTrigger("isHit");
        curhp = curhp - dmg;
        if (curhp <= 0)
        {
            manager.instance.yourDelay = 100000f;
            manager.instance.enemyDelay = 100000f;
            StartCoroutine("die");
        }
    }
    IEnumerator die()
    {
        yield return new WaitForSeconds(1f);
        lifespan -= 30f;
        curhp = 1;
        manager.instance.currentAnimon.anim.SetTrigger("isIdle");
        Destroy(manager.instance.enemy.gameObject);
        manager.instance.inFight = false;
        foreach (ButtonActions i in manager.instance.buttonList)
        {
            i.isDoing = false;
            i.image.sprite = i.def;
            if(i.type == "Home")
            {
                i.isDoing = true;
                i.image.sprite = i.onHover;
                i.onPressHome();
            }
        }
        
    }
    IEnumerator attackProjectile()
    {
        anim.SetTrigger("isTraining");
        selectedAttack.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        anim.SetTrigger("inFight");
        selectedAttack.gameObject.SetActive(false);

        selectedAttack = attackList[UnityEngine.Random.RandomRange(0, attackList.Count)];
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetTrigger("Evolve");
        img = GetComponent<Image>();
        switch (currentState)
        {
            case state.egg:
                lifespan = 10f;
                break;
                
            case state.baby:
                lifespan = 180f;
                break;
                
            case state.child:
                lifespan = 360f;
                break;
                
            case state.teen:
                lifespan = 600f;
                break;
                
            case state.adult:
                lifespan = 420f;
                break;
                    
            case state.death:
                lifespan = 10f;
                str = 0;
                hp = 10;
                agi = 0;
                break;

        }
        timetrackr = Time.time;
        if (manager.instance != null) 
            manager.instance.currentAnimon = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timetrackr >= lifespan)
        {
            foreach (Evolution i in evolutionList)
            {
                if (hp >= i.minHP && agi >= i.minAgi && str >= i.minStr)
                {
                    GameObject temp = (GameObject)Instantiate(Resources.Load(i.monsterName));
                    temp.transform.SetParent(transform.parent);
                    temp.transform.position = transform.position;
                    temp.transform.localScale = transform.localScale;
                    temp.GetComponent<Animon>().curhp = Mathf.Max(1,(int)((float)curhp * (temp.GetComponent<Animon>().hp+hp)/hp));
                    temp.GetComponent<Animon>().hp += hp;
                    temp.GetComponent<Animon>().str += str;
                    temp.GetComponent<Animon>().agi += agi;
                    
                    Destroy(this.gameObject);
                    return;
                }
            }
            GameObject temp2= (GameObject)Instantiate(Resources.Load("Tombstone"));
            temp2.transform.SetParent(transform.parent);
            temp2.transform.position = transform.position;
            temp2.transform.localScale = transform.localScale;
            Destroy(this.gameObject);
            return;
        }
        if(currentActivity == activity.Fight)

        {
            
            this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1);
            timetrackr += Time.deltaTime;

        }
        if(currentActivity == activity.Stats)
        {
            this.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            timetrackr += Time.deltaTime;
        }
        if(currentActivity == activity.Train)
        {
            this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1);
            timeInTraining += Time.deltaTime;
            if(timeInTraining >= 5f)
            {
                curhp += hp / 100 * 10;
                curhp = Math.Min(curhp, hp);
                int tempgain = UnityEngine.Random.Range(1, statgain);
                for (int i = 0; i < tempgain; i++) {
                    int temp = UnityEngine.Random.Range(0, 4);
                    switch (temp)
                    {
                        case 0:
                            if (statpref == stats.HP)
                            {
                                hp += 10;
                                curhp += 10;
                            }
                            if (statpref == stats.Agi)
                                agi += 1;
                            if (statpref == stats.Str)
                                str += 1;
                            break;
                        case 1:
                            hp +=10;
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
                timeInTraining = 0f;
            }
        }
        if(currentActivity == activity.Home)
        {
            this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 1);
            timeInTraining += Time.deltaTime;
            if (timeInTraining >= 5f)
            {
                curhp += hp / 100 * 20;
                curhp = Math.Min(curhp, hp);
                timeInTraining = 0f;
            }
        }

    }
}
