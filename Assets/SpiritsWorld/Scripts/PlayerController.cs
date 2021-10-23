using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Character.Control;
using Game.LevelSpecial;
using InControl;
using KlimLib.SignalBus;
using SceneManagement.SpiritWorld;
using UnityDI;
using UnityEngine;
using DebugTools;

public class PlayerController : MonoBehaviour, ISceneLoadingRecation
{
    [Dependency] private readonly SignalBus _signalBus;

    public PlayerActions CurrentPlayerActions
    {
        get
        {
            if (_GamepadActions == null)
                return _KeyboardActions;

            if (_GamepadActions.Device == null)
                _GamepadActions.Device =
                    InputManager.Devices.FirstOrDefault(_ => _.DeviceClass == InputDeviceClass.Controller); //ToDo: Remove from here
            if (_KeyboardActions == null)
                return _GamepadActions;
            if (_GamepadActions.Device == null)
                return _KeyboardActions;
            if (_KeyboardActions.Device == null)
                return _GamepadActions;
            return _KeyboardActions.Device.LastInputTick > _GamepadActions.Device.LastInputTick
                ? _KeyboardActions
                : _GamepadActions;
        }
    }

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

    private PlayerActions _KeyboardActions;
    private PlayerActions _GamepadActions;

    private void Start()
    {
#if FAST_SKIP_ENABLED
        ContainerHolder.Container.Resolve<TPPointsList>().character = this;
#endif
        (_KeyboardActions, _GamepadActions) = (PlayerActions.CreateWithKeyboardBindings(), PlayerActions.CreateWithJoystickBindings());
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

        if (CurrentPlayerActions.Left.WasPressed)
        {
            AudioSource.PlayClipAtPoint(SwipeSound, transform.position, AudioSettings.SFXVolume);
            aimPos = transform.position + Vector3.left;
            currentSpeed = speed * jumpSpeed;
        }
        else
        if (CurrentPlayerActions.Right.WasPressed)
        {
            AudioSource.PlayClipAtPoint(SwipeSound, transform.position, AudioSettings.SFXVolume);
            aimPos = target.position;
            currentSpeed = speed * jumpSpeed;
        }
        if (CurrentPlayerActions.Return.WasPressed)
        {
            _signalBus.FireSignal(new PlayerActionWasPressedSignal(UniversalPlayerActions.Return));
        }
#if FAST_SKIP_ENABLED
        if (CurrentPlayerActions.FastSkip.WasPressed)
        {
            _signalBus.FireSignal(new PlayerActionWasPressedSignal(UniversalPlayerActions.Teleport));
        }
#endif

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
                // _signalBus.FireSignal(new SpiritWorldGateInSignal(true));
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