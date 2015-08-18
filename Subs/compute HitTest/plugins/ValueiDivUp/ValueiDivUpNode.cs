#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "iDivUp", Category = "Value", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class ValueiDivUpNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Element Count", DefaultValue = 1.0)]
		public ISpread<int> FInput;
		
		[Input("Group Size", DefaultValue = 1.0)]
		public ISpread<int> FGroup;

		[Output("Output")]
		public ISpread<int> FOutput;

		[Import()]
		public ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
		FOutput.SliceCount = SpreadMax;
		for (int i=0; i< SpreadMax; i++)
			{
				int gsize = FGroup[i];
				FOutput[i]  = (FInput[i] + gsize-1)/gsize;
				
			}
		}
	}
}
