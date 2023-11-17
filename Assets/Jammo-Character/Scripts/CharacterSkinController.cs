using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSkinController : MonoBehaviour
{
    Animator animator;
    Renderer[] characterMaterials;
    public ParticleSystem saiyayin1;
    public GameObject ray;
    public GameObject hands;
    private GameObject instantiatedObject;




    public Texture2D[] albedoList;
    [ColorUsage(true,true)]
    public Color[] eyeColors;
    public enum EyePosition { normal, happy, angry, dead}
    public EyePosition eyeState;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterMaterials = GetComponentsInChildren<Renderer>();
        saiyayin1 = GetComponentInChildren<ParticleSystem>();
     


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //ChangeMaterialSettings(0);
            ChangeEyeOffset(EyePosition.normal);
            ChangeAnimatorIdle("normal");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //ChangeMaterialSettings(1);
            ChangeEyeOffset(EyePosition.angry);
            ChangeAnimatorIdle("angry");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //ChangeMaterialSettings(2);
            ChangeEyeOffset(EyePosition.happy);
            ChangeAnimatorIdle("happy");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //ChangeMaterialSettings(3);
            ChangeEyeOffset(EyePosition.dead);
            ChangeAnimatorIdle("dead");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("saiyan", true);
            saiyayin1.Play();
        }
        if (Input.GetKeyDown((KeyCode.LeftControl)))
        {
            animator.SetBool("saiyan", false);
            saiyayin1.Stop();

        }
        if (Input.GetKeyDown(KeyCode.LeftAlt) && instantiatedObject == null)
        {
            InstantiateObjectInHands();
            animator.SetBool("Ray", true);

        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            animator.SetBool("Ray", false);
            if (instantiatedObject != null)
            {
                Destroy(instantiatedObject);
            }
        }

        if (instantiatedObject != null)
        {
            instantiatedObject.transform.position = hands.transform.position;
        }
    }

    void InstantiateObjectInHands()
    {
        // Check if the hands GameObject and objectPrefab are not null
        if (hands != null && ray != null)
        {
            // Instantiate the object in the hands position
            instantiatedObject = Instantiate(ray, hands.transform.position, hands.transform.rotation);
        }
        else
        {
            Debug.LogError("Hands or objectPrefab is null. Assign them in the inspector!");
        }
    }


    void ChangeAnimatorIdle(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    void ChangeMaterialSettings(int index)
    {
        for (int i = 0; i < characterMaterials.Length; i++)
        {
            if (characterMaterials[i].transform.CompareTag("PlayerEyes"))
                characterMaterials[i].material.SetColor("_EmissionColor", eyeColors[index]);
            else
                characterMaterials[i].material.SetTexture("_MainTex",albedoList[index]);
        }
    }

    void ChangeEyeOffset(EyePosition pos)
    {
        Vector2 offset = Vector2.zero;

        switch (pos)
        {
            case EyePosition.normal:
                offset = new Vector2(0, 0);
                break;
            case EyePosition.happy:
                offset = new Vector2(.33f, 0);
                break;
            case EyePosition.angry:
                offset = new Vector2(.66f, 0);
                break;
            case EyePosition.dead:
                offset = new Vector2(.33f, .66f);
                break;
            default:
                break;
        }

        for (int i = 0; i < characterMaterials.Length; i++)
        {
            if (characterMaterials[i].transform.CompareTag("PlayerEyes"))
                characterMaterials[i].material.SetTextureOffset("_MainTex", offset);
        }
    }
}
