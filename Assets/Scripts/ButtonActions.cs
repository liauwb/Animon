using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    public Sprite onHover;
    public Sprite def;
    public string type;
    public bool isDoing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onPressHome()
    {
        manager.instance.background.sprite = manager.instance.backgrounds[0];
        manager.instance.currentAnimon.currentActivity = activity.Home;
        manager.instance.currentAnimon.timeInTraining = 0f;
        manager.instance.monstername.text = "";
        manager.instance.stats.text = ""; manager.instance.inFight = false;
        manager.instance.currentAnimon.anim.SetTrigger("isIdle");
    }


    public void onPressTrain()
    {
        manager.instance.background.sprite = manager.instance.backgrounds[2];
        manager.instance.currentAnimon.currentActivity = activity.Train;
        manager.instance.currentAnimon.timeInTraining = 0f;
        manager.instance.inFight = false;

        manager.instance.monstername.text = "";
        manager.instance.stats.text = "";
    }


    public void onPressFight()
    {
        manager.instance.background.sprite = manager.instance.backgrounds[1];
        manager.instance.currentAnimon.currentActivity = activity.Fight;
        manager.instance.onStartBattle();
        manager.instance.monstername.text = "";
        manager.instance.stats.text = "";
    }


    public void onPressStats()
    {
        manager.instance.inFight = false;

        manager.instance.background.sprite = manager.instance.backgrounds[3];
        manager.instance.monstername.text = manager.instance.currentAnimon.name;
        manager.instance.stats.text = "HP: " + manager.instance.currentAnimon.curhp + "/" + manager.instance.currentAnimon.hp + "\n\nStr: " + manager.instance.currentAnimon.str + "\n\nAgi: " + manager.instance.currentAnimon.agi;
        manager.instance.currentAnimon.currentActivity = activity.Stats;

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (manager.instance.currentAnimon.currentState != state.death && manager.instance.currentAnimon.currentState != state.egg)
        {
            foreach (ButtonActions i in manager.instance.buttonList)
            {
                i.isDoing = false;
                i.image.sprite = i.def;
            }
            isDoing = true;
            image.sprite = onHover;
        }
        manager.instance.punchbag.gameObject.SetActive(false);
        foreach(AdditionalEffect i in manager.instance.currentAnimon.bufflist)
        {
            i.duration = 0;
            i.doEffect(manager.instance.currentAnimon);
        }
        switch (type)
        {
            case "Home":
                onPressHome();

                manager.instance.currentAnimon.anim.SetTrigger("isIdle");
                if(manager.instance.enemy)
                    Destroy(manager.instance.enemy.gameObject);
                foreach (ButtonActions i in manager.instance.buttonList)
                {
                    i.isDoing = false;
                    i.image.sprite = i.def;
                }
                isDoing = true;
                image.sprite = onHover;
                manager.instance.currentAnimon.anim.SetTrigger("isIdle");
                break;

            case "Train":
                if (manager.instance.enemy)
                    Destroy(manager.instance.enemy.gameObject);
                if (manager.instance.currentAnimon.currentState != state.death && manager.instance.currentAnimon.currentState != state.egg)
                {

                    manager.instance.punchbag.gameObject.SetActive(true);
                    manager.instance.currentAnimon.anim.ResetTrigger("isIdle");
                    manager.instance.punchbag.GetComponent<Animator>().Play("PunchBag");
                    manager.instance.currentAnimon.anim.SetTrigger("isTraining");
                    onPressTrain();
                }
                break;

            case "Fight":
                if (manager.instance.currentAnimon.currentState != state.death && manager.instance.currentAnimon.currentState != state.egg && manager.instance.currentAnimon.currentState != state.baby && manager.instance.currentAnimon.currentActivity!=activity.Fight)
                    onPressFight();

                manager.instance.currentAnimon.anim.SetTrigger("isIdle");
                break;

            case "Stats":
                onPressStats();
                manager.instance.currentAnimon.anim.SetTrigger("isIdle");

                if (manager.instance.enemy)
                    Destroy(manager.instance.enemy.gameObject);
                foreach (ButtonActions i in manager.instance.buttonList)
                {
                    i.isDoing = false;
                    i.image.sprite = i.def;
                }
                isDoing = true;
                image.sprite = onHover;
                break;

        }

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = onHover;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isDoing)
            image.sprite = def;
    }
}
