using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorUI : MonoBehaviour
{
    [SerializeField]
    private item_sprites_table images = null;

    [SerializeField]
    private List<SlotData> slots = null;

    // Start is called before the first frame update
    void Start()
    {
        if (UserRecord.Accessorys.Count != 0)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].ID = UserRecord.Accessorys[i];
                if (slots[i].ID != -1)
                {
                    slots[i].SetSprite(images.FindOne(slots[i].ID));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
