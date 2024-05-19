using UnityEngine;
using Dreamteck.Splines;


[RequireComponent(typeof(SplinePositioner))]
[RequireComponent(typeof(Animator))]
public class Trap : MonoBehaviour
{
    [Header("Positioning")]
    [SerializeField] [Range(0f, 1f)] float percentAlongTheSpline;
    [SerializeField] [Range(-.5f, .5f)] float xOffset;
    [Space]
    [SerializeField] [Range(-.5f, .5f)] float yOffset;
    [Header("Activation")]
    [SerializeField] float activationInterval;
    [SerializeField] string animatorActivationBool;
    [Header("Components")]
    [SerializeField] SplinePositioner splinePositioner;
    [SerializeField] Animator animator;


    int _animatorActivationBool;

    float elapsedTime;
    bool toggle;

    void OnValidate()
    {
        splinePositioner ??= GetComponent<SplinePositioner>();
        animator ??= GetComponent<Animator>();

        splinePositioner.SetPercent(percentAlongTheSpline);

        Vector2 offset = splinePositioner.motion.offset;
        offset.x = xOffset;
        offset.y = yOffset;
        splinePositioner.motion.offset = offset;
    }

    void Awake()
    {
        _animatorActivationBool = Animator.StringToHash(animatorActivationBool);

        int siblingIndex = transform.GetSiblingIndex();

        elapsedTime -= siblingIndex;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= activationInterval)
        {
            elapsedTime = 0;

            toggle = !toggle;

            animator.SetBool(_animatorActivationBool, toggle);
        }
    }
}
