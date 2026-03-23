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

            depth = Vector3.Dot(currentTransform.position - raycastCamera.transform.position,raycastCamera.transform.forward);
            startPosition = currentTransform.position;
            startrotation = currentTransform.rotation;
            current.BeginDrag(hit.point);
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
            if (Physics.Raycast(ray, out RaycastHit hit, 200f))
            {      
                var zone = hit.collider.GetComponent<WroldDrop>();
                Debug.Log(zone);
                if (zone != null)
                {              
                    current.Attach(zone.transform);
                    Debug.Log(zone.transform.position);
                    droppedOnZone = zone.Accept(current.Id);
                    zone.DestroyHighlighter();
                }
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


