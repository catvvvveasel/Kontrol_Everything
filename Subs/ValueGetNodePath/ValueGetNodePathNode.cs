#region usings
using System;
using System.ComponentModel.Composition;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;
using VVVV.PluginInterfaces.V2.Graph; //needed for getnodepath
using VVVV.Core.Logging;
using System.IO; //needed for truncating the path
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "GetNodePath", Category = "Value", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class ValueGetNodePathNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Use Descriptive Name", DefaultValue = 0)]
		public ISpread<bool> FUseDescriptive;

		[Output("NodePath")]
		public ISpread<string> FOutput;

		[Import()]
		public ILogger FLogger;
		
		[Import()]
		IPluginHost2 FHost;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			string path = FHost.GetNodePath(FUseDescriptive[0]);
			path = Path.GetDirectoryName(path);
			path = path.Replace(@"\", "/");
			FOutput[0] = path;
		}
	}
}
