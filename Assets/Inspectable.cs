using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Inspectable : NetworkBehaviour {
	public bool mobile = false;

	public bool selected;

	public SpriteRenderer selectorTemplate;

	public MovementController movement;

	SpriteRenderer selector;


	public void Start()
	{
		movement = GetComponent<MovementController>();
	}

	public void deselect()
	{
		selected = false;
		if (selector)
		{
			Destroy(selector.gameObject);
		}
	}

	public void select()
	{
		if (!selected)
		{
			selector = Instantiate(selectorTemplate);
			selector.transform.parent = transform;
			selector.transform.localPosition = Vector3.zero;
			selected = true;
		}
	}

	public void goTo(Vector3 position)
	{
		if (movement)
		{
			movement.goTo(position);
		}
	}

    [Command]
    void CmdDestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
    
    [Client]
    public void DestroySelf()
    {
        if (hasAuthority)
            CmdDestroySelf();
    }


}
