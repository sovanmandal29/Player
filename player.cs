
using UnityEngine;

public class player : MonoBehaviour {

    public float defacc = 30f;
    public Vector3 movement;
    public Transform cam;
    public float rad = 2f;
    public CharacterController c;
    public Transform grndchk;
    public LayerMask grnd;
    public LayerMask wl;
    bool isgrnd;
    float g=-9.81f;
    Vector3 velocity = Vector3.zero;
    public int sensitivity = 100;
    float xxrot = 0f;
    public float h = 50f;
    public float acc = 3f;
    bool iswl;
    float yyrot = 0f,xxmov= 0f,yymov =0f;
    float yv = 0.0f;
    Vector3 YMOV = Vector3.zero;
    Vector3 XMOV = Vector3.zero;
    float timetoslow = 0.1f;
    bool isslide = false;
    Vector3 cp ;
    bool iscrouch = false;
    float t=0f;
    float dist = 0f;
    public GameObject weaponhold;
    Transform x;
    Vector3 y;
    bool hasequipped;
   public Transform handfill;
    public LayerMask ledge;
    Vector3 grav = Vector3.zero;
    int count;
    float p;
    public Animator anim;
    bool recoil = false;
    float tim;
    void Start ()
    {
        
        acc = defacc;
        Cursor.lockState = CursorLockMode.Locked;
        isslide = false;
        iscrouch = false;
        t = 1f;
        hasequipped = false;
        count = 0;
        tim = 0f;
    }        
	
	

	void Update ()
    {
        
        isgrnd = Physics.CheckSphere(grndchk.position,0.2f,grnd);
        iswl = Physics.CheckSphere(grndchk.position,1f,wl);

        // Rotation
        float yrot = Input.GetAxisRaw("Mouse X")*sensitivity*Time.deltaTime;
        float xrot = Input.GetAxisRaw("Mouse Y")*sensitivity * Time.deltaTime;
        xxrot = xxrot - xrot;
        xxrot = Mathf.Clamp(xxrot,-90f,90f);
        yyrot = yyrot - yrot;
        

        if (Input.GetButton("Fire1") && Time.time >= p && weaponhold.transform.GetChild(1).gameObject.activeSelf)
        {
            
                p = Time.time + (1 / 10f);
            xxrot = xxrot - 80f*Time.deltaTime;
            recoil = true;
            
         
        }

        if (weaponhold.transform.GetChild(1).gameObject.activeSelf && recoil == true)
        {
            recoil = false;
        }
        
        transform.Rotate(Vector3.up*yrot);
        cam.localRotation = Quaternion.Euler(xxrot, 0f, 0f);
        

       
        // Movement

        float  ymov = Input.GetAxisRaw("Horizontal");
        float xmov = Input.GetAxisRaw("Vertical");
       
         
        if (Input.GetKey("left shift")  && isgrnd  )
        {
           
            
            if (acc < 80f)
            {
                
              if (isgrnd == true)
                { acc = acc + 4f; }
                
            }
            if (iscrouch)
            {
                iscrouch = false;
                
                t = 0f;
            }
            
                
        }
        else
        {

            if (acc > defacc)
            {
                acc = acc - 2f;
            }
            
            
            if (!isgrnd )
            { 
                acc = 30f;
                 
                
            }

            
        }

        XMOV =   transform.forward * xmov;
        YMOV =   transform.right * ymov;
        
        if (isgrnd && Input.GetKeyDown("left ctrl") && !(Input.GetKey("left shift")))
        {


            if (iscrouch)
            {
                iscrouch = false;
                acc = defacc;
                t = 0f;
            }
            else
            {
                iscrouch = true;


                t = 0f;
                if (!isslide && (acc > defacc))
                {
                    isslide = true;


                    acc = 70f * defacc;
                    movement = movement + (XMOV + YMOV).normalized * acc * Time.deltaTime;
                    c.Move((movement + velocity) * Time.deltaTime);
                    acc = 10f;

                }

            }



        }
       

        if (iswl && Input.GetKey("left shift"))
        {
            acc = 100f; 
            velocity.y = velocity.y  - Time.deltaTime* Mathf.SmoothDamp(30f*g*Time.deltaTime,g*Time.deltaTime, ref yv, 15f);
            if (iscrouch)
            {
                iscrouch = false;
                t = 0f;
            }
        }
        
        
        
        
        movement = movement + (XMOV + YMOV).normalized * acc * Time.deltaTime;
       
        if(Input.GetKeyDown("space") && isgrnd )
        {
            
           velocity.y = Mathf.Sqrt(-2f*g*h*3f);
            velocity = velocity + movement;
            
            
            if (iscrouch)
            {
                iscrouch = false;

                t = 0f;
            }
            

        }

        if (Input.GetKeyDown("space") && iswl)
        {
            RaycastHit hit;
            
            velocity.y = Mathf.Sqrt(-2f * g * h * 3f);
            
            if (Physics.Raycast(transform.position, transform.right, out hit, 3f, wl))
            {
                
                
                velocity = velocity + 20f * hit.normal;
            }
            if (Physics.Raycast(transform.position, -transform.right, out hit, 3f, wl))
            {
               velocity = velocity + 20f * hit.normal;
            }
            if (iscrouch)
            {
                iscrouch = false;

                t = 0f;
            }
        }
         

        if (Input.GetKeyDown("space") && iswl && isgrnd)
        {
            velocity.y = Mathf.Sqrt(-2f * g * h * 5f);
            if (iscrouch)
            {
                iscrouch = false;

                t = 0f;
            }
        }

        if (isslide && (acc <= defacc) && !iscrouch)
        {   
            isslide = false; 
        }
       
        if(Input.GetKeyDown("space") && !iswl && !isgrnd && count < 2 )
        {           
            velocity.y = Mathf.Sqrt(-2f * g * h * 5f);
            count++;
            if(count == 2)
            {
                count = 0;
            }
        }
      
        
     


        if (Input.GetKey("e")&& !hasequipped)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position,cam.transform.forward,out hit,3f))
            { if (hit.transform.parent == transform.parent && hit.rigidbody != null)
                {
                    Rigidbody rb = hit.rigidbody;
                    
                    rb.transform.parent = handfill;
                    rb.transform.localRotation = Quaternion.Euler(0f,0f,0f);
                    rb.transform.localPosition = Vector3.zero;
                    
                    rb.isKinematic = true;
                    parentequip(hit.transform.localPosition, hit.transform.localRotation,rb.gameObject);
                    weaponhold.SetActive(false);
                    hasequipped = true;

                }
            }
        }
        if((Input.GetButton("Fire2") || Input.GetButtonDown("Fire1"))  && hasequipped)
        {
            
            if (Physics.CheckSphere(handfill.transform.position, 0.5f))
            {
                if (handfill.GetChild(0).transform.parent != transform.parent)
                {
                    Debug.Log("change");
                    Rigidbody rb = handfill.GetChild(0).GetComponent<Rigidbody>();
                    rb.transform.parent = transform.parent;
                    rb.isKinematic = false;
                  
                    hasequipped = false;
                    weaponhold.SetActive(true);
                    if (Input.GetButtonDown("Fire1"))
                    {
                        rb.drag = 0.7f;
                        rb.AddForce(2000f * Time.deltaTime * cam.transform.forward, ForceMode.Impulse);
                    }

                }
            }
         
        }
        
       
        if (Input.GetKey("e") )
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10000f, wl))

            {
                grav = hit.normal.normalized * 500f * g;
                transform.up = hit.normal.normalized;


            }
            
        }


        if (Vector3.Magnitude(movement) > 0f)
        {
            anim.SetFloat("speed",1f,0.1f,Time.deltaTime);
        }
        if(Vector3.Magnitude(movement) < 0.5f)
        {
            anim.SetFloat("speed",-0.9f,0.05f,Time.deltaTime);
            Debug.Log("decrease");
        }

        movement = Vector3.Slerp(movement, Vector3.zero, Time.deltaTime * 5f);
        velocity =  Vector3.Slerp(velocity,Vector3.zero,Time.deltaTime);
        

        c.Move((movement + velocity + Vector3.Slerp(Vector3.zero, grav , Time.deltaTime* 0.1f )) * Time.deltaTime);
        
    }

    private void LateUpdate()
    {
        cam.transform.forward = Vector3.Slerp(cam.transform.forward, transform.forward, tim);
       if (tim <= 1f && !recoil )
        {
            tim = Mathf.SmoothDamp(tim, 1f, ref yv, 0.2f);
        }
        if(  Input.GetButton("Fire1")  && !recoil )
        {
            tim = 0f;
            
        }
        if(tim > 0.0f && !recoil)
        {
            tim = Mathf.SmoothDamp(tim, 0f, ref yv, 0.1f);
        }
        if (iscrouch && t < 1.0f)
        { cam.transform.localPosition = (cam.transform.localPosition - new Vector3(0f, Mathf.Lerp(0f, 0.2f, t), 0f));
            t = t + Time.deltaTime * 5f; }
        if(!iscrouch && t < 1.0f)
        {
            cam.transform.localPosition = (cam.transform.localPosition + new Vector3(0f, Mathf.Lerp(0f, 0.2f, t), 0f));
            t = t + Time.deltaTime * 5f;
        }
        
    }
   
    void parentdrop(GameObject g)
    {
     
        parentequip(g.transform.localPosition,g.transform.localRotation,g);

    }
    void parentequip(Vector3 y, Quaternion q, GameObject g)
    {
        
        g.transform.localPosition = y;
        g.transform.localRotation = q;

    }
  
}
