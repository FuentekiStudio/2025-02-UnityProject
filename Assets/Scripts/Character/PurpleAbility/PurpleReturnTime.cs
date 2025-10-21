using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character_Controller))]
[RequireComponent(typeof(Rigidbody2D))]
public class PurpleReturnTime : MonoBehaviour
{
    [Header("Recording")]
    [SerializeField] private float totalRewindSeconds = 2f;       // tiempo total a retroceder
    [SerializeField] private float stepInterval = 0.2f;           // frecuencia de muestreo (s)
    [SerializeField] private bool recordOnlyWhenSelected = true;

    [Header("Playback")]
    [SerializeField] private float perStepRewindTime = 0.2f;      // cuánto tarda ir de un memento al anterior
    [SerializeField] private AnimationCurve rewindCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("SFX")]
    [Tooltip("Drag: Assets/Audio/SFX/Abilities/rewind_time.wav")]
    [SerializeField] private AudioClip rewindSFX;
    [Range(0f, 1f)]
    [SerializeField] private float sfxSpatialBlend = 1f;     // 1 = 3D, 0 = 2D

    private Character_Controller character;
    private Rigidbody2D rb;

    private LimitedStack<PositionMemento> history;
    private float recordTimer;
    private bool isRewinding;
    private float originalGravity;

    // Handle for the reverse sound so we can stop it cleanly
    private AudioSource reverseSfxHandle;


    private int Capacity => Mathf.Max(1, Mathf.RoundToInt(totalRewindSeconds / stepInterval));     // Utilidad


    private void Awake()
    {
        character = GetComponent<Character_Controller>();
        rb = GetComponent<Rigidbody2D>();
        history = new LimitedStack<PositionMemento>(Capacity);
    }

    private void OnEnable()
    {
        // Escuchar cambios de selección para grabar sólo cuando este personaje esté activo
        if (character.PlayerRef != null && character.PlayerRef.partyHandler != null)
            character.PlayerRef.partyHandler.OnSelectedOneChange += OnSelectedOneChanged; // HUD usa el mismo evento. :contentReference[oaicite:3]{index=3}
    }

    private void OnDisable()
    {
        if (character.PlayerRef != null && character.PlayerRef.partyHandler != null)
            character.PlayerRef.partyHandler.OnSelectedOneChange -= OnSelectedOneChanged;
    }

    private void Update()
    {
        if (isRewinding) return;

        // 1) Input: sólo si este Purple es el seleccionado
        if (IsSelected() && Input.GetKeyDown(KeyCode.R))
        {
            if (history != null && history.Count > 0)
                StartCoroutine(RewindRoutine());
            return;
        }

        // 2) Grabación: sólo si está seleccionado (opcional)
        if (!recordOnlyWhenSelected || IsSelected())
        {
            recordTimer += Time.deltaTime;
            if (recordTimer >= stepInterval)
            {
                recordTimer = 0f;
                SaveSnapshot();
            }
        }
        else
        {
            // si dejó de estar seleccionado, no grabamos (y opcionalmente podríamos limpiar (no me odies Santi con tantos comentarios))
        }
    }

    private bool IsSelected()
    {
        var playerController =    character.PlayerRef;
        return playerController != null && playerController.selectedOne == character; // PlayerController expone selectedOne. :contentReference[oaicite:4]{index=4}
    }

    private void SaveSnapshot()
    {
        if (rb == null) return;
        history.Push(new PositionMemento(rb.position, rb.velocity));
    }

    private void OnSelectedOneChanged(Character_Controller newSelected)
    {
        // política: al cambiar, dejamos de grabar si no somos nosotros. No limpiamos; así se puede volver y aún tener "algo" reciente.
        // Si preferimos limpiar al perder el foco, descomentemos lo siguiente:
        // if (newSelected != character) history.Clear();
    }

    private IEnumerator RewindRoutine()
    {
        isRewinding = true;

        // --- SFX start (forward + reverse via Audio_Manager) ---
        PlayRewindSFX_Start();

        // Preparar físicas para un movimiento suave sin apagar colisiones
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Desactivar animaciones de caminar/saltar si queremos: character.animator?.SetFloat(...)

        while (history.Count > 0)
        {
            var snapshot = history.Pop();   // consume hacia el pasado (limpia la pila durante el interín)

            // Lerp/MovePosition hacia ese snapshot durante perStepRewindTime
            Vector2 start = rb.position;
            Vector2 target = snapshot.Position;
            float t = 0f;

            while (t < perStepRewindTime)
            {
                t += Time.deltaTime;
                float k = rewindCurve.Evaluate(Mathf.Clamp01(t / perStepRewindTime));
                Vector2 next = Vector2.Lerp(start, target, k);
                rb.MovePosition(next);      // mantiene colisión activa
                yield return null;
            }

            // al llegar a la keyframe, garantizamos snap final y anulamos velocidad
            rb.MovePosition(target);
            rb.velocity = Vector2.zero;
        }

        // Restaurar físicas
        rb.gravityScale = originalGravity;
        isRewinding = false;

        // --- SFX stop ---
        StopRewindSFX();

        // Reiniciar temporizador para que no grabe inmediatamente el mismo frame
        recordTimer = 0f;
    }

    #region SFX_via_Audio_Manaegr
    private void PlayRewindSFX_Start()
    {
        if (rewindSFX == null || Audio_Manager.Instance == null) return;

        // Plays forward one-shot AND a reverse track in parallel; returns handle to reverse
        reverseSfxHandle = Audio_Manager.Instance.PlayForwardAndReverse(
            rewindSFX, transform, sfxSpatialBlend
        );
    }

    private void StopRewindSFX()
    {
        if (reverseSfxHandle != null && Audio_Manager.Instance != null)
        {
            Audio_Manager.Instance.StopPooled(reverseSfxHandle);
            reverseSfxHandle = null;
        }
    }
    #endregion
}
