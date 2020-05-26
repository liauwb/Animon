using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalEffect : MonoBehaviour
{
    public int StatMod;
    public int multMod;
    public stats Mod;
    public int duration;
    public bool self;
    public string name;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public virtual void doEffect(Animon target)
    {

        duration--;
        if(duration <= 0)
        {
            switch (Mod)
            {
                case stats.Agi:
                    target.agi = (target.agi/ multMod) - StatMod;
                    break;
                case stats.Str:
                    target.str = (target.str / multMod) - StatMod;
                    break;
            }
            target.bufflist.Remove(this);
            Destroy(this);
        }
    }

    public virtual void onGain(Animon target)
    {
        if (target.bufflist.Find(x => x.name == this.name)!= null)
        {
            AdditionalEffect ae = target.bufflist.Find(x => x.name == this.name);
            ae.duration = duration;
            return;
        }
        switch (Mod)
        {
            case stats.Agi:
                target.agi = (target.agi + StatMod) * multMod;
                break;
            case stats.Str:
                target.str = (target.str + StatMod) * multMod;
                break;
        }
        AdditionalEffect temp = target.gameObject.AddComponent<AdditionalEffect>();
        temp.duration = duration;
        temp.StatMod = StatMod;
        temp.Mod = Mod;
        temp.multMod = multMod;
        temp.name = name;
        target.bufflist.Add(temp);
    }
}
