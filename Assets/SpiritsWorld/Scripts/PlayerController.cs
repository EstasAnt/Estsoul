using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using KlimLib.SignalBus;
using SceneManagement.SpiritWorld;
using UnityDI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Dependency] private readonly SignalBus _signalBus;
    
    [Header("Skin")]
    [SerializeField] Sprite White;
    [SerializeField] Sprite Black;
    [SerializeField] SpriteRenderer Body;

    [Header("SFX")]
    [SerializeField] ParticleSystem CollisionPS;
    [SerializeField] AudioClip FailSound;
    [SerializeField] AudioClip SuccessSound;
    [SerializeField] AudioClip SwipeSound;

    [Header("Moving")]
    [SerializeField] Transform[] CheckPoints;
    [SerializeField] float speed = 1;
    [SerializeField] float jumpSpeed = 10;
    [SerializeField] float aimDump = 0.5f;
    [SerializeField] float speedDump = 5f;

    ObjectColor currentColor;
    float currentSpeed;
    int targetIndex = -1;
    Vector3 aimPos;
    Vector3 initPos;

    private void Start()
    {
        ContainerHolder.Container.BuildUp(this);
        initPos = transform.position;
        Reset();
    }

    private void GoToNextTarget()
    {
        currentSpeed = speed;
        Body.sprite = currentColor == ObjectColor.Black ? Black : White;

        if (targetIndex == CheckPoints.Length - 1)
            return;
        targetIndex++;
        aimPos = CheckPoints[targetIndex].position;
        currentSpeed = speed;
    }

    private void Update()
    {
        if (targetIndex < 0)
            return;//no target

        var target = CheckPoints[targetIndex];

        if (Input.GetKeyDown(KeyBindings.Back)
        || Input.GetKeyDown(KeyBindings.BackAlt1)
        || Input.GetKeyDown(KeyBindings.BackAlt2))
        {
            AudioSource.PlayClipAtPoint(SwipeSound, transform.position, AudioSettings.SFXVolume);
            aimPos = transform.position + Vector3.left;
            currentSpeed = speed * jumpSpeed;
        }
        else
        if (Input.anyKeyDown)
        {
            AudioSource.PlayClipAtPoint(SwipeSound, transform.position, AudioSettings.SFXVolume);
            aimPos = target.position;
            currentSpeed = speed * jumpSpeed;
        }

        
        aimPos = Vector3.Lerp(aimPos, target.position, Time.deltaTime * aimDump);
        currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * speedDump);

        transform.position = Vector3.MoveTowards(transform.position, aimPos, Time.deltaTime * currentSpeed);

        if (Vector3.SqrMagnitude(transform.position - target.position) < 0.01f)
            GoToNextTarget();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var marker = other.GetComponent<Marker>();
        if (!marker)
            return;

        switch (marker.Type)
        {
            case ObjectType.Flower:
                PlayCollisionSFX();
                AudioSource.PlayClipAtPoint(SuccessSound, transform.position, AudioSettings.SFXVolume);
                ChangeColor(marker.Color);
                        GoToNextTarget();
                break;
            case ObjectType.Spirit:
                if (marker.Color != currentColor)//Ooops..
                {
                    AudioSource.PlayClipAtPoint(FailSound, transform.position, AudioSettings.SFXVolume);
                    PlayCollisionSFX();
                    StartCoroutine(ResetRoutine());
                }
                break;
            case ObjectType.Gate:
                AudioSource.PlayClipAtPoint(FailSound, transform.position, AudioSettings.SFXVolume);
                PlayCollisionSFX();
                _signalBus.FireSignal(new SpiritWorldGateInSignal(true));
                _loadingUsualWorld = true;
                break;
        }
    }

    private bool _loadingUsualWorld;

    private IEnumerator ResetRoutine()
    {
        enabled = false;
        if(_loadingUsualWorld)
            yield break;
        yield return new WaitForSeconds(1f);
        Reset();
        yield return new WaitForSeconds(1f); 
        enabled = true;
    }

    private void PlayCollisionSFX()
    {
        CollisionPS.Play();
    }

    private void Reset()
    {
        transform.position = initPos;
        targetIndex = -1;
        ChangeColor(ObjectColor.White);
        GoToNextTarget();        
    }

    private void ChangeColor(ObjectColor color)
    {
        currentColor = color;
    }
}

enum ObjectColor
{
    White, Black
}