#region usings
using System;
using System.ComponentModel.Composition;
using System.Drawing;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.Graph;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;
using System.IO;
//needed for truncating the path
using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "PinInfo4_OnlyValues", Category = "VVVV", Help = "Returns details about given Pins", Tags = "")]
	#endregion PluginInfo
	public class VVVVPinInfo4_OnlyValuesNode : IPluginEvaluate
	{
		#region fields & pins
		#pragma warning disable 0649
		[Input("Input", DefaultString = "")]
		ISpread<string> FInput;

		[Input("Update", IsBang = true)]
		IDiffSpread<bool> FUpdate;
		[Input("Index", DefaultValue = 1.0)]
        public ISpread<int> FIndex;		
		
			[Input("Count", DefaultValue = 1.0)]
        public ISpread<int> FCount;

		[Output("Values")]
		ISpread<string> FValues;

//		[Output("Node Name")]
//		public ISpread<string> FNodeName;

		[Import()]
		IHDEHost FHDEHost;

		#pragma warning restore
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{

			FValues.SliceCount = FCount[0];


			for (int i = 0; i < SpreadMax; i++) {
				var nodePath = FInput[i].Substring(0, FInput[i].LastIndexOf('/'));
				var node = FHDEHost.GetNodeFromPath(nodePath);

//				string path = FInput[i].Substring(0, FInput[i].LastIndexOf('/'));
//				path = Path.GetDirectoryName(path);
//				path = path.Replace("\\", "/");
				//path =  Path.GetDirectoryName( path ) ;
//				var node2 = FHDEHost.GetNodeFromPath(path);

				if (FUpdate[i]) {
					if (node != null) {

						var parts = FInput[i].Split('/');
						var pin = node.FindPin(parts[parts.Length - 1]);

							FValues[i] = pin.Spread;

					}
				}
			}
		}
}	}

