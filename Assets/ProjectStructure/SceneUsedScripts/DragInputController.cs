using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragInputController : MonoBehaviour
{


    [Header("Camera (REAL camera with CinemachineBrain)")]
    [SerializeField] private Camera raycastCamera;

    public Camera RaycastCamera { get { return raycastCamera; } }
    //[SerializeField] Animator p_animator;


    // Drag state
    private IWorldDraggable current;
    [SerializeField] private Transform currentTransform;
    private Vector3 startPosition;
    private Quaternion startrotation;
    private float depth;

  //  public Animator P_Animator { get { return p_animator; } }
    private void Awake()
    {
        LessonContext.Init_DragInputController(this);
    }
    void Start()
    {

       InputManager.Instance.pressAction.action.started += OnDragStarted;
       
        InputManager.Instance.pressAction.action.canceled += OnDragCanceled;
    }

    private void Update()
    {
        OnDragPerformed();
    }
    void OnDisable()
    {
        InputManager.Instance.pressAction.action.started -= OnDragStarted;

        InputManager.Instance.pressAction.action.canceled -= OnDragCanceled;

 
    }

    //INPUT LIFECYCLE

    void OnDragStarted(InputAction.CallbackContext ctx)
    {
        TryPick();     
    }

    void OnDragPerformed(/*InputAction.CallbackContext ctx*/)
    {  
        Drag();    
    }

    void OnDragCanceled(InputAction.CallbackContext ctx)
    {
        Drop();
    }

    //CORE LOGIC 
    public Vector2 PointerPosition()
    {
        return InputManager.Instance.dragAction.action.ReadValue<Vector2>();
    }

    void TryPick()
    {
        if (raycastCamera == null ) return;

      Ray  ray = raycastCamera.ScreenPointToRay(PointerPosition());

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Physics.Raycast(ray, out RaycastHit hit, 200f))
        {
            var draggable = hit.collider.GetComponent<IWorldDraggable>();
           
            current = draggable;
            if (current == null) return;
            if (!draggable.IsInteractable) return;
            // Debug.Log("presssed");
            currentTransform = ((MonoBehaviour)draggable).transform; //hit.collider.transform;

            depth = Vector3.Dot(currentTransform.position - raycastCamera.transform.position, raycastCamera.transform.forward) + current.DepthOffset;
            startPosition = currentTransform.position;
            startrotation = currentTransform.rotation;
            current.BeginDrag(hit.point);

            // Lets highlighters hide as soon as the drag starts, rather than waiting
            // for the drop sequence to settle (which never happens on a rejected drop).
            LessonEvents.RaiseInteractionStarted(LessonContext.lessonFlowController.LFC_CurrentSlide);
        }
    }

    void Drag()
    {
        if(current != null)
        {
            Vector2 screenPos = PointerPosition();
            Vector3 screenPoint = new Vector3(screenPos.x, screenPos.y, depth);
            Vector3 worldPos = raycastCamera.ScreenToWorldPoint(screenPoint);
            current.Drag(worldPos);
        }
        
    }

    void Drop()
    {
        if (current != null)
        {

            Ray ray = raycastCamera.ScreenPointToRay(PointerPosition());

            bool droppedOnZone = false;

            // Nothing in the scene is layered, so a single Raycast returns whatever
            // collider happens to be nearest and the real zone behind it is missed.
            // Walk every hit front to back and take the first zone that accepts this id.
            RaycastHit[] hits = Physics.RaycastAll(ray, 200f);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            for (int i = 0; i < hits.Length; i++)
            {
                var zone = hits[i].collider.GetComponent<WroldDrop>();
                if (zone == null) continue;
                if (!zone.Accept(current.Id)) continue;

                current.Attach(zone.transform);
                zone.DestroyHighlighter();
                droppedOnZone = true;
                break;
            }

            if (!droppedOnZone && currentTransform != null)
            {
                Debug.Log("NotAttach");
               // currentTransform.position = startPosition;
                current.Notatach(startPosition,startrotation);
                if(currentTransform.tag == "nail")
                {
                    LessonEvents.RaiseShowNegativePrompt(0);
                }
            }
            current.EndDrag();
            ClearDragState();
        }
    }

    void ClearDragState()
    {
        current = null;
        currentTransform = null;
    }
}


