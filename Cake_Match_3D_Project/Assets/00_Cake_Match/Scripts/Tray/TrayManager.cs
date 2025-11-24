using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TrayManager : MonoBehaviour
{
    [SerializeField] private List<Slot> l_Slots;

    public List<Slot>L_Slots => l_Slots;


    [Button("Init Slot")]
    private void InitDataSlot()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Slot slot = transform.GetChild(i).GetComponent<Slot>();

            if(slot)
            {
                L_Slots.Add(slot);
            }
        }
    }

    public bool isEmptyCakeInTray()
    {
        for (int i = 0; i < l_Slots.Count; i++)
        {
            if (l_Slots[i].isHaveCakeInTray()) return false;
        }

        return true;
    }
}
