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
			if (FUpdate.IsChanged)
			{
				FLabel.SliceCount = SpreadMax;
				FTag.SliceCount = SpreadMax;
				FSubtype.SliceCount = SpreadMax;
				FBounds.SliceCount = SpreadMax;
				FType.SliceCount = SpreadMax;
				FValues.SliceCount = SpreadMax;
				FId.SliceCount = SpreadMax;
				
				for (int i = 0; i < SpreadMax; i++)
				{
					if (FUpdate[i])
					{
						var nodePath = FInput[i].Substring(0, FInput[i].LastIndexOf('/'));
						var node = FHDEHost.GetNodeFromPath(nodePath);
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
}