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
	[PluginInfo(Name = "RadioSlice", Category = "Value", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class ValueRadioSliceNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input")]
		public ISpread<double> FInput;
		
		[Input("DefValue")]
		public ISpread<double> FDefault;

		[Input("Set")]
		public ISpread<float> FSet;
		
		
		[Output("Output")]
		public ISpread<double> FOutput;

		[Import()]
		public ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FOutput.SliceCount = 1;

			if (FSet[0]>= 1.0f )
			{
			FOutput[0]=FDefault[0];	
			}
			
else
			for (int i = 0; i < SpreadMax; i++)
			
			if (FInput[i]>=1.0f)
			{
				FOutput[0] = i ;

			}
 

			
			
			//FLogger.Log(LogType.Debug, "hi tty!");
		}
	}
}
