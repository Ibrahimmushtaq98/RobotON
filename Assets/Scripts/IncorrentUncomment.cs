﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 
using UnityEngine;

public class IncorrentUncomment : comment
{
    public override void UpdateProtocol()
    {
        base.UpdateProtocol();
        if (CorrectCommentObject)
        {
            if (CorrectCommentObject.GetComponent<comment>().isCommented && !doneUpdating)
            {
                doneUpdating = true;
                if (entityType == stateLib.ENTITY_TYPE_INCORRECT_COMMENT)
                {
                    GetComponent<SpriteRenderer>().sprite = descSpriteOn;
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = codeSpriteOn;
                }
                string sNewText = blocktext;
                string[] sNewParts = sNewText.Split('\n');
                if (sNewParts.Length == 1 && commentStyle == "single")
                {
                    // Single line

                    //verify comment color is removed

                    GlobalState.level.Code[index] = textColoration.DecolorizeText(GlobalState.level.Code[index]);

                    GlobalState.level.Code[index] = "";
                }
                else
                {

                    // Multi line
                    for (int i = 0; i < sNewParts.Length; i++)
                    {
                        //GlobalState.level.Code[index+i] = textColoration.DecolorizeText( GlobalState.level.Code[index + i]);
                        GlobalState.level.Code[index + i] = "";
                    }
                }
                lg.DrawInnerXmlLinesToScreen();
            }
        }
    }
    protected override void OnTriggerProtocol(Collider2D collidingObj)
    {
        Debug.Log(collidingObj.name);
        if (collidingObj.name == stringLib.PROJECTILE_DEBUG && !doneUpdating) {
			Destroy(collidingObj.gameObject);
			selectedTool.outputtext.GetComponent<Text>().text = "There are errors with the selected code; \nfigure out what the mistake is, then \nuncomment the correct solution.";
		}
    }
}
