using UnityEngine;
using System.Collections;
using System.Xml.Linq;


public class TippingPointTrialResult : TrialResult
{
	const string ATTRIBUTE_KEYCORRECT = "keycorrect"; 

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
	}
}
