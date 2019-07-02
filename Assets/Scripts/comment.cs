using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI; 
using System.Text.RegularExpressions;

/// <summary>
/// Comment is super class for all the various types comments, 
/// and now inherits from Tools. All children must inherit OnTriggerProtol 
/// and can optionally inherit UpdateProtocol. 
/// </summary>
public abstract class comment : Tools {
	public bool isCommented;
	public string commentStyle;
	public int entityType = -1;
	public int groupid    = -1;
	public int size = -1;

	public string oldtext   = "";
	public string blocktext = "";
	public string errmsg    = "";

	//public GameObject CodeObject;
	public GameObject CorrectCommentObject;
	
	protected Sprite descSpriteOff;
	protected Sprite descSpriteOn;
	protected Sprite codeSpriteOff;
	protected Sprite codeSpriteOn;
    

	protected bool doneUpdating = false;

	protected bool resetting  = false;

	protected float resetTime = 0f;
	protected float timeDelay = 30f;
    protected Animator anim; 

    protected TextColoration textColoration; 
    public void CleanBlocktext()
    {
        if (blocktext.Contains("$bug"))
        {
            Regex ansRgx = new Regex(@"((?<=\$bug).+(?=\$))");
            string answer = ansRgx.Match(blocktext).Value;
            blocktext = blocktext.Replace("$bug" + answer + "$", "");
        }
        if (blocktext.Contains("@"))
        {
            Regex paramRgx = new Regex(@"((?<=\@).+(?=\@))");
            Match match = paramRgx.Match(blocktext);
            while (match.Success)
            {
                string value = match.Value;
                blocktext = blocktext.Replace("@" + value + "@", "");
                match = match.NextMatch();
            }
        }
        if (blocktext.Contains("!!!"))
        {
            blocktext = blocktext.Replace("!!!", "");
        }
        if (blocktext.Contains("???"))
        {
            blocktext = blocktext.Replace("???", "");
        }
        string[] text = blocktext.Split('\n');
        for (int i = 0; i < text.Length; i++)
        {
            GlobalState.level.Code[index + i] = text[i];
        }
    }
    public override void Initialize()
    {
        string path = "Sprites/";
        anim = GetComponent<Animator>();
        descSpriteOff = Resources.LoadAll<Sprite>(path + "dComment")[2];
        descSpriteOn = Resources.LoadAll<Sprite>(path + "dComment")[0]; 
        codeSpriteOff = Resources.LoadAll<Sprite>(path + "cComment")[2];
        codeSpriteOn = Resources.LoadAll<Sprite>(path + "cComment")[0];
        if (entityType == stateLib.ENTITY_TYPE_CORRECT_COMMENT || entityType == stateLib.ENTITY_TYPE_INCORRECT_COMMENT)
        {
            if (isCommented)this.gameObject.GetComponent<SpriteRenderer>().sprite = descSpriteOn;
            else this.gameObject.GetComponent<SpriteRenderer>().sprite = descSpriteOff;
        }
        else if (entityType == stateLib.ENTITY_TYPE_ROBOBUG_COMMENT){
            this.gameObject.GetComponent<SpriteRenderer>().sprite = descSpriteOn; 
        }
        else
        {
            if (isCommented) this.gameObject.GetComponent<SpriteRenderer>().sprite = codeSpriteOn;
            else this.gameObject.GetComponent<SpriteRenderer>().sprite = codeSpriteOff;
        }
        textColoration = new TextColoration();
    }

    // Update is called once per frame
    void Update() {
        UpdateProtocol(); 
	}
    public virtual void UpdateProtocol() {}


	void OnTriggerEnter2D(Collider2D collidingObj) {
        
        OnTriggerProtocol(collidingObj); 
	}
    protected abstract void OnTriggerProtocol(Collider2D collidingObj); 


}
