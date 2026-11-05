using MTM101BaldAPI.Reflection;
using UnityEngine;

namespace EvilSideItems.CustomItems
{
    public class ITM_EvilQuarter : Item
    {
        public override bool Use(PlayerManager pm)
        {
            if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out RaycastHit hitinfo, pm.pc.Reach, pm.pc.ClickLayers))
            {
                if (hitinfo.transform.TryGetComponent(out SodaMachine machine))
                {
                    int usesLeft = (int)machine.ReflectionGetVariable("usesLeft");
                    usesLeft += 1;
                    machine.ReflectionSetVariable("usesLeft", usesLeft);
                    if (usesLeft > 0)
                    {
                        if (SodaMachinePatches.SavedMaterials.ContainsKey(machine.name))
                        {
                            Material[] materials = new Material[2]
                            {
                                machine.GetComponent<MeshRenderer>().materials[0],
                                SodaMachinePatches.SavedMaterials[machine.name]
                            };

                            MeshRenderer meshRenderer = (machine.ReflectionGetVariable("meshRenderer") as MeshRenderer);
                            meshRenderer.sharedMaterials = materials;
                            machine.GetComponent<MeshRenderer>().sharedMaterials = materials;
                            machine.GetComponent<MeshRenderer>().materials = materials;
                        }
                    }
                    Other.PlaySingleIThink("CoinDrop");
                    Destroy(gameObject);
                    return true;
                }
            }
            Destroy(gameObject);
            return false;
        }
    }
}
