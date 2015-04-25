#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;
using System.Drawing;

using VVVV.Core.Logging;
using VVVV.PluginInterfaces.V2.Graph;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "GetSubpatchName", Category = "VVVV", Help = "Basic template with one string in/out", Tags = "")]
	#endregion PluginInfo
	public class VVVVGetSubpatchNameNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Node Path", DefaultString = "")]
		public ISpread<string> FNodePath;

		[Output("Node Name")]
		public ISpread<string> FNodeName;
		
		[Output("Shared Handle")]
		public ISpread<string> FSharedHandle;
		
		[Output("Left Node Pos")]
		ISpread<int> FLeftBounds;
		
		[Output("Node Found")]
		public ISpread<bool> FNodeFound;

		[Import()]
		public ILogger FLogger;
		
		[Import()]
		IHDEHost FHDEHost;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FNodeName.SliceCount = SpreadMax;
			FNodeFound.SliceCount = SpreadMax;
			FLeftBounds.SliceCount = SpreadMax;
			FSharedHandle.SliceCount = SpreadMax;
			
			for (int i = 0; i < SpreadMax; i++){
			
				var nodePath = FNodePath[i];
				var node = FHDEHost.GetNodeFromPath(nodePath);
			    if (node != null){
					//Rectangle
					Rectangle rectangle1 = node.GetBounds(0);
					FLeftBounds[i] = rectangle1.Left;
			    	
					string nodename = node.Name;
					FNodeName[i] = nodename;
			    	FNodeFound[i] = true;
			    	
			   		var SharedHandlePin = node.FindPin("Shared Handle");
			    	if (SharedHandlePin != null)
			   			FSharedHandle[i] = SharedHandlePin.Spread.Trim('|');
			    	else
						FSharedHandle[i] = "Not Found";
			    }
				
				else{			
					string nodename = "";
					FNodeName[i] = nodename;
				}

			}
			//FLogger.Log(LogType.Debug, "Logging to Renderer (TTY)");
		}
	}
}
