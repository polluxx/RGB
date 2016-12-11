using UnityEngine;
using System.Collections;

public class ZetControl : MonoBehaviour {

    public Rigidbody2D rigidbody2d;
    public float playerSpeed = 0.5f;
    public bool faceRight = true;

    private GameObject beam;

    // grounding
    bool grounded = true;
    public Transform groundChk;
    float groundRadius = 0.5f;
    public LayerMask layer;

    public float jumpForce = 0.4f;

    Animator animat;
    //parts animators
    Animator partsAnimator;

    GameObject []parts;


    public float maxSpeed = 4f;
    public float currentMaxSpeed;
    public float maxAnimationWalkSpeed = 0.75f;

    // aim
    private GameObject aim;
    private GameObject weapon;

    private GameObject handsWith;

    // weapon state
    private bool isBattle = false;
    float timeToSpawn = 0;
    public float fireRate = 5;

    // load controllers
    public RuntimeAnimatorController normalStateCtrl;
    public RuntimeAnimatorController battleStateCtrl;

    //material
    //private Renderer playerRend;
    private Texture texturePlayer;

    // Use this for initialization
    void Start()
    {
        animat = GetComponent<Animator>();
        parts = GameObject.FindGameObjectsWithTag("PersonParts");
        rigidbody2d = GetComponent<Rigidbody2D>();
        aim = GameObject.Find("Aim");
        weapon = GameObject.FindGameObjectWithTag("Weapon");
        handsWith = GameObject.Find("HandsWithBlaster");
        changeAnimator();

        // = GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
        if(animat == null)
        {
            animat = GetComponent<Animator>();
        }

        currentMaxSpeed = maxSpeed;

        grounded = Physics2D.OverlapCircle(groundChk.position, groundRadius, layer);
        animat.SetBool("Ground", grounded);
        setMethodToObjectAnimator("bool", "Ground", grounded, 0f);
        animat.SetFloat("verticalSpeed", rigidbody2d.velocity.y);
        setMethodToObjectAnimator("float", "verticalSpeed", false, rigidbody2d.velocity.y);

        if (!grounded) return;

        float move = Input.GetAxis("Horizontal");
        animat.SetFloat("Speed", Mathf.Abs(move));
        animat.SetFloat("RealSpeed", Mathf.Abs(rigidbody2d.velocity.x));
        animat.SetFloat("WalkSpeed", maxAnimationWalkSpeed);

        setMethodToObjectAnimator("float", "Speed", false, Mathf.Abs(move));
        setMethodToObjectAnimator("float", "RealSpeed", false, Mathf.Abs(rigidbody2d.velocity.x));
        setMethodToObjectAnimator("float", "WalkSpeed", false, maxAnimationWalkSpeed);

        float animSpeed = Mathf.Abs(move);
        animSpeed = (animSpeed < 0.4f) ? 0.4f : animSpeed;

        animat.speed = animSpeed;

        if (faceRight && move < 0 || !faceRight && move > 0)
        {
            animat.SetBool("MoveBack", true);
            setMethodToObjectAnimator("bool", "MoveBack", true, 0);
            currentMaxSpeed = maxSpeed / 2;
        }
        else
        {
            animat.SetBool("MoveBack", false);
            setMethodToObjectAnimator("bool", "MoveBack", false, 0);
        }

        rigidbody2d.velocity = new Vector2(move * currentMaxSpeed, rigidbody2d.velocity.y);

        if (move > 0 && !faceRight)
        {
            flip();
        }
        else if (move < 0 && faceRight)
        {
            flip();
        }

        float lessThanPers = aim.transform.position.x - this.transform.position.x;
        if (lessThanPers < 0 && faceRight)
        {
            flip();
        }
        else if (lessThanPers > 0 && !faceRight)
        {
            flip();
        }


        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            isBattle = !isBattle;
            changeAnimator();
        }
    }

    void Update()
    {

        //float moveUP = Input.GetAxis("Vertical");
        if (grounded && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton2))) {

            //animat.SetFloat("verticalSpeed", 0f);

            animat.SetBool("Ground", false);

            rigidbody2d.AddForce(new Vector2(0, jumpForce));
        }

        // fire
        
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            
            if(!isBattle)
            {
                isBattle = true;
                changeAnimator();
            }
            
            if(weapon.activeSelf && Time.time >= timeToSpawn && Input.GetKeyDown(KeyCode.Mouse0))
            {
                weapon.GetComponent<Weapon>().SendMessage("Fire");
                timeToSpawn = Time.time + 1 / fireRate;
            }

            if(Input.GetKeyDown(KeyCode.Mouse1) && weapon.activeSelf && Time.time >= timeToSpawn)
            {
                weapon.GetComponent<Weapon>().SendMessage("Stack");
                timeToSpawn = Time.time + 1 / fireRate;
            }
        }

    }

    void flip()
    {
        faceRight = !faceRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        
        transform.localScale = scale;
    }

    Texture2D flipTexture(Texture2D original)
    {
        Texture2D flipped = new Texture2D(original.width, original.height);
        int x = flipped.width;
        int y = flipped.height;

        for(int i=0; i<x;i++)
        {
            for(int j=0; j< y; j++)
            {
                flipped.SetPixel(x - i - 1, y, original.GetPixel(x,y));
            }
        }
        flipped.Apply();
        return flipped;
    }

    void changeAnimator()
    {
        if (isBattle)
        {
            animat.runtimeAnimatorController = battleStateCtrl;
            //weapon.SetActive(true);
            handsWith.SetActive(true);
        } else
        {
            animat.runtimeAnimatorController = normalStateCtrl;
            //weapon.SetActive(false);           
            handsWith.SetActive(false);
        }
    }

    void setPartsParameters(string type, string name, Animator animator, bool value, float valFloat)
    {
        if (animator == null) return;

        animator.speed = animat.speed;
        //
        switch (type) {
            case "bool":
                animator.SetBool(name, value);
                break;
            case "trigger":
                animator.SetTrigger(name);
                break;
            case "float":
                animator.SetFloat(name, valFloat);
                break;
        }
    }

    void setMethodToObjectAnimator(string type, string name, bool value, float valFloat)
    {
        for(int i=0; i < parts.Length; i++)
        {
            if (parts[i].name == "HandsWithBlaster") continue;
            setPartsParameters(type, name, parts[i].GetComponent<Animator>(), value, valFloat);
        }
    }

}
