using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject joinPopUp;
    [SerializeField] private TextMeshProUGUI joinPopUpText;

    private bool infrontOfPartyMember;
    private bool infrontOfScenePortal;
    private GameObject portal;
    private GameObject joinableMember;
    private PlayerControls playerControls;
    private List<GameObject> overworldCharacters = new List<GameObject>();

    private const string PARTY_JOIN_MESSAGE = " joined the party!";
    private const string NPC_JOINABLE_TAG = "NPCJoinable";
    private const string MOVE_SCENE_TAG = "Portal";

    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    void Start()
    {
        playerControls.Player.Interact.performed += _ => Interact();
        playerControls.Player.MoveScene.performed += _ => MoveScene();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Interact()
    {
        if (infrontOfPartyMember == true && joinableMember != null)
        {
            MemberJoined(joinableMember.GetComponent<JoinableCharacter>().MemberToJoin);
            infrontOfPartyMember = false;
            joinableMember = null;
        }
    }

    private void MemberJoined(PartyMemberInfo partyMember)
    {
        GameObject.FindFirstObjectByType<PartyManager>().AddMemberToPartyByName(partyMember.MemberName);
        joinableMember.GetComponent<JoinableCharacter>().CheckIfJoined();

        joinPopUp.SetActive(true);
        joinPopUpText.text = partyMember.MemberName + PARTY_JOIN_MESSAGE;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == NPC_JOINABLE_TAG)
        {
            infrontOfPartyMember = true;
            joinableMember = other.gameObject;
            joinableMember.GetComponent<JoinableCharacter>().ShowInteractPrompt(true);
        }

        if (other.gameObject.tag == MOVE_SCENE_TAG)
        {
            infrontOfScenePortal = true;
            portal = other.gameObject;
            Debug.Log(portal);
            portal.GetComponent<Portal>().ShowInteractPrompt(true);
        }
    }

    private void SpawnOverworldMembers()
    {
        for (int i = 0; i < overworldCharacters.Count; i++)
        {
            Destroy(overworldCharacters[i]);
        }
        overworldCharacters.Clear();

        List<PartyMember> currentParty = GameObject.FindFirstObjectByType<PartyManager>().GetCurrentParty();

        for (int i = 0; i < currentParty.Count; i++)
        {
            if (i == 0) // first member will be the player
            {
                GameObject player = gameObject; // get the player

                GameObject playerVisual = Instantiate(currentParty[i].MemberOverworldVisualPrefab,
                transform.position, Quaternion.identity); // spawn the member visual

                playerVisual.transform.SetParent(player.transform); // settting the parent to the player

                player.GetComponent<PlayerController>().SetOverworldVisuals(playerVisual.GetComponent<Animator>(),
                playerVisual.GetComponent<SpriteRenderer>(), playerVisual.transform.localScale); // assign the player controller values
                //playerVisual.GetComponent<MemberFollowAI>().enabled = false;
                overworldCharacters.Add(playerVisual); // add the overworld character visual to the list
            }
            else // any other will be a follower
            {
                Vector3 positionToSpawn = transform.position;// get the followers spawn position
                positionToSpawn.x -= i;

                GameObject tempFollower = Instantiate(currentParty[i].MemberOverworldVisualPrefab,
                positionToSpawn, Quaternion.identity);// spawn the follower

                //tempFollower.GetComponent<MemberFollowAI>().SetFollowDistance(i); // set follow ai settings
                overworldCharacters.Add(tempFollower); // add the follower visual to our list
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == NPC_JOINABLE_TAG)
        {
            infrontOfPartyMember = false;
            joinableMember = other.gameObject;
            joinableMember.GetComponent<JoinableCharacter>().ShowInteractPrompt(false);
        }

        if (other.gameObject.tag == MOVE_SCENE_TAG)
        {
            infrontOfScenePortal = false;
            portal = other.gameObject;
            portal.GetComponent<Portal>().ShowInteractPrompt(false);
        }
    }

    private void MoveScene()
    {
        if (infrontOfScenePortal == true)
        {
            SceneManager.LoadSceneAsync("Scene 2");
        }
    }
}
