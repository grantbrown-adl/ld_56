using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger), typeof(AudioSource))]
public class ButtonEventTrigger : MonoBehaviour {
    [SerializeField] private Vector2 _buttonScaleEnter;
    [SerializeField] private Vector2 _buttonScaleExit;
    [SerializeField] private Vector2 _buttonScaleDown;
    [SerializeField] private ParticleSystem _downParticle;
    [SerializeField] private bool _enabled;

    private EventTrigger _eventTrigger;

    private void Awake() {
        _eventTrigger = GetComponent<EventTrigger>();

        AddEventTrigger(EventTriggerType.PointerEnter, PointerEnter);
        AddEventTrigger(EventTriggerType.PointerExit, PointerExit);
        AddEventTrigger(EventTriggerType.PointerDown, PointerDown);

    }
    public void PointerEnter(BaseEventData callback) {
        if (!_enabled) return;
        transform.localScale = _buttonScaleEnter;
        SoundManager.Instance.PlayButtonHover();
    }

    public void PointerExit(BaseEventData callback) {
        if (!_enabled) return;
        transform.localScale = Vector2.one;
    }

    public void PointerDown(BaseEventData callback) {
        if (!_enabled) return;
        Instantiate(_downParticle, new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.identity);
        //_downParticle.Play();
        SoundManager.Instance.PlayButtonSelect();
        transform.localScale = _buttonScaleDown;
    }

    private void AddEventTrigger(EventTriggerType triggerType, UnityEngine.Events.UnityAction<BaseEventData> callback) {
        EventTrigger.Entry entry = new() {
            eventID = triggerType
        };
        entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(callback));
        _eventTrigger.triggers.Add(entry);
    }

    private void PlayDownParticle() {
        if (_downParticle != null) {
            _downParticle.Play();
        }
    }
}
