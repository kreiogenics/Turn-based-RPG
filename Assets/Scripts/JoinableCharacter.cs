using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinableCharacter : MonoBehaviour
{
    public PartyMemberInfo MemberToJoin;
    [SerializeField] private GameObject interactPrompt;

    // Start is called before the first frame update
    void Start()
    {
        CheckIfJoined();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowInteractPrompt(bool showPrompt)
    {
        if (showPrompt == true)
        {
            interactPrompt.SetActive(true);
        }
        else
        {
            interactPrompt.SetActive(false);
        }
    }

    public void CheckIfJoined()
    {
        List<PartyMember> currentParty = GameObject.FindFirstObjectByType<PartyManager>().GetCurrentParty();

        for (int i = 0; i < currentParty.Count; i++)
        {
            if (currentParty[i].MemberName == MemberToJoin.MemberName)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
