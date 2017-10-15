﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;


/// <summary>
/// Contains Trial data for the TippingPoint gameType.
/// </summary>
public class TippingPointTrial : Trial
{
	/// <summary>
	/// The distance ratio that will be targeted.
	/// </summary>
	public float duration = 0;


	#region ACCESSORS

	public float Duration
	{
		get
		{
			return duration;
		}
	}

	#endregion


	public TippingPointTrial(SessionData data, XmlElement n = null) 
		: base(data, n)
	{
	}
	

	/// <summary>
	/// Parses Game specific variables for this Trial from the given XmlElement.
	/// If no parsable attributes are found, or fail, then it will generate some from the given GameData.
	/// Used when parsing a Trial that IS defined in the Session file.
	/// </summary>
	public override void ParseGameSpecificVars(XmlNode n, SessionData session)
	{
		base.ParseGameSpecificVars(n, session);

		TippingPointData data = (TippingPointData)(session.gameData);
		if (!XMLUtil.ParseAttribute(n, TippingPointData.ATTRIBUTE_DURATION, ref duration, true))
		{
			duration = data.GeneratedDuration;
		}
	}


	/// <summary>
	/// Writes any tracked variables to the given XElement.
	/// </summary>
	public override void WriteOutputData(ref XElement elem)
	{
		base.WriteOutputData(ref elem);
		XMLUtil.CreateAttribute(TippingPointData.ATTRIBUTE_DURATION, duration.ToString(), ref elem);
	}
}
