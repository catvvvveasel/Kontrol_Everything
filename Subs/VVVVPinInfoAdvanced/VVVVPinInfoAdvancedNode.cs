#region usings
using System;
using System.ComponentModel.Composition;
using System.Drawing;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.Graph;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "PinInfo",
	Category = "VVVV",
	Help = "Returns details about given Pins",
	Tags = "")]
	#endregion PluginInfo
	public class VVVVPinInfoNode : IPluginEvaluate
	{
		#region fields & pins
		#pragma warning disable 0649
		[Input("Input", DefaultString = "")]
		ISpread<string> FInput;
		
		[Input("Update", IsBang = true)]
		IDiffSpread<bool> FUpdate;
		
		[Output("Node Label")]
		ISpread<string> FLabel;
		
		[Output("Node Tag")]
		ISpread<string> FTag;
		
		[Output("Type")]
		ISpread<string> FType;
		
		[Output("Subtype")]
		ISpread<string> FSubtype;
		
		[Output("ValueType")]
		ISpread<string> FValueType;
		
		[Output("SliderBehavior")]
		ISpread<string> FSliderBehavior;
		
		
		[Output("Value Minimum")]
		ISpread<string> FValueMin;
		
		[Output("Value Maximum")]
		ISpread<string> FValueMax;
		
		[Output("Behavior")]
		ISpread<string> FBehavior;
		
		
		[Output("Enum Entries")]
		ISpread<string> FEnumEntries;
		
		[Output("Enum Entry Count")]
		ISpread<int> FEnumEntryCount;
		
		[Output("Bounds")]
		ISpread<int> FBounds;
		
		[Output("Pin ID")]
		ISpread<int> FId;
		
		[Output("Values")]
		ISpread<string> FValues;
		
		[Import()]
		IHDEHost FHDEHost;
		
		#pragma warning restore
		#endregion fields & pins
		
		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{

				FLabel.SliceCount = SpreadMax;
				FTag.SliceCount = SpreadMax;
				FSubtype.SliceCount = SpreadMax;
				FBounds.SliceCount = SpreadMax;
				FType.SliceCount = SpreadMax;
				FValues.SliceCount = SpreadMax;
				FId.SliceCount = SpreadMax;
				FEnumEntries.SliceCount = SpreadMax;
				FEnumEntryCount.SliceCount = SpreadMax;
				FValueType.SliceCount = SpreadMax;
				FSliderBehavior.SliceCount = SpreadMax;
				FBehavior.SliceCount = SpreadMax;
				FValueMin.SliceCount = SpreadMax;
				FValueMax.SliceCount = SpreadMax;
			
				for (int i = 0; i < SpreadMax; i++)
				{
					var nodePath = FInput[i].Substring(0, FInput[i].LastIndexOf('/'));
					var node = FHDEHost.GetNodeFromPath(nodePath);
					if (FUpdate[i])
					{
						if (node != null)
						{
							//Rectangle
							Rectangle rectangle1 = node.GetBounds(0);
							FBounds[i] = rectangle1.Left;
							
							FLabel[i] = node.LabelPin.Spread.Trim('|');
							var tag = node.FindPin("Tag");
							if (tag != null)
							FTag[i] = tag.Spread.Trim('|');
							else
							FTag[i] = "";
							
							var parts = FInput[i].Split('/');
							var pin = node.FindPin(parts[parts.Length - 1]);
							
							if (pin != null){
								FSubtype[i] = pin.SubType;
								FType[i] = pin.Type;
								FValues[i] = pin.Spread;
								FId[i] = node.ID;
								
								if (pin.Type == "Value"){
									var valuetypepin = node.FindPin("Value Type");
									FValueType[i] = valuetypepin.Spread;
								
									var sliderbehaviorpin = node.FindPin("Slider Behavior");
									FSliderBehavior[i] = sliderbehaviorpin.Spread;
								
									var behaviorpin = node.FindPin("Behavior");
									FBehavior[i] = behaviorpin.Spread;
									
									var minimumpin = node.FindPin("Minimum");
									FValueMin[i] = minimumpin.Spread;
									
									var maximumpin = node.FindPin("Maximum");
									FValueMax[i] = maximumpin.Spread;
									
								}
								
								if (pin.Type == "Enumeration"){
									string enumname = pin.SubType; //get the pins subtype
									enumname = enumname.Substring(enumname.IndexOf(',')+2, enumname.LastIndexOf(',')-enumname.IndexOf(',')-2 ); //cut out the part between the two commas (its the enum name)
									int enumcount = EnumManager.GetEnumEntryCount(enumname); //gets the enumentrycount. used to iterate through the entry indices 
									string enumentry = EnumManager.GetEnumEntry(enumname,0); //get the first enum entry
								
									for (int j = 1; j < enumcount; j++){ // get the rest of the enum entries
										enumentry = enumentry + ", " + EnumManager.GetEnumEntry(enumname,j); //add the entries up to one string
									}
								FEnumEntries[i] = enumentry;
								FEnumEntryCount[i] = enumcount;
								}
							    else {
									FEnumEntries[i] = "";	
							    	FEnumEntryCount[i] = 0;
							    }
							}

							else{
								FSubtype[i] = "Pin not found.";
								FType[i] = "Pin not found.";
							}
							

						}
						else
						{
							FLabel[i] = "Node not found.";
							FSubtype[i] = "Pin not found.";
						}
					}
				}
		}
	}
}