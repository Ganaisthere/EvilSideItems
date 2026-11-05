using MTM101BaldAPI.Reflection;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace EvilSideItems.CustomItems
{
    public class ITM_EvilNotebook : Item
    {
        public override bool Use(PlayerManager pm)
        {
            BaseGameManager BGM = Singleton<BaseGameManager>.Instance;
            int randomNum = Random.Range(0, 2);
            if (BGM.FoundNotebooks >= BGM.Ec.notebookTotal)
            {
                BGM.CollectNotebooks(1);
                Singleton<CoreGameManager>.Instance.AddPoints(50, pm.playerNumber, true);
            }
            else if (BGM.Ec.notebookTotal <= 0)
            {
                Other.PlaySingleIThink("ErrorMaybe");
                Destroy(gameObject);
                return false;
            }
            else if (BGM.FoundNotebooks <= 0 || randomNum == 0)
            {
                List<Activity> activities = new List<Activity>();
                foreach (Activity act in pm.ec.activities)
                {
                    if (!(act.ReflectionGetVariable("notebook") as Notebook).hidden)
                    {
                        activities.Add(act);
                    }
                }
                if (activities.Count <= 0)
                {
                    Other.PlaySingleIThink("ErrorMaybe");
                    Destroy(gameObject);
                    return false;
                }
                Activity actGet = activities[Random.Range(0, activities.Count - 1)];
                if (!(bool)actGet.ReflectionGetVariable("completed"))
                {
                    actGet.Completed(pm.playerNumber, true);
                }
                Notebook notebook = actGet.ReflectionGetVariable("notebook") as Notebook;
                if (notebook != null)
                {
                    notebook.Clicked(pm.playerNumber);
                }
                Singleton<CoreGameManager>.Instance.AddPoints(25, pm.playerNumber, true);
            }
            else
            {
                List<Activity> activities = new List<Activity>();
                foreach (Activity act in pm.ec.activities)
                {
                    bool noActivityNC = (act.ReflectionGetVariable("notebook") as Notebook).hidden && act.GetType() == typeof(NoActivity);
                    bool activityNC = act.GetType() != typeof(NoActivity) && (act.ReflectionGetVariable("notebook") as Notebook).hidden && (bool)act.ReflectionGetVariable("completed");
                    if (activityNC || noActivityNC)
                    {
                        activities.Add(act);
                    }
                }
                if (activities.Count <= 0)
                {
                    Other.PlaySingleIThink("ErrorMaybe");
                    Destroy(gameObject);
                    return false;
                }
                Activity actGet = activities[Random.Range(0, activities.Count - 1)];
                BGM.CollectNotebooks(-1);
                Other.PlaySingleIThink("Boink");
                try
                {
                    actGet.InstantReset();
                }
                catch (Exception exp)
                {
                    BasePlugin.Log("WTF Why It's Broken In actGet.InstantReset() ?!?!?! - " + actGet.GetType().Name, 2);
                    BasePlugin.Log(exp, 2);
                }
            }
            Destroy(gameObject);
            return true;
        }
    }
}
