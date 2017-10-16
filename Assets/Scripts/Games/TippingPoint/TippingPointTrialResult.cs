using UnityEngine;
using System.Collections;
using System.Xml.Linq;


public class TippingPointTrialResult : TrialResult
{
	const string ATTRIBUTE_KEYCORRECT = "keycorrect"; 
	const string ATTRIBUTE_SIDE = "side"; 

	/// <summary>
	/// Is true if the key pressed on the trial was correct.
	/// </summary>
	public bool keyCorrect = false;


	public TippingPointTrialResult(Trial t) : base(t)
	{
	}


	public override void WriteOutputData(ref XElement elem)
	{
		base.WriteOutputData(ref elem);
		XMLUtil.CreateAttribute(ATTRIBUTE_KEYCORRECT, keyCorrect.ToString(), ref elem); //Add attribute for key status
		XMLUtil.CreateAttribute(ATTRIBUTE_SIDE, ((TippingPointTrial)trial).side, ref elem); //Add attribute for what side the square was on
	}
}
